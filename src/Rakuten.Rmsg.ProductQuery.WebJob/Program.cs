// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="Program.cs" company="Rakuten">
//   Copyright (c) Rakuten. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.WebJob
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;

    using Microsoft.Azure.WebJobs;
    using Microsoft.WindowsAzure.Storage;
    using Microsoft.WindowsAzure.Storage.Blob;

    using Rakuten.Azure.WebJobs;
    using Rakuten.Gpc.Api.Client;
    using Rakuten.Net.Http;
    using Rakuten.Rmsg.ProductQuery.Configuration;
    using Rakuten.Rmsg.ProductQuery.WebJob.Api;
    using Rakuten.Rmsg.ProductQuery.WebJob.Entities;
    using Rakuten.Rmsg.ProductQuery.WebJob.Linking;
    using Rakuten.Threading.Tasks.Dataflow;

    using UriTemplate = Rakuten.UriTemplate;

    /// <summary>
    /// An application that will execute as a WebJob in Microsoft's Azure.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// The main entry point of the application.
        /// </summary>
        public static void Main()
        {
            // Generate the connection strings to the diagnostics storage.
            IApiContext apiContext = new ApiContextFactory(new AppSettingsConfigurationSource()).Create();

            // Construct a delegate that will create a ApiClient instance.
            Func<ApiClient> createApiClient = () => CreateApiClient(apiContext);

            // Create a template to get a collection of data sources.
            var dataSourcesLink = new DataSourcesLink(new UriTemplate("/v1/datasources"));

            // Start a task that will fetch a collection of data sources.
            Task<IEnumerable<DataSource>> getDataSourcesTask = GetDataSourcesCommand.Execute(
                dataSourcesLink, 
                createApiClient);

            // Create a new database connection.
            var databaseContext = new ProductQueryContext();

            // Create a connection to blob storage.
            CloudBlobClient blobClient = 
                CloudStorageAccount.Parse(apiContext.StorageConnectionString).CreateCloudBlobClient();

            CloudBlobContainer blobContainer = blobClient.GetContainerReference(apiContext.BlobContainerName);

            // Create a product search template.
            var productSearchLink = new ProductSearchLink(new UriTemplate(
                "/v1/product?filter=(ISBN eq '{gtin}' or EAN eq '{gtin}' or JAN eq '{gtin}' or UPC eq '{gtin}')&culture={culture}&skip={skip}&top={top}"));

            var productLink = new ProductLink(new UriTemplate("/v1/product/{gran}?culture={culture}"));

            // Create a client through which requests can be made.
            var client = CreateApiClient(apiContext);

            // Create a cache for a collection of products.
            var searchCache = new ConcurrentDictionaryCache<IEnumerable<Product>>(
                parameters => ExecuteSearchCommand.GetProducts(client, productSearchLink, parameters).Result);

            // Create a cache for a single product.
            var productCache = new ConcurrentDictionaryCache<Product>(
                parameters => GetProductCommand.GetProductAsync(client, productLink, parameters).Result);

            // Wait for the collection of data sources to be returned.
            var dataSources = getDataSourcesTask.Result;

            // Create the delegate that the bound function will invoke.
            ProcessProductQueryFile.Process = (message, writer) =>
            {
                var transform1 = CreateParseFileTransform(message.Id, new CultureInfo(message.Culture));
                var transform2 = CreateGetQueryItemTransform(databaseContext);
                var transform3 = CreateCreateQueryItemTransform(databaseContext);
                var transform4 = CreateProductSearchTransform(searchCache);
                var transform5 = CreateFilterProductsTransform(dataSources);

                var dataflow = new ProcessFileDataflow(
                    TransformBlockFactory.Create<Message, Stream>(m =>
                        DownloadFileCommand.Execute(blobContainer, m, new MemoryStream())),
                    TransformManyBlockFactory.Create<Stream, ItemMessageState>(file => transform1(file, writer)),
                    TransformBlockFactory.Create<ItemMessageState, QueryMessageState>(state => transform2(state, writer)),
                    TransformBlockFactory.Create<QueryMessageState, QueryMessageState>(state => transform3(state, writer)),
                    TransformBlockFactory.Create<QueryMessageState, ProductsMessageState>(state => transform4(state, writer)),
                    TransformBlockFactory.Create<ProductsMessageState, QueryMessageState>(state => transform5(state, writer)),
                    null,
                    null,
                    null);

                dataflow.Post(message);
                dataflow.Complete();

                dataflow.Completion.Wait();

                writer.WriteLine("process.");
            };

            // Create a new host specifying the configuration.
            var host = new JobHost(new JobHostConfiguration(apiContext.DiagnosticsStorageConnectionString)
            {
                NameResolver = new CloudConfigNameResolver(),
                ServiceBusConnectionString = apiContext.ServiceBusConnectionString
            });

            // The following code ensures that the WebJob will be running continuously
            host.RunAndBlock();
        }

        /// <summary>
        /// Creates and returns a new <see cref="ApiClient"/>.
        /// </summary>
        /// <param name="environment">Environmental settings for the current GPC instance.</param>
        /// <param name="uriBuilders">
        /// The collection of <see cref="IUriBuilder"/> implementations to use when constructing URIs.
        /// </param>
        /// <returns>A new <see cref="ApiClient"/>.</returns>
        private static ApiClient CreateApiClient(IApiContext environment, params IUriBuilder[] uriBuilders)
        {
            var context = new ApiClientContext(environment);

            return new ApiClient(
                   context,
                   new UriBuilderMediator(uriBuilders),
                   new HttpRequestHandler(
                       context,
                       new HttpClient(),
                       () => Task.FromResult(new AuthenticationHeaderValue("Basic", context.AuthorizationToken))),
                   new HttpResponseHandler(
                       new HttpResponseValidator(
                           new ExceptionRegister())));
        }

        /// <summary>
        /// Returns a delegate that when executed will attempt to write a record to persistent storage for this query.
        /// </summary>
        /// <param name="context">The instance through which the persistent storage can be queried.</param>
        /// <returns>A <see cref="Func{T1,T2,TResult}"/>.</returns>
        private static Func<QueryMessageState, TextWriter, Task<QueryMessageState>> CreateCreateQueryItemTransform(
            ProductQueryContext context)
        {
            return async (state, writer) =>
            {
                try
                {
                    var query = await CreateProductQueryItemCommand.Execute(context, state.Id, state.Item.GtinValue);

                    return new QueryMessageState(state.Id, state.Culture, state.Item, query);
                }
                catch (Exception ex)
                {
                    writer.WriteLine("An issue was encountered creating a query record: " + ex.ToString());

                    return null;
                }
            };
        }

        /// <summary>
        /// Returns a delegate that when executed will filter a collection of products down to a single product using
        /// the spcified collection of data sources.
        /// </summary>
        /// <param name="dataSources">The collection of data sources to be used when filtering the products.</param>
        /// <returns>A <see cref="Func{T1,T2,TResult}"/>.</returns>
        private static Func<ProductsMessageState, TextWriter, Task<QueryMessageState>> CreateFilterProductsTransform(
            IEnumerable<DataSource> dataSources)
        {
            return async (state, writer) =>
            {
                try
                {
                    var product = await FilterProductsCommand.Execute(dataSources, state.Products);
                    state.Query.Gran = product.Id;

                    return new QueryMessageState(state.Id, state.Culture, state.Item, state.Query);
                }
                catch (Exception ex)
                {
                    writer.WriteLine("An issue was encountered filtering the collection of products: " + ex.ToString());

                    return null;
                }
            };
        }

        ////private static Func<MessageState, TextWriter, Task<QueryMessageState>> CreateGetProductTransform(
        ////    ICache<Product> cache)
        ////{
        ////    Contract.Requires(cache != null);

        ////    return async (state, writer) =>
        ////    {
        ////        try
        ////        {
        ////            var products = await GetProductCommand.Execute(
        ////                cache,
        ////                state.Query.Gran,
        ////                state.Culture);

        ////            return new QueryMessageState(state.Id, state.Culture, state.Item, state.Query, products);
        ////        }
        ////        catch (Exception ex)
        ////        {
        ////            writer.WriteLine("An issue was encountered parsing the uploaded file: " + ex.ToString());

        ////            return null;
        ////        }
        ////    };
        ////}

        /// <summary>
        /// Returns a delegate that when executed will attempt to retrieve the corresponding record from persistent 
        /// storage of a <see cref="Item"/>.
        /// </summary>
        /// <param name="context">The instance through which the persistent storage can be queried.</param>
        /// <returns>A <see cref="Func{T1,T2,TResult}"/>.</returns>
        private static Func<ItemMessageState, TextWriter, Task<QueryMessageState>> CreateGetQueryItemTransform(
            ProductQueryContext context)
        {
            return async (state, writer) =>
            {
                try
                {
                    var queryItem = await GetProductQueryItemCommand.Execute(context, state.Id, state.Item.GtinValue);

                    return new QueryMessageState(state.Id, state.Culture, state.Item, queryItem);
                }
                catch (Exception ex)
                {
                    writer.WriteLine("An issue was encountered parsing the uploaded file: " + ex.ToString());

                    return null;
                }
            };
        }

        /// <summary>
        /// Returns a delegate that when executed will take the stream and attempt to convert it into a collection of 
        /// <see cref="MessageState"/> instances.
        /// </summary>
        /// <param name="id">The unique identifier for the current query.</param>
        /// <param name="culture">The culture in which the product data should be expressed.</param>
        /// <returns>A <see cref="Func{T1,T2,TResult}"/>.</returns>
        private static Func<Stream, TextWriter, Task<IEnumerable<ItemMessageState>>> CreateParseFileTransform(
            Guid id, 
            CultureInfo culture)
        {
            Contract.Requires(id != Guid.Empty);

            return async (stream, writer) =>
            {
                try
                {
                    IEnumerable<Item> items = await ParseFileCommand.Execute(null, stream);

                    return from item in items select new ItemMessageState(id, culture, item);
                }
                catch (Exception ex)
                {
                    writer.WriteLine("An issue was encountered parsing the uploaded file: " + ex.ToString());

                    return null;
                }
            };
        }

        /// <summary>
        /// Returns a delegate that when executed will search for products with a specific GTIN and are expressed in a
        /// specific culture.
        /// </summary>
        /// <param name="cache">An instance that provides a method of caching collections of product.</param>
        /// <returns>A <see cref="Func{T1,T2,TResult}"/>.</returns>
        private static Func<QueryMessageState, TextWriter, Task<ProductsMessageState>> CreateProductSearchTransform(
            ICache<IEnumerable<Product>> cache)
        {
            Contract.Requires(cache != null);

            return async (state, writer) =>
            {
                try
                {
                    var products = await ExecuteSearchCommand.Execute(
                        cache, 
                        state.Item.GtinValue, 
                        state.Culture);

                    return new ProductsMessageState(state.Id, state.Culture, state.Item, state.Query, products);
                }
                catch (Exception ex)
                {
                    writer.WriteLine("An issue was encountered retrieving a collection of products: " + ex.ToString());

                    return null;
                }
            };
        }
    }
}
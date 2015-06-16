// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="Program.cs" company="Rakuten">
//   Copyright (c) Rakuten. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.WebJob
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading.Tasks;
    using System.Threading.Tasks.Dataflow;

    using LumenWorks.Framework.IO.Csv;

    using Microsoft.Azure.WebJobs;
    using Microsoft.WindowsAzure.Storage;
    using Microsoft.WindowsAzure.Storage.Blob;

    using Rakuten.Azure.WebJobs;
    using Rakuten.Gpc.Api.Client;
    using Rakuten.IO.Delimited.Serialization;
    using Rakuten.Net.Http;
    using Rakuten.Rmsg.ProductQuery.Configuration;
    using Rakuten.Rmsg.ProductQuery.WebJob.Api;
    using Rakuten.Rmsg.ProductQuery.WebJob.Linking;

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

            // Create the delegate that the bound function will invoke.
            ProcessProductQueryFile.Process = (message, writer) =>
            {
                // Construct a delegate that will create a ApiClient instance.
                Func<ApiClient> createApiClient = () => CreateApiClient(apiContext);

                // Create a new database connection.
                var databaseContext = new ProductQueryContext();

                // Create a connection to blob storage.
                CloudBlobClient blobClient =
                    CloudStorageAccount.Parse(apiContext.StorageConnectionString).CreateCloudBlobClient();

                CloudBlobContainer blobContainer = blobClient.GetContainerReference(apiContext.BlobContainerName);

                // Create a template to get a collection of data sources.
                var dataSourcesLink = new DataSourcesLink(new UriTemplate("/v1/datasources"));

                // Start a task that will fetch a collection of data sources.
                Task<IEnumerable<DataSource>> getDataSourcesTask = GetDataSourcesCommand.Execute(
                    dataSourcesLink,
                    createApiClient);

                // Start a task that will fetch the status that represents the completed status.
                Task<ProductQueryStatus> getCompletedStatusTask = GetCompletedProductQueryStatusCommand.Execute(
                    databaseContext);

                // Create a cache for a collection of products.
                var searchCache = CreateProductsCache(createApiClient);

                // Create a cache for a single product.
                var productCache = CreateProductCache(createApiClient);

                var serializer = new LumenWorksSerializer<Item>();

                Task.WaitAll(getDataSourcesTask, getCompletedStatusTask);

                var memoryStream = new MemoryStream();

                var parseFileTransform = ParseFileTransformFactory.Create(
                    stream1 => Task.Run(() =>
                    {
                        using (var streamReader = new StreamReader(stream1, true))
                        using (var delimitedReader = new CsvReader(streamReader, true, ','))
                        {
                            return serializer.ReadFileByHeaders(
                                delimitedReader,
                                exception => writer.WriteLine(exception.ToString())).ToList().AsEnumerable();
                        }
                    }),
                    message.Id,
                    new CultureInfo(message.Culture));
                var getQueryItemTransform = GetQueryItemTransformFactory.Create(
                    (id, gtin) => GetProductQueryItemCommand.Execute(
                        (guid, s) => FindProductQueryItemCommand.Execute(databaseContext, guid, s),
                        id, 
                        gtin));
                var createQueryItemTransform = CreateQueryItemTransformFactory.Create(
                    (guid, s) => 
                        CreateProductQueryItemCommand.Execute(
                            entity => AddProductQueryItemCommand.Execute(databaseContext, entity), 
                            guid, 
                            s));
                var productSearchTransform = ProductSearchTransformFactory.Create(
                    (type, gtin, culture) => 
                        searchCache.GetOrAddAsync(string.Concat(type, gtin, culture.Name), type, gtin, culture.Name));
                var filterProductsTransform = FilterProductsTransformFactory.Create(
                    products => FilterProductsCommand.Execute(getDataSourcesTask.Result, products));
                var updateEntityTransform = UpdateEntityTransformFactory.Create(
                    entity => UpdateProductQueryItemCommand.Execute(databaseContext, entity));
                var getProductTransform = GetProductTransformFactory.Create(
                    (gran, culture) =>
                        productCache.GetOrAddAsync(string.Concat(gran, culture.Name), gran, culture.Name));
                var mergeProductTransform = MergeProductTransformFactory.Create(MergeProductCommand.Execute);

                var dataflow = new ProcessFileDataflow(
                    new TransformBlock<Message, Stream>(msg => DownloadFileCommand.Execute(
                        (blobName, content) => DownloadCloudBlobCommand.Execute(blobContainer, blobName, content), 
                        msg,
                        memoryStream)),
                    new TransformManyBlock<Stream, ItemMessageState>(file => parseFileTransform(file, writer)),
                    new TransformBlock<ItemMessageState, ItemMessageState>(
                        state => getQueryItemTransform(state, writer)),
                    new TransformBlock<ItemMessageState, ItemMessageState>(
                        state => createQueryItemTransform(state, writer)),
                    new TransformBlock<ItemMessageState, ItemMessageState>(
                        state => productSearchTransform(state, writer)),
                    new TransformBlock<ItemMessageState, ItemMessageState>(
                        state => filterProductsTransform(state, writer)),
                    new TransformBlock<ItemMessageState, ItemMessageState>(
                        state => updateEntityTransform(state, writer)),
                    new TransformBlock<ItemMessageState, ItemMessageState>(
                        state => getProductTransform(state, writer)),
                    new TransformBlock<ItemMessageState, ItemMessageState>(
                        state => mergeProductTransform(state, writer)),
                    new TransformBlock<ItemMessageState, Item>(state => Task.FromResult(state.Item)));

                dataflow.Post(message);
                dataflow.Complete();

                var items = new List<Item>();

                // While we are receiving items from the dataflow and the dataflow has not finished.
                while (!(dataflow.Completion.IsCanceled || dataflow.Completion.IsCompleted || dataflow.Completion.IsFaulted))
                {
                    IList<Item> newItems;

                    // Receive a batch of items from the dataflow.
                    if (dataflow.TryReceiveAll(out newItems))
                    {
                        items.AddRange(newItems);
                    }
                }

                // Create a stream to serialize to.
                var writeStream = new MemoryStream();

                // Write out the list of items collected from the dataflow to the blob.
                ParseItemsCommand.Execute(items, new StreamWriter(writeStream, Encoding.UTF8), serializer).Wait();

                WriteBlobCommand.Execute(
                    (file, filename) => UploadCloudBlobCommand.Execute(blobContainer, writeStream, filename), 
                    message,
                    writeStream)
                    .Wait();

                // Mark the product query as processed.
                UpdateProductQueryCommand.Execute(databaseContext, getCompletedStatusTask.Result, message.Id).Wait();

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
            var context = new ApiClientContext(
                environment.GpcCoreApiBaseAddress,
                environment.AuthenticationToken);

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
        /// Creates a new <see cref="ConcurrentDictionaryCache{T}"/> instance that will <see cref="Product"/>s.
        /// </summary>
        /// <param name="createApiClient">A delegate that will create a new <see cref="ApiClient"/> instance.</param>
        /// <returns>An instance that will cache instances of <see cref="Product"/>.</returns>
        private static ConcurrentDictionaryCache<Product> CreateProductCache(Func<ApiClient> createApiClient)
        {
            return new ConcurrentDictionaryCache<Product>(
                parameters => GetProductCommand.Execute(
                    createApiClient, 
                    new ProductLink(new UriTemplate("/v1/product/{gran}?culture={culture}")), 
                    parameters).Result);
        }

        /// <summary>
        /// Creates a new <see cref="ConcurrentDictionaryCache{T}"/> instance that will cache a collection of 
        /// <see cref="Product"/>s.
        /// </summary>
        /// <param name="createApiClient">A delegate that will create a new <see cref="ApiClient"/> instance.</param>
        /// <returns>An instance that will cache collections of <see cref="Product"/>.</returns>
        private static ConcurrentDictionaryCache<IEnumerable<Product>> CreateProductsCache(
            Func<ApiClient> createApiClient)
        {
            return new ConcurrentDictionaryCache<IEnumerable<Product>>(
                parameters => ExecuteSearchCommand.Execute(
                    createApiClient,
                    new ProductSearchLink(new UriTemplate(
                        "/v1/product?filter={filter}&culture={culture}&skip={skip}&top={top}")),
                    parameters).Result);
        }
    }
}
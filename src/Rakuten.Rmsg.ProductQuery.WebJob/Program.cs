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
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;
    using System.Threading.Tasks.Dataflow;

    using Microsoft.Azure.WebJobs;
    using Microsoft.WindowsAzure.Storage;
    using Microsoft.WindowsAzure.Storage.Blob;

    using Rakuten.Azure.WebJobs;
    using Rakuten.Gpc.Api.Client;
    using Rakuten.IO.Delimited.Serialization;
    using Rakuten.Net.Http;
    using Rakuten.Rmsg.ProductQuery.Configuration;
    using Rakuten.Rmsg.ProductQuery.WebJob.Api;
    using Rakuten.Rmsg.ProductQuery.WebJob.Entities;
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

            // Create a cache for a collection of products.
            var searchCache = CreateProductsCache(createApiClient);

            // Create a cache for a single product.
            var productCache = CreateProductCache(createApiClient);

            var serializer = new LumenWorksSerializer<Item>();

            // Wait for the collection of data sources to be returned.
            var dataSources = getDataSourcesTask.Result;

            // Create the delegate that the bound function will invoke.
            ProcessProductQueryFile.Process = (message, writer) =>
            {
                var parseFileTransform = ParseFileTransformFactory.Create(
                    () => ParseFileCommand.Execute(
                        serializer, 
                        new MemoryStream()), 
                        message.Id, 
                        new CultureInfo(message.Culture));
                var getQueryItemTransform = GetQueryItemTransformFactory.Create(
                    (id, gtin) => GetProductQueryItemCommand.Execute(databaseContext, id, gtin));
                var createQueryItemTransform = CreateQueryItemTransformFactory.Create(
                    (guid, s) => CreateProductQueryItemCommand.Execute(databaseContext, guid, s));
                var productSearchTransform = ProductSearchTransformFactory.Create(
                    (type, gtin, culture) => ExecuteSearchCommand.Execute(searchCache, type, gtin, culture));
                var filterProductsTransform = FilterProductsTransformFactory.Create(
                    products => FilterProductsCommand.Execute(dataSources, products));
                var updateEntityTransform = UpdateEntityTransformFactory.Create(
                    query => UpdateEntityBlockCommand.Execute(databaseContext, query));
                var getProductTransform = GetProductTransformFactory.Create(
                    (gran, culture) => GetProductCommand.Execute(productCache, gran, culture));
                var mergeProductTransform = MergeProductTransformFactory.Create(MergeProductCommand.Execute);

                var dataflow = new ProcessFileDataflow(
                    new TransformBlock<Message, Stream>(msg => DownloadFileCommand.Execute(
                        (blobName, content) => DownloadCloudBlob(blobContainer, blobName, content), 
                        msg, 
                        new MemoryStream())),
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

                bool itemsReceived = false;

                // While we are receiving items from the dataflow and the dataflow has not finished.
                while (!itemsReceived && (dataflow.Completion.IsCanceled || dataflow.Completion.IsCompleted || dataflow.Completion.IsFaulted))
                {
                    IList<Item> newItems;

                    // Receive a batch of items from the dataflow.
                    itemsReceived = dataflow.TryReceiveAll(out newItems);
                    if (itemsReceived)
                    {
                        items.AddRange(newItems);
                    }
                }

                // Write out the list of items collected from the dataflow to the blob.
                var stream = ParseItemsCommand.Execute(items, new MemoryStream(), serializer).Result;

                WriteBlobCommand.Execute(
                    (file, filename) => UploadCloudBlob(blobContainer, stream, filename), 
                    message, 
                    stream)
                    .Wait();

                // Mark the product query as processed.
                UpdateProductQueryCommand.Execute(databaseContext, message.Id).Wait();

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
        /// Creates a new <see cref="ConcurrentDictionaryCache{T}"/> instance that will <see cref="Product"/>s.
        /// </summary>
        /// <param name="createApiClient">A delegate that will create a new <see cref="ApiClient"/> instance.</param>
        /// <returns>An instance that will cache instances of <see cref="Product"/>.</returns>
        private static ConcurrentDictionaryCache<Product> CreateProductCache(Func<ApiClient> createApiClient)
        {
            return new ConcurrentDictionaryCache<Product>(
                parameters => GetProductCommand.GetProductAsync(
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
                parameters => ExecuteSearchCommand.GetProducts(
                    createApiClient,
                    new ProductSearchLink(new UriTemplate(
                        "/v1/product?filter=ISBN eq '{gtin}'&culture={culture}&skip={skip}&top={top}")),
                    new ProductSearchLink(new UriTemplate(
                        "/v1/product?filter=EAN eq '{gtin}'&culture={culture}&skip={skip}&top={top}")),
                    new ProductSearchLink(new UriTemplate(
                        "/v1/product?filter=JAN eq '{gtin}'&culture={culture}&skip={skip}&top={top}")),
                    new ProductSearchLink(new UriTemplate(
                        "/v1/product?filter=UPC eq '{gtin}'&culture={culture}&skip={skip}&top={top}")),
                    parameters).Result);
        }

        /// <summary>
        /// Downloads the specified file from the given container to the given <see cref="Stream"/>.
        /// </summary>
        /// <param name="container">The container in which the specified blob is located.</param>
        /// <param name="filename">A string containing the name of the blob.</param>
        /// <param name="stream">The <see cref="Stream"/> to which the file will be downloaded.</param>
        /// <returns>
        /// A <see cref="Task"/> the represents the asynchronous operation where the task result will be the stream 
        /// containing the blob contents.
        /// </returns>
        private static async Task<Stream> DownloadCloudBlob(
            CloudBlobContainer container, 
            string filename, 
            Stream stream)
        {
            // Get a reference to the blob in the container.
            CloudBlockBlob blob = container.GetBlockBlobReference(filename);

            // Ensure the blob exists.
            if (!blob.Exists())
            {
                throw new InvalidOperationException("The specified blob was not found within the given container.");
            }

            await blob.DownloadToStreamAsync(stream);

            return stream;
        }

        /// <summary>
        /// Uploads the content from the given stream into a blob contained within the given container.
        /// </summary>
        /// <param name="container">The container in which the specified blob is located.</param>
        /// <param name="stream">The <see cref="Stream"/> to which the content will be written.</param>
        /// <param name="filename">A string containing the name of the blob.</param>
        /// <returns>A <see cref="Task"/> the represents the asynchronous operation.</returns>
        private static async Task UploadCloudBlob(CloudBlobContainer container, Stream stream, string filename)
        {
            // Get a reference to the blob in the container.
            CloudBlockBlob blob = container.GetBlockBlobReference(filename);

            await blob.UploadFromStreamAsync(stream);
        }
    }
}
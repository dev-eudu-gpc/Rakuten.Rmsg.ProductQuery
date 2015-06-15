//------------------------------------------------------------------------------
// <copyright file="AzureStorageSteps.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.Web.Http.Tests.Integration
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.WindowsAzure.Storage;
    using Microsoft.WindowsAzure.Storage.Blob;
    using Newtonsoft.Json;
    using Rakuten.Rmsg.ProductQuery.Configuration;
    using TechTalk.SpecFlow;

    /// <summary>
    /// Steps pertaining to Azure storage.
    /// </summary>
    [Binding]
    public class AzureStorageSteps
    {
        /// <summary>
        /// The context under which this instance is operating.
        /// </summary>
        private readonly IApiContext apiContext;

        /// <summary>
        /// A means to interact with storage.
        /// </summary>
        private readonly IStorage storage;

        /// <summary>
        /// Initializes a new instance of the <see cref="AzureStorageSteps"/> class
        /// </summary>
        /// <param name="apiContext">The context under which this instance is operating.</param>
        /// <param name="storage">A means to interact with storage.</param>
        public AzureStorageSteps(
            IApiContext apiContext,
            IStorage storage)
        {
            Contract.Requires(apiContext != null);
            Contract.Requires(storage != null);

            this.apiContext = apiContext;
            this.storage = storage;
        }

        /// <summary>
        /// Uploads a file to blob storage.
        /// </summary>
        [Given(@"the file is uploaded to blob storage")]
        public void GivenTheFileIsUploadedToBlobStorage()
        {
            ////// Create a connection to blob storage.
            ////CloudBlobClient blobClient = CloudStorageAccount
            ////    .Parse(this.apiContext.StorageConnectionString)
            ////    .CreateCloudBlobClient();

            ////CloudBlobContainer blobContainer = blobClient.GetContainerReference(this.apiContext.BlobContainerName);

            // Get the product query from the response body
            var content = ScenarioStorage.HttpResponseMessage.Content.ReadAsStringAsync().Result;
            var productQuery = JsonConvert.DeserializeObject<ProductQuery>(content);

            ////CloudBlockBlob blob = blobContainer.GetBlockBlobReference(filename);

            // Get the file
            /////blob.UploadFromFile(ScenarioStorage.ProductQueryFileName, FileMode.Open);

            this.storage.UploadFile(
                this.apiContext.StorageConnectionString,
                this.apiContext.BlobContainerName,
                productQuery.GetBlobName(),
                ScenarioStorage.ProductQueryFileName);
        }

        /// <summary>
        /// Retrieves the product query file from storage.
        /// </summary>
        [When(@"the file is retrieved from storage")]
        public void WhenTheFileIsRetrievedFromStorage()
        {
            // Get the product query from the response body
            var content = ScenarioStorage.HttpResponseMessage.Content.ReadAsStringAsync().Result;
            var productQuery = JsonConvert.DeserializeObject<ProductQuery>(content);

            // Get a reference to the blob in the container.
            CloudBlockBlob blob = this.storage.GetBlob(
                this.apiContext.StorageConnectionString,
                this.apiContext.BlobContainerName,
                productQuery.GetBlobName());

            // Ensure the blob exists.
            if (!blob.Exists())
            {
                throw new InvalidOperationException("The specified blob was not found within the given container.");
            }

            ////await blob.DownloadToStreamAsync(stream);
        }
    }
}

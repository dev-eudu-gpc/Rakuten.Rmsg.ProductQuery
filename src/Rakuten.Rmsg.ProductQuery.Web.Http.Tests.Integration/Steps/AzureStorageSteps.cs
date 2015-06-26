//------------------------------------------------------------------------------
// <copyright file="AzureStorageSteps.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.Web.Http.Tests.Integration.Steps
{
    using System;
    using System.Diagnostics.Contracts;
    using System.IO;
    using Microsoft.WindowsAzure.Storage.Blob;
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
        /// An object for storing and retrieving information from the scenario context.
        /// </summary>
        private readonly ScenarioStorage scenarioStorage;

        /// <summary>
        /// Initializes a new instance of the <see cref="AzureStorageSteps"/> class
        /// </summary>
        /// <param name="apiContext">The context under which this instance is operating.</param>
        /// <param name="storage">A means to interact with storage.</param>
        /// <param name="scenarioStorage">An object for sharing information between steps.</param>
        public AzureStorageSteps(
            IApiContext apiContext,
            IStorage storage,
            ScenarioStorage scenarioStorage)
        {
            Contract.Requires(apiContext != null);
            Contract.Requires(scenarioStorage != null);
            Contract.Requires(storage != null);

            this.apiContext = apiContext;
            this.scenarioStorage = scenarioStorage;
            this.storage = storage;
        }

        /// <summary>
        /// Uploads a file to blob storage.
        /// </summary>
        [Given(@"the file is uploaded to blob storage")]
        public void GivenTheFileIsUploadedToBlobStorage()
        {
            // Get the product query from the response to the call to create the product query
            var productQuery = this.scenarioStorage.Creation.ResponseProductQuery;

            // Upload to storage
            this.storage.UploadFile(
                this.apiContext.StorageConnectionString,
                this.apiContext.BlobContainerName,
                productQuery.GetBlobName(),
                this.scenarioStorage.Files.SourceFileName);
        }

        /// <summary>
        /// Retrieves the product query results file from storage.
        /// </summary>
        [When(@"the results file is retrieved from storage")]
        public void WhenTheResultsFileIsRetrievedFromStorage()
        {
            // Get the product query from the response to the call to create the product query
            var productQuery = this.scenarioStorage.Creation.ResponseProductQuery;

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

            // Download the file and store it locally
            var fileName = Path.Combine(System.IO.Path.GetTempPath(), "rmsg-product-query-result.tmp");

            blob.DownloadToFile(fileName, FileMode.Create);

            // Store pertinent details for subsequent steps
            this.scenarioStorage.Files.ResultFileName = fileName;
        }
    }
}

//------------------------------------------------------------------------------
// <copyright file="ProductQueryResponseSteps.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.Web.Http.Tests.Integration.Steps
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Rakuten.Rmsg.ProductQuery.Configuration;
    using Rakuten.Rmsg.ProductQuery.Web.Http.Tests.Integration.Resources;
    using TechTalk.SpecFlow;

    /// <summary>
    /// Steps for working with <see cref="ProductQuery"/> objects in an HTTP response body.
    /// </summary>
    [Binding]
    public class ProductQueryResponseSteps
    {
        /// <summary>
        /// The context under which this instance is operating.
        /// </summary>
        private readonly IApiContext apiContext;

        /// <summary>
        /// An object for storing and retrieving information from the scenario context.
        /// </summary>
        private readonly ScenarioStorage scenarioStorage;

        /// <summary>
        /// The object for interacting with storage.
        /// </summary>
        private readonly IStorage storage;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductQueryResponseSteps"/> class
        /// </summary>
        /// <param name="apiContext">A context for the API.</param>
        /// <param name="storage">A means to interact with storage for the product query</param>
        /// <param name="scenarioStorage">An object for sharing information between steps.</param>
        public ProductQueryResponseSteps(
            IApiContext apiContext,
            IStorage storage,
            ScenarioStorage scenarioStorage)
        {
            Contract.Requires(apiContext != null);
            Contract.Requires(storage != null);
            Contract.Requires(scenarioStorage != null);

            this.apiContext = apiContext;
            this.storage = storage;
            this.scenarioStorage = scenarioStorage;
        }

        /// <summary>
        /// Verifies that the date created of the product query from the database that is stored
        /// in the scenario context is not null
        /// </summary>
        [Then(@"the culture of the product query in the database matches the culture in the new product query")]
        public void ThenTheCultureOfTheProductQueryInTheDatabaseMatchesTheCultureInTheNewProductQuery()
        {
            using (var database = new ProductQueryDbContext())
            {
                var databaseEntity = database.rmsgProductQueries
                    .Single(q => q.rmsgProductQueryID == this.scenarioStorage.Creation.SourceProductQuery.IdAsGuid);

                Assert.AreEqual(
                    this.scenarioStorage.Creation.SourceProductQuery.Culture,
                    databaseEntity.culture,
                    true);
            }
        }

        /// <summary>
        /// Verifies that the date created of the product query from the database that is stored
        /// in the scenario context is not null
        /// </summary>
        [Then(@"the date created of the product query from the database is not null")]
        public void ThenTheDateCreatedOfTheProductQueryInTheDatabaseIsNotNull()
        {
            using (var database = new ProductQueryDbContext())
            {
                var databaseEntity = database.rmsgProductQueries
                    .Single(q => q.rmsgProductQueryID == this.scenarioStorage.Creation.SourceProductQuery.IdAsGuid);

                Assert.IsNotNull(databaseEntity.dateCreated);
            }
        }

        /// <summary>
        /// Verifies that the status of the product query in the database matches the specified status.
        /// </summary>
        /// <param name="expectedStatus">The expected status.</param>
        [Then(@"the status of the product query in the database is (.*)")]
        public void ThenTheStatusOfTheProductQueryInTheDatabaseIs(string expectedStatus)
        {
            ProductQueryStatus parsedStatus;
            Enum.TryParse<ProductQueryStatus>(expectedStatus, out parsedStatus);

            Assert.IsNotNull(parsedStatus, string.Format("An invalid status of '{0}' was passed into the step.", expectedStatus));

            using (var database = new ProductQueryDbContext())
            {
                var databaseEntity = database.rmsgProductQueries
                    .Single(q => q.rmsgProductQueryID == this.scenarioStorage.Creation.SourceProductQuery.IdAsGuid);

                Assert.AreEqual(parsedStatus, (ProductQueryStatus)databaseEntity.rmsgProductQueryStatusID);
            }
        }

        /// <summary>
        /// Verifies that the URI of the product query from the database that is stored
        /// in the scenario context is as expected
        /// </summary>
        [Then(@"the URI of the product query from the database matches the storage blob URI")]
        public void ThenTheURIOfTheProductQueryFromTheDatabaseMatchesTheStorageBlobURI()
        {
            using (var database = new ProductQueryDbContext())
            {
                var databaseEntity = database.rmsgProductQueries
                    .Single(q => q.rmsgProductQueryID == this.scenarioStorage.Creation.SourceProductQuery.IdAsGuid);

                // Construct the expected URI
                string blobName = string.Format(
                    "{0}/{1}.txt",
                    databaseEntity.dateCreated.ToString("yyyy-MM-dd"),
                    databaseEntity.rmsgProductQueryID);

                var expectedUri = this.storage.GetSharedAccessSignature(
                    this.apiContext.StorageConnectionString,
                    this.apiContext.BlobContainerName,
                    blobName);

                // Assert
                Assert.AreEqual(expectedUri.AbsoluteUri, databaseEntity.uri);
            }
        }
    }
}
//------------------------------------------------------------------------------
// <copyright file="ProductQueryResponseSteps.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.Web.Http.Tests.Integration.Steps
{
    using System;
    using System.Diagnostics.Contracts;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Rakuten.Rmsg.ProductQuery.Configuration;
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
        /// The object for interacting with storage.
        /// </summary>
        private readonly IStorage storage;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductQueryResponseSteps"/> class
        /// </summary>
        /// <param name="apiContext">A context for the API.</param>
        /// <param name="storage">A means to interact with storage for the product query</param>
        public ProductQueryResponseSteps(
            IApiContext apiContext,
            IStorage storage)
        {
            Contract.Requires(apiContext != null);
            Contract.Requires(storage != null);

            this.apiContext = apiContext;
            this.storage = storage;
        }

        /// <summary>
        /// Verifies that the date created of the product query from the database that is stored
        /// in the scenario context is not null
        /// </summary>
        [Then(@"the culture of the product query from the database matches the culture in the new product query")]
        public void ThenTheCultureOfTheProductQueryFromTheDatabaseMatchesTheCultureInTheNewProductQuery()
        {
            // Get the new product query that was submitted to the API
            var productQuery = ScenarioStorage.NewProductQuery;

            // Assert
            Assert.AreEqual(productQuery.Culture, ScenarioStorage.ProductQueryFromDatabase.culture, true);
        }

        /// <summary>
        /// Verifies that the date created of the product query from the database that is stored
        /// in the scenario context is not null
        /// </summary>
        [Then(@"the date created of the product query from the database is not null")]
        public void ThenTheDateCreatedOfTheProductQueryFromTheDatabaseIsNotNull()
        {
            Assert.IsNotNull(ScenarioStorage.ProductQueryFromDatabase.dateCreated);
        }

        /// <summary>
        /// Verifies that the status of the product query from the database that is stored
        /// in the scenario context matches the specified status.
        /// </summary>
        /// <param name="expectedStatus">The expected status.</param>
        [Then(@"the status of the product query from the database is (.*)")]
        public void ThenTheStatusOfTheProductQueryFromTheDatabaseIs(string expectedStatus)
        {
            Assert.AreEqual(
                (int)Enum.Parse(typeof(ProductQueryStatus), expectedStatus),
                ScenarioStorage.ProductQueryFromDatabase.rmsgProductQueryStatusID);
        }

        /// <summary>
        /// Verifies that the URI of the product query from the database that is stored
        /// in the scenario context is as expected
        /// </summary>
        [Then(@"the URI of the product query from the database matches the storage blob URI")]
        public void ThenTheURIOfTheProductQueryFromTheDatabaseMatchesTheStorageBlobURI()
        {
            // Get the new product query that is stored in scenario context
            var productQuery = ScenarioStorage.ProductQueryFromDatabase;

            // Construct the expected URI
            string blobName = string.Format(
                "{0}/{1}.txt",
                productQuery.dateCreated.ToString("yyyy-MM-dd"),
                productQuery.rmsgProductQueryID);

            var expectedUri = this.storage.GetSharedAccessSignature(
                this.apiContext.StorageConnectionString,
                this.apiContext.BlobContainerName,
                blobName);

            // Assert
            Assert.AreEqual(expectedUri.AbsoluteUri, productQuery.uri);
        }
    }
}
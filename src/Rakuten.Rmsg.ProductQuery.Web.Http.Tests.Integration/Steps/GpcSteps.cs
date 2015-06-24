//------------------------------------------------------------------------------
// <copyright file="GpcSteps.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.Web.Http.Tests.Integration
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using Newtonsoft.Json;
    using Rakuten.Rmsg.ProductQuery.Configuration;
    using TechTalk.SpecFlow;

    /// <summary>
    /// Steps pertaining to the GPC core API.
    /// </summary>
    [Binding]
    public class GpcSteps
    {
        /// <summary>
        /// A client for communicating with the GPC core API.
        /// </summary>
        private readonly GpcApiClient apiClient;

        /// <summary>
        /// The context under which the test is operating.
        /// </summary>
        private readonly IApiContext databaseContext;

        /// <summary>
        /// A list of all data sources.
        /// </summary>
        private readonly List<DataSource> dataSources;

        /// <summary>
        /// Initializes a new instance of the <see cref="GpcSteps"/> class
        /// </summary>
        /// <param name="apiClient">A client for communicating with the GPC core API.</param>
        /// <param name="apiContext">The context under which the test is operating.</param>
        /// <param name="dataSources">A list of all data sources.</param>
        public GpcSteps(
            GpcApiClient apiClient,
            IApiContext apiContext,
            List<DataSource> dataSources)
        {
            Contract.Requires(apiClient != null);
            Contract.Requires(apiContext != null);
            Contract.Requires(dataSources != null);

            this.apiClient = apiClient;
            this.databaseContext = apiContext;
            this.dataSources = dataSources;
        }

        /// <summary>
        /// Creates a new product with a valid EAN.
        /// </summary>
        /// <param name="culture">The culture for the product</param>
        /// <param name="identifier">The identifier to use for the product.</param>
        [Given(@"a new product with a culture of (.*) has been created in GPC using (.*) as the identifier")]
        public void GivenANewProductHasBeenCreatedInGPCForTheSpecifiedCultureUsingTheSpecifiedIdentifier(
            string culture,
            IdentifierType identifier)
        {
            // Create the object
            var sourceProduct = ProductFactory.CreateMinimumProduct(
                culture: culture,
                dataSourceName: this.dataSources.GetHighestTrustScore(culture).Name);

            // Call GPC
            var response = this.apiClient.CreateProduct(sourceProduct).Result;

            // Make sure it was successful.
            response.EnsureSuccessStatusCode();

            // Get the product and store it in scenario storage
            var product = JsonConvert.DeserializeObject<Product>(response.Content.ReadAsStringAsync().Result);

            // Store the product in scenario storage
            ScenarioStorage.Products = new List<Product> { product };
        }

        /// <summary>
        /// Prepares a new product with an EAN that does not exist but does not
        /// create the product in GPC.
        /// </summary>
        /// <param name="culture">The culture of the product</param>
        [Given(@"a new product with a culture of (.*) and an EAN that does not exist has been prepared but not created")]
        public void GivenANewProductWithASpecifiedCultureAndAnEANThatDoesNotExistHasBeenPreparedButNotCreated(string culture)
        {
            // Find an EAN that does not exist in GPC
            var ean = GtinFactory.CreateEan();
            bool exists = true;

            while (exists)
            {
                var result = this.apiClient.GetProductAsync(ean, culture).Result;
                var content = JsonConvert.DeserializeObject<IEnumerable<Product>>(result.Content.ReadAsStringAsync().Result);

                exists = content.Count() > 0;
            }

            // Create the object
            var product = ProductFactory.CreateMinimumProduct(
                culture: culture,
                identifierValue: ean);

            // Store the product in scenario storage
            ScenarioStorage.Products = new List<Product> { product };
        }

        /// <summary>
        /// Creates a new product with a valid EAN.
        /// </summary>
        /// <param name="culture">The culture for the product</param>
        [Given(@"a new product with a culture of (.*) has been created in GPC")]
        public void GivenANewProductWithASpecifiedCultureHasBeenCreatedInGPC(string culture)
        {
            // Create the object
            var sourceProduct = ProductFactory.CreateMinimumProduct(
                culture: culture,
                dataSourceName: this.dataSources.GetHighestTrustScore(culture).Name);

            // Call GPC
            var response = this.apiClient.CreateProduct(sourceProduct).Result;

            // Make sure it was successful.
            response.EnsureSuccessStatusCode();

            // Get the product and store it in scenario storage
            var product = JsonConvert.DeserializeObject<Product>(response.Content.ReadAsStringAsync().Result);

            // Store the product in scenario storage
            ScenarioStorage.Products = new List<Product> { product };
        }

        /// <summary>
        /// Creates another new product with the same EAN and data source as the one
        /// in scenario storage.  This new product replaces the original in scenario storage.
        /// </summary>
        [Given(@"another new product with the same EAN and data source but a more recent updated date has been created")]
        public void GivenAnotherNewProductWithTheSameEANAndDataSourceButAMoreRecentUpdatedDateHasBeenCreated()
        {
            // TODO: We're talking about single products here but scenario storage only has a list of
            //       products therefore we have a conflict of understanding with regards to what has
            //       been set up in previous steps.

            // Get the product already created
            var sourceProduct = ScenarioStorage.Products.Single();

            // Create a new product with the same EAN but a lower data source trust score
            var newProduct = ProductFactory.CreateMinimumProduct(
                culture: sourceProduct.Culture,
                identifierValue: sourceProduct.GetEAN(),
                dataSourceName: sourceProduct.DataSource);

            // Call the API
            var response = this.apiClient.CreateProduct(newProduct).Result;

            response.EnsureSuccessStatusCode();

            // Replace the original product in scenario storage with the new one
            var content = JsonConvert.DeserializeObject<Product>(response.Content.ReadAsStringAsync().Result);
            ScenarioStorage.Products.Remove(sourceProduct);
            ScenarioStorage.Products.Add(content);
        }

        /// <summary>
        /// Creates another new product with the same EAN as that one in scenario storage
        /// but with a lower data source trust score.
        /// </summary>
        [Given(@"another new product with the same EAN but a lower data source trust score has been created")]
        public void GivenAnotherNewProductWithTheSameEANButALowerDataSourceTrustScoreHasBeenCreated()
        {
            // TODO: We're talking about single products here but scenario storage only has a list of
            //       products therefore we have a conflict of understanding with regards to what has
            //       been set up in previous steps.

            // Get the product already created
            var sourceProduct = ScenarioStorage.Products.Single();

            // Create a new product with the same EAN but a lower data source trust score
            var newProduct = ProductFactory.CreateMinimumProduct(
                culture: sourceProduct.Culture,
                identifierValue: sourceProduct.GetEAN(),
                dataSourceName: this.dataSources.GetLowestTrustScore(sourceProduct.Culture).Name);

            // Call the API
            var response = this.apiClient.CreateProduct(newProduct).Result;

            response.EnsureSuccessStatusCode();
        }

        /// <summary>
        /// Creates another new product with the same EAN, data source and
        /// update date as that one in scenario storage
        /// </summary>
        [Given(@"another new product with the same EAN, data source and updated date has been created")]
        public void GivenAnotherNewProductWithTheSameEANDataSourceAndUpdatedDateHasBeenCreated()
        {
            // TODO: We're talking about single products here but scenario storage only has a list of
            //       products therefore we have a conflict of understanding with regards to what has
            //       been set up in previous steps.

            // Get the product already created
            var sourceProduct = ScenarioStorage.Products.Single();

            // Create a new product with the same EAN but a lower data source trust score
            var newProduct = ProductFactory.CreateMinimumProduct(
                culture: sourceProduct.Culture,
                identifierValue: sourceProduct.GetEAN(),
                dataSourceName: sourceProduct.DataSource);

            // Call the API to create the product
            var response = this.apiClient.CreateProduct(newProduct).Result;

            response.EnsureSuccessStatusCode();

            var content = JsonConvert.DeserializeObject<Product>(response.Content.ReadAsStringAsync().Result);

            // Update the updated date for the new product to the same as the 
            // first product
            content.TimestampUpdate = sourceProduct.TimestampUpdate;
            using (var databaseContext = new ProductQueryDbContext())
            {
                databaseContext.products
                    .Where(p => p.GRAN == content.Id)
                    .Single()
                    .updateTime = sourceProduct.TimestampUpdate.Value;

                databaseContext.SaveChanges();
            }

            // Replace the original product in scenario storage with the new one
            ScenarioStorage.Products.Remove(sourceProduct);
            ScenarioStorage.Products.Add(content);
        }

        /// <summary>
        /// Improves all products in scenario storage to a new product.
        /// </summary>
        [Given(@"the product is improved")]
        public void GivenTheProductIsImproved()
        {
            foreach (var sourceProduct in ScenarioStorage.Products)
            {
                // Create the product to improve to
                var newProduct = ProductFactory.CreateMinimumProduct(sourceProduct.Culture);
                var createResponse = this.apiClient.CreateProduct(newProduct).Result;
                var createdProduct = JsonConvert.DeserializeObject<Product>(createResponse.Content.ReadAsStringAsync().Result);

                // Improve the original product
                var improveResponse = this.apiClient.ImproveProduct(new ProductImprovement
                {
                    Id = sourceProduct.Id,
                    ImprovedId = createdProduct.Id,
                    ImprovementCount = 1
                }).Result;

                improveResponse.EnsureSuccessStatusCode();
            }
        }
    }
}

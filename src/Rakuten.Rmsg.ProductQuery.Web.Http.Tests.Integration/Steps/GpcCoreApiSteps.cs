//------------------------------------------------------------------------------
// <copyright file="GpcCoreApiSteps.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.Web.Http.Tests.Integration
{
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using Newtonsoft.Json;
    using TechTalk.SpecFlow;

    /// <summary>
    /// Steps pertaining to the GPC core API.
    /// </summary>
    [Binding]
    public class GpcCoreApiSteps
    {
        /// <summary>
        /// A client for communicating with the GPC core API.
        /// </summary>
        private readonly GpcApiClient apiClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="GpcCoreApiSteps"/> class
        /// </summary>
        /// <param name="apiClient">A client for communicating with the GPC core API.</param>
        public GpcCoreApiSteps(GpcApiClient apiClient)
        {
            Contract.Requires(apiClient != null);

            this.apiClient = apiClient;
        }

        /// <summary>
        /// Creates a new product with a valid EAN.
        /// </summary>
        /// <param name="culture">The culture for the product</param>
        [Given(@"a new product has been created in GPC for the culture (.*)")]
        public void GivenANewProductHasBeenCreatedInGPCForTheSpecifiedCulture(string culture)
        {
            // Create the object
            var sourceProduct = ProductFactory.CreateMinimumProduct(culture);

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
        /// Improves all products in scenario storage to a new product.
        /// </summary>
        [Given(@"the product is improved")]
        public void GivenTheProductIsImproved()
        {
            foreach (var sourceProduct in ScenarioStorage.Products)
            {
                // Create the product to improve to
                var response = this.apiClient.CreateProduct(ProductFactory.CreateMinimumProduct(sourceProduct.Culture)).Result;
                var newProduct = JsonConvert.DeserializeObject<Product>(response.Content.ReadAsStringAsync().Result);

                // Improve the original product
                var result = this.apiClient.ImproveProduct(new ProductImprovement
                {
                    Id = sourceProduct.Id,
                    ImprovedId = newProduct.Id,
                    ImprovementCount = 1
                }).Result;

                result.EnsureSuccessStatusCode();
            }
        }
    }
}

//------------------------------------------------------------------------------
// <copyright file="GpcCoreApiSteps.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.Web.Http.Tests.Integration
{
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
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
        public void GivenANewProductWithAnEANHasBeenCreatedInGPCForTheSpecifiedCulture(string culture)
        {
            // Create the object
            var product = ProductFactory.CreateMinimumProduct(culture);

            // Call GPC
            var result = this.apiClient.CreateProduct(product).Result;

            // Make sure it was successful.
            result.EnsureSuccessStatusCode();

            // Store the product in scenario storage
            ScenarioStorage.Products = new List<Product> { product };
        }
    }
}

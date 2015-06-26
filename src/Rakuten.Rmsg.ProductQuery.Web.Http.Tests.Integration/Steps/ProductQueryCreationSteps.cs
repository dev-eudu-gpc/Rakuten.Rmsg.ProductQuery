//------------------------------------------------------------------------------
// <copyright file="ProductQueryCreationSteps.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.Web.Http.Tests.Integration.Steps
{
    using System.Diagnostics.Contracts;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Newtonsoft.Json;
    using Rakuten.Rmsg.ProductQuery.Web.Http.Tests.Integration.Resources;
    using TechTalk.SpecFlow;

    /// <summary>
    /// Steps pertaining to creation of a product query.
    /// </summary>
    [Binding]
    public class ProductQueryCreationSteps
    {
        /// <summary>
        /// An object for storing and retrieving information from the scenario context.
        /// </summary>
        private readonly ScenarioStorage scenarioStorage;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductQueryCreationSteps"/> class
        /// </summary>
        /// <param name="scenarioStorage">An object for sharing information between steps.</param>
        public ProductQueryCreationSteps(ScenarioStorage scenarioStorage)
        {
            Contract.Requires(scenarioStorage != null);

            this.scenarioStorage = scenarioStorage;
        }

        /// <summary>
        /// Prepares a new product query request that has an invalid culture 
        /// and stores it in the scenario context.
        /// </summary>
        [Given(@"a new product query with an invalid culture has been prepared")]
        public void GivenANewProductQueryWithAnInvalidCultureHasBeenPrepared()
        {
            this.scenarioStorage.Creation.SourceProductQuery = ProductQueryFactory.Create(culture: "I am not a culture");
        }

        /// <summary>
        /// Prepares a new product query request that has an invalid GUID 
        /// for the identifier and stores it in the scenario context.
        /// </summary>
        [Given(@"a new product query with an invalid guid has been prepared")]
        public void GivenANewProductQueryWithAnInvalidGuidHasBeenPrepared()
        {
            this.scenarioStorage.Creation.SourceProductQuery = ProductQueryFactory.Create(id: "I am not a guid");
        }

        /// <summary>
        /// Prepares a valid new product query object for use in a scenario
        /// and stores it in the scenario context.
        /// </summary>
        /// <param name="culture">The culture for the product query.</param>
        [Given(@"a valid new product query has been prepared for the culture (.*)")]
        public void GivenAValidNewProductQueryHasBeenPreparedForTheSpecifiedCulture(string culture)
        {
            this.scenarioStorage.Creation.SourceProductQuery = ProductQueryFactory.Create(culture: culture);
        }

        /// <summary>
        /// Updates the culture of the stored new product query to a specified value.
        /// </summary>
        /// <param name="newCulture">The new culture for the stored new product query.</param>
        [Given(@"the culture of the new product query is updated to (.*)")]
        public void GivenTheCultureOfTheNewProductQueryIsUpdatedTo(string newCulture)
        {
            this.scenarioStorage.Creation.SourceProductQuery.Culture = newCulture;
        }

        /// <summary>
        /// Verifies that the product query in the response body
        /// has the specified status.
        /// </summary>
        /// <param name="expectedStatus">The expected status of the product query.</param>
        [Then(@"the product query in the response body has a status of (.*)")]
        public void ThenTheProductQueryInTheResponseBodyHasAStatusOf(string expectedStatus)
        {
            // Arrange
            var content = this.scenarioStorage.LastResponse.Content.ReadAsStringAsync().Result;
            var productQuery = JsonConvert.DeserializeObject<ProductQuery>(content);

            // Assert
            Assert.AreEqual(expectedStatus, productQuery.Status, true);
        }

        /// <summary>
        /// Verifies that the product query in the response body
        /// has the correct self link.
        /// </summary>
        [Then(@"the product query in the response body has the correct self link")]
        public void ThenTheProductQueryInTheResponseBodyHasTheCorrectSelfLink()
        {
            // Arrange
            var productQuery = this.scenarioStorage.Creation.ResponseProductQuery;
            var responseMessage = this.scenarioStorage.Creation.ResponseMessage;

            // Assert
            Assert.IsNotNull(productQuery.Links.Self);
            Assert.AreEqual(responseMessage.RequestMessage.RequestUri.PathAndQuery, productQuery.Links.Self.Href, true);
        }
    }
}

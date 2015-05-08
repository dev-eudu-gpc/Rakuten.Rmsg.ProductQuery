//------------------------------------------------------------------------------
// <copyright file="NewProductQuerySteps.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.Web.Http.Tests.Integration
{
    using System.Net.Http;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Newtonsoft.Json;
    using TechTalk.SpecFlow;

    /// <summary>
    /// Steps pertaining to the product query resource
    /// </summary>
    [Binding]
    public class NewProductQuerySteps
    {
        /// <summary>
        /// Prepares a new product query request that has an invalid culture 
        /// and stores it in the scenario context.
        /// </summary>
        [Given(@"a new product query with an invalid culture has been prepared")]
        public void GivenANewProductQueryWithAnInvalidCultureHasBeenPrepared()
        {
            ScenarioStorage.NewProductQuery = ProductQueryFactory.Create(culture: "No, I'M Sparticus!");
        }

        /// <summary>
        /// Prepares a new product query request that has an invalid GUID 
        /// for the identifier and stores it in the scenario context.
        /// </summary>
        [Given(@"a new product query with an invalid guid has been prepared")]
        public void GivenANewProductQueryWithAnInvalidGuidHasBeenPrepared()
        {
            ScenarioStorage.NewProductQuery = ProductQueryFactory.Create(id: "I'm Sparticus!");
        }

        /// <summary>
        /// Prepares a valid new product query object for use in a scenario
        /// and stores it in the scenario context.
        /// </summary>
        [Given(@"a valid new product query has been prepared")]
        public void GivenAValidNewProductQueryHasBeenPrepared()
        {
            ScenarioStorage.NewProductQuery = ProductQueryFactory.Create();
        }

        /// <summary>
        /// Prepares a valid new product query object for use in a scenario
        /// and stores it in the scenario context.
        /// </summary>
        /// <param name="culture">The culture for the new product query.</param>
        [Given(@"a valid new product query with a culture of (.*) has been prepared")]
        public void GivenAValidNewProductQueryWithASpecifiedCultureHasBeenPrepared(string culture)
        {
            ScenarioStorage.NewProductQuery = ProductQueryFactory.Create(culture: culture);
        }

        /// <summary>
        /// Updates the culture of the stored new product query to a specified value.
        /// </summary>
        /// <param name="newCulture">The new culture for the stored new product query.</param>
        [Given(@"the culture of the new product query is updated to (.*)")]
        public void GivenTheCultureOfTheNewProductQueryIsUpdatedTo(string newCulture)
        {
            ScenarioStorage.NewProductQuery.Culture = newCulture;
        }

        /// <summary>
        /// Verifies that the product query in the response body
        /// has the same enclosure link as that found in the database.
        /// </summary>
        [Then(@"the product query has the correct enclosure link")]
        public void ThenTheProductQueryHasTheCorrectEnclosureLink()
        {
            // Arrange
            var content = ScenarioStorage.HttpResponseMessage.Content.ReadAsStringAsync().Result;
            var productQuery = JsonConvert.DeserializeObject<ProductQuery>(content);

            // Assert
            Assert.AreEqual(
                ScenarioStorage.ProductQueryFromDatabase.uri,
                productQuery.Links.Enclosure.Href,
                true);
        }

        /// <summary>
        /// Verifies that the product query in the response body
        /// has the correct self link.
        /// </summary>
        [Then(@"the product query has the correct self link")]
        public void ThenTheProductQueryHasTheCorrectSelfLink()
        {
            // Arrange
            var content = ScenarioStorage.HttpResponseMessage.Content.ReadAsStringAsync().Result;
            var productQuery = JsonConvert.DeserializeObject<ProductQuery>(content);

            // Assert
            Assert.AreEqual(
                ScenarioStorage.HttpResponseMessage.RequestMessage.RequestUri.PathAndQuery,
                productQuery.Links.Self.Href);
        }

        /// <summary>
        /// Verifies that the product query in the response body
        /// has the specified status.
        /// </summary>
        /// <param name="expectedStatus">The expected status of the product query.</param>
        [Then(@"the product query status is (.*)")]
        public void ThenTheProductQueryStatusIs(string expectedStatus)
        {
            // Arrange
            var content = ScenarioStorage.HttpResponseMessage.Content.ReadAsAsync<ProductQuery>().Result;

            // Assert
            Assert.AreEqual(expectedStatus, content.Status, true);
        }
    }
}

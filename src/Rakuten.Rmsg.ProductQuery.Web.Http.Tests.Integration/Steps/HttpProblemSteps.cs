//------------------------------------------------------------------------------
// <copyright file="HttpProblemSteps.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.Web.Http.Tests.Integration
{
    using System.Linq;
    using System.Net.Http;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using TechTalk.SpecFlow;

    /// <summary>
    /// Steps for working with HTTP problem objects in response content.
    /// </summary>
    [Binding]
    public class HttpProblemSteps
    {
        /// <summary>
        /// Verifies that an HTTP problem can be de-serialized from the content
        /// of the response in the scenario context.  If so, the object
        /// is persisted to the scenario context.
        /// </summary>
        [Then(@"an HTTP problem can be retrieved from the response body")]
        public void ThenAnHTTPProblemCanBeRetrievedFromTheResponseBody()
        {
            // Arrange
            var content = ScenarioStorage.HttpResponseMessage.Content.ReadAsAsync<HttpProblem>().Result;

            // Store the problem object in scenario context for subsequent steps
            ScenarioStorage.HttpProblem = content;

            // Assert
            Assert.IsNotNull(content);
        }

        /// <summary>
        /// Verifies that the detail property of the HTTP problem found in the
        /// response content is the expected value.
        /// </summary>
        /// <param name="detailTemplate">The expected value for the detail property.</param>
        [Then(@"the HTTP problem detail is (.*)")]
        public void ThenTheHttpProblemDetailIs(string detailTemplate)
        {
            // Arrange
            var expectedDetail = detailTemplate
                .Replace("{id}", ScenarioStorage.NewProductQuery.Id)
                .Replace("{culture}", ScenarioStorage.NewProductQuery.Culture)
                .Replace("{status}", ScenarioStorage.NewProductQuery.Status);

            // Assert
            Assert.AreEqual(expectedDetail, ScenarioStorage.HttpProblem.Detail, true);
        }

        /// <summary>
        /// Verifies that the HTTP problem in the scenario context is of
        /// the specified type.
        /// </summary>
        /// <param name="expectedType">The expected type.</param>
        [Then(@"the HTTP problem is of type (.*)")]
        public void ThenTheHTTPProblemIsOfType(string expectedType)
        {
            Assert.AreEqual(expectedType, ScenarioStorage.HttpProblem.Type, true);
        }

        /// <summary>
        /// Verifies that the HTTP problem in scenario storage has the specified title.
        /// </summary>
        /// <param name="expectedTitle">The expected title.</param>
        [Then(@"the HTTP problem title is (.*)")]
        public void ThenTheHTTPProblemTitleIs(string expectedTitle)
        {
            Assert.AreEqual(expectedTitle, ScenarioStorage.HttpProblem.Title, true);
        }

        /// <summary>
        /// Verifies that the HTTP problem in scenario storage has a link
        /// of the specified type.
        /// </summary>
        /// <param name="expectedLinkType">The link type to assert.</param>
        [Then(@"the HTTP problem contains a link of relation type (.*)")]
        public void ThenTheHTTPProblemContainsALinkOfType(string expectedLinkType)
        {
            // TODO: Http Problem serialization from the API needs to include relation type for Links. 
            Assert.IsTrue(ScenarioStorage.HttpProblem.Links.Contains(new Link { RelationType = expectedLinkType }));
        }

        /// <summary>
        /// Verifies that the HREF property of the specified link type has the
        /// same link as the self link of the product query.
        /// </summary>
        /// <param name="linkRelationType">The relation type of the link to verify.</param>
        [Then(@"the HTTP problem link of relation type (.*) has an href that points to the product query")]
        public void ThenTheHTTPProblemLinkTypeHasAnHrefThatPointsToTheProductQuery(string linkRelationType)
        {
            // Try and get the specified link
            var link = ScenarioStorage.HttpProblem.Links.FirstOrDefault(l => l.RelationType == linkRelationType);

            // Assert
            Assert.IsNotNull(link);
            Assert.AreEqual(link.Target, ScenarioStorage.NewProductQuery.Links.Self.Href, true);
        }
    }
}
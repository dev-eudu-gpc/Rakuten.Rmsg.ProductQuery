//------------------------------------------------------------------------------
// <copyright file="HttpProblemSteps.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.Web.Http.Tests.Integration
{
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
        /// Verifies that the detail property of the HTTP problem found in the
        /// response content is the expected value.
        /// </summary>
        /// <param name="detailTemplate">The expected value for the detail property.</param>
        [Then(@"the HTTP problem detail is (.*)")]
        public void TheHttpProblemDetailIs(string detailTemplate)
        {
            // Arrange
            var expectedDetail = detailTemplate
                .Replace("{id}", ScenarioStorage.NewProductQuery.Id)
                .Replace("{culture}", ScenarioStorage.NewProductQuery.Culture);

            // Assert
            Assert.AreEqual(expectedDetail, ScenarioStorage.HttpProblem.Detail, true);
        }

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
        /// Verifies that the HTTP problem in the scenario context 
        /// has the specified title.
        /// </summary>
        /// <param name="expectedTitle">The expected title.</param>
        [Then(@"the HTTP problem title is (.*)")]
        public void ThenTheHTTPProblemTitleIs(string expectedTitle)
        {
            Assert.AreEqual(expectedTitle, ScenarioStorage.HttpProblem.Title, true);
        }
    }
}
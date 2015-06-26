//------------------------------------------------------------------------------
// <copyright file="HttpProblemSteps.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.Web.Http.Tests.Integration.Steps
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using TechTalk.SpecFlow;

    /// <summary>
    /// Steps for working with HTTP problem objects in response content.
    /// </summary>
    [Binding]
    public class HttpProblemSteps
    {
        /// <summary>
        /// An object for storing and retrieving information from the scenario context.
        /// </summary>
        private readonly ScenarioStorage scenarioStorage;

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpProblemSteps"/> class
        /// </summary>
        /// <param name="scenarioStorage">An object for sharing information between steps.</param>
        public HttpProblemSteps(ScenarioStorage scenarioStorage)
        {
            Contract.Requires(scenarioStorage != null);

            this.scenarioStorage = scenarioStorage;
        }

        /// <summary>
        /// Verifies that the HTTP problem in scenario storage has a link
        /// of the specified type.
        /// </summary>
        /// <param name="expectedLinkType">The link type to assert.</param>
        [Then(@"the HTTP problem contains a link of relation type (.*)")]
        public void ThenTheHTTPProblemContainsALinkOfType(string expectedLinkType)
        {
            // Arrange
            var link = this.scenarioStorage.LastResponseHttpProblem.Links.FirstOrDefault(
                l => l.RelationType.Equals(expectedLinkType, StringComparison.InvariantCultureIgnoreCase));

            // Assert
            Assert.IsNotNull(link, "Could not find a link of relation type " + expectedLinkType);
        }

        /// <summary>
        /// Verifies that the detail property of the HTTP problem found in the
        /// response content is the expected value when the request was a product
        /// query monitor request.
        /// </summary>
        /// <param name="detailTemplate">The expected value for the detail property.</param>
        [Then(@"the HTTP problem detail for the product query monitor request is (.*)")]
        public void ThenTheHttpProblemDetailForTheProductQueryMonitorRequestIs(string detailTemplate)
        {
            // Arrange
            var request = this.scenarioStorage.Monitor.SourceRequest;

            var expectedDetail = detailTemplate
                .Replace("{datetime}", string.Format("{0}/{1}/{2}/{3}/{4}", request.Year, request.Month, request.Day, request.Hour, request.Minute))
                .Replace("{id}", request.Id);

            // Assert
            Assert.AreEqual(expectedDetail, this.scenarioStorage.Monitor.ResponseHttpProblem.Detail, true);
        }

        /// <summary>
        /// Verifies that the detail property of the HTTP problem found in the
        /// response content is the expected value when the request was a 
        /// product query related request.
        /// </summary>
        /// <param name="detailTemplate">The expected value for the detail property.</param>
        [Then(@"the HTTP problem detail for the product query request is (.*)")]
        public void ThenTheHttpProblemDetailForTheProductQueryRequestIs(string detailTemplate)
        {
            // Arrange
            var productQuery = this.scenarioStorage.Creation.SourceProductQuery;

            var expectedDetail = detailTemplate
                .Replace("{id}", productQuery.Id)
                .Replace("{culture}", productQuery.Culture)
                .Replace("{status}", productQuery.Status);

            // Assert
            Assert.AreEqual(
                expectedDetail,
                this.scenarioStorage.Creation.ResponseHttpProblem.Detail,
                true);
        }

        /// <summary>
        /// Verifies that the detail property of the HTTP problem found in the
        /// response content of a ready for processing request is the expected 
        /// value when the request was a product query related request.
        /// </summary>
        /// <param name="detailTemplate">The expected value for the detail property.</param>
        [Then(@"the HTTP problem detail for the ready for processing request is (.*)")]
        public void ThenTheHTTPProblemDetailForTheReadyForProcessingRequestIs(string detailTemplate)
        {
            // Arrange
            var productQuery = this.scenarioStorage.Creation.SourceProductQuery;

            var expectedDetail = detailTemplate
                .Replace("{id}", productQuery.Id)
                .Replace("{culture}", productQuery.Culture)
                .Replace("{status}", productQuery.Status);

            // Assert
            Assert.AreEqual(
                expectedDetail,
                this.scenarioStorage.ReadyForProcessing.ResponseHttpProblem.Detail,
                true);
        }

        /// <summary>
        /// Verifies that the HTTP problem in the scenario context is of
        /// the specified type.
        /// </summary>
        /// <param name="expectedType">The expected type.</param>
        [Then(@"the HTTP problem is of type (.*)")]
        public void ThenTheHTTPProblemIsOfType(string expectedType)
        {
            Assert.AreEqual(
                expectedType,
                this.scenarioStorage.LastResponseHttpProblem.Type,
                true);
        }

        /// <summary>
        /// Verifies that the HREF property of the specified link type has the
        /// same link as the self link of the product query.
        /// </summary>
        /// <param name="linkRelationType">The relation type of the link to verify.</param>
        [Then(@"the HTTP problem link of relation type (.*) has an href that points to the product query")]
        public void ThenTheHTTPProblemLinkTypeHasAnHrefThatPointsToTheProductQuery(string linkRelationType)
        {
            // Arrange
            var link = this.scenarioStorage.LastResponseHttpProblem.Links
                .FirstOrDefault(l => l.RelationType == linkRelationType);

            // Assert
            Assert.IsNotNull(link);
            Assert.AreEqual(
                this.scenarioStorage.Creation.SourceProductQuery.Links.Self.Href,
                link.Target,
                true);
        }

        /// <summary>
        /// Verifies that the HTTP problem in scenario storage has the specified title.
        /// </summary>
        /// <param name="expectedTitle">The expected title.</param>
        [Then(@"the HTTP problem title is (.*)")]
        public void ThenTheHTTPProblemTitleIs(string expectedTitle)
        {
            Assert.AreEqual(
                expectedTitle,
                this.scenarioStorage.LastResponseHttpProblem.Title,
                true);
        }
    }
}
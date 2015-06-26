//------------------------------------------------------------------------------
// <copyright file="HttpResponseSteps.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.Web.Http.Tests.Integration.Steps
{
    using System;
    using System.Diagnostics.Contracts;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Windows.Media.Imaging;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Rakuten.Rmsg.ProductQuery.Configuration;
    using TechTalk.SpecFlow;

    /// <summary>
    /// Steps for working with the status code of an <see cref="HttpResponseMessage"/>
    /// </summary>
    [Binding]
    public class HttpResponseSteps
    {
        /// <summary>
        /// A context for the product query API.
        /// </summary>
        private readonly IApiContext apiContext;

        /// <summary>
        /// An object for storing and retrieving information from the scenario context.
        /// </summary>
        private readonly ScenarioStorage scenarioStorage;

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpResponseSteps"/> class
        /// </summary>
        /// <param name="apiContext">A context for the product query API.</param>
        /// <param name="scenarioStorage">An object for sharing information between steps.</param>
        public HttpResponseSteps(
            IApiContext apiContext,
            ScenarioStorage scenarioStorage)
        {
            Contract.Requires(apiContext != null);
            Contract.Requires(scenarioStorage != null);

            this.apiContext = apiContext;
            this.scenarioStorage = scenarioStorage;
        }

        /// <summary>
        /// Verifies that the HTTP content type header is the specified value.
        /// </summary>
        /// <param name="expectedValue">The expected value of the content type header.</param>
        [Then(@"the HTTP content type is (.*)")]
        public void ThenTheHTTPContentTypeIs(string expectedValue)
        {
            Assert.AreEqual(
                expectedValue,
                this.scenarioStorage.LastResponse.Content.Headers.ContentType.MediaType,
                true);
        }

        /// <summary>
        /// Verifies that the location header of the HTTP response in scenario storage
        /// is a product query group status URI for the current date time
        /// </summary>
        [Then(@"the HTTP location header for the product query monitor request is the current date/time")]
        public void ThenTheHTTPLocationHeaderForTheProductQueryMonitorRequestIsTheCurrentDateTime()
        {
            // TODO: [WB 12-May-2015] Potentially volatile test as we can't guarantee that UtcNow is 
            // the same at this point as what it was when the server generated the response message.

            // Get the group of the product query
            using (var database = new ProductQueryDbContext())
            {
                // Get the group
                var group = database.rmsgProductQueries
                    .Single(q => q.rmsgProductQueryID == this.scenarioStorage.Creation.SourceProductQuery.IdAsGuid)
                    .rmsgProductQueryGroup;

                // Arrange
                var expectedDateTime = DateTime.UtcNow;
                var expectedLocationHeader = string.Format(
                    "/product-query-group/{0}/status/{1}/{2}/{3}/{4}/{5}",
                    group.rmsgProductQueryGroupID,
                    expectedDateTime.Year.ToString("00"),
                    expectedDateTime.Month.ToString("00"),
                    expectedDateTime.Day.ToString("00"),
                    expectedDateTime.Hour.ToString("00"),
                    expectedDateTime.Minute.ToString("00"));

                // Assert
                Assert.AreEqual(
                    expectedLocationHeader,
                    this.scenarioStorage.Monitor.ResponseMessage.Headers.Location.ToString(),
                    true);
            }
        }

        /// <summary>
        /// Verifies that the location header of the HTTP response in scenario storage
        /// is as specified.
        /// </summary>
        /// <param name="expectedLocationHeaderTemplate">The template to match against</param>
        [Then(@"the HTTP location header is (.*)")]
        public void ThenTheHTTPLocationHeaderIs(string expectedLocationHeaderTemplate)
        {
            // Arrange
            var expectedLocationHeader = expectedLocationHeaderTemplate
                .Replace("{id}", this.scenarioStorage.Creation.SourceProductQuery.Id);

            // Assert
            Assert.AreEqual(
                expectedLocationHeader,
                this.scenarioStorage.Creation.ResponseMessage.Headers.Location.ToString(),
                true);
        }

        /// <summary>
        /// Verifies that the retry-after header of the response in scenario storage
        /// has the same value as the config entry that defines the progress map interval.
        /// </summary>
        [Then(@"the HTTP retry-after header has the same value as the progress map interval in seconds")]
        public void ThenTheHTTPRetry_AfterHeaderHasTheSameValueAsTheProgressMapIntervalInSeconds()
        {
            Assert.AreEqual(
                this.apiContext.ProgressMapIntervalInSeconds,
                this.scenarioStorage.Monitor.ResponseMessage.Headers.RetryAfter.Delta.Value.TotalSeconds);
        }

        /// <summary>
        /// Verifies that the status code of the response in scenario storage
        /// is the specified value.
        /// </summary>
        /// <param name="expectedStatusCode">The expected status code</param>
        [Then(@"the HTTP status code is (.*)")]
        public void ThenTheHttpStatusCodeIs(int expectedStatusCode)
        {
            Assert.AreEqual(
                (HttpStatusCode)expectedStatusCode,
                this.scenarioStorage.LastResponse.StatusCode);
        }

        /// <summary>
        /// Verifies that the content of the response in scenario storage is an image.
        /// </summary>
        [Then(@"the response body contains an image")]
        public void ThenTheResponseBodyContainsAnImage()
        {
            Exception caughtException = null;
            BitmapImage bitmap = null;

            try
            {
                Stream contentStream = this.scenarioStorage.LastResponse.Content.ReadAsStreamAsync().Result;
                bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.StreamSource = contentStream;
                bitmap.EndInit();
            }
            catch (NotSupportedException ex)
            {
                caughtException = ex;
            }

            Assert.IsNull(caughtException, "Unable to create a BitMap image from the response content stream.");
            Assert.IsNotNull(bitmap, "The bitmap created from the response content stream is null.");
        }
    }
}
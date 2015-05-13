//------------------------------------------------------------------------------
// <copyright file="HttpResponseSteps.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.Web.Http.Tests.Integration
{
    using System;
    using System.Diagnostics.Contracts;
    using System.IO;
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
        /// Initializes a new instance of the <see cref="HttpResponseSteps"/> class
        /// </summary>
        /// <param name="apiContext">A context for the product query API.</param>
        public HttpResponseSteps(IApiContext apiContext)
        {
            Contract.Requires(apiContext != null);

            this.apiContext = apiContext;
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
            var expectedLocationHeader = expectedLocationHeaderTemplate.Replace("{id}", ScenarioStorage.NewProductQuery.Id);

            // Assert
            Assert.AreEqual(
                expectedLocationHeader,
                ScenarioStorage.HttpResponseMessage.Headers.Location.ToString(),
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

            // Arrange
            var requestParameters = ScenarioStorage.ProductQueryMonitorRequest;
            var expectedDateTime = DateTime.UtcNow;
            var expectedLocationHeader = string.Format(
                "/product-query-group/{0}/status/{1}/{2}/{3}/{4}/{5}",
                requestParameters.Id,
                expectedDateTime.Year.ToString("00"),
                expectedDateTime.Month.ToString("00"),
                expectedDateTime.Day.ToString("00"),
                expectedDateTime.Hour.ToString("00"),
                expectedDateTime.Minute.ToString("00"));

            // Assert
            Assert.AreEqual(
                expectedLocationHeader,
                ScenarioStorage.HttpResponseMessage.Headers.Location.ToString(),
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
                ScenarioStorage.HttpResponseMessage.Headers.RetryAfter.Delta.Value.TotalSeconds);
        }

        /// <summary>
        /// Verifies that the status code of the response in scenario storage
        /// is the specified value.
        /// </summary>
        /// <param name="expectedStatusCode">The expected status code</param>
        [Then(@"the HTTP status code is (.*)")]
        public void ThenTheHttpStatusCodeIs(int expectedStatusCode)
        {
            // Assert
            Assert.AreEqual(
                (HttpStatusCode)expectedStatusCode,
                ScenarioStorage.HttpResponseMessage.StatusCode);
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
                ScenarioStorage.HttpResponseMessage.Content.Headers.ContentType.MediaType,
                true);
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
                Stream contentStream = ScenarioStorage.HttpResponseMessage.Content.ReadAsStreamAsync().Result;
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
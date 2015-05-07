//------------------------------------------------------------------------------
// <copyright file="HttpResponseSteps.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.Web.Http.Tests.Integration
{
    using System.Net;
    using System.Net.Http;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using TechTalk.SpecFlow;

    /// <summary>
    /// Steps for working with the status code of an <see cref="HttpResponseMessage"/>
    /// </summary>
    [Binding]
    public class HttpResponseSteps
    {
        /// <summary>
        /// Verifies that the location header of the HTTP response is as specified.
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
        /// Verifies that the status code of the response in the current scenario context is 400.
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
    }
}
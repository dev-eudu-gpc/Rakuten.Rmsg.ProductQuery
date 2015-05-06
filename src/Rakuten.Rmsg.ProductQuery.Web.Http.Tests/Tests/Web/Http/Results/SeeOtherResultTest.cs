//------------------------------------------------------------------------------
// <copyright file="SeeOtherResultTest.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.Web.Http.Tests
{
    using System;
    using System.Net;
    using System.Net.Http.Fakes;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Web.Http.Results;
    using Microsoft.QualityTools.Testing.Fakes;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Defines tests for the <see cref="SeeOtherResult"/> class.
    /// </summary>
    [TestClass]
    public class SeeOtherResultTest
    {
        /// <summary>
        /// Verifies that the status code of the result is correct.
        /// </summary>
        [TestMethod]
        public void SeeOtherResultSetsCorrectHttpStatusCode()
        {
            using (var context = ShimsContext.Create())
            {
                // Arrange
                ShimHttpRequestMessageExtensions.CreateResponseHttpRequestMessageHttpStatusCode = 
                    (request, statusCode) => new StubHttpResponseMessage(statusCode);

                var seeOtherResult = new SeeOtherResult(new Uri("http://somewhere.com"), new StubHttpRequestMessage());

                // Act
                var result = seeOtherResult.Execute();

                // Assert
                Assert.IsNotNull(result);
                Assert.AreEqual(HttpStatusCode.SeeOther, result.StatusCode);
            }
        }

        /// <summary>
        /// Verifies that the status code of the result is correct when
        /// executing asynchronously
        /// </summary>
        /// <returns>A task that performs the test.</returns>
        [TestMethod]
        public async Task SeeOtherResultSetsCorrectHttpStatusCodeAsync()
        {
            using (var context = ShimsContext.Create())
            {
                // Arrange
                ShimHttpRequestMessageExtensions.CreateResponseHttpRequestMessageHttpStatusCode =
                    (request, statusCode) => new StubHttpResponseMessage(statusCode);

                var seeOtherResult = new SeeOtherResult(new Uri("http://somewhere.com"), new StubHttpRequestMessage());

                // Act
                var result = await seeOtherResult.ExecuteAsync(new CancellationToken());

                // Assert
                Assert.IsNotNull(result);
                Assert.AreEqual(HttpStatusCode.SeeOther, result.StatusCode);
            }
        }

        /// <summary>
        /// Verifies that the default value of the retry header of the result 
        /// is correct when no value is provided.
        /// </summary>
        [TestMethod]
        public void SeeOtherResultSetsCorrectRetryAfterHeaderDefault()
        {
            using (var context = ShimsContext.Create())
            {
                // Arrange
                ShimHttpRequestMessageExtensions.CreateResponseHttpRequestMessageHttpStatusCode =
                    (request, statusCode) => new StubHttpResponseMessage(statusCode);

                var seeOtherResult = new SeeOtherResult(
                    new Uri("http://somewhere.com"),
                    new StubHttpRequestMessage());

                // Act
                var result = seeOtherResult.Execute();

                // Assert
                Assert.IsNotNull(result);
                Assert.AreEqual(0, result.Headers.RetryAfter.Delta.Value.TotalSeconds);
            }
        }

        /// <summary>
        /// Verifies that the default value of the retry header of the result 
        /// is correct when no value is provided and executed asynchronously.
        /// </summary>
        /// <returns>A task that performs the test.</returns>
        [TestMethod]
        public async Task SeeOtherResultSetsCorrectRetryAfterHeaderDefaultAsync()
        {
            using (var context = ShimsContext.Create())
            {
                // Arrange
                ShimHttpRequestMessageExtensions.CreateResponseHttpRequestMessageHttpStatusCode =
                    (request, statusCode) => new StubHttpResponseMessage(statusCode);

                var seeOtherResult = new SeeOtherResult(
                    new Uri("http://somewhere.com"),
                    new StubHttpRequestMessage());

                // Act
                var result = await seeOtherResult.ExecuteAsync(new CancellationToken());

                // Assert
                Assert.IsNotNull(result);
                Assert.AreEqual(0, result.Headers.RetryAfter.Delta.Value.TotalSeconds);
            }
        }

        /// <summary>
        /// Verifies that the retry header of the result is correct when a value is provided.
        /// </summary>
        [TestMethod]
        public void SeeOtherResultSetsCorrectRetryAfterHeaderWhenValueProvided()
        {
            using (var context = ShimsContext.Create())
            {
                // Arrange
                int retryAfter = 60;

                ShimHttpRequestMessageExtensions.CreateResponseHttpRequestMessageHttpStatusCode =
                    (request, statusCode) => new StubHttpResponseMessage(statusCode);

                var seeOtherResult = new SeeOtherResult(
                    new Uri("http://somewhere.com"),
                    retryAfter,
                    new StubHttpRequestMessage());

                // Act
                var result = seeOtherResult.Execute();

                // Assert
                Assert.IsNotNull(result);
                Assert.AreEqual(retryAfter, result.Headers.RetryAfter.Delta.Value.TotalSeconds);
            }
        }

        /// <summary>
        /// Verifies that the retry header of the result is correct when a value is provided
        /// when executed asynchronously.
        /// </summary>
        /// <returns>A task that performs the test.</returns>
        [TestMethod]
        public async Task SeeOtherResultSetsCorrectRetryAfterHeaderWhenValueProvidedAsync()
        {
            using (var context = ShimsContext.Create())
            {
                // Arrange
                int retryAfter = 60;

                ShimHttpRequestMessageExtensions.CreateResponseHttpRequestMessageHttpStatusCode =
                    (request, statusCode) => new StubHttpResponseMessage(statusCode);

                var seeOtherResult = new SeeOtherResult(
                    new Uri("http://somewhere.com"),
                    retryAfter,
                    new StubHttpRequestMessage());

                // Act
                var result = await seeOtherResult.ExecuteAsync(new CancellationToken());

                // Assert
                Assert.IsNotNull(result);
                Assert.AreEqual(retryAfter, result.Headers.RetryAfter.Delta.Value.TotalSeconds);
            }
        }
    }
}
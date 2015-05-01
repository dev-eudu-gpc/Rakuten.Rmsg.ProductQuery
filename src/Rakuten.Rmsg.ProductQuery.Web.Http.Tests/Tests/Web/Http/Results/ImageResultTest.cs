//------------------------------------------------------------------------------
// <copyright file="ImageResultTest.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.Web.Http.Tests
{
    using System.IO;
    using System.Net;
    using System.Net.Http.Fakes;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Web.Http.Results;
    using Microsoft.QualityTools.Testing.Fakes;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Defines tests for the <see cref="ImageResult"/> class.
    /// </summary>
    [TestClass]
    public class ImageResultTest
    {
        /// <summary>
        /// Verifies that the status code of the result is correct.
        /// </summary>
        [TestMethod]
        public void ImageResultSetsCorrectHttpStatusCode()
        {
            using (var context = ShimsContext.Create())
            {
                // Arrange
                ShimHttpRequestMessageExtensions.CreateResponseHttpRequestMessageHttpStatusCode = 
                    (request, statusCode) => new StubHttpResponseMessage(statusCode);

                var stream = new MemoryStream();

                var imageResult = new ImageResult(stream, new StubHttpRequestMessage());

                // Act
                var result = imageResult.Execute();

                // Assert
                Assert.IsNotNull(result);
                Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            }
        }

        /// <summary>
        /// Verifies that the status code of the result is correct when
        /// executing asynchronously
        /// </summary>
        /// <returns>A task that performs the test.</returns>
        [TestMethod]
        public async Task ImageResultSetsCorrectHttpStatusCodeAsync()
        {
            using (var context = ShimsContext.Create())
            {
                // Arrange
                ShimHttpRequestMessageExtensions.CreateResponseHttpRequestMessageHttpStatusCode =
                    (request, statusCode) => new StubHttpResponseMessage(statusCode);

                var stream = new MemoryStream();

                var imageResult = new ImageResult(stream, new StubHttpRequestMessage());

                // Act
                var result = await imageResult.ExecuteAsync(new CancellationToken());

                // Assert
                Assert.IsNotNull(result);
                Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            }
        }

        /// <summary>
        /// Verifies that the default value of the cache control of the result 
        /// is correct when no value is provided.
        /// </summary>
        [TestMethod]
        public void ImageResultSetsCorrectCacheControlHeaderDefault()
        {
            using (var context = ShimsContext.Create())
            {
                // Arrange
                ShimHttpRequestMessageExtensions.CreateResponseHttpRequestMessageHttpStatusCode =
                    (request, statusCode) => new StubHttpResponseMessage(statusCode);

                var stream = new MemoryStream();

                var imageResult = new ImageResult(stream, new StubHttpRequestMessage());

                // Act
                var result = imageResult.Execute();

                // Assert
                Assert.IsNotNull(result);
                Assert.AreEqual(7, result.Headers.CacheControl.MaxAge.Value.TotalDays);
                Assert.AreEqual(true, result.Headers.CacheControl.Public);
            }
        }

        /// <summary>
        /// Verifies that the default value of the cache control header of the result 
        /// is correct when no value is provided and executed asynchronously.
        /// </summary>
        /// <returns>A task that performs the test.</returns>
        [TestMethod]
        public async Task ImageResultSetsCorrectCacheControlHeaderDefaultAsync()
        {
            using (var context = ShimsContext.Create())
            {
                // Arrange
                ShimHttpRequestMessageExtensions.CreateResponseHttpRequestMessageHttpStatusCode =
                    (request, statusCode) => new StubHttpResponseMessage(statusCode);

                var stream = new MemoryStream();

                var imageResult = new ImageResult(stream, new StubHttpRequestMessage());

                // Act
                var result = await imageResult.ExecuteAsync(new CancellationToken());

                // Assert
                Assert.IsNotNull(result);
                Assert.AreEqual(7, result.Headers.CacheControl.MaxAge.Value.TotalDays);
                Assert.AreEqual(true, result.Headers.CacheControl.Public);
            }
        }

        /// <summary>
        /// Verifies that the cache control header of the result is correct when a value is provided.
        /// </summary>
        [TestMethod]
        public void ImageResultSetsCorrectCacheControlHeaderWhenValueProvided()
        {
            using (var context = ShimsContext.Create())
            {
                // Arrange
                int cacheControlLifeSpanInDays = 3;

                ShimHttpRequestMessageExtensions.CreateResponseHttpRequestMessageHttpStatusCode =
                    (request, statusCode) => new StubHttpResponseMessage(statusCode);

                var stream = new MemoryStream();

                var imageResult = new ImageResult(stream, cacheControlLifeSpanInDays, new StubHttpRequestMessage());

                // Act
                var result = imageResult.Execute();

                // Assert
                Assert.IsNotNull(result);
                Assert.AreEqual(cacheControlLifeSpanInDays, result.Headers.CacheControl.MaxAge.Value.TotalDays);
                Assert.AreEqual(true, result.Headers.CacheControl.Public);
            }
        }

        /// <summary>
        /// Verifies that the retry header of the result is correct when a value is provided
        /// when executed asynchronously.
        /// </summary>
        /// <returns>A task that performs the test.</returns>
        [TestMethod]
        public async Task ImageResultSetsCorrectCacheControlHeaderWhenValueProvidedAsync()
        {
            using (var context = ShimsContext.Create())
            {
                // Arrange
                int cacheControlLifeSpanInDays = 3;

                ShimHttpRequestMessageExtensions.CreateResponseHttpRequestMessageHttpStatusCode =
                    (request, statusCode) => new StubHttpResponseMessage(statusCode);

                var stream = new MemoryStream();

                var imageResult = new ImageResult(stream, cacheControlLifeSpanInDays, new StubHttpRequestMessage());

                // Act
                var result = await imageResult.ExecuteAsync(new CancellationToken());

                // Assert
                Assert.IsNotNull(result);
                Assert.AreEqual(cacheControlLifeSpanInDays, result.Headers.CacheControl.MaxAge.Value.TotalDays);
                Assert.AreEqual(true, result.Headers.CacheControl.Public);
            }
        }

        /// <summary>
        /// Verifies that the content type header of the result is correct.
        /// </summary>
        [TestMethod]
        public void ImageResultSetsCorrectContentTypeHeader()
        {
            using (var context = ShimsContext.Create())
            {
                // Arrange
                ShimHttpRequestMessageExtensions.CreateResponseHttpRequestMessageHttpStatusCode =
                    (request, statusCode) => new StubHttpResponseMessage(statusCode);

                var stream = new MemoryStream();

                var imageResult = new ImageResult(stream, new StubHttpRequestMessage());

                // Act
                var result = imageResult.Execute();

                // Assert
                Assert.IsNotNull(result);
                Assert.AreEqual("image/png", result.Content.Headers.ContentType.MediaType, true);
            }
        }

        /// <summary>
        /// Verifies that the content type header of the result is correct.
        /// when executed asynchronously.
        /// </summary>
        /// <returns>A task that performs the test.</returns>
        [TestMethod]
        public async Task ImageResultSetsCorrectContentTypeHeaderAsync()
        {
            using (var context = ShimsContext.Create())
            {
                // Arrange
                ShimHttpRequestMessageExtensions.CreateResponseHttpRequestMessageHttpStatusCode =
                    (request, statusCode) => new StubHttpResponseMessage(statusCode);

                var stream = new MemoryStream();

                var imageResult = new ImageResult(stream, new StubHttpRequestMessage());

                // Act
                var result = await imageResult.ExecuteAsync(new CancellationToken());

                // Assert
                Assert.IsNotNull(result);
                Assert.AreEqual("image/png", result.Content.Headers.ContentType.MediaType, true);
            }
        }
    }
}
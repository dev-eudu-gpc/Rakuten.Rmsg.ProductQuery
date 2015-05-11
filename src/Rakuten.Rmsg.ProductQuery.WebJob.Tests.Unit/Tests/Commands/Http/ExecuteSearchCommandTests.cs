// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="ExecuteSearchCommandTests.cs" company="Rakuten">
//   Copyright (c) Rakuten. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.WebJob.Tests.Unit
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Rakuten.Fakes;
    using Rakuten.Gpc.Api.Client.Fakes;
    using Rakuten.Net.Http.Fakes;
    using Rakuten.Rmsg.ProductQuery.WebJob.Api;
    using Rakuten.Rmsg.ProductQuery.WebJob.Linking.Fakes;

    /// <summary>
    /// A suite of tests for the <see cref="ExecuteSearchCommand"/> class.
    /// </summary>
    [TestClass]
    public class ExecuteSearchCommandTests
    {
        /// <summary>
        /// Ensures that when a product search is performed using an EAN a non null collection is returned.
        /// </summary>
        /// <returns>A <see cref="Task"/> the represents the asynchronous operation.</returns>
        [TestMethod]
        public async Task ExecuteSearchCommandEanSearchRetrievesANonNullCollection()
        {
            // Arrange
            var searchLink = new StubProductSearchLink(new StubUriTemplate(
                "/v1/product?filter={filter}&culture={culture}&skip={skip}&top={top}"));

            // Act
            var products = await ExecuteSearchCommand.Execute(
                () => CreateApiClient(message => Task.FromResult(Enumerable.Empty<Product>())),
                searchLink,
                new[] { "EAN", "111111111116", "en-GB" });

            // Assert
            Assert.IsNotNull(products);
        }

        /// <summary>
        /// Ensures that multiple pages of a product search are handled.
        /// </summary>
        /// <returns>A <see cref="Task"/> the represents the asynchronous operation.</returns>
        [TestMethod]
        public async Task ExecuteSearchCommandHandlesAPageWithNoSearchResults()
        {
            // Arrange
            var searchLink = new StubProductSearchLink(new StubUriTemplate(
                "/v1/product?filter={filter}&culture={culture}&skip={skip}&top={top}"));

            // Act
            var products = await ExecuteSearchCommand.Execute(
                () => CreateApiClient(message => Task.FromResult(Enumerable.Empty<Product>())),
                searchLink,
                new[] { "ISBN", "111111111116", "en-GB" });

            // Assert
            Assert.AreEqual(0, products.Count());
        }

        /// <summary>
        /// Ensures that multiple pages of a product search are handled.
        /// </summary>
        /// <returns>A <see cref="Task"/> the represents the asynchronous operation.</returns>
        [TestMethod]
        public async Task ExecuteSearchCommandHandlesASinglePageOfProduct()
        {
            // Arrange
            var searchLink = new StubProductSearchLink(new StubUriTemplate(
                "/v1/product?filter={filter}&culture={culture}&skip={skip}&top={top}"));

            int requestNumber = 0;

            Func<HttpResponseMessage, Task<IEnumerable<Product>>> handleResponse = message =>
            {
                var searchResults = new List<Product>();

                if (requestNumber == 0)
                {
                    for (int i = 0; i < 20; i++)
                    {
                        searchResults.Add(new Product());
                    }
                }

                requestNumber++;

                return Task.FromResult(searchResults.AsEnumerable());
            };

            // Act
            var products = await ExecuteSearchCommand.Execute(
                () => CreateApiClient(handleResponse),
                searchLink,
                new[] { "ISBN", "111111111116", "en-GB" });

            // Assert
            Assert.AreEqual(20, products.Count());
        }

        /// <summary>
        /// Ensures that multiple pages of a product search are handled.
        /// </summary>
        /// <returns>A <see cref="Task"/> the represents the asynchronous operation.</returns>
        [TestMethod]
        public async Task ExecuteSearchCommandHandlesLessThanASinglePageOfProduct()
        {
            // Arrange
            var searchLink = new StubProductSearchLink(new StubUriTemplate(
                "/v1/product?filter={filter}&culture={culture}&skip={skip}&top={top}"));

            Func<HttpResponseMessage, Task<IEnumerable<Product>>> handleResponse = message =>
            {
                var searchResults = new List<Product>();

                for (int i = 0; i < 2; i++)
                {
                    searchResults.Add(new Product());
                }

                return Task.FromResult(searchResults.AsEnumerable());
            };

            // Act
            var products = await ExecuteSearchCommand.Execute(
                () => CreateApiClient(handleResponse),
                searchLink,
                new[] { "ISBN", "111111111116", "en-GB" });

            // Assert
            Assert.AreEqual(2, products.Count());
        }

        /// <summary>
        /// Ensures that multiple pages of a product search are handled.
        /// </summary>
        /// <returns>A <see cref="Task"/> the represents the asynchronous operation.</returns>
        [TestMethod]
        public async Task ExecuteSearchCommandHandlesMultiplePagesOfProduct()
        {
            // Arrange
            var searchLink = new StubProductSearchLink(new StubUriTemplate(
                "/v1/product?filter={filter}&culture={culture}&skip={skip}&top={top}"));

            int requestNumber = 0;

            Func<HttpResponseMessage, Task<IEnumerable<Product>>> handleResponse = message =>
            {
                var searchResults = new List<Product>();

                if (requestNumber == 0)
                {
                    for (int i = 0; i < 20; i++)
                    {
                        searchResults.Add(new Product());
                    }
                }
                else
                {
                    searchResults.Add(new Product());   
                }

                requestNumber++;

                return Task.FromResult(searchResults.AsEnumerable());
            };

            // Act
            var products = await ExecuteSearchCommand.Execute(
                () => CreateApiClient(handleResponse),
                searchLink,
                new[] { "ISBN", "111111111116", "en-GB" });

            // Assert
            Assert.AreEqual(21, products.Count());
        }

        /// <summary>
        /// Ensures that when a product search is performed using an ISBN a non null collection is returned.
        /// </summary>
        /// <returns>A <see cref="Task"/> the represents the asynchronous operation.</returns>
        [TestMethod]
        public async Task ExecuteSearchCommandIsbnSearchRetrievesANonNullCollection()
        {
            // Arrange
            var searchLink = new StubProductSearchLink(new StubUriTemplate(
                "/v1/product?filter={filter}&culture={culture}&skip={skip}&top={top}"));

            // Act
            var products = await ExecuteSearchCommand.Execute(
                () => CreateApiClient(message => Task.FromResult(Enumerable.Empty<Product>())),
                searchLink,
                new[] { "ISBN", "111111111116", "en-GB" });

            // Assert
            Assert.IsNotNull(products);
        }

        /// <summary>
        /// Ensures that when a product search is performed using an JAN a non null collection is returned.
        /// </summary>
        /// <returns>A <see cref="Task"/> the represents the asynchronous operation.</returns>
        [TestMethod]
        public async Task ExecuteSearchCommandJanSearchRetrievesANonNullCollection()
        {
            // Arrange
            var searchLink = new StubProductSearchLink(new StubUriTemplate(
                "/v1/product?filter={filter}&culture={culture}&skip={skip}&top={top}"));

            // Act
            var products = await ExecuteSearchCommand.Execute(
                () => CreateApiClient(message => Task.FromResult(Enumerable.Empty<Product>())),
                searchLink,
                new[] { "JAN", "111111111116", "en-GB" });

            // Assert
            Assert.IsNotNull(products);
        }

        /// <summary>
        /// Ensures that a <see cref="CultureNotFoundException"/> is thrown when an invalid culture is passed.
        /// </summary>
        /// <returns>A <see cref="Task"/> the represents the asynchronous operation.</returns>
        public async Task ExecuteSearchCommandThrowsACultureNotFoundException()
        {
            // Arrange
            var searchLink = new StubProductSearchLink(new StubUriTemplate(
                "/v1/product?filter={filter}&culture={culture}&skip={skip}&top={top}"));

            Exception exception = null;

            // Act
            try
            {
                await ExecuteSearchCommand.Execute(
                    () => CreateApiClient(message => Task.FromResult(Enumerable.Empty<Product>())),
                    searchLink,
                    new[] { "ISBN", "111111111116", "xx-XX" });
            }
            catch (Exception ex)
            {
                exception = ex;
            }

            // Assert
            Assert.IsInstanceOfType(exception, typeof(CultureNotFoundException));
        }

        /// <summary>
        /// Ensures that when a product search is performed using an UPC a non null collection is returned.
        /// </summary>
        /// <returns>A <see cref="Task"/> the represents the asynchronous operation.</returns>
        [TestMethod]
        public async Task ExecuteSearchCommandUpcSearchRetrievesANonNullCollection()
        {
            // Arrange
            var searchLink = new StubProductSearchLink(new StubUriTemplate(
                "/v1/product?filter={filter}&culture={culture}&skip={skip}&top={top}"));

            // Act
            var products = await ExecuteSearchCommand.Execute(
                () => CreateApiClient(message => Task.FromResult(Enumerable.Empty<Product>())),
                searchLink,
                new[] { "UPC", "111111111116", "en-GB" });

            // Assert
            Assert.IsNotNull(products);
        }

        /// <summary>
        /// Creates a configured <see cref="StubApiClient"/> instance that will use the specified delegate.
        /// </summary>
        /// <typeparam name="T">The type expected back from the response.</typeparam>
        /// <param name="stub">The delegate to be used when handling the response.</param>
        /// <returns>A new <see cref="StubApiClient"/> instance.</returns>
        [TestMethod]
        private static StubApiClient CreateApiClient<T>(Func<HttpResponseMessage, Task<T>> stub)
        {
            var stubIHttpResponseHandler = new StubIHttpResponseHandler();
            stubIHttpResponseHandler.ReadAsyncOf1HttpResponseMessage(message => stub(message));

            return new StubApiClient(
                new StubIApiClientContext
                {
                    AuthorizationTokenGet = () => "1234567890",
                    BaseAddressGet = () => new Uri("http://localhost")
                },
                new StubIHttpRequestHandler
                {
                    GetAsyncUri = uri => Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK))
                },
                stubIHttpResponseHandler);
        }
    }
}
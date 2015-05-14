// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="GetProductCommandTests.cs" company="Rakuten">
//   Copyright (c) Rakuten. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.WebJob.Tests.Unit
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Rakuten.Fakes;
    using Rakuten.Rmsg.ProductQuery.WebJob.Api;
    using Rakuten.Rmsg.ProductQuery.WebJob.Linking.Fakes;

    /// <summary>
    /// A suite of tests for the <see cref="GetProductCommand"/> class.
    /// </summary>
    [TestClass]
    public class GetProductCommandTests
    {
        /// <summary>
        /// Ensures that a non-null value is returned.
        /// </summary>
        /// <returns>A <see cref="Task"/> the represents the asynchronous operation.</returns>
        [TestMethod]
        public async Task GetProductCommandReturnsANonNullValue()
        {
            // Arrange
            var link = new StubProductLink(new StubUriTemplate(
                "/v1/product/{gran}?culture={culture}"));

            // Act
            var product = await GetProductCommand.Execute(
                () => StubApiClientFactory.Create(message => Task.FromResult(new Product())),
                link,
                new[] { "000000000000", "en-GB" });

            // Assert
            Assert.IsNotNull(product);
        }

        /// <summary>
        /// Ensures that a <see cref="CultureNotFoundException"/> is thrown when an invalid culture is passed.
        /// </summary>
        /// <returns>A <see cref="Task"/> the represents the asynchronous operation.</returns>
        [TestMethod]
        public async Task GetProductCommandThrowsACultureNotFoundExceptionWithAnInvalidCulture()
        {
            // Arrange
            var link = new StubProductLink(new StubUriTemplate(
                "/v1/product/{gran}?culture={culture}"));

            Exception exception = null;

            // Act
            try
            {
                await GetProductCommand.Execute(
                    () => StubApiClientFactory.Create(message => Task.FromResult(new Product())),
                    link,
                    new[] { "000000000000", "xx-XX" });
            }
            catch (Exception ex)
            {
                exception = ex;
            }

            // Assert
            Assert.IsInstanceOfType(exception, typeof(CultureNotFoundException));
        }
    }
}
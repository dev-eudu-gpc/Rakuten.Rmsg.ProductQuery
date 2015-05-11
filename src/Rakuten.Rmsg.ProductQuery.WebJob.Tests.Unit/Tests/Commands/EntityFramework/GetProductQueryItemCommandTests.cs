// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="GetProductQueryItemCommandTests.cs" company="Rakuten">
//   Copyright (c) Rakuten. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.WebJob.Tests.Unit
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Rakuten.Rmsg.ProductQuery.WebJob.Entities;

    /// <summary>
    /// A suite of tests for the <see cref="GetProductQueryItemCommand"/> class.
    /// </summary>
    [TestClass]
    public class GetProductQueryItemCommandTests
    {
        /// <summary>
        /// Ensures that a non-null record is returned when a matching record is found.
        /// </summary>
        /// <returns>A <see cref="Task"/> the represents the asynchronous operation.</returns>
        [TestMethod]
        public async Task GetProductQueryItemCommandReturnsANonNullResultOnAMatchingRecord()
        {
            // Act
            var result = await GetProductQueryItemCommand.Execute(
                (guid, s) =>
                    Task.FromResult(new List<ProductQueryItem>
                    {
                        new ProductQueryItem()
                    }),
                Guid.Empty,
                "000000000000");

            // Assert
            Assert.IsNotNull(result);
        }

        /// <summary>
        /// Ensures that the result returned has the same GRAN as the matching record.
        /// </summary>
        /// <returns>A <see cref="Task"/> the represents the asynchronous operation.</returns>
        [TestMethod]
        public async Task GetProductQueryItemCommandReturnsAResultWithAMatchingGranToAMatchingRecord()
        {
            // Arrange
            string gran = "000000000000";

            var item = new ProductQueryItem
            {
                Gran = gran,
                Gtin = "111111111116",
                ProductQueryId = Guid.NewGuid()
            };

            // Act
            var result = await GetProductQueryItemCommand.Execute(
                (guid, s) =>
                    Task.FromResult(new List<ProductQueryItem>
                    {
                        item
                    }),
                Guid.Empty,
                "000000000000");

            // Assert
            Assert.AreEqual(gran, result.Gran);
        }

        /// <summary>
        /// Ensures that the result returned has the same GTIN as the matching record.
        /// </summary>
        /// <returns>A <see cref="Task"/> the represents the asynchronous operation.</returns>
        [TestMethod]
        public async Task GetProductQueryItemCommandReturnsAResultWithAMatchingGtinToAMatchingRecord()
        {
            // Arrange
            string gtin = "111111111116";

            var item = new ProductQueryItem
            {
                Gran = "000000000000",
                Gtin = gtin,
                ProductQueryId = Guid.NewGuid()
            };

            // Act
            var result = await GetProductQueryItemCommand.Execute(
                (guid, s) =>
                    Task.FromResult(new List<ProductQueryItem>
                    {
                        item
                    }),
                Guid.Empty,
                "000000000000");

            // Assert
            Assert.AreEqual(gtin, result.Gtin);
        }

        /// <summary>
        /// Ensures that the result returned has the same identifier as the matching record.
        /// </summary>
        /// <returns>A <see cref="Task"/> the represents the asynchronous operation.</returns>
        [TestMethod]
        public async Task GetProductQueryItemCommandReturnsAResultWithAMatchingIdentifierToAMatchingRecord()
        {
            // Arrange
            var id = Guid.NewGuid();

            var item = new ProductQueryItem
            {
                Gran = "000000000000",
                Gtin = "111111111116",
                ProductQueryId = id
            };

            // Act
            var result = await GetProductQueryItemCommand.Execute(
                (guid, s) =>
                    Task.FromResult(new List<ProductQueryItem>
                    {
                        item
                    }),
                Guid.Empty,
                "000000000000");

            // Assert
            Assert.AreEqual(id, result.ProductQueryId);
        }

        /// <summary>
        /// Ensures that <see langword="null"/> is returned when no matching records were found.
        /// </summary>
        /// <returns>A <see cref="Task"/> the represents the asynchronous operation.</returns>
        [TestMethod]
        public async Task GetProductQueryItemCommandReturnsNullOnNoMatchingRecords()
        {
            // Act
            var result = await GetProductQueryItemCommand.Execute(
                (guid, s) =>
                    Task.FromResult(new List<ProductQueryItem>()),
                Guid.Empty,
                "000000000000");

            // Assert
            Assert.IsNull(result);
        }

        /// <summary>
        /// Ensures that a <see cref="InvalidOperationException"/> is thrown when multiple matching records are found.
        /// </summary>
        /// <returns>A <see cref="Task"/> the represents the asynchronous operation.</returns>
        [TestMethod]
        public async Task GetProductQueryItemCommandThrowsAnInvalidOperationExceptionWithMultipleRecords()
        {
            // Arrange
            Exception exception = null;

            // Act
            try
            {
                await GetProductQueryItemCommand.Execute(
                    (guid, s) => 
                        Task.FromResult(new List<ProductQueryItem>
                        {
                            new ProductQueryItem(),
                            new ProductQueryItem()
                        }),
                    Guid.Empty,
                    "000000000000");
            }
            catch (Exception ex)
            {
                exception = ex;
            }

            // Assert
            Assert.IsInstanceOfType(exception, typeof(InvalidOperationException));
        }
    }
}
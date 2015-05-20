// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="FilterProductsCommandTests.cs" company="Rakuten">
//   Copyright (c) Rakuten. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.WebJob.Tests.Unit
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Rakuten.Rmsg.ProductQuery.WebJob.Api;

    /// <summary>
    /// A suite of tests for the <see cref="FilterProductsCommand"/> class.
    /// </summary>
    [TestClass]
    public class FilterProductsCommandTests
    {
        /// <summary>
        /// Ensures that a non-null result is returned when the collection of products is filtered.
        /// </summary>
        /// <returns>A <see cref="Task"/> the represents the asynchronous operation.</returns>
        [TestMethod]
        public async Task FilterProductsCommandReturnsANonNullResult()
        {
            // Arrange
            var dataSources = GenerateDataSources();

            var products = new List<Product>
            {
                new Product
                {
                    Id = "000000000000",
                    ImprovedId = "000000000000",
                    Name = "Hewlett-Packard (HP) EliteBook 8460p",
                    CultureCode = "en-GB",
                    DataSource = "GECP"
                }
            };

            // Act
            var product = await FilterProductsCommand.Execute(dataSources, products);

            // Assert
            Assert.IsNotNull(product);
        }

        /// <summary>
        /// Ensures that the product that has most recently been updated when the collection of products is filtered.
        /// </summary>
        /// <returns>A <see cref="Task"/> the represents the asynchronous operation.</returns>
        [TestMethod]
        public async Task FilterProductsCommandReturnsTheMostRecentlyUpdatedProduct()
        {
            // Arrange
            var dataSources = GenerateDataSources();

            var updated = DateTime.Now;

            var products = new List<Product>
            {
                new Product
                {
                    Id = "000000000001",
                    ImprovedId = "000000000001",
                    Name = "Hewlett-Packard (HP) EliteBook 8460p",
                    CultureCode = "en-GB",
                    DataSource = "GECP",
                    TimestampUpdate = updated
                },
                new Product
                {
                    Id = "000000000000",
                    ImprovedId = "000000000000",
                    Name = "Hewlett-Packard (HP) EliteBook 8460p",
                    CultureCode = "en-GB",
                    DataSource = "GECP",
                    TimestampUpdate = updated.AddHours(1)
                }
            };

            // Act
            var product = await FilterProductsCommand.Execute(dataSources, products);

            // Assert
            Assert.AreEqual("000000000000", product.Id);
        }

        /// <summary>
        /// Ensures that the product that has most recently been added when the collection of products is filtered.
        /// </summary>
        /// <returns>A <see cref="Task"/> the represents the asynchronous operation.</returns>
        [TestMethod]
        public async Task FilterProductsCommandReturnsTheMostRecentProduct()
        {
            // Arrange
            var dataSources = GenerateDataSources();

            var updated = DateTime.Now;

            var products = new List<Product>
            {
                new Product
                {
                    Id = "000000000001",
                    ImprovedId = "000000000001",
                    Name = "Hewlett-Packard (HP) EliteBook 8460p",
                    CultureCode = "en-GB",
                    DataSource = "GECP",
                    TimestampUpdate = updated
                },
                new Product
                {
                    Id = "000000000000",
                    ImprovedId = "000000000000",
                    Name = "Hewlett-Packard (HP) EliteBook 8460p",
                    CultureCode = "en-GB",
                    DataSource = "GECP",
                    TimestampUpdate = updated
                }
            };

            // Act
            var product = await FilterProductsCommand.Execute(dataSources, products);

            // Assert
            Assert.AreEqual("000000000001", product.Id);
        }

        /// <summary>
        /// Ensures that only a product that has not been improved is returned when a collection of products is 
        /// filtered.
        /// </summary>
        /// <returns>A <see cref="Task"/> the represents the asynchronous operation.</returns>
        [TestMethod]
        public async Task FilterProductsCommandReturnsThePrimaryProduct()
        {
            // Arrange
            var dataSources = GenerateDataSources();

            var products = new List<Product>
            {
                new Product
                {
                    Id = "000000000001",
                    ImprovedId = "000000000001",
                    Name = "Hewlett-Packard (HP) EliteBook 8460p",
                    CultureCode = "en-GB",
                    DataSource = "GECP"
                },
                new Product
                {
                    Id = "000000000000",
                    ImprovedId = "000000000001",
                    Name = "Hewlett-Packard (HP) EliteBook 8460p",
                    CultureCode = "en-GB",
                    DataSource = "GECP"
                }
            };

            // Act
            var product = await FilterProductsCommand.Execute(dataSources, products);

            // Assert
            Assert.AreEqual("000000000001", product.Id);
        }

        /// <summary>
        /// Ensures that the product with the highest data source is returned when the collection of products is 
        /// filtered.
        /// </summary>
        /// <returns>A <see cref="Task"/> the represents the asynchronous operation.</returns>
        [TestMethod]
        public async Task FilterProductsCommandReturnsTheProductWithTheMostTrustedDataSource()
        {
            // Arrange
            var dataSources = GenerateDataSources();

            var products = new List<Product>
            {
                new Product
                {
                    Id = "000000000001",
                    ImprovedId = "000000000001",
                    Name = "Hewlett-Packard (HP) EliteBook 8460p",
                    CultureCode = "en-GB",
                    DataSource = "GECP"
                },
                new Product
                {
                    Id = "000000000000",
                    ImprovedId = "000000000000",
                    Name = "Hewlett-Packard (HP) EliteBook 8460p",
                    CultureCode = "en-GB",
                    DataSource = "Play.com"
                }
            };

            // Act
            var product = await FilterProductsCommand.Execute(dataSources, products);

            // Assert
            Assert.AreEqual("000000000000", product.Id);
        }

        /// <summary>
        /// Generates and returns a collection of <see cref="DataSource"/>s.
        /// </summary>
        /// <returns>A collection of data sources.</returns>
        private static IEnumerable<DataSource> GenerateDataSources()
        {
            return new List<DataSource>
            {
                new DataSource
                {
                    Id = Guid.Empty.ToString("N"),
                    Name = "GECP",
                    CultureCode = "en-GB",
                    TrustScore = 0
                },
                new DataSource
                {
                    Id = Guid.Empty.ToString("N"),
                    Name = "Play.com",
                    CultureCode = "en-GB",
                    TrustScore = 1
                }
            };
        }
    }
}
// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="CreateProductQueryItemCommandTests.cs" company="Rakuten">
//   Copyright (c) Rakuten. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.WebJob.Tests.Unit
{
    using System;
    using System.Threading.Tasks;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// A suite of tests for the <see cref="CreateProductQueryItemCommand"/> class.
    /// </summary>
    [TestClass]
    public class CreateProductQueryItemCommandTests
    {
        /// <summary>
        /// Ensures that the entity returned from the command has the same GTIN as the one specified.
        /// </summary>
        /// <returns>A <see cref="Task"/> the represents the asynchronous operation.</returns>
        [TestMethod]
        public async Task CreateProductQueryItemCommandReturnsAEntityWithTheCorrectGtin()
        {
            // Arrange
            string gtin = "111111111116";

            // Act
            var entity = await CreateProductQueryItemCommand.Execute(Task.FromResult, Guid.NewGuid(), gtin);

            // Assert
            Assert.AreEqual(gtin, entity.Gtin);
        }

        /// <summary>
        /// Ensures that the entity returned from the command has the same identifier as the one specified.
        /// </summary>
        /// <returns>A <see cref="Task"/> the represents the asynchronous operation.</returns>
        [TestMethod]
        public async Task CreateProductQueryItemCommandReturnsAEntityWithTheCorrectIdentifier()
        {
            // Arrange
            var id = Guid.NewGuid();

            // Act
            var entity = await CreateProductQueryItemCommand.Execute(Task.FromResult, id, "111111111116");

            // Assert
            Assert.AreEqual(id, entity.ProductQueryId);
        }

        /// <summary>
        /// Ensures that the entity persisted is returned from the command.
        /// </summary>
        /// <returns>A <see cref="Task"/> the represents the asynchronous operation.</returns>
        [TestMethod]
        public async Task CreateProductQueryItemCommandReturnsANonNullEntity()
        {
            // Act
            var entity = await CreateProductQueryItemCommand.Execute(Task.FromResult, Guid.NewGuid(), "111111111116");

            // Assert
            Assert.IsNotNull(entity);
        }
    }
}
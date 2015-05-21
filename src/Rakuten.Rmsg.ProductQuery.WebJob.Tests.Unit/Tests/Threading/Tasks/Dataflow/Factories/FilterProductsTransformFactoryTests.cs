// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="FilterProductsTransformFactoryTests.cs" company="Rakuten">
//   Copyright (c) Rakuten. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.WebJob.Tests.Unit
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Threading.Tasks;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Rakuten.Rmsg.ProductQuery.WebJob.Api;

    /// <summary>
    /// A suite of tests for the <see cref="FilterProductsTransformFactory"/> class.
    /// </summary>
    [TestClass]
    public class FilterProductsTransformFactoryTests
    {
        /// <summary>
        /// Ensures that the delegate returned from the factory populates the GRAN  on the 
        /// <see cref="ProductQueryItem"/> on the message state.
        /// </summary>
        /// <returns>A <see cref="Task"/> the represents the asynchronous operation.</returns>
        [TestMethod]
        public async Task FilterProductsTransformFactoryCreateReturnsADelegateThatPopulatesTheGran()
        {
            // Arrange
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);

            Func<IEnumerable<Product>, Task<Product>> command =
                products => Task.FromResult(new Product { Id = "000000000000" });

            var transform = FilterProductsTransformFactory.Create(command);

            var state = new ItemMessageState(
                Guid.NewGuid(),
                new CultureInfo("en-GB"),
                new Item(),
                new ProductQueryItem());

            // Act
            state = await transform(state, writer);

            // Assert
            Assert.AreEqual("000000000000", state.Query.Gran);
        }

        /// <summary>
        /// Ensures that the delegate returned from the factory retains the same identifier on the returned 
        /// <see cref="ItemMessageState"/> as the one supplied.
        /// </summary>
        /// <returns>A <see cref="Task"/> the represents the asynchronous operation.</returns>
        [TestMethod]
        public async Task FilterProductsTransformFactoryCreateReturnsADelegateThatRetainsTheSameCulture()
        {
            // Arrange
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);

            Func<IEnumerable<Product>, Task<Product>> command =
                products => Task.FromResult(new Product { Id = "000000000000" });

            var transform = FilterProductsTransformFactory.Create(command);

            var state = new ItemMessageState(Guid.NewGuid(), new CultureInfo("en-GB"), new Item());

            // Act
            state = await transform(state, writer);

            // Assert
            Assert.AreEqual("en-GB", state.Culture.Name);
        }

        /// <summary>
        /// Ensures that the delegate returned from the factory retains the same identifier on the returned 
        /// <see cref="ItemMessageState"/> as the one supplied.
        /// </summary>
        /// <returns>A <see cref="Task"/> the represents the asynchronous operation.</returns>
        [TestMethod]
        public async Task FilterProductsTransformFactoryCreateReturnsADelegateThatRetainsTheSameIdentifier()
        {
            // Arrange
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);

            Func<IEnumerable<Product>, Task<Product>> command =
                products => Task.FromResult(new Product { Id = "000000000000" });

            var transform = FilterProductsTransformFactory.Create(command);

            var id = Guid.NewGuid();

            var state = new ItemMessageState(id, new CultureInfo("en-GB"), new Item());

            // Act
            state = await transform(state, writer);

            // Assert
            Assert.AreEqual(id, state.Id);
        }

        /// <summary>
        /// Ensures that the delegate returned from the factory handles an exception and writes a message to the 
        /// <see cref="TextWriter"/> instance.
        /// </summary>
        /// <returns>A <see cref="Task"/> the represents the asynchronous operation.</returns>
        [TestMethod]
        public async Task FilterProductsTransformFactoryCreateReturnsADelegateThatWritesAnExceptionToAStream()
        {
            // Arrange
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);

            Func<IEnumerable<Product>, Task<Product>> command =
                products => { throw new Exception("Ninja Cat riding a Fire-breathing Unicorn"); };

            var transform = FilterProductsTransformFactory.Create(command);

            var state = new ItemMessageState(Guid.NewGuid(), new CultureInfo("en-GB"), new Item());

            // Act
            await transform(state, writer);

            // Assert
            writer.Flush();

            stream.Position = 0;
            var message = new StreamReader(stream).ReadToEnd();

            Assert.IsTrue(!string.IsNullOrWhiteSpace(message));
        }
    }
}
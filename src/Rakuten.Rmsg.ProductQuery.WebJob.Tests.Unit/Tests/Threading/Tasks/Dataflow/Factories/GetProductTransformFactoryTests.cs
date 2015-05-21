// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="GetProductTransformFactoryTests.cs" company="Rakuten">
//   Copyright (c) Rakuten. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.WebJob.Tests.Unit
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Threading.Tasks;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Rakuten.Rmsg.ProductQuery.WebJob.Api;

    /// <summary>
    /// A suite of tests for the <see cref="GetProductTransformFactory"/> class.
    /// </summary>
    [TestClass]
    public class GetProductTransformFactoryTests
    {
        /// <summary>
        /// Ensures that the delegate returned from the factory populates the product.
        /// </summary>
        /// <returns>A <see cref="Task"/> the represents the asynchronous operation.</returns>
        [TestMethod]
        public async Task GetProductTransformFactoryCreateReturnsADelegateThatPopulatesTheProduct()
        {
            // Arrange
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);

            Func<string, CultureInfo, Task<Product>> command = (s, info) => Task.FromResult(new Product());

            var transform = GetProductTransformFactory.Create(command);

            var state = new ItemMessageState(
                Guid.NewGuid(),
                new CultureInfo("en-GB"),
                new Item(),
                new ProductQueryItem());

            // Act
            state = await transform(state, writer);

            // Assert
            Assert.IsNotNull(state.Product);
        }

        /// <summary>
        /// Ensures that the delegate returned from the factory retains the same identifier on the returned 
        /// <see cref="ItemMessageState"/> as the one supplied.
        /// </summary>
        /// <returns>A <see cref="Task"/> the represents the asynchronous operation.</returns>
        [TestMethod]
        public async Task GetProductTransformFactoryCreateReturnsADelegateThatRetainsTheSameCulture()
        {
            // Arrange
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);

            Func<string, CultureInfo, Task<Product>> command = (s, info) => Task.FromResult(new Product());

            var transform = GetProductTransformFactory.Create(command);

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
        public async Task GetProductTransformFactoryCreateReturnsADelegateThatRetainsTheSameIdentifier()
        {
            // Arrange
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);

            Func<string, CultureInfo, Task<Product>> command = (s, info) => Task.FromResult(new Product());

            var transform = GetProductTransformFactory.Create(command);

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
        public async Task GetProductTransformFactoryCreateReturnsADelegateThatWritesAnExceptionToAStream()
        {
            // Arrange
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);

            Func<string, CultureInfo, Task<Product>> command =
                (s, info) => { throw new Exception("Ninja Cat riding a Fire-breathing Unicorn"); };

            var transform = GetProductTransformFactory.Create(command);

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
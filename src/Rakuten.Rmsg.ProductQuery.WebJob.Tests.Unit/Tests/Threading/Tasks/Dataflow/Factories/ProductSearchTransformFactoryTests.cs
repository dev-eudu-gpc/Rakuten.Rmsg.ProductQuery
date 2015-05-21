// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="ProductSearchTransformFactoryTests.cs" company="Rakuten">
//   Copyright (c) Rakuten. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.WebJob.Tests.Unit
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Rakuten.Rmsg.ProductQuery.WebJob.Api;

    /// <summary>
    /// A suite of tests for the <see cref="ProductSearchTransformFactory"/> class.
    /// </summary>
    [TestClass]
    public class ProductSearchTransformFactoryTests
    {
        /// <summary>
        /// Ensures that the delegate returned from the factory retains the same identifier on the returned 
        /// <see cref="ItemMessageState"/> as the one supplied.
        /// </summary>
        /// <returns>A <see cref="Task"/> the represents the asynchronous operation.</returns>
        [TestMethod]
        public async Task ProductSearchTransformFactoryCreateReturnsADelegateThatRetainsTheSameCulture()
        {
            // Arrange
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);

            Func<string, string, CultureInfo, Task<IEnumerable<Product>>> command =
                (s, s1, arg3) => Task.FromResult(Enumerable.Empty<Product>());

            var transform = ProductSearchTransformFactory.Create(command);

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
        public async Task ProductSearchTransformFactoryCreateReturnsADelegateThatRetainsTheSameIdentifier()
        {
            // Arrange
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);

            Func<string, string, CultureInfo, Task<IEnumerable<Product>>> command =
                (s, s1, arg3) => Task.FromResult(Enumerable.Empty<Product>());

            var transform = ProductSearchTransformFactory.Create(command);

            var id = Guid.NewGuid();

            var state = new ItemMessageState(id, new CultureInfo("en-GB"), new Item());

            // Act
            state = await transform(state, writer);

            // Assert
            Assert.AreEqual(id, state.Id);
        }

        /// <summary>
        /// Ensures that the delegate returned from the factory returns a collection of <see cref="Product"/>.
        /// </summary>
        /// <returns>A <see cref="Task"/> the represents the asynchronous operation.</returns>
        [TestMethod]
        public async Task ProductSearchTransformFactoryCreateReturnsADelegateThatReturnsACollectionOfState()
        {
            // Arrange
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);

            Func<string, string, CultureInfo, Task<IEnumerable<Product>>> command =
                (s, s1, arg3) => Task.FromResult(Enumerable.Empty<Product>());

            var transform = ProductSearchTransformFactory.Create(command);

            var state = new ItemMessageState(Guid.NewGuid(), new CultureInfo("en-GB"), new Item());

            // Act
            state = await transform(state, writer);

            // Assert
            Assert.IsNotNull(state.Products);
        }

        /// <summary>
        /// Ensures that the delegate returned from the factory handles an exception and writes a message to the 
        /// <see cref="TextWriter"/> instance.
        /// </summary>
        /// <returns>A <see cref="Task"/> the represents the asynchronous operation.</returns>
        [TestMethod]
        public async Task ProductSearchTransformFactoryCreateReturnsADelegateThatWritesAnExceptionToAStream()
        {
            // Arrange
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);

            Func<string, string, CultureInfo, Task<IEnumerable<Product>>> command =
                (s, s1, arg3) => { throw new Exception("Ninja Cat riding a Fire-breathing Unicorn"); };

            var transform = ProductSearchTransformFactory.Create(command);

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
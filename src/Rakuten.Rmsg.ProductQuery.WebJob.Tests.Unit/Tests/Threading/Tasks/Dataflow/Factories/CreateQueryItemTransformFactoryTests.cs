// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="CreateQueryItemTransformFactoryTests.cs" company="Rakuten">
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

    /// <summary>
    /// A suite of tests for the <see cref="CreateQueryItemTransformFactory"/> class.
    /// </summary>
    [TestClass]
    public class CreateQueryItemTransformFactoryTests
    {
        /// <summary>
        /// Ensures that the delegate returned from the factory populates the <see cref="ProductQueryItem"/> on the 
        /// message state.
        /// </summary>
        /// <returns>A <see cref="Task"/> the represents the asynchronous operation.</returns>
        [TestMethod]
        public async Task CreateQueryItemTransformFactoryCreateReturnsADelegateThatDefinesAProductQueryItem()
        {
            // Arrange
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);

            Func<Guid, string, Task<ProductQueryItem>> command =
                (guid, s) => Task.FromResult(new ProductQueryItem { ProductQueryId = guid, Gtin = s });

            var transform = CreateQueryItemTransformFactory.Create(command);

            var state = new ItemMessageState(Guid.NewGuid(), new CultureInfo("en-GB"), new Item());

            // Act
            state = await transform(state, writer);

            // Assert
            Assert.IsNotNull(state.Query);
        }

        /// <summary>
        /// Ensures that the delegate returned from the factory retains the same identifier on the returned 
        /// <see cref="ItemMessageState"/> as the one supplied.
        /// </summary>
        /// <returns>A <see cref="Task"/> the represents the asynchronous operation.</returns>
        [TestMethod]
        public async Task CreateQueryItemTransformFactoryCreateReturnsADelegateThatRetainsTheSameCulture()
        {
            // Arrange
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);

            Func<Guid, string, Task<ProductQueryItem>> command =
                (guid, s) => Task.FromResult(new ProductQueryItem { ProductQueryId = guid, Gtin = s });

            var transform = CreateQueryItemTransformFactory.Create(command);

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
        public async Task CreateQueryItemTransformFactoryCreateReturnsADelegateThatRetainsTheSameIdentifier()
        {
            // Arrange
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);

            Func<Guid, string, Task<ProductQueryItem>> command =
                (guid, s) => Task.FromResult(new ProductQueryItem { ProductQueryId = guid, Gtin = s });

            var transform = CreateQueryItemTransformFactory.Create(command);

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
        public async Task CreateQueryItemTransformFactoryCreateReturnsADelegateThatWritesAnExceptionToAStream()
        {
            // Arrange
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);

            Func<Guid, string, Task<ProductQueryItem>> command =
                (guid, s) => { throw new Exception("Ninja Cat riding a Fire-breathing Unicorn"); };

            var transform = CreateQueryItemTransformFactory.Create(command);

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
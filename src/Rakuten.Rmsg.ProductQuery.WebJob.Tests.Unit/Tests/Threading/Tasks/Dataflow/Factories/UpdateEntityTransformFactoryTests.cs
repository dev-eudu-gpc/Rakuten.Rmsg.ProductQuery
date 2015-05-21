// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="UpdateEntityTransformFactoryTests.cs" company="Rakuten">
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
    /// A suite of tests for the <see cref="UpdateEntityTransformFactory"/> class.
    /// </summary>
    [TestClass]
    public class UpdateEntityTransformFactoryTests
    {
        /// <summary>
        /// Ensures that the delegate returned from the factory retains the same identifier on the returned 
        /// <see cref="ItemMessageState"/> as the one supplied.
        /// </summary>
        /// <returns>A <see cref="Task"/> the represents the asynchronous operation.</returns>
        [TestMethod]
        public async Task UpdateEntityTransformFactoryCreateReturnsADelegateThatRetainsTheSameCulture()
        {
            // Arrange
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);

            Func<ProductQueryItem, Task> command = item => Task.FromResult(0);

            var transform = UpdateEntityTransformFactory.Create(command);

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
        public async Task UpdateEntityTransformFactoryCreateReturnsADelegateThatRetainsTheSameIdentifier()
        {
            // Arrange
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);

            Func<ProductQueryItem, Task> command = item => Task.FromResult(0);

            var transform = UpdateEntityTransformFactory.Create(command);

            var id = Guid.NewGuid();

            var state = new ItemMessageState(id, new CultureInfo("en-GB"), new Item());

            // Act
            state = await transform(state, writer);

            // Assert
            Assert.AreEqual(id, state.Id);
        }

        /// <summary>
        /// Ensures that the delegate returned from the factory returns a <see cref="ItemMessageState"/> instance.
        /// </summary>
        /// <returns>A <see cref="Task"/> the represents the asynchronous operation.</returns>
        [TestMethod]
        public async Task UpdateEntityTransformFactoryCreateReturnsADelegateThatReturnsAState()
        {
            // Arrange
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);

            Func<ProductQueryItem, Task> command = item => Task.FromResult(0);

            var transform = UpdateEntityTransformFactory.Create(command);

            var state = new ItemMessageState(Guid.NewGuid(), new CultureInfo("en-GB"), new Item());

            // Act
            state = await transform(state, writer);

            // Assert
            Assert.IsNotNull(state);
        }

        /// <summary>
        /// Ensures that the delegate returned from the factory handles an exception and writes a message to the 
        /// <see cref="TextWriter"/> instance.
        /// </summary>
        /// <returns>A <see cref="Task"/> the represents the asynchronous operation.</returns>
        [TestMethod]
        public async Task UpdateEntityTransformFactoryCreateReturnsADelegateThatWritesAnExceptionToAStream()
        {
            // Arrange
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);

            Func<ProductQueryItem, Task> command = 
                item => { throw new Exception("Ninja Cat riding a Fire-breathing Unicorn"); };

            var transform = UpdateEntityTransformFactory.Create(command);

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
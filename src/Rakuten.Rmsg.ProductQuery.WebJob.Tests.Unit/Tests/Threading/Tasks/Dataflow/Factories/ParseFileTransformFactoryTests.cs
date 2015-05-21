// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="ParseFileTransformFactoryTests.cs" company="Rakuten">
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

    /// <summary>
    /// A suite of tests for the <see cref="ParseFileTransformFactory"/> class.
    /// </summary>
    [TestClass]
    public class ParseFileTransformFactoryTests
    {
        /// <summary>
        /// Ensures that the delegate returned from the factory retains the same identifier on the returned 
        /// <see cref="ItemMessageState"/> as the one supplied.
        /// </summary>
        /// <returns>A <see cref="Task"/> the represents the asynchronous operation.</returns>
        [TestMethod]
        public async Task ParseFileTransformFactoryCreateReturnsADelegateThatRetainsTheSameCulture()
        {
            // Arrange
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);

            Func<Stream, Task<IEnumerable<Item>>> command = s => Task.FromResult(Enumerable.Empty<Item>());

            var id = Guid.NewGuid();

            var transform = ParseFileTransformFactory.Create(command, id, new CultureInfo("en-GB"));

            // Act
            IEnumerable<ItemMessageState> states = await transform(new MemoryStream(), writer);

            // Assert
            Assert.IsTrue(states.All(s => s.Culture.Name.Equals("en-GB")));
        }

        /// <summary>
        /// Ensures that the delegate returned from the factory retains the same identifier on the returned 
        /// <see cref="ItemMessageState"/> as the one supplied.
        /// </summary>
        /// <returns>A <see cref="Task"/> the represents the asynchronous operation.</returns>
        [TestMethod]
        public async Task ParseFileTransformFactoryCreateReturnsADelegateThatRetainsTheSameIdentifier()
        {
            // Arrange
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);

            Func<Stream, Task<IEnumerable<Item>>> command = s => Task.FromResult(Enumerable.Empty<Item>());

            var id = Guid.NewGuid();

            var transform = ParseFileTransformFactory.Create(command, id, new CultureInfo("en-GB"));

            // Act
            IEnumerable<ItemMessageState> states = await transform(new MemoryStream(), writer);

            // Assert
            Assert.IsTrue(states.All(s => s.Id.Equals(id)));
        }

        /// <summary>
        /// Ensures that the delegate returned from the factory returns a collection of <see cref="ItemMessageState"/>.
        /// </summary>
        /// <returns>A <see cref="Task"/> the represents the asynchronous operation.</returns>
        [TestMethod]
        public async Task ParseFileTransformFactoryCreateReturnsADelegateThatReturnsACollectionOfState()
        {
            // Arrange
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);

            Func<Stream, Task<IEnumerable<Item>>> command = s => Task.FromResult(Enumerable.Empty<Item>());

            var transform = ParseFileTransformFactory.Create(command, Guid.NewGuid(), new CultureInfo("en-GB"));

            // Act
            IEnumerable<ItemMessageState> states = await transform(new MemoryStream(), writer);

            // Assert
            Assert.IsNotNull(states);
        }

        /// <summary>
        /// Ensures that the delegate returned from the factory handles an exception and writes a message to the 
        /// <see cref="TextWriter"/> instance.
        /// </summary>
        /// <returns>A <see cref="Task"/> the represents the asynchronous operation.</returns>
        [TestMethod]
        public async Task ParseFileTransformFactoryCreateReturnsADelegateThatWritesAnExceptionToAStream()
        {
            // Arrange
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);

            Func<Stream, Task<IEnumerable<Item>>> command =
                s => { throw new Exception("Ninja Cat riding a Fire-breathing Unicorn"); };

            var transform = ParseFileTransformFactory.Create(command, Guid.NewGuid(), new CultureInfo("en-GB"));

            // Act
            await transform(new MemoryStream(), writer);

            // Assert
            writer.Flush();

            stream.Position = 0;
            var message = new StreamReader(stream).ReadToEnd();

            Assert.IsTrue(!string.IsNullOrWhiteSpace(message));
        }
    }
}
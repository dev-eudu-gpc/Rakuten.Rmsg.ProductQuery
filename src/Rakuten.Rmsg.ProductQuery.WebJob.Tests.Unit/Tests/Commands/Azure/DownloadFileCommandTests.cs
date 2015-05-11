// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="DownloadFileCommandTests.cs" company="Rakuten">
//   Copyright (c) Rakuten. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.WebJob.Tests.Unit
{
    using System;
    using System.IO;
    using System.Text;
    using System.Threading.Tasks;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// A suite of tests for the <see cref="DownloadFileCommand"/> class.
    /// </summary>
    [TestClass]
    public class DownloadFileCommandTests
    {
        /// <summary>
        /// Ensures that the command to download a file from blob storage throws an exception when an invalid URI is 
        /// passed.
        /// </summary>
        /// <returns>A <see cref="Task"/> the represents the asynchronous operation.</returns>
        [TestMethod]
        public async Task DownloadFileCommandThrowsAUriFormatExceptionWithAnInvalidBlobUri()
        {
            // Arrange
            const string UriString = "invalid-uri";

            Func<string, Stream, Task<Stream>> downloadBlob =
                (s, stream) => Task.FromResult<Stream>(new MemoryStream());

            var message = new Message(Guid.NewGuid(), "en-GB", new Link() { Target = UriString });

            UriFormatException exception = null;

            // Act
            try
            {
                await DownloadFileCommand.Execute(downloadBlob, message, new MemoryStream());
            }
            catch (UriFormatException ex)
            {
                exception = ex;
            }

            // Assert
            Assert.IsInstanceOfType(exception, typeof(UriFormatException));
        }

        /// <summary>
        /// Ensures that the command to download a file from blob storage handles an invalid blob location.
        /// </summary>
        /// <returns>A <see cref="Task"/> the represents the asynchronous operation.</returns>
        [TestMethod]
        public async Task DownloadFileCommandThrowsAInvalidOperationExceptionWithAnInvalidBlobUri()
        {
            // Arrange
            const string UriString = "http://localhost";

            Func<string, Stream, Task<Stream>> downloadBlob = 
                (s, stream) => Task.FromResult<Stream>(new MemoryStream());

            var message = new Message(Guid.NewGuid(), "en-GB", new Link() { Target = UriString });

            Exception exception = null;

            // Act
            try
            {
                await DownloadFileCommand.Execute(downloadBlob, message, new MemoryStream());
            }
            catch (Exception ex)
            {
                exception = ex;
            }

            // Assert
            Assert.IsInstanceOfType(exception, typeof(InvalidOperationException));
        }

        /// <summary>
        /// Ensures that a stream is returned that contains the downloaded file content.
        /// </summary>
        /// <returns>A <see cref="Task"/> the represents the asynchronous operation.</returns>
        [TestMethod]
        public async Task DownloadFileCommandReturnsAStreamContainingBlobContent()
        {
            // Arrange
            const string UriString = "http://localhost/0001-01-01/file.txt";

            Func<string, Stream, Task<Stream>> downloadBlob =
                (s, stream) => Task.FromResult(ToStream("Some content."));

            var message = new Message(Guid.NewGuid(), "en-GB", new Link() { Target = UriString });

            // Act
            var file = await DownloadFileCommand.Execute(downloadBlob, message, new MemoryStream());

            // Assert
            var content = new StreamReader(file).ReadToEnd();

            Assert.AreEqual("Some content.", content);
        }

        /// <summary>
        /// Ensures that the filename for the blob is created correctly from the URI in the <see cref="Message"/>.
        /// </summary>
        /// <returns>A <see cref="Task"/> the represents the asynchronous operation.</returns>
        [TestMethod]
        public async Task DownloadFileCommandBuildsACorrectlyFormattedFilename()
        {
            // Arrange
            const string UriString = "http://localhost/0001-01-01/file.txt";

            Func<string, Stream, Task<Stream>> downloadBlob = (s, stream) =>
            {
                // Assert
                Assert.AreEqual("0001-01-01/file.txt", s);

                return Task.FromResult<Stream>(new MemoryStream());
            };

            var message = new Message(Guid.NewGuid(), "en-GB", new Link() { Target = UriString });

            // Act
            await DownloadFileCommand.Execute(downloadBlob, message, new MemoryStream());
        }

        /// <summary>
        /// Converts the given string content into a <see cref="Stream"/>.
        /// </summary>
        /// <param name="value">The string content.</param>
        /// <returns>A stream containing the string content.</returns>
        private static Stream ToStream(string value)
        {
            return new MemoryStream(Encoding.UTF8.GetBytes(value ?? string.Empty));
        }
    }
}
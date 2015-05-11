// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="WriteBlobCommandTests.cs" company="Rakuten">
//   Copyright (c) Rakuten. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.WebJob.Tests.Unit
{
    using System;
    using System.IO;
    using System.Threading.Tasks;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// A suite of tests for the <see cref="WriteBlobCommand"/> class.
    /// </summary>
    [TestClass]
    public class WriteBlobCommandTests
    {
        /// <summary>
        /// Ensures that the command to upload a stream to blob storage throws an exception when an invalid URI is 
        /// passed.
        /// </summary>
        /// <returns>A <see cref="Task"/> the represents the asynchronous operation.</returns>
        [TestMethod]
        public async Task WriteBlobCommandThrowsAUriFormatExceptionWithAnInvalidBlobUri()
        {
            // Arrange
            const string UriString = "invalid-uri";

            Func<Stream, string, Task<Stream>> uploadBlob =
                (s, stream) => Task.FromResult<Stream>(new MemoryStream());

            var message = new Message(Guid.NewGuid(), "en-GB", new Link { Target = UriString });

            Exception exception = null;

            // Act
            try
            {
                await WriteBlobCommand.Execute(uploadBlob, message, new MemoryStream());
            }
            catch (Exception ex)
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
        public async Task WriteBlobCommandThrowsAInvalidOperationExceptionWithAnInvalidBlobUri()
        {
            // Arrange
            const string UriString = "http://localhost";

            Func<Stream, string, Task<Stream>> uploadBlob =
                (s, stream) => Task.FromResult<Stream>(new MemoryStream());

            var message = new Message(Guid.NewGuid(), "en-GB", new Link { Target = UriString });

            Exception exception = null;

            // Act
            try
            {
                await WriteBlobCommand.Execute(uploadBlob, message, new MemoryStream());
            }
            catch (Exception ex)
            {
                exception = ex;
            }

            // Assert
            Assert.IsInstanceOfType(exception, typeof(InvalidOperationException));
        }

        /// <summary>
        /// Ensures that the filename for the blob is created correctly from the URI in the <see cref="Message"/>.
        /// </summary>
        /// <returns>A <see cref="Task"/> the represents the asynchronous operation.</returns>
        [TestMethod]
        public async Task WriteBlobCommandBuildsACorrectlyFormattedFilename()
        {
            // Arrange
            const string UriString = "http://localhost/0001-01-01/file.txt";

            Func<string, Stream, Task<Stream>> uploadBlob = (s, stream) =>
            {
                // Assert
                Assert.AreEqual("0001-01-01/file.txt", s);

                return Task.FromResult(stream);
            };

            var message = new Message(Guid.NewGuid(), "en-GB", new Link { Target = UriString });

            // Act
            await DownloadFileCommand.Execute(uploadBlob, message, new MemoryStream());
        }
    }
}
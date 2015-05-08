// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="WriteBlobCommand.cs" company="Rakuten">
//   Copyright (c) Rakuten. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.WebJob
{
    using System;
    using System.Diagnostics.Contracts;
    using System.IO;
    using System.Threading.Tasks;

    using Microsoft.WindowsAzure.Storage.Blob;

    /// <summary>
    /// The write blob command.
    /// </summary>
    internal class WriteBlobCommand
    {
        /// <summary>
        /// Writes the contents of the <see cref="Stream"/> into a <see cref="CloudBlockBlob"/>.
        /// </summary>
        /// <param name="container">The container to which the blob should be written.</param>
        /// <param name="message">An instance representing the queue message to be processed.</param>
        /// <param name="stream">The <see cref="Stream"/> containing the contents to be written.</param>
        /// <returns>A <see cref="Task"/> the represents the asynchronous operation.</returns>
        public static async Task Execute(CloudBlobContainer container, Message message, Stream stream)
        {
            Contract.Requires(container != null);
            Contract.Requires(message != null);
            Contract.Requires(stream != null);

            var enclosure = new Uri(message.Link.Target);

            if (enclosure.Segments.Length < 3)
            {
                throw new InvalidOperationException("The given blob reference was not of the correct format.");
            }

            // Parse the blob name from the URI.
            var filename = string.Format(
                "{0}/{1}",
                enclosure.Segments[enclosure.Segments.Length - 2],
                enclosure.Segments[enclosure.Segments.Length - 1]);

            // Get a reference to the blob in the container.
            CloudBlockBlob blob = container.GetBlockBlobReference(filename);

            await blob.UploadFromStreamAsync(stream);
        }
    }
}
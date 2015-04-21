// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="DownloadFileCommand.cs" company="Rakuten">
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
    /// Reads the Cloud Blob from the specified container identified by the URI to the given stream.
    /// </summary>
    internal class DownloadFileCommand
    {
        /// <summary>
        /// Reads a Cloud Blob identified by the URI from the given container to a <see cref="Stream"/>.
        /// </summary>
        /// <param name="container">The container that holds the specified Blob.</param>
        /// <param name="uri">A <see cref="Uri"/> identifying the Blob.</param>
        /// <param name="stream">The <see cref="Stream"/> to which the contents of the Blob should be read.</param>
        /// <returns>A <see cref="Task"/> the represents the asynchronous operation.</returns>
        public static async Task<Stream> Execute(CloudBlobContainer container, Uri uri, Stream stream)
        {
            Contract.Requires(container != null);
            Contract.Requires(uri != null);
            Contract.Requires(stream != null);

            // Parse the blob name from the URI.
            var filename = uri.Segments[uri.Segments.Length - 1];

            // Get a reference to the blob in the container.
            CloudBlockBlob blob = container.GetBlockBlobReference(filename);

            // Ensure the blob exists.
            if (!blob.Exists())
            {
                throw new InvalidOperationException("The specified blob was not found within the given container.");
            }

            await blob.DownloadToStreamAsync(stream);

            return stream;
        }
    }
}
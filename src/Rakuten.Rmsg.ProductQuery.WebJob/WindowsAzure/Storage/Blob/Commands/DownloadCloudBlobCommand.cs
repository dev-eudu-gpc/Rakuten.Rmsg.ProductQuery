// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="DownloadCloudBlobCommand.cs" company="Rakuten">
//   Copyright (c) Rakuten. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.WebJob
{
    using System;
    using System.IO;
    using System.Threading.Tasks;

    using Microsoft.WindowsAzure.Storage.Blob;

    /// <summary>
    /// Downloads the contents of a <see cref="CloudBlockBlob"/> to a <see cref="Stream"/>.
    /// </summary>
    internal class DownloadCloudBlobCommand
    {
        /// <summary>
        /// Downloads the specified file from the given container to the given <see cref="Stream"/>.
        /// </summary>
        /// <param name="container">The container in which the specified blob is located.</param>
        /// <param name="filename">A string containing the name of the blob.</param>
        /// <param name="stream">The <see cref="Stream"/> to which the file will be downloaded.</param>
        /// <returns>
        /// A <see cref="Task"/> the represents the asynchronous operation where the task result will be the stream 
        /// containing the blob contents.
        /// </returns>
        public static async Task<Stream> Execute(CloudBlobContainer container, string filename, Stream stream)
        {
            // Get a reference to the blob in the container.
            CloudBlockBlob blob = container.GetBlockBlobReference(filename);

            // Ensure the blob exists.
            if (!blob.Exists())
            {
                throw new InvalidOperationException("The specified blob was not found within the given container.");
            }

            await blob.DownloadToStreamAsync(stream);

            stream.Seek(0, SeekOrigin.Begin);

            return stream;
        }
    }
}
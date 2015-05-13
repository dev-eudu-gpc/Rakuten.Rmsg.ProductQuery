// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="UploadCloudBlobCommand.cs" company="Rakuten">
//   Copyright (c) Rakuten. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.WebJob
{
    using System.IO;
    using System.Threading.Tasks;

    using Microsoft.WindowsAzure.Storage.Blob;

    /// <summary>
    /// Writes the given <see cref="Stream"/> into a <see cref="CloudBlockBlob"/>.
    /// </summary>
    internal class UploadCloudBlobCommand
    {
        /// <summary>
        /// Uploads the content from the given stream into a blob contained within the given container.
        /// </summary>
        /// <param name="container">The container in which the specified blob is located.</param>
        /// <param name="stream">The <see cref="Stream"/> to which the content will be written.</param>
        /// <param name="filename">A string containing the name of the blob.</param>
        /// <returns>A <see cref="Task"/> the represents the asynchronous operation.</returns>
        public static async Task Execute(CloudBlobContainer container, Stream stream, string filename)
        {
            // Get a reference to the blob in the container.
            CloudBlockBlob blob = container.GetBlockBlobReference(filename);

            await blob.UploadFromStreamAsync(stream);
        }
    }
}
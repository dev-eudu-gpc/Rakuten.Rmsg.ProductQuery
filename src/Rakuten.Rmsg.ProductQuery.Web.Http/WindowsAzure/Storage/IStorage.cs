//---------------------------------------------------------------------------------------------------------------------
// <copyright file="IStorage.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//---------------------------------------------------------------------------------------------------------------------
namespace Rakuten.WindowsAzure.Storage
{
    using System;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines an object that interacts with storage.
    /// </summary>
    internal interface IStorage
    {
        /// <summary>
        /// Asynchronously downloads the content as a string of the specified Blob within a 
        /// <see cref="Microsoft.WindowsAzure.Storage.Blob.CloudBlobContainer"/> instance with the given name.
        /// </summary>
        /// <param name="connectionString">
        /// A valid connection string.
        /// </param>
        /// <param name="containerName">
        /// A string containing the name of the container.
        /// </param>
        /// <param name="blobName">
        /// A string containing the name of the Blob.
        /// </param>
        /// <param name="fileEncoding">
        /// The encoding with which to read in the file
        /// </param>
        /// <returns>
        /// The specified Blob's content as a string.
        /// </returns>
        Task<string> DownloadCloudBlockBlobTextAsync(string connectionString, string containerName, string blobName, Encoding fileEncoding);

        /// <summary>
        /// Opens an IO stream to an Azure block blob file in order to read
        /// the first 4 bytes of the file which can be used to determine the
        /// files encoding
        /// </summary>
        /// <param name="connectionString">
        /// A valid connection string.
        /// </param>
        /// <param name="containerName">
        /// A string containing the name of the container.
        /// </param>
        /// <param name="blobName">
        /// A string containing the name of the Blob.
        /// </param>
        /// <returns>
        /// The specified Blob's encoding.
        /// </returns>
        Encoding DetermineCloudBlockBlobEncoding(string connectionString, string containerName, string blobName);

        /// <summary>
        /// Generates the shard access signature for the specified Blob within a 
        /// <see cref="Microsoft.WindowsAzure.Storage.Blob.CloudBlobContainer"/> instance with the given name.
        /// </summary>
        /// <param name="connectionString">
        /// A valid connection string.
        /// </param>
        /// <param name="containerName">
        /// A string containing the name of the container.
        /// </param>
        /// <param name="blobName">
        /// A string containing the name of the Blob.
        /// </param>
        /// <returns>
        /// An endpoint at which the Blob can be accessed using a shared access signature.
        /// </returns>
        Uri GetSharedAccessSignature(string connectionString, string containerName, string blobName);
    }
}
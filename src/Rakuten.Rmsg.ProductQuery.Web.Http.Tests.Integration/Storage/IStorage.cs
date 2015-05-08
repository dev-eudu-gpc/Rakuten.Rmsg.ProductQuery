//---------------------------------------------------------------------------------------------------------------------
// <copyright file="IStorage.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//---------------------------------------------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.Web.Http.Tests.Integration
{
    using System;

    /// <summary>
    /// Defines an object that interacts with storage.
    /// </summary>
    public interface IStorage
    {
        /// <summary>
        /// Generates the shard access signature for the specified Blob within a 
        /// <see cref="Microsoft.WindowsAzure.Storage.Blob.CloudBlobContainer"/> instance with the given name.
        /// </summary>
        /// <param name="connectionString">A valid connection string.</param>
        /// <param name="containerName">A string containing the name of the container.</param>
        /// <param name="blobName">A string containing the name of the Blob.</param>
        /// <returns>An endpoint at which the Blob can be accessed using a shared access signature.</returns>
        Uri GetSharedAccessSignature(string connectionString, string containerName, string blobName);
    }
}
//---------------------------------------------------------------------------------------------------------------------
// <copyright file="AzureStorage.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//---------------------------------------------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.Web.Http.Tests.Integration
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using Microsoft.WindowsAzure.Storage;
    using Microsoft.WindowsAzure.Storage.Blob;
    using Microsoft.WindowsAzure.Storage.Shared.Protocol;

    /// <summary>
    /// Represents an implementation that interacts with storage on Microsoft Azure.
    /// </summary>
    internal class AzureStorage : IStorage
    {
        /// <summary>
        /// The name to give to the shared access policy on the container.
        /// </summary>
        private const string PolicyName = "uploads_policy";

        /// <summary>
        /// The headers that are to be allowed on a CORS request. 
        /// </summary>
        private static readonly IList<string> AllowedHeaders = new List<string> { "*" };

        /// <summary>
        /// The domain names to be allowed via CORS. 
        /// </summary>
        private static readonly IList<string> AllowedOrigins = new List<string> { "*" };

        /// <summary>
        /// The response headers that should be exposed to the client via CORS. 
        /// </summary>
        private static readonly IList<string> ExposedHeaders = new List<string> { "*" };

        /// <summary>
        /// Determines whether a blob exists for a given product query.
        /// </summary>
        /// <param name="connectionString">The connection string for storage.</param>
        /// <param name="containerName">The name of the container in which the blob should be found.</param>
        /// <param name="productQueryId">The identifier of the product query.</param>
        /// <param name="dateCreated">The date/time on which the product query was created.</param>
        public void BlobExists(
            string connectionString,
            string containerName,
            Guid productQueryId,
            DateTime dateCreated)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(connectionString));
            Contract.Requires(!string.IsNullOrWhiteSpace(containerName));

            CloudBlobClient blobClient = GetCloudBlobClient(connectionString);
            CloudBlobContainer blobContainer = GetCloudBlobContainer(blobClient, containerName);
            var list = blobContainer.ListBlobs();
            var c = list.Count();
        }

        /// <summary>
        /// Generates the shard access signature for the specified Blob within a 
        /// <see cref="Microsoft.WindowsAzure.Storage.Blob.CloudBlobContainer"/> instance with the given name.
        /// </summary>
        /// <param name="connectionString">A valid connection string.</param>
        /// <param name="containerName">A string containing the name of the container.</param>
        /// <param name="blobName">A string containing the name of the Blob.</param>
        /// <returns>An endpoint at which the Blob can be accessed using a shared access signature.</returns>
        public Uri GetSharedAccessSignature(string connectionString, string containerName, string blobName)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentException("connectionString");
            }

            if (string.IsNullOrWhiteSpace(containerName))
            {
                throw new ArgumentException("containerName");
            }

            if (string.IsNullOrWhiteSpace(blobName))
            {
                throw new ArgumentException("blobName");
            }

            CloudBlobClient blobClient = GetCloudBlobClient(connectionString);
            ConfigureCloudBlobClient(blobClient);

            CloudBlobContainer blobContainer = GetCloudBlobContainer(blobClient, containerName);

            DateTimeOffset expiryTime = DateTime.UtcNow.AddHours(2);

            return GetSharedAccessSignature(blobContainer, blobName);
        }

        /// <summary>
        /// Instantiates a new BLOB service client.
        /// </summary>
        /// <param name="connectionString">A valid connection string.</param>
        /// <returns> A BLOB service client.</returns>
        private static CloudBlobClient GetCloudBlobClient(string connectionString)
        {
            return CloudStorageAccount.Parse(connectionString).CreateCloudBlobClient();
        }

        /// <summary>
        /// Instantiates a new <see cref="CloudBlobContainer"/> instance.
        /// </summary>
        /// <param name="client">A <see cref="CloudBlobClient"/> instance.</param>
        /// <param name="containerName">A string containing the name of the container.</param>
        /// <returns>A new <see cref="CloudBlobContainer"/> instance with the specified name.</returns>
        private static CloudBlobContainer GetCloudBlobContainer(CloudBlobClient client, string containerName)
        {
            var container = client.GetContainerReference(containerName);
            container.CreateIfNotExists();

            return container;
        }

        /// <summary>
        /// Instantiates a new <see cref="CloudBlobContainer"/> instance configured with CORS support.
        /// </summary>
        /// <param name="client">A <see cref="CloudBlobClient"/> instance.</param>
        private static void ConfigureCloudBlobClient(CloudBlobClient client)
        {
            var serviceProperties = new ServiceProperties
            {
                Cors = new CorsProperties(),
                HourMetrics = null,
                MinuteMetrics = null,
                Logging = null,
            };

            serviceProperties.Cors.CorsRules.Add(new CorsRule
            {
                AllowedHeaders = AllowedHeaders,
                AllowedMethods = CorsHttpMethods.Put,
                AllowedOrigins = AllowedOrigins,
                ExposedHeaders = ExposedHeaders,
                MaxAgeInSeconds = 1800
            });

            client.SetServiceProperties(serviceProperties);
        }

        /// <summary>
        /// Generates the shard access signature for the specified BLOB within the the specified container.
        /// </summary>
        /// <param name="container">The container within which the BLOB should exist.</param>
        /// <param name="blobName">The name of the BLOB within the specified container.</param>
        /// <returns>The endpoint of the BLOB combined with the shared access signature.</returns>
        private static Uri GetSharedAccessSignature(CloudBlobContainer container, string blobName)
        {
            // Get a reference to a blob within the container.
            CloudBlockBlob blob = container.GetBlockBlobReference(blobName);

            // Generate the shared access signature on the blob.
            string signature = blob.GetSharedAccessSignature(null, PolicyName);
            Contract.Assume(signature != null && signature.Length > 1);

            // Build the URI, stripping off the leading character (a ?) from the SAS.
            return new UriBuilder(blob.Uri) { Query = signature.Substring(1) }.Uri;
        }
    }
}
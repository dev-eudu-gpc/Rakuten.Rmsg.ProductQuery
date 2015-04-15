//---------------------------------------------------------------------------------------------------------------------
// <copyright file="AzureStorage.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//---------------------------------------------------------------------------------------------------------------------
namespace Rakuten.WindowsAzure.Storage
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.WindowsAzure.Storage;
    using Microsoft.WindowsAzure.Storage.Blob;
    using Microsoft.WindowsAzure.Storage.Shared.Protocol;
    using Rakuten.Gpc;
    using Rakuten.Gpc.Api;

    /// <summary>
    /// Represents an implementation that interacts with storage on Microsoft Azure.
    /// </summary>
    public class AzureStorage : IStorage
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
        public Task<string> DownloadCloudBlockBlobTextAsync(
            string connectionString, 
            string containerName, 
            string blobName,
            Encoding fileEncoding)
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

            CloudBlobContainer blobContainer = GetCloudBlobContainer(blobClient, containerName);

            CloudBlockBlob blockBlob = blobContainer.GetBlockBlobReference(blobName);

            // Inform the client that the specified upload was not found.
            if (!blockBlob.Exists())
            {
                // TODO: [MM, 23/10] Should this be an "internal" exception?
                throw new ObjectNotFoundException(new ProductUploadNotFoundException(blobName));
            }

            return DownloadTextAsync(blockBlob, fileEncoding);
        }

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
        public Encoding DetermineCloudBlockBlobEncoding(
            string connectionString,
            string containerName,
            string blobName)
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

            CloudBlobContainer blobContainer = GetCloudBlobContainer(blobClient, containerName);

            CloudBlockBlob blockBlob = blobContainer.GetBlockBlobReference(blobName);

            // Inform the client that the specified upload was not found.
            if (!blockBlob.Exists())
            {
                // TODO: [MM, 23/10] Should this be an "internal" exception?
                throw new ObjectNotFoundException(new ProductUploadNotFoundException(blobName));
            }

            // Read the BOM
            var bom = new byte[4];

            using (var file = blockBlob.OpenRead())
            {
                file.Read(bom, 0, 4);
            }

            return GetEncoding(bom);
        }

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

            AddSharedAccessPolicy(expiryTime, SharedAccessBlobPermissions.Write, blobContainer);

            return GetSharedAccessSignature(blobContainer, blobName);
        }

        /// <summary>
        /// Adds the specified <see cref="Microsoft.WindowsAzure.Storage.Blob.SharedAccessBlobPolicy"/> to the 
        /// collection of shared access policies on the specified 
        /// <see cref="Microsoft.WindowsAzure.Storage.Blob.CloudBlobContainer"/>.
        /// </summary>
        /// <param name="expiryTime">
        /// The expiry time for a shared access signature associated with this shared access policy.
        /// </param>
        /// <param name="permissions">
        /// The permissions for a shared access signature associated with this shared access policy.
        /// </param>
        /// <param name="container">
        /// The <see cref="CloudBlobContainer"/> instance to which the shared access policy should be applied.
        /// </param>
        private static void AddSharedAccessPolicy(
            DateTimeOffset expiryTime, 
            SharedAccessBlobPermissions permissions,
            CloudBlobContainer container)
        {
            // Create a new stored access policy specify its period of life and the permissions available.
            var policy = new SharedAccessBlobPolicy
            {
                SharedAccessExpiryTime = expiryTime,
                Permissions = permissions
            };

            // Set the container's existing permissions.
            var containerPermissions = new BlobContainerPermissions
            {
                PublicAccess = BlobContainerPublicAccessType.Blob
            };

            // Add the new policy to the container's permissions.
            containerPermissions.SharedAccessPolicies.Clear();

            //// permissions.PublicAccess = BlobContainerPublicAccessType.Off;
            containerPermissions.SharedAccessPolicies.Add(PolicyName, policy);

            container.SetPermissions(containerPermissions);
        }

        /// <summary>
        /// Initiates an asynchronous operation to download a BLOB's contents as a string.
        /// </summary>
        /// <param name="blob">The BLOB to download.</param>
        /// <param name="encoding">An object that indicates the text encoding to use.</param>
        /// <returns>
        /// A <see cref="Task{T}"/> object of type <see cref="String"/> that represents the asynchronous operation.
        /// </returns>
        private static Task<string> DownloadTextAsync(CloudBlockBlob blob, Encoding encoding)
        {
            return blob.DownloadTextAsync(encoding, accessCondition: null, options: null, operationContext: null);
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

        /// <summary>
        /// Determines a text file's encoding by analyzing its byte order mark (BOM).
        /// Defaults to ASCII when detection of the text file's end fails.
        /// </summary>
        /// <param name="bom">The bytes to analyze to determine the encoding of a file.</param>
        /// <returns>The detected encoding.</returns>
        private static Encoding GetEncoding(byte[] bom)
        {
            // ensure the BOM has the expected shape
            if (bom.Length == 4)
            {
                // Analyze the BOM and return the detected encoding
                if (bom[0] == 0x2b && bom[1] == 0x2f && bom[2] == 0x76)
                {
                    return Encoding.UTF7;
                }

                if (bom[0] == 0xef && bom[1] == 0xbb && bom[2] == 0xbf)
                {
                    return Encoding.UTF8;
                }

                if (bom[0] == 0xff && bom[1] == 0xfe)
                {
                    return Encoding.Unicode;
                }

                if (bom[0] == 0xfe && bom[1] == 0xff)
                {
                    return Encoding.BigEndianUnicode;
                }

                if (bom[0] == 0 && bom[1] == 0 && bom[2] == 0xfe && bom[3] == 0xff)
                {
                    return Encoding.UTF32;
                }
            }

            // return the default
            return Encoding.ASCII;
        }
    }
}
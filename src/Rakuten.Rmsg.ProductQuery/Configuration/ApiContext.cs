//----------------------------------------------------------------------------------------------------------------------
// <copyright file="ApiContext.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.Configuration
{
    using System.Diagnostics.Contracts;

    /// <summary>
    /// The current set of conditions under which this instance should operate.
    /// </summary>
    internal class ApiContext : IApiContext
    {
        /// <summary>
        /// The name of the blob container.
        /// </summary>
        private readonly string blobContainerName;

        /// <summary>
        /// The mask for the name of the file in blob storage.
        /// </summary>
        private readonly string blobFileNameMask;

        /// <summary>
        /// The connection string for the database.
        /// </summary>
        private readonly string databaseConnectionString;

        /// <summary>
        /// The connection string for the diagnostics storage account.
        /// </summary>
        private readonly string diagnosticsStorageConnectionString;

        /// <summary>
        /// The environment in which this GPC is running.
        /// </summary>
        private readonly string environmentName;

        /// <summary>
        /// The geographical region in which this GPC instance is running.
        /// </summary>
        private readonly string region;

        /// <summary>
        /// The maximum number of queries per query group
        /// </summary>
        private readonly int maximumQueriesPerGroup;

        /// <summary>
        /// The connection string for the storage account.
        /// </summary>
        private readonly string storageConnectionString;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiContext"/> class.
        /// </summary>
        /// <param name="blobContainerName">The name of the blob container.</param>
        /// <param name="blobFileNameMask">The mask for the name of the file in blob storage</param>
        /// <param name="databaseConnectionString">The connection string for the database.</param>
        /// <param name="environmentName">The environment in which the application is running.</param>
        /// <param name="maximumQueriesPerGroup">The maximum number of queries per query group.</param>
        /// <param name="region">The geographical region in which the application is running.</param>
        /// <param name="storageConnectionString">The connection string for the storage account.</param>
        public ApiContext(
            string blobContainerName,
            string blobFileNameMask,
            string databaseConnectionString,
            string diagnosticsStorageConnectionString,
            string environmentName,
            int maximumQueriesPerGroup,
            string region,
            string storageConnectionString)
        {
            Contract.Requires(blobContainerName != null);
            Contract.Requires(blobFileNameMask != null);
            Contract.Requires(databaseConnectionString != null);
            Contract.Requires(diagnosticsStorageConnectionString != null);
            Contract.Requires(environmentName != null);
            Contract.Requires(region != null);
            Contract.Requires(storageConnectionString != null);

            this.blobContainerName = blobContainerName;
            this.blobFileNameMask = blobFileNameMask;
            this.databaseConnectionString = databaseConnectionString;
            this.diagnosticsStorageConnectionString = diagnosticsStorageConnectionString;
            this.environmentName = environmentName;
            this.maximumQueriesPerGroup = maximumQueriesPerGroup;
            this.region = region;
            this.storageConnectionString = storageConnectionString;
        }

        /// <summary>
        /// Gets the blob container name.
        /// </summary>
        public string BlobContainerName
        {
            get
            {
                return this.blobContainerName;
            }
        }

        /// <summary>
        /// Gets the mask for the name of the file in blob storage
        /// </summary>
        public string BlobFileNameMask 
        {
            get { return this.blobFileNameMask; }
        }

        /// <summary>
        /// Gets the connection string for the database.
        /// </summary>
        public string DatabaseConnectionString
        {
            get { return this.databaseConnectionString; }
        }

        /// <summary>
        /// Gets the connection string to the storage account where diagnostics information should be written.
        /// </summary>
        public string DiagnosticsStorageConnectionString
        {
            get
            {
                return this.diagnosticsStorageConnectionString;
            }
        }

        /// <summary>
        /// Gets the environment in which this GPC is running.
        /// </summary>
        public string EnvironmentName
        {
            get { return this.environmentName; }
        }

        /// <summary>
        /// Gets the maximum number of queries per query group
        /// </summary>
        public int MaximumQueriesPerGroup
        {
            get { return this.maximumQueriesPerGroup; }
        }

        /// <summary>
        /// Gets the geographical region in which this GPC instance is running.
        /// </summary>
        public string Region
        {
            get { return this.region; }
        }

        /// <summary>
        /// Gets the connection string to the storage account.
        /// </summary>
        public string StorageConnectionString
        {
            get { return this.storageConnectionString; }
        }
    }
}
//----------------------------------------------------------------------------------------------------------------------
// <copyright file="ApiContext.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.Configuration
{
    using System;
    using System.Diagnostics.Contracts;

    /// <summary>
    /// The current set of conditions under which this instance should operate.
    /// </summary>
    internal class ApiContext : IApiContext
    {
        /// <summary>
        /// The authentication token to be supplied when making requests.
        /// </summary>
        private readonly string authenticationToken;

        /// <summary>
        /// The base address to be used when sending requests.
        /// </summary>
        private readonly Uri baseAddress;

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
        /// The number of seconds between progress maps
        /// </summary>
        private readonly int progressMapIntervalInSeconds;

        /// <summary>
        /// The estimated proportion of product query processing that is used by the finalization process.
        /// </summary>
        private readonly decimal proportionOfTimeAllocatedForFinalization;

        /// <summary>
        /// The geographical region in which this GPC instance is running.
        /// </summary>
        private readonly string region;

        /// <summary>
        /// The maximum number of queries per query group
        /// </summary>
        private readonly int maximumQueriesPerGroup;

        /// <summary>
        /// The connection string to the service bus.
        /// </summary>
        private readonly string serviceBusConnectionString;

        /// <summary>
        /// The connection string for the storage account.
        /// </summary>
        private readonly string storageConnectionString;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiContext"/> class.
        /// </summary>
        /// <param name="authenticationToken">The authentication token to be used when making requests.</param>
        /// <param name="baseAddress">The address to which requests should be made.</param>
        /// <param name="blobContainerName">The name of the blob container.</param>
        /// <param name="blobFileNameMask">The mask for the name of the file in blob storage</param>
        /// <param name="databaseConnectionString">The connection string for the database.</param>
        /// <param name="diagnosticsStorageConnectionString">
        /// The connection string to the diagnostics storage account.
        /// </param>
        /// <param name="environmentName">The environment in which the application is running.</param>
        /// <param name="maximumQueriesPerGroup">The maximum number of queries per query group.</param>
        /// <param name="progressMapIntervalInSeconds">The number of seconds between progress maps.</param>
        /// <param name="proportionOfTimeAllocatedForFinalization">
        /// The estimated proportion of product query processing that is used by the finalization process.
        /// </param>
        /// <param name="region">The geographical region in which the application is running.</param>
        /// <param name="serviceBusConnectionString">The connection string to the service bus.</param>
        /// <param name="storageConnectionString">The connection string for the storage account.</param>
        public ApiContext(
            string authenticationToken,
            Uri baseAddress,
            string blobContainerName,
            string blobFileNameMask,
            string databaseConnectionString,
            string diagnosticsStorageConnectionString,
            string environmentName,
            int maximumQueriesPerGroup,
            int progressMapIntervalInSeconds,
            decimal proportionOfTimeAllocatedForFinalization,
            string region,
            string serviceBusConnectionString,
            string storageConnectionString)
        {
            Contract.Requires(authenticationToken != null);
            Contract.Requires(baseAddress != null);
            Contract.Requires(blobContainerName != null);
            Contract.Requires(blobFileNameMask != null);
            Contract.Requires(databaseConnectionString != null);
            Contract.Requires(diagnosticsStorageConnectionString != null);
            Contract.Requires(environmentName != null);
            Contract.Requires(region != null);
            Contract.Requires(serviceBusConnectionString != null);
            Contract.Requires(storageConnectionString != null);

            this.authenticationToken = authenticationToken;
            this.baseAddress = baseAddress;
            this.blobContainerName = blobContainerName;
            this.blobFileNameMask = blobFileNameMask;
            this.databaseConnectionString = databaseConnectionString;
            this.diagnosticsStorageConnectionString = diagnosticsStorageConnectionString;
            this.environmentName = environmentName;
            this.maximumQueriesPerGroup = maximumQueriesPerGroup;
            this.progressMapIntervalInSeconds = progressMapIntervalInSeconds;
            this.proportionOfTimeAllocatedForFinalization = proportionOfTimeAllocatedForFinalization;
            this.region = region;
            this.serviceBusConnectionString = serviceBusConnectionString;
            this.storageConnectionString = storageConnectionString;
        }

        /// <summary>
        /// Gets the authentication token to be used when making requests to the external API.
        /// </summary>
        public string AuthenticationToken
        {
            get
            {
                return this.authenticationToken;
            }
        }

        /// <summary>
        /// Gets the base address of the Internet resource when sending requests.
        /// </summary>
        public Uri BaseAddress
        {
            get
            {
                return this.baseAddress;
            }
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
        /// Gets the number of seconds between progress maps
        /// </summary>
        public int ProgressMapIntervalInSeconds 
        {
            get { return this.progressMapIntervalInSeconds; }
        }

        /// <summary>
        /// Gets the estimated proportion of product query processing that is used by the finalisation process.
        /// </summary>
        public decimal ProportionOfTimeAllocatedForFinalization 
        {
            get { return this.proportionOfTimeAllocatedForFinalization; }
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
        /// Gets the connection string to the Service Bus in which a queue will be utilized.
        /// </summary>
        public string ServiceBusConnectionString
        {
            get { return this.serviceBusConnectionString; }
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
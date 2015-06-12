//----------------------------------------------------------------------------------------------------------------------
// <copyright file="IApiContext.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.Configuration
{
    using System;

    /// <summary>
    /// Provides configuration settings for the context in which the application is operating
    /// </summary>
    public interface IApiContext
    {
        /// <summary>
        /// Gets the authentication token to be used when making requests to the external API.
        /// </summary>
        string AuthenticationToken { get; }

        /// <summary>
        /// Gets the blob container name.
        /// </summary>
        string BlobContainerName { get; }

        /// <summary>
        /// Gets the mask for file names in blob storage
        /// </summary>
        string BlobFileNameMask { get; }

        /// <summary>
        /// Gets the connection string for the database.
        /// </summary>
        string DatabaseConnectionString { get; }

        /// <summary>
        /// Gets the connection string to the storage account where diagnostics information should be written.
        /// </summary>
        string DiagnosticsStorageConnectionString { get; }

        /// <summary>
        /// Gets the environment in which this GPC is running.
        /// </summary>
        string EnvironmentName { get; }

        /// <summary>
        /// Gets the base address of the GPC core API.
        /// </summary>
        Uri GpcCoreApiBaseAddress { get; }

        /// <summary>
        /// Gets the maximum number of queries per query group
        /// </summary>
        int MaximumQueriesPerGroup { get; }

        /// <summary>
        /// Gets the name of the queue.
        /// </summary>
        string MessageQueueName { get; }

        /// <summary>
        /// Gets the base address of the product query API.
        /// </summary>
        Uri ProductQueryApiBaseAddress { get; }

        /// <summary>
        /// Gets the number of seconds between progress maps
        /// </summary>
        int ProgressMapIntervalInSeconds { get; }

        /// <summary>
        /// Gets the estimated proportion of product query processing that is used by the finalization process.
        /// </summary>
        decimal ProportionOfTimeAllocatedForFinalization { get; }

        /// <summary>
        /// Gets the geographical region in which this GPC instance is running.
        /// </summary>
        string Region { get; }

        /// <summary>
        /// Gets the connection string to the Service Bus in which a queue will be utilized.
        /// </summary>
        string ServiceBusConnectionString { get; }

        /// <summary>
        /// Gets a connection string for a storage account that can be used for persisting configuration information.
        /// </summary>
        string StorageConnectionString { get; }
    }
}
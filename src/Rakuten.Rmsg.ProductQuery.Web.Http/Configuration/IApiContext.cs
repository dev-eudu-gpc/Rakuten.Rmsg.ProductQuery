//----------------------------------------------------------------------------------------------------------------------
// <copyright file="IApiContext.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.Web.Http.Configuration
{
    /// <summary>
    /// Provides configuration settings for the context in which the application is operating
    /// </summary>
    public interface IApiContext
    {
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
        /// Gets the environment in which this GPC is running.
        /// </summary>
        string EnvironmentName { get; }

        /// <summary>
        /// Gets the maximum number of queries per query group
        /// </summary>
        int MaximumQueriesPerGroup { get; }

        /// <summary>
        /// Gets the geographical region in which this GPC instance is running.
        /// </summary>
        string Region { get; }

        /// <summary>
        /// Gets a connection string for a storage account that can be used for persisting configuration information.
        /// </summary>
        string StorageConnectionString { get; }
    }
}
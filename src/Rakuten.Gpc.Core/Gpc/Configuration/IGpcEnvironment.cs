//----------------------------------------------------------------------------------------------------------------------
// <copyright file="IGpcEnvironment.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------------------------------------------------
namespace Rakuten.Gpc.Configuration
{
    using System;

    /// <summary>
    /// Provides environmental settings for GPC.
    /// </summary>
    public interface IGpcEnvironment
    {
        /// <summary>
        /// Gets the unique identifier of the UI API within Active Directory.
        /// </summary>
        string ApiAADApplicationId { get; }

        /// <summary>
        /// Gets the uniquely identifying URI of the API application in Active Directory.
        /// </summary>
        Uri ApiAADApplicationIdUri { get; }

        /// <summary>
        /// Gets the base URI for core API requests.
        /// </summary>
        Uri CoreApiBaseAddress { get; }

        /// <summary>
        /// Gets the credentials to use in the authorization header when connecting to GPC services.
        /// </summary>
        string Credentials { get; }

        /// <summary>
        /// Gets the connection string for the database.
        /// </summary>
        string DatabaseConnectionString { get; }

        /// <summary>
        /// Gets the username (email) of the Active Directory administrator.
        /// </summary>
        string DirectoryAdministrator { get; }

        /// <summary>
        /// Gets the password of the Active Directory administrator.
        /// </summary>
        string DirectoryAdministratorPassword { get; }

        /// <summary>
        /// Gets the authority of the Active Directory instance.
        /// </summary>
        string DirectoryAuthority { get; }

        /// <summary>
        /// Gets the domain of the Active Directory instance.
        /// </summary>
        string DirectoryTenant { get; }

        /// <summary>
        /// Gets the environment in which this GPC is running.
        /// </summary>
        string EnvironmentName { get; }

        /// <summary>
        /// Gets the host name for the GPC services.
        /// </summary>
        string Host { get; }

        /// <summary>
        /// Gets the IP addresses that are authorized to access GPC when IP address restrictions are in force.
        /// </summary>
        string IPAddressAddressRestrictions { get; }

        /// <summary>
        /// Gets a value indicating whether IP address restrictions should be applied to this GPC instance.
        /// </summary>
        bool IPAddressAddressRestrictionsEnabled { get; }

        /// <summary>
        /// Gets the geographical region in which this GPC instance is running.
        /// </summary>
        string Region { get; }

        /// <summary>
        /// Gets a connection string for a storage account that can be used for persisting configuration information.
        /// </summary>
        string StorageConnectionString { get; }

        /// <summary>
        /// Gets the base URI for UI API requests.
        /// </summary>
        Uri UIApiBaseAddress { get; }

        /// <summary>
        /// Gets the base URI for the GPC UI.
        /// </summary>
        Uri UIBaseAddress { get; }

        /// <summary>
        /// Gets the unique identifier of the web application within Active Directory.
        /// </summary>
        string WebAADApplicationId { get; }
    }
}
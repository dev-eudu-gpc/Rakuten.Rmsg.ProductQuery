//----------------------------------------------------------------------------------------------------------------------
// <copyright file="GpcEnvironment.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------------------------------------------------
namespace Rakuten.Gpc.Configuration
{
    using System;
    using System.Diagnostics.Contracts;

    /// <summary>
    /// Provides environmental settings for GPC.
    /// </summary>
    public class GpcEnvironment : IGpcEnvironment
    {
        /// <summary>
        /// Gets the unique identifier of the UI API within Active Directory.
        /// </summary>
        private readonly string apiAADApplicationId;

        /// <summary>
        /// Gets the identifying URI of the API application in Active Directory.
        /// </summary>
        private readonly Uri apiApplicationIdUri;

        /// <summary>
        /// The base URI for core API requests.
        /// </summary>
        private readonly Uri coreApiBaseAddress;

        /// <summary>
        /// The credentials to use in the authorization header when connecting to GPC services.
        /// </summary>
        private readonly string credentials;

        /// <summary>
        /// The connection string for the database.
        /// </summary>
        private readonly string databaseConnectionString;

        /// <summary>
        /// The username (email) of the Active Directory administrator.
        /// </summary>
        private readonly string directoryAdministrator;

        /// <summary>
        /// The password of the Active Directory administrator.
        /// </summary>
        private readonly string directoryAdministratorPassword;

        /// <summary>
        /// Gets the authority of the Active Directory instance.
        /// </summary>
        private readonly string directoryAuthority;

        /// <summary>
        /// The domain of the Active Directory instance.
        /// </summary>
        private readonly string directoryTenant;

        /// <summary>
        /// The environment in which this GPC is running.
        /// </summary>
        private readonly string environmentName;

        /// <summary>
        /// The host name for the GPC services.
        /// </summary>
        private readonly string host;

        /// <summary>
        /// The IP addresses that are authorized to access GPC when IP address restrictions are in force.
        /// </summary>
        private readonly string ipAddressRestrictions;

        /// <summary>
        /// Indicates whether IP address restrictions should be applied to this GPC instance.
        /// </summary>
        private readonly bool ipAddressRestrictionsEnabled;

        /// <summary>
        /// The geographical region in which this GPC instance is running.
        /// </summary>
        private readonly string region;

        /// <summary>
        /// The connection string for the storage account.
        /// </summary>
        private readonly string storageConnectionString;

        /// <summary>
        /// The base URI for UI API requests.
        /// </summary>
        private readonly Uri uiApiBaseAddress;

        /// <summary>
        /// The base URI for the GPC UI.
        /// </summary>
        private readonly Uri uiBaseAddress;

        /// <summary>
        /// The unique identifier of the web application within active directory.
        /// </summary>
        private readonly string webAADApplicationId;

        /// <summary>
        /// Initializes a new instance of the <see cref="GpcEnvironment"/> class.
        /// </summary>
        /// <param name="environmentName">The environment in which this GPC is running.</param>
        /// <param name="region">The geographical region in which this GPC instance is running.</param>
        /// <param name="host">The host name for the GPC services.</param>
        /// <param name="uiBaseAddress">The base URI for the GPC UI.</param>
        /// <param name="uiApiBaseAddress">The base URI for UI API requests.</param>
        /// <param name="coreApiBaseAddress">The base URI for core API requests.</param>
        /// <param name="ipAddressRestrictions">
        /// The IP addresses that are authorized to access GPC when IP address restrictions are in force.
        /// </param>
        /// <param name="ipAddressRestrictionsEnabled">
        /// Indicates whether IP address restrictions should be applied to this GPC instance.
        /// </param>
        /// <param name="credentials">
        /// The credentials to use in the authorization header when connecting to GPC services.
        /// </param>
        /// <param name="apiAADApplicationId">The ID of the API application within Active Directory.</param>
        /// <param name="apiApplicationIdUri">A URI identifying the API within Active Directory.</param>
        /// <param name="webAADApplicationId">The ID of the web application within Active Directory.</param>
        /// <param name="directoryAdministrator">The username (email) of the Active Directory administrator.</param>
        /// <param name="directoryAdministratorPassword">The password of the Active Directory administrator.</param>
        /// <param name="directoryAuthority">The authority of the Active Directory instance..</param>
        /// <param name="directoryTenant">The domain of the Active Directory instance.</param>
        /// <param name="databaseConnectionString">The connection string for the database.</param>
        /// <param name="storageConnectionString">The connection string for the storage account.</param>
        public GpcEnvironment(
            string environmentName,
            string region,
            string host,
            Uri uiBaseAddress,
            Uri uiApiBaseAddress,
            Uri coreApiBaseAddress,
            string ipAddressRestrictions,
            bool ipAddressRestrictionsEnabled,
            string credentials,
            string apiAADApplicationId,
            Uri apiApplicationIdUri,
            string webAADApplicationId,
            string directoryAdministrator,
            string directoryAdministratorPassword,
            string directoryAuthority,
            string directoryTenant,
            string databaseConnectionString,
            string storageConnectionString)
        {
            Contract.Requires(environmentName != null);
            Contract.Requires(region != null);
            Contract.Requires(host != null);
            Contract.Requires(uiBaseAddress != null);
            Contract.Requires(uiApiBaseAddress != null);
            Contract.Requires(coreApiBaseAddress != null);
            Contract.Requires(credentials != null);
            Contract.Requires(apiAADApplicationId != null);
            Contract.Requires(apiApplicationIdUri != null);
            Contract.Requires(
                webAADApplicationId != null,
                "webApplicationId cannot be null. Do you need to configure Azure Active Directory for this environment?");
            Contract.Requires(directoryAdministrator != null);
            Contract.Requires(directoryAdministratorPassword != null);
            Contract.Requires(directoryTenant != null);
            Contract.Requires(databaseConnectionString != null);
            Contract.Requires(storageConnectionString != null);

            this.coreApiBaseAddress = coreApiBaseAddress;
            this.credentials = credentials;
            this.databaseConnectionString = databaseConnectionString;
            this.directoryAdministrator = directoryAdministrator;
            this.directoryAdministratorPassword = directoryAdministratorPassword;
            this.directoryAuthority = directoryAuthority;
            this.directoryTenant = directoryTenant;
            this.apiAADApplicationId = apiAADApplicationId;
            this.apiApplicationIdUri = apiApplicationIdUri;
            this.webAADApplicationId = webAADApplicationId;
            this.environmentName = environmentName;
            this.host = host;
            this.ipAddressRestrictions = ipAddressRestrictions;
            this.ipAddressRestrictionsEnabled = ipAddressRestrictionsEnabled;
            this.region = region;
            this.storageConnectionString = storageConnectionString;
            this.uiApiBaseAddress = uiApiBaseAddress;
            this.uiBaseAddress = uiBaseAddress;
        }

        /// <summary>
        /// Gets the unique identifier of the UI API within Active Directory.
        /// </summary>
        public string ApiAADApplicationId
        {
            get { return this.apiAADApplicationId; }
        }

        /// <summary>
        /// Gets the uniquely identifying URI of the API application in Active Directory.
        /// </summary>
        public Uri ApiAADApplicationIdUri
        {
            get { return this.apiApplicationIdUri; }
        }

        /// <summary>
        /// Gets the base URI for core API requests.
        /// </summary>
        public Uri CoreApiBaseAddress
        {
            get { return this.coreApiBaseAddress; }
        }

        /// <summary>
        /// Gets the credentials to use in the authorization header when connecting to GPC services.
        /// </summary>
        public string Credentials
        {
            get { return this.credentials; }
        }

        /// <summary>
        /// Gets the connection string for the database.
        /// </summary>
        public string DatabaseConnectionString
        {
            get { return this.databaseConnectionString; }
        }

        /// <summary>
        /// Gets the username (email) of the Active Directory administrator.
        /// </summary>
        public string DirectoryAdministrator
        {
            get { return this.directoryAdministrator; }
        }

        /// <summary>
        /// Gets the password of the Active Directory administrator.
        /// </summary>
        public string DirectoryAdministratorPassword
        {
            get { return this.directoryAdministratorPassword; }
        }

        /// <summary>
        /// Gets the authority of the Active Directory instance.
        /// </summary>
        public string DirectoryAuthority
        {
            get { return this.directoryAuthority; }
        }

        /// <summary>
        /// Gets the domain of the Active Directory instance.
        /// </summary>
        public string DirectoryTenant
        {
            get { return this.directoryTenant; }
        }

        /// <summary>
        /// Gets the environment in which this GPC is running.
        /// </summary>
        public string EnvironmentName
        {
            get { return this.environmentName; }
        }

        /// <summary>
        /// Gets the host name for the GPC services.
        /// </summary>
        public string Host
        {
            get { return this.host; }
        }

        /// <summary>
        /// Gets the IP addresses that are authorized to access GPC when IP address restrictions are in force.
        /// </summary>
        public string IPAddressAddressRestrictions
        {
            get { return this.ipAddressRestrictions; }
        }

        /// <summary>
        /// Gets a value indicating whether IP address restrictions should be applied to this GPC instance.
        /// </summary>
        public bool IPAddressAddressRestrictionsEnabled
        {
            get { return this.ipAddressRestrictionsEnabled; }
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

        /// <summary>
        /// Gets the base URI for UI API requests.
        /// </summary>
        public Uri UIApiBaseAddress
        {
            get { return this.uiApiBaseAddress; }
        }

        /// <summary>
        /// Gets the base URI for the GPC UI.
        /// </summary>
        public Uri UIBaseAddress
        {
            get { return this.uiBaseAddress; }
        }

        /// <summary>
        /// Gets the unique identifier of the web application within Active Directory.
        /// </summary>
        public string WebAADApplicationId
        {
            get { return this.webAADApplicationId; }
        }
    }
}
//----------------------------------------------------------------------------------------------------------------------
// <copyright file="GpcEnvironmentFactory.cs" company="Rakuten">
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
    public class GpcEnvironmentFactory
    {
        /// <summary>
        /// A template used to generate the database connection string.
        /// </summary>
        private const string DatabaseConnectionStringTemplate = "Server=tcp:{0}.database.windows.net,1433;" +
            "Database={1}-gpc;User ID={2};Password={3};" +
            "Trusted_Connection=False;Encrypt=True;Connection Timeout=30";

        /// <summary>
        /// A set of configuration sources to examine for environmental configuration.
        /// </summary>
        private readonly ConfigurationSource[] environmentConfiguration;

        /// <summary>
        /// Provides access to configuration held in a storage account.
        /// </summary>
        private readonly Func<string, ConfigurationSource> storedConfigurationConnector;

        /// <summary>
        /// Initializes a new instance of the <see cref="GpcEnvironmentFactory"/> class.
        /// </summary>
        /// <param name="storedConfigurationConnector">
        /// A delegate that will provide access to configuration held in a storage account given its connection string.
        /// </param>
        /// <param name="environmentConfiguration">
        /// A set of configuration sources to examine for environmental configuration.
        /// </param>
        public GpcEnvironmentFactory(Func<string, ConfigurationSource> storedConfigurationConnector, params ConfigurationSource[] environmentConfiguration)
        {
            Contract.Requires(storedConfigurationConnector != null);
            Contract.Requires(environmentConfiguration != null);
            Contract.Requires(environmentConfiguration.Length != 0);

            this.environmentConfiguration = environmentConfiguration;
            this.storedConfigurationConnector = storedConfigurationConnector;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GpcEnvironment"/> class.
        /// </summary>
        /// <returns>A new instance of the <see cref="GpcEnvironment"/> class.</returns>
        public IGpcEnvironment Create()
        {
            ConfigurationSource source;
            string environmentName;

            // Search environment configuration sources for the environment name.
            for (int i = 0;;)
            {
                source = this.environmentConfiguration[i];
                if ((environmentName = source.GetConfigurationSettingValue(SettingKey.EnvironmentName)) != null)
                {
                    environmentName = environmentName.ToLowerInvariant();
                    break;
                }

                if (++i == this.environmentConfiguration.Length)
                {
                    throw new InvalidOperationException("Environment name not configured.");
                }
            }

            string runTarget = source.GetConfigurationSettingValue(SettingKey.RunTarget);
            if (runTarget != null && string.CompareOrdinal(runTarget, "Local") == 0)
            {
                environmentName += "local";
            }

            string region = source.GetConfigurationSettingValue(SettingKey.Region);
            if (region == null)
            {
                throw new InvalidOperationException("Region not configured.");
            }

            region = string.CompareOrdinal(region, "North Europe") == 0 ? "n eur" : region.ToLowerInvariant();

            string storageAccountKey = source.GetConfigurationSettingValue(SettingKey.StorageAccountKey);
            if (storageAccountKey == null)
            {
                throw new InvalidOperationException("Storage account key not configured.");
            }

            string storageConnectionString = string.Format(
                "DefaultEndpointsProtocol=https;AccountName={0}gpc{1}app;AccountKey={2}",
                environmentName,
                region.Replace(" ", string.Empty),
                storageAccountKey);

            // Connect to the appropriate storage account to retrieve settings
            Contract.Assume(this.storedConfigurationConnector != null);
            source = this.storedConfigurationConnector(storageConnectionString);

            var settings = source.GetConfigurationSettingValues(
                SettingKey.ApiApplicationId,
                SettingKey.ApiApllicationIdUri,
                SettingKey.Credentials,
                SettingKey.CoreApiBaseAddress,
                SettingKey.DatabasePassword,
                SettingKey.DatabaseServer,
                SettingKey.DatabaseUser,
                SettingKey.DirectoryAdministrator,
                SettingKey.DirectoryAdministratorPassword,
                SettingKey.DirectoryAuthority,
                SettingKey.DirectoryTenant,
                SettingKey.Host,
                SettingKey.IPAddressRestrictions,
                SettingKey.IPAddressRestrictionsEnabled,
                SettingKey.UIApiBaseAddress,
                SettingKey.UIBaseAddress,
                SettingKey.WebApplicationId);

            string credentials = settings[SettingKey.Credentials];
            if (credentials == null)
            {
                throw new InvalidOperationException("Credentials not configured.");
            }

            if (credentials.StartsWith("Basic "))
            {
                credentials = credentials.Substring(6);
            }

            string databaseServer = settings[SettingKey.DatabaseServer];
            if (databaseServer == null)
            {
                throw new InvalidOperationException("Database server not configured.");
            }

            string databaseUser = settings[SettingKey.DatabaseUser];
            if (databaseUser == null)
            {
                throw new InvalidOperationException("Database user not configured.");
            }

            string databasePassword = settings[SettingKey.DatabasePassword];
            if (databasePassword == null)
            {
                throw new InvalidOperationException("Database password not configured.");
            }

            string databaseConnectionString = string.Format(
                DatabaseConnectionStringTemplate,
                databaseServer,
                environmentName,
                databaseUser,
                databasePassword);

            string directoryAdministrator = settings[SettingKey.DirectoryAdministrator];
            if (directoryAdministrator == null)
            {
                throw new InvalidOperationException("Directory administrator not configured.");
            }

            string directoryAdministratorPassword = settings[SettingKey.DirectoryAdministratorPassword];
            if (directoryAdministratorPassword == null)
            {
                throw new InvalidOperationException("Directory administrator password not configured.");
            }

            // We are not checking for an undefined setting here as only the interface needs this setting.
            string directoryAuthority = null;

            // Search configuration sources for the Active Directory authority.
            for (int i = 0; i < this.environmentConfiguration.Length; i++)
            {
                source = this.environmentConfiguration[i];
                if ((directoryAuthority = source.GetConfigurationSettingValue(SettingKey.DirectoryAuthority)) != null)
                {
                    break;
                }
            }

            string directoryTenant = settings[SettingKey.DirectoryTenant];
            if (directoryTenant == null)
            {
                throw new InvalidOperationException("Directory tenant not configured.");
            }

            string webApplicationId = settings[SettingKey.WebApplicationId];
            if (webApplicationId == null)
            {
                throw new InvalidOperationException("Web application ID not configured.");
            }

            string apiApplicationId = settings[SettingKey.ApiApplicationId];
            if (apiApplicationId == null)
            {
                throw new InvalidOperationException("API application ID not configured.");
            }

            Uri apiApplicationUri;

            string apiApplicationUriString = settings[SettingKey.ApiApllicationIdUri];
            if (apiApplicationUriString == null)
            {
                throw new InvalidOperationException("API application URI not configured.");
            }

            try
            {
                apiApplicationUri = new Uri(apiApplicationUriString, UriKind.Absolute);
            }
            catch (UriFormatException)
            {
                throw new InvalidOperationException("API application URI not valid.");
            }

            string settingValue = settings[SettingKey.IPAddressRestrictionsEnabled];
            bool ipAddressRestrictionsEnabled = settingValue != null && bool.Parse(settingValue);
            string ipAddressRestrictions = settings[SettingKey.IPAddressRestrictions];

            string host = settings[SettingKey.Host];
            if (host == null)
            {
                throw new InvalidOperationException("Host not configured.");
            }

            settingValue = settings[SettingKey.CoreApiBaseAddress];
            Contract.Assume(!string.IsNullOrEmpty(settingValue), "Setting for CoreApiBaseAddress missing");
            var coreApiBaseAddress = new Uri(settingValue);

            settingValue = settings[SettingKey.UIApiBaseAddress];
            Contract.Assume(!string.IsNullOrEmpty(settingValue), "Setting for UIApiBaseAddress missing");
            var uiApiBaseAddress = new Uri(settingValue);

            settingValue = settings[SettingKey.UIBaseAddress];
            Contract.Assume(!string.IsNullOrEmpty(settingValue), "Setting for UIBaseAddress missing");
            var uiBaseAddress = new Uri(settingValue);

            return new GpcEnvironment(
                environmentName,
                region,
                host,
                uiBaseAddress,
                uiApiBaseAddress,
                coreApiBaseAddress,
                ipAddressRestrictions,
                ipAddressRestrictionsEnabled,
                credentials,
                apiApplicationId,
                apiApplicationUri,
                webApplicationId,
                directoryAdministrator,
                directoryAdministratorPassword,
                directoryAuthority,
                directoryTenant,
                databaseConnectionString,
                storageConnectionString);
        }

        /// <summary>
        /// Provides names of the keys for the GPC configuration settings.
        /// </summary>
        private static class SettingKey
        {
            /// <summary>
            /// Indicates the setting for the unique identifier of the API in Active Directory.
            /// </summary>
            public const string ApiApplicationId = "Gpc.UI.Api.App.Id";

            /// <summary>
            /// Indicates the setting for the URI identifying the API in Active Directory.
            /// </summary>
            public const string ApiApllicationIdUri = "Gpc.UI.Api.App.Uri.Id";

            /// <summary>
            /// Indicates the setting for the base URI for core API requests.
            /// </summary>
            public const string CoreApiBaseAddress = "Gpc.Website.BaseUri";

            /// <summary>
            /// Indicates the setting for the credentials to use in the authorization header
            /// when connecting to GPC services.
            /// </summary>
            public const string Credentials = "Gpc.ApiAuthToken";

            /// <summary>
            /// Indicates the setting for the database server.
            /// </summary>
            public const string DatabaseServer = "Gpc.Azure.SqlServer.Name";

            /// <summary>
            /// Indicates the setting for the database password.
            /// </summary>
            public const string DatabasePassword = "Gpc.Azure.SqlServer.Login.Password";

            /// <summary>
            /// Indicates the setting for the database user.
            /// </summary>
            public const string DatabaseUser = "Gpc.Azure.SqlServer.Login.Username";

            /// <summary>
            /// Indicates the setting for the username (email) of the Active Directory administrator.
            /// </summary>
            public const string DirectoryAdministrator = "Gpc.UI.ClientApp.Username";

            /// <summary>
            /// Indicates the setting for the password of the Active Directory administrator.
            /// </summary>
            public const string DirectoryAdministratorPassword = "Gpc.UI.ClientApp.Password";

            /// <summary>
            /// Indicates the setting for the Active Directory instance authority.
            /// </summary>
            public const string DirectoryAuthority = "ida:AADInstance";

            /// <summary>
            /// Indicates the setting for the Active Directory domain.
            /// </summary>
            public const string DirectoryTenant = "Gpc.Azure.AD.Tenant";

            /// <summary>
            /// Indicates the setting for the environment name.
            /// </summary>
            public const string EnvironmentName = "Gpc.Environment.Name";

            /// <summary>
            /// Indicates the setting for the host name for GPC services.
            /// </summary>
            public const string Host = "Gpc.Domain.Name";

            /// <summary>
            /// Indicates the setting for whether access to the GPC services should be restricted by IP address.
            /// </summary>
            public const string IPAddressRestrictions = "Gpc.Website.IpRestrictions";

            /// <summary>
            /// Indicates the setting for whether access to the GPC services should be restricted by IP address.
            /// </summary>
            public const string IPAddressRestrictionsEnabled = "Gpc.Website.IpRestrictions.Enabled";

            /// <summary>
            /// Indicates the setting for the geographical region in which this GPC instance is running.
            /// </summary>
            public const string Region = "Gpc.Environment.Region";

            /// <summary>
            /// Indicates the setting for the run target of the current GPC instance.
            /// </summary>
            public const string RunTarget = "Gpc.RunTarget";

            /// <summary>
            /// Indicates the setting for the connection string to the storage account.
            /// </summary>
            public const string StorageAccountKey = "Gpc.App.Azure.Storage.PrimaryKey";

            /// <summary>
            /// Indicates the setting for the base URI for UI API requests.
            /// </summary>
            public const string UIApiBaseAddress = "Gpc.UI.Api.BaseUri";

            /// <summary>
            /// Indicates the setting for the base URI for the GPC UI.
            /// </summary>
            public const string UIBaseAddress = "Gpc.UI.BaseUri";

            /// <summary>
            /// Indicates the setting for the unique identifier of the web application in Active Directory.
            /// </summary>
            public const string WebApplicationId = "Gpc.UI.ClientApp.Id";
        }
    }
}
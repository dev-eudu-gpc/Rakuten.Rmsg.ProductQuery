﻿//----------------------------------------------------------------------------------------------------------------------
// <copyright file="ApiContextFactory.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.Configuration
{
    using System;
    using System.Diagnostics.Contracts;

    /// <summary>
    /// Provides factory methods for configuration settings.
    /// </summary>
    public class ApiContextFactory
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
        /// Initializes a new instance of the <see cref="ApiContextFactory"/> class.
        /// </summary>
        /// <param name="environmentConfiguration">
        /// A set of configuration sources to examine for environmental configuration.
        /// </param>
        public ApiContextFactory(params ConfigurationSource[] environmentConfiguration)
        {
            Contract.Requires(environmentConfiguration != null);
            Contract.Requires(environmentConfiguration.Length != 0);

            this.environmentConfiguration = environmentConfiguration;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiContext"/> class.
        /// </summary>
        /// <returns>A new instance of the <see cref="ApiContext"/> class.</returns>
        public IApiContext Create()
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

            // Modify the name if the run target is local
            string runTarget = source.GetConfigurationSettingValue(SettingKey.RunTarget);
            if (runTarget != null && string.CompareOrdinal(runTarget, "Local") == 0)
            {
                environmentName += "local";
            }

            // Get the region configuration setting
            string region = source.GetConfigurationSettingValue(SettingKey.Region);
            if (region == null)
            {
                throw new InvalidOperationException("Region not configured.");
            }

            region = string.CompareOrdinal(region, "North Europe") == 0 ? "n eur" : region.ToLowerInvariant();

            // Get the storage account key
            string storageAccountKey = source.GetConfigurationSettingValue(SettingKey.StorageAccountKey);
            if (storageAccountKey == null)
            {
                throw new InvalidOperationException("Storage account key not configured.");
            }

            string storageConnectionString = GenerateStorageAccountConnectionString(
                environmentName,
                region,
                "app",
                storageAccountKey);

            // Get the storage account key for diagnostic storage
            storageAccountKey = source.GetConfigurationSettingValue(SettingKey.DiagnosticStorageAccountKey);

            if (storageAccountKey == null)
            {
                throw new InvalidOperationException("Diagnostic storage account key not configured.");
            }

            string diagnosticSorageConnectionString = GenerateStorageAccountConnectionString(
                environmentName,
                region,
                "diag",
                storageAccountKey);

            // Configure the database connection string
            string databaseServer = source.GetConfigurationSettingValue(SettingKey.DatabaseServer);
            if (databaseServer == null)
            {
                throw new InvalidOperationException("Database server name not configured.");
            }

            string databaseUser = source.GetConfigurationSettingValue(SettingKey.DatabaseUser);
            if (databaseUser == null)
            {
                throw new InvalidOperationException("Database user not configured.");
            }

            string databasePassword = source.GetConfigurationSettingValue(SettingKey.DatabasePassword);
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

            // Configure the service bus connection string
            string serviceBusKey =
                source.GetConfigurationSettingValue(SettingKey.ServiceBusAccessKey);

            if (serviceBusKey == null)
            {
                throw new InvalidOperationException("Service Bus connection string not configured.");
            }

            string serviceBusConnectionString = GenerateServiceBusConnectionString(
                environmentName,
                region,
                "app",
                serviceBusKey);

            // Get the maximum number of queries allowed per query group
            int maxQueriesPerGroup = GetSettingInt(source, SettingKey.MaximumQueriesPerGroup);

            // Get progress map settings
            int progressMapInterval = GetSettingInt(source, SettingKey.ProgressMapIntervalInSeconds);
            decimal proportionOfTimeAllocatedForFinalization = GetSettingDecimal(source, SettingKey.ProportionOfTimeAllocatedForFinalization);

            // Get the blob file name mask
            string blobFileNameMask = source.GetConfigurationSettingValue(SettingKey.BlobFileNameMask);
            if (blobFileNameMask == null)
            {
                throw new InvalidOperationException("Blob file name mask is not configured");
            }

            // Get the blob container name 
            string blobContainerName = source.GetConfigurationSettingValue(SettingKey.BlobContainerName);
            if (blobContainerName == null)
            {
                throw new InvalidOperationException("Blob container name is not configured");
            }

            // Get the message queue name
            string messageQueueName = source.GetConfigurationSettingValue(SettingKey.MessageQueueName);
            if (messageQueueName == null)
            {
                throw new InvalidOperationException("Message queue name is not configured");
            }

            // Get the base address of the product query API.
            string productQueryApiBaseAddressString = source.GetConfigurationSettingValue(SettingKey.ProductQueryApiBaseAddress);
            if (productQueryApiBaseAddressString == null)
            {
                throw new InvalidOperationException("The product query API base address is not configured.");
            }

            var productQueryApiBaseAddress = new Uri(productQueryApiBaseAddressString);

            // Get the base address of the GPC core API.
            string gpcCoreApiBaseAddressString = source.GetConfigurationSettingValue(SettingKey.GpcCoreApiBaseAddress);
            if (gpcCoreApiBaseAddressString == null)
            {
                throw new InvalidOperationException("The GPC core API base address is not configured.");
            }

            var gpcCoreApiBaseAddress = new Uri(gpcCoreApiBaseAddressString);

            // Get the blob file name mask
            string authenticationToken = source.GetConfigurationSettingValue(SettingKey.AuthenticationToken);
            if (authenticationToken == null)
            {
                throw new InvalidOperationException("The authentication token is not configured");
            }

            return new ApiContext(
                authenticationToken,
                blobContainerName,
                blobFileNameMask,
                databaseConnectionString,
                diagnosticSorageConnectionString,
                environmentName,
                gpcCoreApiBaseAddress,
                maxQueriesPerGroup,
                messageQueueName,
                productQueryApiBaseAddress,
                progressMapInterval,
                proportionOfTimeAllocatedForFinalization,
                region,
                serviceBusConnectionString,
                storageConnectionString);
        }

        /// <summary>
        /// Generates and returns a storage account connection string using the specified details.
        /// </summary>
        /// <param name="environmentName">The currently targeted environment.</param>
        /// <param name="region">The region in which the storage account is located.</param>
        /// <param name="instance">A <see cref="string"/> identifying the account.</param>
        /// <param name="accountKey">The shared key assigned to the storage account.</param>
        /// <returns>The generate connection string.</returns>
        private static string GenerateStorageAccountConnectionString(
            string environmentName, 
            string region, 
            string instance,
            string accountKey)
        {
            const string ConnectionStringTemplate = "DefaultEndpointsProtocol=https;AccountName={0};AccountKey={1}";

            // Create the connection string to the storage account.
            string accountName = string.Format(
                "{0}gpc{1}{2}",
                environmentName,
                region.Replace(" ", string.Empty),
                instance);

            return string.Format(
                ConnectionStringTemplate,
                accountName,
                accountKey);
        }

        /// <summary>
        /// Generates and returns a service bus connection string using the specified details.
        /// </summary>
        /// <param name="environmentName">The currently targeted environment.</param>
        /// <param name="region">The region in which the storage account is located.</param>
        /// <param name="instance">A <see cref="string"/> identifying the account.</param>
        /// <param name="accountKey">The shared key assigned to the storage account.</param>
        /// <returns>The generate connection string.</returns>
        private static string GenerateServiceBusConnectionString(
            string environmentName,
            string region,
            string instance,
            string accountKey)
        {
            ////const string ConnectionStringTemplate = "Endpoint=sb://{0}.servicebus.windows.net/;SharedAccessKeyName=RootManagerSharedAccessKey;SharedSecretValue={1}";
            const string ConnectionStringTemplate = "Endpoint=sb://{0}.servicebus.windows.net/;SharedSecretIssuer=owner;SharedSecretValue={1}";

            // Create the connection string to the storage account.
            string accountName = string.Format(
                "{0}-gpc-{1}-bus",
                environmentName,
                region.Replace(" ", "-"));

            return string.Format(
                ConnectionStringTemplate,
                accountName,
                accountKey);
        }

        /// <summary>
        /// Gets a decimal value from a config source.
        /// </summary>
        /// <param name="source">The config source.</param>
        /// <param name="keyName">The name of the setting in the config source.</param>
        /// <returns>A value of the config setting as a decimal</returns>
        private static decimal GetSettingDecimal(ConfigurationSource source, string keyName)
        {
            string valueString = source.GetConfigurationSettingValue(keyName);

            if (valueString == null)
            {
                throw new InvalidOperationException(string.Format("{0} is not configured", keyName));
            }

            decimal value;

            if (!decimal.TryParse(valueString, out value))
            {
                throw new InvalidOperationException(string.Format("{0} setting must be a double.", keyName));
            }

            return value;
        }

        /// <summary>
        /// Gets an integer value from a config source.
        /// </summary>
        /// <param name="source">The config source.</param>
        /// <param name="keyName">The name of the setting in the config source.</param>
        /// <returns>A value of the config setting as an integer</returns>
        private static int GetSettingInt(ConfigurationSource source, string keyName)
        {
            string valueString = source.GetConfigurationSettingValue(keyName);

            if (valueString == null)
            {
                throw new InvalidOperationException(string.Format("{0} is not configured", keyName));
            }

            int valueInt;

            if (!int.TryParse(valueString, out valueInt))
            {
                throw new InvalidOperationException(string.Format("{0} setting must be an integer.", keyName));
            }

            return valueInt;
        }

        /// <summary>
        /// Provides names of the keys for the GPC configuration settings.
        /// </summary>
        private static class SettingKey
        {
            /// <summary>
            /// Indicates the setting for the authentication token.
            /// </summary>
            public const string AuthenticationToken = "Rakuten.Rmsg.ProductQuery.AuthenticationToken";

            /// <summary>
            /// Indicates the setting for the blob container name.
            /// </summary>
            public const string BlobContainerName = "Rakuten.Rmsg.ProductQuery.BlobContainerName";

            /// <summary>
            /// Indicates the setting for the mask for the name of the file in blob storage.
            /// </summary>
            public const string BlobFileNameMask = "Rakuten.Rmsg.ProductQuery.BlobFileNameMask";

            /// <summary>
            /// Indicates the setting for the database server.
            /// </summary>
            public const string DatabaseServer = "Rakuten.Gpc.Azure.SqlServer.Name";

            /// <summary>
            /// Indicates the setting for the database password.
            /// </summary>
            public const string DatabasePassword = "Rakuten.Gpc.Azure.SqlServer.Login.Password";

            /// <summary>
            /// Indicates the setting for the database user.
            /// </summary>
            public const string DatabaseUser = "Rakuten.Gpc.Azure.SqlServer.Login.Username";

            /// <summary>
            /// Indicates the setting name for the storage account for diagnostic data.
            /// </summary>
            public const string DiagnosticStorageAccountKey = "Rakuten.Gpc.Diag.Azure.Storage.PrimaryKey";

            /// <summary>
            /// Indicates the setting for the environment name.
            /// </summary>
            public const string EnvironmentName = "Rakuten.Rmsg.ProductQuery.Environment.Name";

            /// <summary>
            /// Indicates the setting name for the GPC core API base address.
            /// </summary>
            public const string GpcCoreApiBaseAddress = "Rakuten.Gpc.Api.Core.BaseAddress";

            /// <summary>
            /// Indicates the setting for the maximum number of queries per query group
            /// </summary>
            public const string MaximumQueriesPerGroup = "Rakuten.Rmsg.ProductQuery.MaximumQueriesPerGroup";

            /// <summary>
            /// Indicates the setting for the message queue name
            /// </summary>
            public const string MessageQueueName = "Rakuten.Rmsg.ProductQuery.MessageQueueName";

            /// <summary>
            /// Indicates the setting for the base address.
            /// </summary>
            public const string ProductQueryApiBaseAddress = "Rakuten.Rmsg.ProductQuery.BaseAddress";

            /// <summary>
            /// Indicates the setting for the number of seconds between progress maps
            /// </summary>
            public const string ProgressMapIntervalInSeconds = "Rakuten.Rmsg.ProductQuery.ProgressMapIntervalInSeconds";

            /// <summary>
            /// Indicates the setting for the estimated proportion of product query processing 
            /// that is used by the finalization process.
            /// </summary>
            public const string ProportionOfTimeAllocatedForFinalization = "Rakuten.Rmsg.ProductQuery.ProportionOfTimeAllocatedForFinalization";

            /// <summary>
            /// Indicates the setting for the geographical region in which this GPC instance is running.
            /// </summary>
            public const string Region = "Rakuten.Rmsg.ProductQuery.Environment.Region";

            /// <summary>
            /// Indicates the setting for the run target of the current GPC instance.
            /// </summary>
            public const string RunTarget = "Rakuten.Rmsg.ProductQuery.RunTarget";

            /// <summary>
            /// Indicates the setting for the connection string to the service bus/
            /// </summary>
            public const string ServiceBusAccessKey = "Rakuten.Gpc.Azure.ServiceBus.AccessKey";

            /// <summary>
            /// Indicates the setting for the connection string to the storage account.
            /// </summary>
            public const string StorageAccountKey = "Rakuten.Gpc.App.Azure.Storage.PrimaryKey";
        }
    }
}
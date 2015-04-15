//----------------------------------------------------------------------------------------------------------------------
// <copyright file="ApiContextFactory.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.Web.Http.Configuration
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

            string storageConnectionString = string.Format(
                "DefaultEndpointsProtocol=https;AccountName={0}gpc{1}app;AccountKey={2}",
                environmentName,
                region.Replace(" ", string.Empty),
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

            // Get the maximum number of queryes allowed per query group
            string maxQueriesPerGroupString = source.GetConfigurationSettingValue(SettingKey.MaximumQueriesPerGroup);
            if (maxQueriesPerGroupString == null)
            {
                throw new InvalidOperationException("Maximum queries per group is not configured.");
            }

            int maxQueriesPerGroup;

            if (!int.TryParse(maxQueriesPerGroupString, out maxQueriesPerGroup))
            {
                throw new InvalidOperationException("Maximum queries per group must be an integer.");
            }

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

            return new ApiContext(
                blobContainerName,
                blobFileNameMask,
                databaseConnectionString,
                environmentName,
                maxQueriesPerGroup,
                region,
                storageConnectionString);
        }

        /// <summary>
        /// Provides names of the keys for the GPC configuration settings.
        /// </summary>
        private static class SettingKey
        {
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
            /// Indicates the setting for the environment name.
            /// </summary>
            public const string EnvironmentName = "Rakuten.Rmsg.ProductQuery.Environment.Name";

            /// <summary>
            /// Indicates the setting for the maximum number of queries per query group
            /// </summary>
            public const string MaximumQueriesPerGroup = "Rakuten.Rmsg.ProductQuery.MaximumQueriesPerGroup";

            /// <summary>
            /// Indicates the setting for the geographical region in which this GPC instance is running.
            /// </summary>
            public const string Region = "Rakuten.Rmsg.ProductQuery.Environment.Region";

            /// <summary>
            /// Indicates the setting for the run target of the current GPC instance.
            /// </summary>
            public const string RunTarget = "Rakuten.Rmsg.ProductQuery.RunTarget";

            /// <summary>
            /// Indicates the setting for the connection string to the storage account.
            /// </summary>
            public const string StorageAccountKey = "Rakuten.Gpc.App.Azure.Storage.PrimaryKey";
        }
    }
}
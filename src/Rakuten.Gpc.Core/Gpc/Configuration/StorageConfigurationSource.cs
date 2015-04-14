//----------------------------------------------------------------------------------------------------------------------
// <copyright file="StorageConfigurationSource.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------------------------------------------------
namespace Rakuten.Gpc.Configuration
{
    using System.Collections.Generic;
    using System.Collections.Immutable;

    using Microsoft.WindowsAzure.Storage;
    using Microsoft.WindowsAzure.Storage.Table;

    /// <summary>
    /// Provides access to configuration data stored in an Azure storage account.
    /// </summary>
    public class StorageConfigurationSource : ConfigurationSource
    {
        /// <summary>
        /// A filter that targets the storage partition containing configuration settings.
        /// </summary>
        private readonly string isPartition;

        /// <summary>
        /// An Azure table containing the configuration data.
        /// </summary>
        private readonly CloudTable table;

        /// <summary>
        /// Initializes a new instance of the <see cref="StorageConfigurationSource"/> class.
        /// </summary>
        /// <param name="connectionString">
        /// Connection details for the storage account that contains configuration data.
        /// </param>
        public StorageConfigurationSource(string connectionString)
        {
            // Connect to the storage account to retrieve settings.
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connectionString);
            CloudTableClient client = storageAccount.CreateCloudTableClient();
            this.table = client.GetTableReference("ConfigSetting");
            this.isPartition = TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, "AllRoles");
        }

        /// <summary>
        /// Retrieves the value of a setting from an Azure storage account.
        /// </summary>
        /// <param name="configurationSettingName">The name of the configuration setting.</param>
        /// <returns>A <see cref="string"/> that contains the value of the configuration setting.</returns>
        public override string GetConfigurationSettingValue(string configurationSettingName)
        {
            // The most common case for Azure storage is retrieving a set of values,
            // so we optimize that method and call it here to reduce code duplication.
            return this.GetConfigurationSettingValues(new[] { configurationSettingName })[configurationSettingName];
        }

        /// <summary>
        /// Retrieves the values of a collection of settings from an Azure storage account.
        /// </summary>
        /// <param name="configurationSettingNames">The names of the configuration settings.</param>
        /// <returns>A lookup that contains the values of the configuration settings.</returns>
        public override ImmutableDictionary<string, string> GetConfigurationSettingValues(
            params string[] configurationSettingNames)
        {
            if (configurationSettingNames == null || configurationSettingNames.Length == 0)
            {
                return ImmutableDictionary<string, string>.Empty;
            }

            string isSetting = TableQuery.GenerateFilterCondition(
                "RowKey",
                QueryComparisons.Equal,
                configurationSettingNames[0]);

            for (int i = 1; i < configurationSettingNames.Length; i++)
            {
                isSetting = TableQuery.CombineFilters(
                    isSetting,
                    TableOperators.Or,
                    TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, configurationSettingNames[i]));
            }

            isSetting = TableQuery.CombineFilters(this.isPartition, TableOperators.And, isSetting);

            var query = new TableQuery<DynamicTableEntity>()
                .Where(isSetting)
                .Select(new[] { "RowKey", "Value" });

            EntityResolver<KeyValuePair<string, string>> resolver = (partitionKey, rowKey, timestamp, properties, etag) =>
                new KeyValuePair<string, string>(
                    rowKey,
                    properties.ContainsKey("Value") ? properties["Value"].StringValue : null);

            var builder = ImmutableDictionary.CreateBuilder<string, string>();
            builder.AddRange(this.table.ExecuteQuery(query, resolver));

            // Add any missing configuration keys with null value.
            foreach (string configurationSettingName in configurationSettingNames)
            {
                if (!builder.ContainsKey(configurationSettingName))
                {
                    builder.Add(configurationSettingName, null);
                }
            }

            return builder.ToImmutable();
        }
    }
}
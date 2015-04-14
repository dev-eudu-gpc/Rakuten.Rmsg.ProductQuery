//----------------------------------------------------------------------------------------------------------------------
// <copyright file="EnvironmentConfigurationSource.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------------------------------------------------
namespace Rakuten.Gpc.Configuration
{
    using System;

    /// <summary>
    /// Provides access to configuration data stored as environment variables.
    /// </summary>
    public class EnvironmentConfigurationSource : ConfigurationSource
    {
        /// <summary>
        /// Retrieves the value of a setting from environment variables.
        /// </summary>
        /// <param name="configurationSettingName">The name of the configuration setting.</param>
        /// <returns>A <see cref="string"/> that contains the value of the configuration setting.</returns>
        public override string GetConfigurationSettingValue(string configurationSettingName)
        {
            return Environment.GetEnvironmentVariable(configurationSettingName);
        }
    }
}
//----------------------------------------------------------------------------------------------------------------------
// <copyright file="ServiceConfigurationSource.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------------------------------------------------
namespace Rakuten.Gpc.Configuration
{
    using System;
    using Microsoft.Azure;

    /// <summary>
    /// Provides access to configuration data stored as service configuration.
    /// </summary>
    public class ServiceConfigurationSource : ConfigurationSource
    {
        /// <summary>
        /// Retrieves the value of a setting from the service's configuration.
        /// </summary>
        /// <param name="configurationSettingName">The name of the configuration setting.</param>
        /// <returns>A <see cref="string"/> that contains the value of the configuration setting.</returns>
        public override string GetConfigurationSettingValue(string configurationSettingName)
        {
            if (configurationSettingName == null)
            {
                throw new ArgumentNullException("configurationSettingName");
            }

            if (configurationSettingName.Length == 0)
            {
                throw new ArgumentException("Setting name cannot be empty.", "configurationSettingName");
            }
            
            return CloudConfigurationManager.GetSetting(configurationSettingName);
        }
    }
}
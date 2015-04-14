//----------------------------------------------------------------------------------------------------------------------
// <copyright file="ConfigurationSource.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------------------------------------------------
namespace Rakuten.Gpc.Configuration
{
    using System.Collections.Immutable;

    /// <summary>
    /// Provides access to configuration data.
    /// </summary>
    public abstract class ConfigurationSource
    {
        /// <summary>
        /// Retrieves the value of a setting.
        /// </summary>
        /// <param name="configurationSettingName">The name of the configuration setting.</param>
        /// <returns>A <see cref="string"/> that contains the value of the configuration setting.</returns>
        public abstract string GetConfigurationSettingValue(string configurationSettingName);

        /// <summary>
        /// Retrieves the values of a collection of settings.
        /// </summary>
        /// <param name="configurationSettingNames">The names of the configuration settings.</param>
        /// <returns>A lookup that contains the values of the configuration settings.</returns>
        public virtual ImmutableDictionary<string, string> GetConfigurationSettingValues(
            params string[] configurationSettingNames)
        {
            if (configurationSettingNames == null || configurationSettingNames.Length == 0)
            {
                return ImmutableDictionary<string, string>.Empty;
            }

            return configurationSettingNames.ToImmutableDictionary(name => name, this.GetConfigurationSettingValue);
        }
    }
}
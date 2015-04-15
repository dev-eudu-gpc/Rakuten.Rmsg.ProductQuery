//----------------------------------------------------------------------------------------------------------------------
// <copyright file="AppSettingsConfigurationSource.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.Web.Http.Configuration
{
    using System;
    using System.Configuration;

    /// <summary>
    /// Provides access to configuration data stored in appSettings in web.config
    /// </summary>
    public class AppSettingsConfigurationSource : ConfigurationSource
    {
        /// <summary>
        /// Retrieves the value of a setting from appSettings in web.config
        /// </summary>
        /// <param name="configurationSettingName">The name of the configuration setting.</param>
        /// <returns>A <see cref="string"/> that contains the value of the configuration setting.</returns>
        public override string GetConfigurationSettingValue(string configurationSettingName)
        {
            return ConfigurationManager.AppSettings[configurationSettingName];
        }
    }
}
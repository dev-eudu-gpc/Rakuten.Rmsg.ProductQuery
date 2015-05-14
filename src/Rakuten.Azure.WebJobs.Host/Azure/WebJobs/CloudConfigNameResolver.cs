// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="CloudConfigNameResolver.cs" company="Rakuten">
//   Copyright (c) Rakuten. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------
namespace Rakuten.Azure.WebJobs
{
    using System;
    using Microsoft.Azure;
    using Microsoft.Azure.WebJobs;

    /// <summary>
    /// Attempts to retrieve placeholder values in WebJobs SDK constructor parameters using the local config file.
    /// </summary>
    public class CloudConfigNameResolver : INameResolver
    {
        /// <summary>
        /// Attempts to retrieve a placeholder parameter using the local config file.
        /// </summary>
        /// <param name="name">The placeholder name.</param>
        /// <returns>The value that was defined in the local config file.</returns>
        /// <exception cref="InvalidOperationException">
        /// No configuration value could be found for the specified placeholder name.
        /// </exception>
        public string Resolve(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("A placeholder name must be specified.", "name");
            }

            return CloudConfigurationManager.GetSetting(name);
        }
    }
}
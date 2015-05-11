// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="ICache{T}.cs" company="Rakuten">
//   Copyright (c) Rakuten. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.WebJob
{
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines an instance that will cache results against a given key.
    /// </summary>
    /// <typeparam name="T">The <see cref="Type"/> to be cached.</typeparam>
    internal interface ICache<T>
    {
        /// <summary>
        /// Returns a cached value for the key adding a new key/value pair to the cache if the key does not already 
        /// exist.
        /// </summary>
        /// <param name="key">The key of the element to add.</param>
        /// <param name="parameters">The values required to generate a new value.</param>
        /// <returns>
        /// The value for the key. This will be either the existing value for the key if the key is already in the 
        /// cache, or the new value for the key if the key was not in the cache.
        /// </returns>
        Task<T> GetOrAddAsync(string key, params string[] parameters);
    }
}
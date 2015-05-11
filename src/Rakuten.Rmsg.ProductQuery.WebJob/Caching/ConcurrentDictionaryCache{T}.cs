// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="ConcurrentDictionaryCache{T}.cs" company="Rakuten">
//   Copyright (c) Rakuten. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.WebJob
{
    using System;
    using System.Collections.Concurrent;
    using System.Threading.Tasks;

    /// <summary>
    /// Provides a simple method of caching results.
    /// </summary>
    /// <typeparam name="T">The <see cref="Type"/> to be cached.</typeparam>
    internal class ConcurrentDictionaryCache<T> : ICache<T>
    {
        /// <summary>
        /// A local cache of instances with a <see cref="string"/> index.
        /// </summary>
        private readonly ConcurrentDictionary<string, T> cache = new ConcurrentDictionary<string, T>();

        /// <summary>
        /// A function that when executed will generate a new value to be indexed.
        /// </summary>
        private readonly Func<string[], T> valueFactory; 

        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleCache{T}"/> class.
        /// </summary>
        /// <param name="valueFactory">The function used to generate a value for the key.</param>
        public ConcurrentDictionaryCache(Func<string[], T> valueFactory)
        {
            this.valueFactory = valueFactory;
        }

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
        public Task<T> GetOrAddAsync(string key, params string[] parameters)
        {
            return Task.Run(() => this.cache.GetOrAdd(key, this.AddAsync(key, this.valueFactory, parameters)));
        }

        /// <summary>
        /// Attempts to add the specified key and value to the cache by using the specified function.
        /// </summary>
        /// <param name="key">The key of the element to add.</param>
        /// <param name="valueFactory">The function used to generate a value for the key</param>
        /// <param name="parameters">The values required to generate a new value.</param>
        /// <returns>The value for the key.</returns>
        private T AddAsync(string key, Func<string[], T> valueFactory, string[] parameters)
        {
            var value = valueFactory(parameters);

            this.cache.TryAdd(key, value);

            return value;
        }
    }
}
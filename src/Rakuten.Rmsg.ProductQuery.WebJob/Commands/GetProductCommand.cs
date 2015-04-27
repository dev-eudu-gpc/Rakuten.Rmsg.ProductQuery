// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="GetProductCommand.cs" company="Rakuten">
//   Copyright (c) Rakuten. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.WebJob
{
    using System.Diagnostics.Contracts;
    using System.Globalization;
    using System.Threading.Tasks;

    using Rakuten.Net.Http;
    using Rakuten.Rmsg.ProductQuery.WebJob.Api;
    using Rakuten.Rmsg.ProductQuery.WebJob.Linking;

    /// <summary>
    /// Attempts to retrieve product information for a specified product.
    /// </summary>
    internal class GetProductCommand
    {
        /// <summary>
        /// Retrieves a product with the specified GRAN in the given culture.
        /// </summary>
        /// <param name="cache">An instance that provides a method of caching product instances.</param>
        /// <param name="gran">The GRAN.</param>
        /// <param name="culture">The culture in which the product information should be retrieved.</param>
        /// <returns>A <see cref="Task"/> the represents the asynchronous operation.</returns>
        public static async Task<Product> Execute(ICache<Product> cache, string gran, CultureInfo culture)
        {
            Contract.Requires(cache != null);
            Contract.Requires(culture != null);

            return await cache.GetOrAddAsync(string.Concat(gran, culture.Name), gran, culture.Name);
        }

        /// <summary>
        /// Gets the product with the specified GRAN expressed in the given culture.
        /// </summary>
        /// <param name="client">The <see cref="ApiClient"/> to be used when making requests over HTTP.</param>
        /// <param name="link">A <see cref="LinkTemplate"/> to build the URI to get a product.</param>
        /// <param name="parameters">The parameters required to fetch a new collection of products.</param>
        /// <returns>A <see cref="Task"/> the represents the asynchronous operation.</returns>
        public static async Task<Product> GetProductAsync(ApiClient client, ProductLink link, string[] parameters)
        {
            Contract.Requires(client != null);
            Contract.Requires(link != null);
            Contract.Requires(parameters.Length == 2);

            var culture = new CultureInfo(parameters[1]);

            var uri = link.ForGran(parameters[0]).ForCulture(culture).ToUri();

            return await client.GetAsync<Product>(uri);
        }
    }
}
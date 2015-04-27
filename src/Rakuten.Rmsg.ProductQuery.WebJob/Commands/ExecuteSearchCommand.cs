// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="ExecuteSearchCommand.cs" company="Rakuten">
//   Copyright (c) Rakuten. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.WebJob
{
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Globalization;
    using System.Threading.Tasks;

    using Rakuten.Net.Http;
    using Rakuten.Rmsg.ProductQuery.WebJob.Api;
    using Rakuten.Rmsg.ProductQuery.WebJob.Linking;

    /// <summary>
    /// Executes a search for a product for a Global Trade Identification Number (GTIN).
    /// </summary>
    internal class ExecuteSearchCommand
    {
        /// <summary>
        /// The maximum number of records to return within a single request.
        /// </summary>
        private const int PageSize = 1000;

        /// <summary>
        /// Search for a specific product by GTIN in a given culture.
        /// </summary>
        /// <param name="cache">An instance that provides a method of caching collections of product.</param>
        /// <param name="gtin">The GTIN.</param>
        /// <param name="culture">The culture in which the product information should be retrieved.</param>
        /// <returns>A <see cref="Task"/> the represents the asynchronous operation.</returns>
        public static async Task<IEnumerable<Product>> Execute(
            ICache<IEnumerable<Product>> cache, 
            string gtin, 
            CultureInfo culture)
        {
            Contract.Requires(cache != null);
            Contract.Requires(culture != null);

            return await cache.GetOrAddAsync(string.Concat(gtin, culture.Name), gtin, culture.Name);
        }

        /// <summary>
        /// Fetches a collection of <see cref="Product"/>s that match the specified GTIN and are expressed in the 
        /// supplied culture.
        /// </summary>
        /// <param name="client">The <see cref="ApiClient"/> to be used when making requests over HTTP.</param>
        /// <param name="link">A <see cref="LinkTemplate"/> to build the URI to perform a product search.</param>
        /// <param name="parameters">The parameters required to fetch a new collection of products.</param>
        /// <returns>A <see cref="Task"/> the represents the asynchronous operation.</returns>
        public static async Task<IEnumerable<Product>> GetProducts(
            ApiClient client,
            ProductSearchLink link,
            string[] parameters)
        {
            Contract.Requires(client != null);
            Contract.Requires(link != null);
            Contract.Requires(parameters.Length == 2);

            var products = new List<Product>();

            var culture = new CultureInfo(parameters[1]);

            var template = link.ForGtin(parameters[0]).ForCulture(culture).Taking(PageSize);

            // Starting on the first page.
            var pageCount = 1;

            // Only get the next page when the number of records returned in the last batch was the maximum.
            while (((pageCount * PageSize) - products.Count) == PageSize)
            {
                // Set the number of records to skip.
                var uri = template.Skipping((PageSize - 1) * pageCount).ToUri();

                products.AddRange(await client.GetAsync<IEnumerable<Product>>(uri));

                pageCount++;
            }

            return products;
        }
    }
}
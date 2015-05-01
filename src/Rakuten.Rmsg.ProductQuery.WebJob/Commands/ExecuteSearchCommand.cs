// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="ExecuteSearchCommand.cs" company="Rakuten">
//   Copyright (c) Rakuten. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.WebJob
{
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
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
        /// A cached collection of <see cref="Product"/> indexed by a composite key of GTIN and culture.
        /// </summary>
        private static readonly ConcurrentDictionary<string, Task<IEnumerable<Product>>> CachedProducts;

        /// <summary>
        /// Initializes static members of the <see cref="ExecuteSearchCommand"/> class.
        /// </summary>
        static ExecuteSearchCommand()
        {
            CachedProducts = new ConcurrentDictionary<string, Task<IEnumerable<Product>>>();
        }

        /// <summary>
        /// Search for a specific product by GTIN in a given culture.
        /// </summary>
        /// <param name="client">The <see cref="ApiClient"/> to be used when making requests over HTTP.</param>
        /// <param name="link">A <see cref="LinkTemplate"/> to build the URI to perform a product search.</param>
        /// <param name="gtin">The GTIN.</param>
        /// <param name="culture">The culture in which the product information should be retrieved.</param>
        /// <returns>A <see cref="Task"/> the represents the asynchronous operation.</returns>
        public static Task<IEnumerable<Product>> Execute(
            ApiClient client, 
            ProductSearchLink link,
            string gtin, 
            CultureInfo culture)
        {
            return CachedProducts.GetOrAdd(string.Concat(gtin, culture.Name), GetProducts(client, link, gtin, culture));
        }

        /// <summary>
        /// Fetches a collection of <see cref="Product"/>s that match the specified GTIN and are expressed in the 
        /// supplied culture.
        /// </summary>
        /// <param name="client">The <see cref="ApiClient"/> to be used when making requests over HTTP.</param>
        /// <param name="link">A <see cref="LinkTemplate"/> to build the URI to perform a product search.</param>
        /// <param name="gtin">The GTIN.</param>
        /// <param name="culture">The culture in which the product information should be retrieved.</param>
        /// <returns>A <see cref="Task"/> the represents the asynchronous operation.</returns>
        private static async Task<IEnumerable<Product>> GetProducts(
            ApiClient client,
            ProductSearchLink link,
            string gtin,
            CultureInfo culture)
        {
            var products = new List<Product>();

            var template = link.ForGtin(gtin).ForCulture(culture).Taking(PageSize);

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

            CachedProducts.TryAdd(string.Concat(gtin, culture.Name), Task.FromResult(products.AsEnumerable()));

            return products;
        }
    }
}
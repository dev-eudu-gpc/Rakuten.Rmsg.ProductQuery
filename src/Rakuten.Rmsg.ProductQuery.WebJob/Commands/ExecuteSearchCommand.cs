// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="ExecuteSearchCommand.cs" company="Rakuten">
//   Copyright (c) Rakuten. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.WebJob
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
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
        /// Search for a specific product by GTIN in a given culture.
        /// </summary>
        /// <param name="cache">An instance that provides a method of caching collections of product.</param>
        /// <param name="type">The type of the GTIN.</param>
        /// <param name="gtin">The GTIN.</param>
        /// <param name="culture">The culture in which the product information should be retrieved.</param>
        /// <returns>A <see cref="Task"/> the represents the asynchronous operation.</returns>
        public static async Task<IEnumerable<Product>> Execute(
            ICache<IEnumerable<Product>> cache, 
            string type,
            string gtin, 
            CultureInfo culture)
        {
            Contract.Requires(cache != null);
            Contract.Requires(culture != null);

            return await cache.GetOrAddAsync(string.Concat(type, gtin, culture.Name), gtin, culture.Name);
        }

        /// <summary>
        /// Fetches a collection of <see cref="Product"/>s that match the specified GTIN and are expressed in the 
        /// supplied culture.
        /// </summary>
        /// <param name="createClient">A delegate that will create a new <see cref="ApiClient"/> instance.</param>
        /// <param name="isbnLink">
        /// A <see cref="LinkTemplate"/> to build the URI to perform a product search by ISBN.
        /// </param>
        /// <param name="eanLink">
        /// A <see cref="LinkTemplate"/> to build the URI to perform a product search by EAN.
        /// </param>
        /// <param name="janLink">
        /// A <see cref="LinkTemplate"/> to build the URI to perform a product search by JAN.
        /// </param>
        /// <param name="upcLink">
        /// A <see cref="LinkTemplate"/> to build the URI to perform a product search by UPC.
        /// </param>
        /// <param name="parameters">The parameters required to fetch a new collection of products.</param>
        /// <returns>A <see cref="Task"/> the represents the asynchronous operation.</returns>
        public static async Task<IEnumerable<Product>> GetProducts(
            Func<ApiClient> createClient,
            ProductSearchLink isbnLink,
            ProductSearchLink eanLink,
            ProductSearchLink janLink,
            ProductSearchLink upcLink,
            string[] parameters)
        {
            Contract.Requires(isbnLink != null);
            Contract.Requires(eanLink != null);
            Contract.Requires(janLink != null);
            Contract.Requires(upcLink != null);
            Contract.Requires(parameters.Length == 3);

            var culture = new CultureInfo(parameters[2]);

            ProductSearchLink searchLink;

            switch (parameters[0].ToLower())
            {
                case "isbn":
                    searchLink = isbnLink.ForGtin(parameters[1]).ForCulture(culture).Taking(PageSize);
                    break;
                case "ean":
                    searchLink = eanLink.ForGtin(parameters[1]).ForCulture(culture).Taking(PageSize);
                    break;
                case "jan":
                    searchLink = janLink.ForGtin(parameters[1]).ForCulture(culture).Taking(PageSize);
                    break;
                case "upc":
                    searchLink = upcLink.ForGtin(parameters[1]).ForCulture(culture).Taking(PageSize);
                    break;
                default:
                    return Enumerable.Empty<Product>();
            }

            Contract.Assume(searchLink != null);

            var products = new List<Product>();

            // Starting on the first page.
            var pageCount = 1;

            // Create the ApiClient instance.
            var client = createClient();
            Contract.Assume(client != null);

            // Only get the next page when the number of records returned in the last batch was the maximum.
            while (((pageCount * PageSize) - products.Count) == PageSize)
            {
                // Set the number of records to skip.
                var uri = searchLink.Skipping((PageSize - 1) * pageCount).ToUri();

                products.AddRange(await client.GetAsync<IEnumerable<Product>>(uri));

                pageCount++;
            }

            return products;
        }
    }
}
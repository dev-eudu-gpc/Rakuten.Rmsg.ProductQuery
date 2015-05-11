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
        private const int PageSize = 20;

        /// <summary>
        /// Fetches a collection of <see cref="Product"/>s that match the specified GTIN and are expressed in the 
        /// supplied culture.
        /// </summary>
        /// <param name="createClient">A delegate that will create a new <see cref="ApiClient"/> instance.</param>
        /// <param name="link">
        /// A <see cref="LinkTemplate"/> to build the URI to perform a product search.
        /// </param>
        /// <param name="parameters">The parameters required to fetch a new collection of products.</param>
        /// <returns>A <see cref="Task"/> the represents the asynchronous operation.</returns>
        public static async Task<IEnumerable<Product>> Execute(
            Func<ApiClient> createClient,
            ProductSearchLink link,
            string[] parameters)
        {
            Contract.Requires(link != null);
            Contract.Requires(parameters.Length == 3);

            var culture = new CultureInfo(parameters[2]);

            ProductSearchLink searchLink;

            switch (parameters[0].ToLower())
            {
                case "ean":
                    searchLink = link.WithEanFilter(parameters[1]);
                    break;
                case "isbn":
                    searchLink = link.WithIsbnFilter(parameters[1]);
                    break;
                case "jan":
                    searchLink = link.WithJanFilter(parameters[1]);
                    break;
                case "upc":
                    searchLink = link.WithUpcFilter(parameters[1]);
                    break;
                default:
                    return Enumerable.Empty<Product>();
            }

            searchLink = searchLink.ForCulture(culture).Taking(PageSize);

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
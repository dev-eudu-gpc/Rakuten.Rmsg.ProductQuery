// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="GetProductQueryItemCommand.cs" company="Rakuten">
//   Copyright (c) Rakuten. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.WebJob
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;

    using Rakuten.Rmsg.ProductQuery.WebJob.Entities;

    /// <summary>
    /// Retrieves the record of a GTIN search within the context of a query.
    /// </summary>
    internal class GetProductQueryItemCommand
    {
        /// <summary>
        /// Attempts to retrieve the record from the database for the specified GTIN within the context of the the given
        /// query.
        /// </summary>
        /// <param name="context">The <see cref="DbContext"/> instance using which the database can be queried.</param>
        /// <param name="id">The unique identifier of the query.</param>
        /// <param name="gtin">The Global Trade Identification Number (GTIN).</param>
        /// <returns>The matching record if one was located; otherwise, null.</returns>
        public static async Task<ProductQueryItem> Execute(ProductQueryContext context, Guid id, string gtin)
        {
            var query = await(
                from item in context.ProductQueryItems
                where item.ProductQueryId == id && item.Gtin == gtin
                select item).ToListAsync();

            if (query.Count > 1)
            {
                throw new InvalidOperationException(
                    "More than one query item was found for GTIN " + gtin + " and query " + id);
            }

            return !query.Any() ? null : query.First();
        }
    }
}
// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="GetProductQueryItemCommand.cs" company="Rakuten">
//   Copyright (c) Rakuten. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.WebJob
{
    using System;
    using System.Collections.Generic;
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
        /// <param name="queryAsync">A delegate that will asynchronously find records with matching criteria.</param>
        /// <param name="id">The unique identifier of the query.</param>
        /// <param name="gtin">The Global Trade Identification Number (GTIN).</param>
        /// <returns>A <see cref="Task"/> the represents the asynchronous operation.</returns>
        public static async Task<ProductQueryItem> Execute(
            Func<Guid, string, Task<List<ProductQueryItem>>> queryAsync, 
            Guid id, 
            string gtin)
        {
            var query = await queryAsync(id, gtin);

            if (query.Count > 1)
            {
                throw new InvalidOperationException(
                    "More than one query item was found for GTIN " + gtin + " and query " + id);
            }

            return !query.Any() ? null : query[0];
        }
    }
}
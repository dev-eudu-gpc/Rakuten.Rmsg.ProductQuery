// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="FindProductQueryItemCommand.cs" company="Rakuten">
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

    /// <summary>
    /// The get product query item command.
    /// </summary>
    internal class FindProductQueryItemCommand
    {
        /// <summary>
        /// Retrieves a list of records from the database for the specified GTIN within the context of the the given
        /// query.
        /// </summary>
        /// <param name="context">The <see cref="DbContext"/> instance using which the database can be queried.</param>
        /// <param name="id">The unique identifier of the query.</param>
        /// <param name="gtin">The Global Trade Identification Number (GTIN).</param>
        /// <returns>A <see cref="Task"/> the represents the asynchronous operation.</returns>
        public static async Task<List<ProductQueryItem>> Execute(ProductQueryContext context, Guid id, string gtin)
        {
            return await (
                from item in context.ProductQueryItems 
                where item.ProductQueryId == id && item.Gtin == gtin 
                select item)
                .ToListAsync();
        }
    }
}
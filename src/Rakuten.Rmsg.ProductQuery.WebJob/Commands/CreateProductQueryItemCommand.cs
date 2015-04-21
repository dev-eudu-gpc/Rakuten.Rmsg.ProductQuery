// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="CreateProductQueryItemCommand.cs" company="Rakuten">
//   Copyright (c) Rakuten. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.WebJob
{
    using System;
    using System.Data.Entity;
    using System.Threading.Tasks;

    using Rakuten.Rmsg.ProductQuery.WebJob.Entities;

    /// <summary>
    /// Creates a record of a product query within the persistent storage.
    /// </summary>
    public class CreateProductQueryItemCommand
    {
        /// <summary>
        /// Create the record of a product query by GTIN in the persistent storage.
        /// </summary>
        /// <param name="context">The <see cref="DbContext"/> instance using which the database can be queried.</param>
        /// <param name="id">The unique identifier of the query.</param>
        /// <param name="gtin">The Global Trade Identification Number (GTIN).</param>
        /// <returns>A <see cref="Task"/> the represents the asynchronous operation.</returns>
        public static async Task<ProductQueryItem> Execute(ProductQueryContext context, Guid id, string gtin)
        {
            var query = new ProductQueryItem { Gtin = gtin, ProductQueryId = id };

            context.ProductQueryItems.Add(query);
            await context.SaveChangesAsync();

            return query;
        }
    }
}
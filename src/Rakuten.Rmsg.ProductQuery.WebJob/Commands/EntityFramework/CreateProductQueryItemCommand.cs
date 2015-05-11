// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="CreateProductQueryItemCommand.cs" company="Rakuten">
//   Copyright (c) Rakuten. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.WebJob
{
    using System;
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
        /// <param name="saveAsync">A delegate that will asynchronously attempt to persist the record..</param>
        /// <param name="id">The unique identifier of the query.</param>
        /// <param name="gtin">The Global Trade Identification Number (GTIN).</param>
        /// <returns>A <see cref="Task"/> the represents the asynchronous operation.</returns>
        public static async Task<ProductQueryItem> Execute(Func<ProductQueryItem, Task> saveAsync, Guid id, string gtin)
        {
            var item = new ProductQueryItem { Gtin = gtin, ProductQueryId = id };

            await saveAsync(item);

            return item;
        }
    }
}
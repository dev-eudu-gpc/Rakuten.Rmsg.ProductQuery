// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="AddProductQueryItemCommand.cs" company="Rakuten">
//   Copyright (c) Rakuten. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.WebJob
{
    using System.Data.Entity;
    using System.Threading.Tasks;

    /// <summary>
    /// Persists a new <see cref="ProductQueryItem"/> into storage.
    /// </summary>
    internal class AddProductQueryItemCommand
    {
        /// <summary>
        /// Asynchronously persists a <see cref="ProductQueryItem"/> to storage.
        /// </summary>
        /// <param name="context">The <see cref="DbContext"/> to be used when persisting data.</param>
        /// <param name="entity">The details of the record to be persisted.</param>
        /// <returns>A <see cref="Task"/> the represents the asynchronous operation.</returns>
        public static Task Execute(ProductQueryContext context, ProductQueryItem entity)
        {
            context.ProductQueryItems.Attach(entity);
            context.Entry(entity).State = EntityState.Added;

            return context.SaveChangesAsync();
        }
    }
}
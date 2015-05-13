// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="UpdateProductQueryItemCommand.cs" company="Rakuten">
//   Copyright (c) Rakuten. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.WebJob
{
    using System.Data.Entity;
    using System.Threading.Tasks;

    /// <summary>
    /// Persists changes to a <see cref="ProductQueryItem"/> indicating that the value in the GRAN column has changed.
    /// </summary>
    internal class UpdateProductQueryItemCommand
    {
        /// <summary>
        /// Asynchronously updates a <see cref="ProductQueryItem"/> modifying the value in the GRAN column.
        /// </summary>
        /// <param name="context">The <see cref="DbContext"/> to be used when persisting data.</param>
        /// <param name="entity">The details of the record to be persisted.</param>
        /// <returns>A <see cref="Task"/> the represents the asynchronous operation.</returns>
        public static Task Execute(ProductQueryContext context, ProductQueryItem entity)
        {
            context.ProductQueryItems.Attach(entity);
            context.Entry(entity).Property(i => i.Gran).IsModified = true;

            return context.SaveChangesAsync();
        }
    }
}
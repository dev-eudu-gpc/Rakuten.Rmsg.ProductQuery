// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="UpdateEntityBlockCommand.cs" company="Rakuten">
//   Copyright (c) Rakuten. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.WebJob
{
    using System.Data.Entity;
    using System.Threading.Tasks;

    using Rakuten.Rmsg.ProductQuery.WebJob.Entities;

    /// <summary>
    /// Updates the record of the product query in the persistent storage.
    /// </summary>
    internal class UpdateEntityBlockCommand
    {
        /// <summary>
        /// Updates the record of the given item in the persistent storage specifying that just the GRAN has been 
        /// altered.
        /// </summary>
        /// <param name="context">The <see cref="DbContext"/> instance using which the database can be queried.</param>
        /// <param name="item">The instance representing the record to update.</param>
        /// <returns>A <see cref="Task"/> the represents the asynchronous operation.</returns>
        public static Task Execute(ProductQueryContext context, ProductQueryItem item)
        {
            return Task.Run(async () =>
            {
                context.ProductQueryItems.Attach(item);
                context.Entry(item).Property(i => i.Gran).IsModified = true;

                return await context.SaveChangesAsync();
            });
        }
    }
}
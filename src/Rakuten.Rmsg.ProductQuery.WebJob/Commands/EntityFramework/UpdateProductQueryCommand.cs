// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="UpdateProductQueryCommand.cs" company="Rakuten">
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
    /// Updates the status of a query within the database.
    /// </summary>
    internal class UpdateProductQueryCommand
    {
        /// <summary>
        /// Updates the specified product query record in the database updating its status to complete.
        /// </summary>
        /// <param name="context">The <see cref="DbContext"/> instance using which the database can be queried.</param>
        /// <param name="id">The unique identifier given to this query.</param>
        /// <returns>A <see cref="Task"/> the represents the asynchronous operation.</returns>
        public static async Task Execute(ProductQueryContext context, Guid id)
        {
            // Retrieve the identifier of the complete status.
            var status = await context.ProductQueryStatus.SingleOrDefaultAsync(s => s.Name == "completed");

            if (status == null)
            {
                throw new InvalidOperationException("The completed status could not be found.");
            }

            // Retrieve the product query record.
            var query = await context.ProductQueries.SingleOrDefaultAsync(q => q.Id == id);

            if (query == null)
            {
                throw new InvalidOperationException("A matching product query record could not be found: " + id);
            }

            // Update the status and save the changes.
            query.StatusId = status.Id;

            context.ProductQueries.Attach(query);
            context.Entry(query).Property(q => q.StatusId).IsModified = true;

            await context.SaveChangesAsync();
        }
    }
}
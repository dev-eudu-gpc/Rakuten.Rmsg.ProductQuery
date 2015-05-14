// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="GetCompletedProductQueryStatusCommand.cs" company="Rakuten">
//   Copyright (c) Rakuten. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.WebJob
{
    using System;
    using System.Data.Entity;
    using System.Threading.Tasks;

    /// <summary>
    /// Retrieves the record that represents the completed status for a product query.
    /// </summary>
    internal class GetCompletedProductQueryStatusCommand
    {
        /// <summary>
        /// Retrieves the status from the persistent storage that represents the completed status.
        /// </summary>
        /// <param name="context">The <see cref="DbContext"/> instance using which the database can be queried.</param>
        /// <returns>A <see cref="Task"/> the represents the asynchronous operation.</returns>
        public static async Task<ProductQueryStatus> Execute(ProductQueryContext context)
        {
            var status = await context.ProductQueryStatus.SingleOrDefaultAsync(s => s.Name == "completed");

            if (status == null)
            {
                throw new InvalidOperationException("The completed status could not be found.");
            }

            return status;
        }
    }
}
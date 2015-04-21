//------------------------------------------------------------------------------
// <copyright file="ProductQueryProgress.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.Web.Http
{
    using System;
    using System.Diagnostics.Contracts;

    /// <summary>
    /// Represents the progress of a product query.
    /// </summary>
    public class ProductQueryProgress
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProductQueryProgress"/> class
        /// </summary>
        public ProductQueryProgress()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductQueryProgress"/> class
        /// </summary>
        /// <param name="index">The index of the product query within it's group.</param>
        /// <param name="status">The status of the product query.</param>
        /// <param name="itemCount">The number of items in the product query.</param>
        /// <param name="completedItemCount">The number of items that have been completed.</param>
        public ProductQueryProgress(
            int index,
            string status,
            int itemCount,
            int completedItemCount)
        {
            Contract.Requires(status != null);

            this.CompletedItemCount = completedItemCount;
            this.Index = index;
            this.ItemCount = itemCount;
            this.Status = status;
        }

        /// <summary>
        /// Gets or sets the number of items in the product query that have been completed.
        /// </summary>
        public int CompletedItemCount { get; set; }

        /// <summary>
        /// Gets or sets the index of the product query within it's group.
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// Gets or sets the number of items in the product query
        /// </summary>
        public int ItemCount { get; set; }

        /// <summary>
        /// Gets the percentage of items in the query that have been completed.
        /// </summary>
        public double PercentageComplete
        {
            get
            {
                // Calculate the percentage of items completed.
                double percentage = this.ItemCount > 0 ? (double)((double)this.CompletedItemCount / this.ItemCount) : 0d;
                        
                switch (this.Status.ToLowerInvariant())
                {
                    case "new":
                        return 0;

                    case "submitted":
                        // A product query is only 100% completed when it is also in 
                        // the "completed" status.
                        return percentage == 100d ? 99d : percentage;

                    case "completed":
                        return percentage;

                    default:
                        return 0;
                }
            }
        }

        /// <summary>
        /// Gets or sets the status of the product query.
        /// </summary>
        public string Status { get; set; }
    }
}
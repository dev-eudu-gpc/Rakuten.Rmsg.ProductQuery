//------------------------------------------------------------------------------
// <copyright file="ProductQueryProgress.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.Web.Http
{
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
        /// <param name="proportionOfTimeAllocatedForFinalization">The estimated proportion of product query processing that is used by the finalization process.</param>
        public ProductQueryProgress(
            int index,
            ProductQueryStatus status,
            int itemCount,
            int completedItemCount,
            decimal proportionOfTimeAllocatedForFinalization)
        {
            this.CompletedItemCount = completedItemCount;
            this.Index = index;
            this.ItemCount = itemCount;
            this.ProportionOfTimeAllocatedForFinalization = proportionOfTimeAllocatedForFinalization;
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
        public decimal PercentageComplete
        {
            get
            {
                switch (this.Status)
                {
                    case ProductQueryStatus.New:
                        return 0m;

                    case ProductQueryStatus.Submitted:
                        // Calculate the percentage of items completed.
                        decimal percentage = this.ItemCount > 0 ? (decimal)this.CompletedItemCount / this.ItemCount : 0m;
                        
                        // Adjust the percentage to allow for finalization and return it;
                        return percentage * (1 - this.ProportionOfTimeAllocatedForFinalization);

                    case ProductQueryStatus.Completed:
                        return 1m;

                    default:
                        return 0m;
                }
            }
        }

        /// <summary>
        /// Gets or sets the estimated proportion of product query processing that is used by the finalization process.
        /// </summary>
        public decimal ProportionOfTimeAllocatedForFinalization { get; set; }

        /// <summary>
        /// Gets or sets the status of the product query.
        /// </summary>
        public ProductQueryStatus Status { get; set; }
    }
}
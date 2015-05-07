// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="FilterProductsCommand.cs" company="Rakuten">
//   Copyright (c) Rakuten. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.WebJob
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Rakuten.Rmsg.ProductQuery.WebJob.Api;

    /// <summary>
    /// Identifies the most appropriate product from a collection of products.
    /// </summary>
    internal class FilterProductsCommand
    {
        /// <summary>
        /// From the given list of products identifies the product that has the highest valued data source and has been 
        /// updated most recently whilst not having been improved.
        /// </summary>
        /// <param name="dataSources">A collection of <see cref="DataSource"/>s.</param>
        /// <param name="products">The collection of products.</param>
        /// <returns>A <see cref="Task"/> the represents the asynchronous operation.</returns>
        public static Task<Product> Execute(IEnumerable<DataSource> dataSources, IEnumerable<Product> products)
        {
            var array = dataSources as DataSource[] ?? dataSources.ToArray();

            return Task.Run(() => (
                from product in products
                where product.ImprovedId == product.Id
                from dataSource in array
                where string.Compare(product.DataSource, dataSource.Name, StringComparison.OrdinalIgnoreCase) == 0
                orderby dataSource.TrustScore descending, product.TimestampUpdate descending, product.Id descending
                select product).First());
        }
    }
}
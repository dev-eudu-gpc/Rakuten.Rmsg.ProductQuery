// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="ProductsMessageState.cs" company="Rakuten">
//   Copyright (c) Rakuten. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.WebJob
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.Diagnostics.Contracts;
    using System.Globalization;

    using Rakuten.Rmsg.ProductQuery.WebJob.Api;
    using Rakuten.Rmsg.ProductQuery.WebJob.Entities;

    /// <summary>
    /// Represents the current state of a message being processed through the DataFlow pipeline.
    /// </summary>
    internal class ProductsMessageState : QueryMessageState
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProductsMessageState"/> class.
        /// </summary>
        /// <param name="id">The unique identifier of the current query.</param>
        /// <param name="culture">The culture in which the product data has been requested.</param>
        /// <param name="item">The details of the product query request.</param>
        /// <param name="query">The record of the GTIN search.</param>
        /// <param name="products">A collection of products with the specified GTIN.</param>
        public ProductsMessageState(
            Guid id, 
            CultureInfo culture, 
            Item item, 
            ProductQueryItem query, 
            IEnumerable<Product> products)
            : base(id, culture, item, query)
        {
            Contract.Requires(products != null);

            this.Products = products.ToImmutableArray();
        }

        /// <summary>
        /// Gets the collection of products with the specified GTIN.
        /// </summary>
        public ImmutableArray<Product> Products { get; private set; }
    }
}
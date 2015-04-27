// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="ProductMessageState.cs" company="Rakuten">
//   Copyright (c) Rakuten. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.WebJob
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Globalization;

    using Rakuten.Rmsg.ProductQuery.WebJob.Api;

    /// <summary>
    /// Represents the current state of a message being processed through the DataFlow pipeline.
    /// </summary>
    internal class ProductMessageState : ItemMessageState
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProductMessageState"/> class.
        /// </summary>
        /// <param name="id">The unique identifier of the current query.</param>
        /// <param name="culture">The culture in which the product data has been requested.</param>
        /// <param name="item">The details of the product query request.</param>
        /// <param name="product">The identified products details.</param>
        public ProductMessageState(Guid id, CultureInfo culture, Item item, Product product)
            : base(id, culture, item)
        {
            Contract.Requires(product != null);

            this.Product = product;
        }

        /// <summary>
        /// Gets the information pertaining to the requested product.
        /// </summary>
        public Product Product { get; private set; }
    }
}
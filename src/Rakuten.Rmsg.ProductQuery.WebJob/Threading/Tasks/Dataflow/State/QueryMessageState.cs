// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="QueryMessageState.cs" company="Rakuten">
//   Copyright (c) Rakuten. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.WebJob
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Globalization;

    using Rakuten.Rmsg.ProductQuery.WebJob.Entities;

    /// <summary>
    /// Represents the current state of a message being processed through the DataFlow pipeline.
    /// </summary>
    internal class QueryMessageState : ItemMessageState
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="QueryMessageState"/> class.
        /// </summary>
        /// <param name="id">The unique identifier of the current query.</param>
        /// <param name="culture">The culture in which the product data has been requested.</param>
        /// <param name="item">The details of the product query request.</param>
        /// <param name="query">The record of the GTIN search.</param>
        public QueryMessageState(Guid id, CultureInfo culture, Item item, ProductQueryItem query)
            : base(id, culture, item)
        {
            Contract.Requires(query != null);

            this.Query = query;
        }

        /// <summary>
        /// Gets the database record of the search.
        /// </summary>
        public ProductQueryItem Query { get; private set; }
    }
}
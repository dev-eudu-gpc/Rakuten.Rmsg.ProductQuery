// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="MessageState.cs" company="Rakuten">
//   Copyright (c) Rakuten. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.WebJob
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Globalization;
    using System.Linq;

    using Rakuten.Rmsg.ProductQuery.WebJob.Api;
    using Rakuten.Rmsg.ProductQuery.WebJob.Entities;
    using Rakuten.Threading.Tasks.Dataflow;

    /// <summary>
    /// Represents the current state of a message being processed through the DataFlow pipeline.
    /// </summary>
    internal class MessageState : IMessageState
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MessageState"/> class.
        /// </summary>
        /// <param name="id">The unique identifier of the current query.</param>
        /// <param name="culture">The culture in which the product data has been requested.</param>
        /// <param name="item">The details of the product query request..</param>
        /// <param name="query">The record of the GTIN search.</param>
        /// <param name="products">A collection of products with the specified GTIN.</param>
        public MessageState(
            Guid id, 
            CultureInfo culture, 
            Item item, 
            ProductQueryItem query, 
            IEnumerable<Product> products)
            : this(id, culture, item, query)
        {
            Contract.Requires(products != null);

            this.Products = products;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageState"/> class.
        /// </summary>
        /// <param name="id">The unique identifier of the current query.</param>
        /// <param name="culture">The culture in which the product data has been requested.</param>
        /// <param name="item">The details of the product query request..</param>
        /// <param name="query">The record of the GTIN search.</param>
        public MessageState(Guid id, CultureInfo culture, Item item, ProductQueryItem query)
            : this(id, culture, item)
        {
            Contract.Requires(query != null);

            this.Query = query;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageState"/> class.
        /// </summary>
        /// <param name="id">The unique identifier of the current query.</param>
        /// <param name="culture">The culture in which the product data has been requested.</param>
        /// <param name="item">The details of the product query request..</param>
        public MessageState(Guid id, CultureInfo culture, Item item)
        {
            Contract.Requires(id != Guid.Empty);
            Contract.Requires(item != null);

            this.Id = id;
            this.Item = item;

            this.Culture = culture;

            this.Exceptions = Enumerable.Empty<Exception>();
        }

        /// <summary>
        /// Gets the culture in which product data has been requested.
        /// </summary>
        public CultureInfo Culture { get; private set; }

        /// <summary>
        /// Gets the collection of exceptions that detail the issues encountered when processing this message.
        /// </summary>
        public IEnumerable<Exception> Exceptions { get; private set; }

        /// <summary>
        /// Gets the unique identifier given to this query.
        /// </summary>
        public Guid Id { get; private set; }

        /// <summary>
        /// Gets the unique identifier given to this query.
        /// </summary>
        public Item Item { get; private set; }

        /// <summary>
        /// Gets the collection of products with the specified GTIN.
        /// </summary>
        public IEnumerable<Product> Products { get; private set; } 

        /// <summary>
        /// Gets the database record of the search.
        /// </summary>
        public ProductQueryItem Query { get; private set; }
    }
}
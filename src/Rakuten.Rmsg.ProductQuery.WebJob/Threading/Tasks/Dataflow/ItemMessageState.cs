// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="ItemMessageState.cs" company="Rakuten">
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
    using System.Linq;

    using Rakuten.Rmsg.ProductQuery.WebJob.Api;
    using Rakuten.Threading.Tasks.Dataflow;

    /// <summary>
    /// The item message state 2.
    /// </summary>
    internal class ItemMessageState : MessageState
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ItemMessageState"/> class.
        /// </summary>
        /// <param name="id">The unique identifier of the current query.</param>
        /// <param name="culture">The culture in which the product data has been requested.</param>
        /// <param name="item">The details of the product query request.</param>
        /// <param name="product">The identified products details.</param>
        /// <param name="exceptions">
        /// A collection of <see cref="Exception"/>s detailing any issues encountered whilst processing this message.
        /// </param>
        public ItemMessageState(
            Guid id, 
            CultureInfo culture, 
            Item item, 
            Product product, 
            IEnumerable<Exception> exceptions = null)
            : this(id, culture, item, exceptions)
        {
            Contract.Requires(product != null);

            this.Product = product;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemMessageState"/> class.
        /// </summary>
        /// <param name="id">The unique identifier of the current query.</param>
        /// <param name="culture">The culture in which the product data has been requested.</param>
        /// <param name="item">The details of the product query request.</param>
        /// <param name="query">The record of the GTIN search.</param>
        /// <param name="products">A collection of products with the specified GTIN.</param>
        /// <param name="exceptions">
        /// A collection of <see cref="Exception"/>s detailing any issues encountered whilst processing this message.
        /// </param>
        public ItemMessageState(
            Guid id, 
            CultureInfo culture, 
            Item item, 
            ProductQueryItem query, 
            IEnumerable<Product> products,
            IEnumerable<Exception> exceptions = null)
            : this(id, culture, item, query, exceptions)
        {
            Contract.Requires(products != null);

            this.Products = products.ToImmutableArray();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemMessageState"/> class.
        /// </summary>
        /// <param name="id">The unique identifier of the current query.</param>
        /// <param name="culture">The culture in which the product data has been requested.</param>
        /// <param name="item">The details of the product query request.</param>
        /// <param name="query">The record of the GTIN search.</param>
        /// <param name="exceptions">
        /// A collection of <see cref="Exception"/>s detailing any issues encountered whilst processing this message.
        /// </param>
        public ItemMessageState(
            Guid id, 
            CultureInfo culture, 
            Item item, 
            ProductQueryItem query, 
            IEnumerable<Exception> exceptions = null)
            : this(id, culture, item, exceptions)
        {
            Contract.Requires(query != null);

            this.Query = query;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemMessageState"/> class.
        /// </summary>
        /// <param name="id">The unique identifier of the current query.</param>
        /// <param name="culture">The culture in which the product data has been requested.</param>
        /// <param name="item">The details of the product query request.</param>
        /// <param name="exceptions">
        /// A collection of <see cref="Exception"/>s detailing any issues encountered whilst processing this message.
        /// </param>
        public ItemMessageState(Guid id, CultureInfo culture, Item item, IEnumerable<Exception> exceptions = null)
        {
            Contract.Requires(id != Guid.Empty);
            Contract.Requires(item != null);

            this.Id = id;
            this.Item = item;

            this.Culture = culture;

            this.Exceptions = exceptions == null ? new ImmutableArray<Exception>() : exceptions.ToImmutableArray();
        }

        /// <summary>
        /// Gets the culture in which product data has been requested.
        /// </summary>
        public CultureInfo Culture { get; private set; }

        /// <summary>
        /// Gets the unique identifier given to this query.
        /// </summary>
        public Guid Id { get; private set; }

        /// <summary>
        /// Gets the unique identifier given to this query.
        /// </summary>
        public Item Item { get; private set; }

        /// <summary>
        /// Gets the information pertaining to the requested product.
        /// </summary>
        public Product Product { get; private set; }

        /// <summary>
        /// Gets the collection of products with the specified GTIN.
        /// </summary>
        public ImmutableArray<Product> Products { get; private set; }

        /// <summary>
        /// Gets the database record of the search.
        /// </summary>
        public ProductQueryItem Query { get; private set; }

        /// <summary>
        /// Inserts a new <see cref="Exception"/> into the collection of exceptions.
        /// </summary>
        /// <param name="exception">An <see cref="Exception"/> detailing an issue encountered.</param>
        /// <returns>A new <see cref="ItemMessageState"/> instance containing the exception.</returns>
        public ItemMessageState AddException(Exception exception)
        {
            var exceptions = this.Exceptions.IsDefaultOrEmpty ? new List<Exception>() : this.Exceptions.ToList();
            exceptions.Add(exception);

            if (this.Product != null)
            {
                return new ItemMessageState(this.Id, this.Culture, this.Item, this.Product, exceptions);
            }

            if (this.Products != null)
            {
                return new ItemMessageState(this.Id, this.Culture, this.Item, this.Query, this.Products, exceptions);
            }

            return this.Query != null ? 
                new ItemMessageState(this.Id, this.Culture, this.Item, this.Query, exceptions) : 
                new ItemMessageState(this.Id, this.Culture, this.Item, exceptions);
        }
    }
}
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
    using System.Linq;

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
        /// <param name="item">The details of the product query request..</param>
        /// <param name="queryItem">The record of the GTIN search.</param>
        public MessageState(Guid id, Item item, ProductQueryItem queryItem)
            : this(id, item)
        {
            Contract.Requires(item != null);

            this.QueryItem = queryItem;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageState"/> class.
        /// </summary>
        /// <param name="id">The unique identifier of the current query.</param>
        /// <param name="item">The details of the product query request..</param>
        public MessageState(Guid id, Item item)
        {
            Contract.Requires(id != Guid.Empty);
            Contract.Requires(item != null);

            this.Id = id;
            this.Item = item;
            this.Exceptions = Enumerable.Empty<Exception>();
        }

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
        /// Gets the database record of the search.
        /// </summary>
        public ProductQueryItem QueryItem { get; private set; }
    }
}
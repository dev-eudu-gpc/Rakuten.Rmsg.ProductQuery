// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="ItemMessageState.cs" company="Rakuten">
//   Copyright (c) Rakuten. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.WebJob
{
    using System;
    using System.Collections.Immutable;
    using System.Diagnostics.Contracts;
    using System.Globalization;

    using Rakuten.Threading.Tasks.Dataflow;

    /// <summary>
    /// Represents the current state of a message being processed through the DataFlow pipeline.
    /// </summary>
    internal class ItemMessageState : MessageState
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ItemMessageState"/> class.
        /// </summary>
        /// <param name="id">The unique identifier of the current query.</param>
        /// <param name="culture">The culture in which the product data has been requested.</param>
        /// <param name="item">The details of the product query request.</param>
        public ItemMessageState(Guid id, CultureInfo culture, Item item)
        {
            Contract.Requires(id != Guid.Empty);
            Contract.Requires(item != null);

            this.Id = id;
            this.Item = item;

            this.Culture = culture;

            this.Exceptions = new ImmutableArray<Exception>();
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
    }
}
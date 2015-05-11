//------------------------------------------------------------------------------
// <copyright file="DispatchMessageCommandParameters.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.Web.Http.Commands
{
    using System;
    using System.Diagnostics.Contracts;

    /// <summary>
    /// The parameters required for dispatching a product query message to the queue.
    /// </summary>
    internal class DispatchMessageCommandParameters
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DispatchMessageCommandParameters"/> class.
        /// </summary>
        /// <param name="id">The identifier of the product query.</param>
        /// <param name="cultureName">The culture of the product query.</param>
        /// <param name="blobLink">The product query to dispatch a message for.</param>
        public DispatchMessageCommandParameters(Guid id, string cultureName, Link blobLink)
        {
            Contract.Requires(blobLink != null);
            Contract.Requires(cultureName != null);
            Contract.Requires(id != null);

            this.BlobLink = blobLink;
            this.CultureName = cultureName;
            this.Id = id;
        }

        /// <summary>
        /// Gets the blob uri for the product query.
        /// </summary>
        public Link BlobLink { get; private set; }

        /// <summary>
        /// Gets or sets the culture of the product query.
        /// </summary>
        public string CultureName { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the product query.
        /// </summary>
        public Guid Id { get; set; }
    }
}
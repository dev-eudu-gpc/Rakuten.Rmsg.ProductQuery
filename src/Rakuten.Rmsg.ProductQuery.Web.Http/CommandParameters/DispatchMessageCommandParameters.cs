//------------------------------------------------------------------------------
// <copyright file="DispatchMessageCommandParameters.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.Web.Http.Commands
{
    using System;
    using System.Diagnostics.Contracts;
    using Rakuten.Rmsg.ProductQuery.Web.Http.Links;

    /// <summary>
    /// The parameters required for dispatching a product query message to the queue.
    /// </summary>
    public class DispatchMessageCommandParameters
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DispatchMessageCommandParameters"/> class.
        /// </summary>
        /// <param name="productQuery">The product query to dispatch a message for.</param>
        public DispatchMessageCommandParameters(Link productQuery)
        {
            Contract.Requires(productQuery != null);

            this.ProductQuery = productQuery;
        }

        /// <summary>
        /// Gets the product query to dispatch a message for.
        /// </summary>
        public Link ProductQuery { get; private set; }
    }
}
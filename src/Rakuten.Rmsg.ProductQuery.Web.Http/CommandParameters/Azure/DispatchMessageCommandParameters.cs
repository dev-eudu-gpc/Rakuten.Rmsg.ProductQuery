//------------------------------------------------------------------------------
// <copyright file="DispatchMessageCommandParameters.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.Web.Http.Commands
{
    using System.Diagnostics.Contracts;

    /// <summary>
    /// The parameters required for dispatching a product query message to the queue.
    /// </summary>
    internal class DispatchMessageCommandParameters
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DispatchMessageCommandParameters"/> class.
        /// </summary>
        /// <param name="blobLink">The product query to dispatch a message for.</param>
        public DispatchMessageCommandParameters(Link blobLink)
        {
            Contract.Requires(blobLink != null);

            this.BlobLink = blobLink;
        }

        /// <summary>
        /// Gets the product query to dispatch a message for.
        /// </summary>
        public Link BlobLink { get; private set; }
    }
}
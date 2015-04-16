//------------------------------------------------------------------------------
// <copyright file="ReadyForProcessingCommandParameters.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.Web.Http.Commands
{
    using System;
    using System.Diagnostics.Contracts;

    /// <summary>
    /// The parameters required for flagging a product query as ready for processing.
    /// </summary>
    public class ReadyForProcessingCommandParameters
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReadyForProcessingCommandParameters"/> class
        /// </summary>
        /// <param name="id">The unique identifier for the product query</param>
        public ReadyForProcessingCommandParameters(Guid id)
        {
            Contract.Requires(id != null);

            this.Id = id;
        }

        /// <summary>
        /// Gets the unique identifier of the product query
        /// </summary>
        public Guid Id { get; private set; }
    }
}
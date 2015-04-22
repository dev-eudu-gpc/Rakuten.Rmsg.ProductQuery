//------------------------------------------------------------------------------
// <copyright file="CreateCommandParameters.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.Web.Http.Commands
{
    using System;
    using System.Diagnostics.Contracts;

    /// <summary>
    /// The parameters required for creating a new product query.
    /// </summary>
    public class CreateCommandParameters
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateCommandParameters"/> class
        /// </summary>
        /// <param name="id">The unique identifier for the product query</param>
        public CreateCommandParameters(Guid id)
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
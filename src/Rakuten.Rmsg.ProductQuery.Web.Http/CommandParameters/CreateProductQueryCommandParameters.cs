//------------------------------------------------------------------------------
// <copyright file="CreateProductQueryCommandParameters.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.Web.Http.Commands
{
    using System;
    using System.Diagnostics.Contracts;

    /// <summary>
    /// The parameters for the <see cref="CreateProductQueryCommand"/> class.
    /// </summary>
    public class CreateProductQueryCommandParameters
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateProductQueryCommandParameters"/> class
        /// </summary>
        /// <param name="id">The unique identifier for the product query</param>
        public CreateProductQueryCommandParameters(Guid id)
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
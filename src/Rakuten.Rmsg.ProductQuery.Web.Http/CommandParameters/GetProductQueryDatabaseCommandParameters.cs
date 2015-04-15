//------------------------------------------------------------------------------
// <copyright file="GetProductQueryDatabaseCommandParameters.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.Web.Http.Commands
{
    using System;
    using System.Diagnostics.Contracts;

    /// <summary>
    /// The parameters for the <see cref="GetProductQueryDatabaseCommand"/> class.
    /// </summary>
    public class GetProductQueryDatabaseCommandParameters
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetProductQueryDatabaseCommandParameters"/> class.
        /// </summary>
        /// <param name="id">The unique identifier for the product query.</param>
        public GetProductQueryDatabaseCommandParameters(Guid id)
        {
            Contract.Requires(id != null);

            this.Id = id;
        }

        /// <summary>
        /// Gets the unique identifier of the product query.
        /// </summary>
        public Guid Id { get; private set; }
    }
}
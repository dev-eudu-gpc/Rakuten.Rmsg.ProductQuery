//------------------------------------------------------------------------------
// <copyright file="CreateProductQueryDatabaseCommandParameters.cs" company="Rakuten">
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
    public class CreateProductQueryDatabaseCommandParameters
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateProductQueryDatabaseCommandParameters"/> class.
        /// </summary>
        /// <param name="id">The unique identifier for the product query.</param>
        /// <param name="dateCreated">The date/time on which the product query was created.</param>
        public CreateProductQueryDatabaseCommandParameters(
            Guid id,
            DateTime dateCreated)
        {
            Contract.Requires(id != null);
            Contract.Requires(dateCreated != null);

            this.Id = id;
            this.DateCreated = dateCreated;
        }

        /// <summary>
        /// Gets the unique identifier of the product query.
        /// </summary>
        public Guid Id { get; private set; }

        /// <summary>
        /// Gets the date and time on which the product query was created
        /// </summary>
        public DateTime DateCreated { get; private set; }
    }
}
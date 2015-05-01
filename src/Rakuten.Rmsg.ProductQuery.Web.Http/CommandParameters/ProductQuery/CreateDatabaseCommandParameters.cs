//------------------------------------------------------------------------------
// <copyright file="CreateDatabaseCommandParameters.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.Web.Http.Commands
{
    using System;
    using System.Diagnostics.Contracts;
using System.Globalization;

    /// <summary>
    /// The parameters required for inserting a new product query into the database.
    /// </summary>
    internal class CreateDatabaseCommandParameters : ProductQueryCommandParameters
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateDatabaseCommandParameters"/> class.
        /// </summary>
        /// <param name="id">The unique identifier for the product query.</param>
        /// <param name="culture">The culture for the products in the query.</param>
        /// <param name="dateCreated">The date/time on which the product query was created.</param>
        public CreateDatabaseCommandParameters(
            Guid id,
            CultureInfo culture,
            DateTime dateCreated)
            : base(id, culture)
        {
            Contract.Requires(culture != null);

            this.DateCreated = dateCreated;
        }

        /// <summary>
        /// Gets the date and time on which the product query was created
        /// </summary>
        public DateTime DateCreated { get; private set; }
    }
}
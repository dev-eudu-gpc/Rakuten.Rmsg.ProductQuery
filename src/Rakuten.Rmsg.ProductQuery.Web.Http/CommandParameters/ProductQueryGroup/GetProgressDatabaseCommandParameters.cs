//------------------------------------------------------------------------------
// <copyright file="GetProgressDatabaseCommandParameters.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.Web.Http.Commands
{
    using System;
    using System.Diagnostics.Contracts;

    /// <summary>
    /// The parameters required for obtaining, from a database, the progress of all product 
    /// queries within a given product query group.
    /// </summary>
    internal class GetProgressDatabaseCommandParameters
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetProgressDatabaseCommandParameters"/> class.
        /// </summary>
        /// <param name="id">The unique identifier for the product query group.</param>
        /// <param name="dateTime">The point in time that the status is determined.</param>
        public GetProgressDatabaseCommandParameters(
            Guid id,
            DateTime dateTime)
        {
            this.Datetime = dateTime;
            this.Id = id;
        }

        /// <summary>
        /// Gets the point in time that the progress is determined.
        /// </summary>
        public DateTime Datetime { get; private set; }

        /// <summary>
        /// Gets the unique identifier of the product query group.
        /// </summary>
        public Guid Id { get; private set; }
    }
}
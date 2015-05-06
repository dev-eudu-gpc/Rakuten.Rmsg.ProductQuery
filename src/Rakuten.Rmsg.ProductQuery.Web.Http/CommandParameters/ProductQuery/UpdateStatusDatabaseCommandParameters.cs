﻿//------------------------------------------------------------------------------
// <copyright file="UpdateStatusDatabaseCommandParameters.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.Web.Http.Commands
{
    using System;
    using System.Diagnostics.Contracts;

    /// <summary>
    /// The parameters required for updating the status of a product query in the database
    /// </summary>
    internal class UpdateStatusDatabaseCommandParameters
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateStatusDatabaseCommandParameters"/> class.
        /// </summary>
        /// <param name="id">The unique identifier for the product query.</param>
        /// <param name="newStatus">The new status for the product query.</param>
        public UpdateStatusDatabaseCommandParameters(
            Guid id,
            string newStatus)
        {
            Contract.Requires(newStatus != null);

            this.Id = id;
            this.NewStatus = newStatus;
        }

        /// <summary>
        /// Gets the unique identifier of the product query.
        /// </summary>
        public Guid Id { get; private set; }

        /// <summary>
        /// Gets the new status for the product query.
        /// </summary>
        public string NewStatus { get; private set; }
    }
}
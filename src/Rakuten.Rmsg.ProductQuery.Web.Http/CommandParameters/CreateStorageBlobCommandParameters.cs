//------------------------------------------------------------------------------
// <copyright file="CreateStorageBlobCommandParameters.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.Web.Http.Commands
{
    using System;
    using System.Diagnostics.Contracts;

    /// <summary>
    /// The parameters for the <see cref="CreateStorageBlobCommand"/> class.
    /// </summary>
    public class CreateStorageBlobCommandParameters
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateStorageBlobCommandParameters"/> class.
        /// </summary>
        /// <param name="dateCreated">Date/time on which the product query was created.</param>
        /// <param name="id">The unique identifier for the product query.</param>
        public CreateStorageBlobCommandParameters(DateTime dateCreated, Guid id)
        {
            Contract.Requires(dateCreated != null);
            Contract.Requires(id != null);

            this.Id = id;
        }

        /// <summary>
        /// Gets the date/time on which the product query was created.
        /// </summary>
        public DateTime DateCreated { get; private set; }

        /// <summary>
        /// Gets the unique identifier of the product query.
        /// </summary>
        public Guid Id { get; private set; }
    }
}
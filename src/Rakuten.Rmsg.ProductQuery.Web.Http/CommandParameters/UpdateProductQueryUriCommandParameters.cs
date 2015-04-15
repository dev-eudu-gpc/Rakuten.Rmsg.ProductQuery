//------------------------------------------------------------------------------
// <copyright file="UpdateProductQueryUriCommandParameters.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.Web.Http.Commands
{
    using System;
    using System.Diagnostics.Contracts;

    /// <summary>
    /// The parameters for the <see cref="UpdateProductQueryUriCommand"/> class.
    /// </summary>
    public class UpdateProductQueryUriCommandParameters
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateProductQueryUriCommandParameters"/> class.
        /// </summary>
        /// <param name="id">The unique identifier for the product query.</param>
        /// <param name="uri">The uri for the product query blob in storage.</param>
        public UpdateProductQueryUriCommandParameters(
            Guid id,
            Uri uri)
        {
            Contract.Requires(id != null);
            Contract.Requires(uri != null);

            this.Id = id;
            this.Uri = uri;
        }

        /// <summary>
        /// Gets the unique identifier of the product query.
        /// </summary>
        public Guid Id { get; private set; }

        /// <summary>
        /// Gets the uri for the product query blob in storage.
        /// </summary>
        public Uri Uri { get; private set; }
    }
}
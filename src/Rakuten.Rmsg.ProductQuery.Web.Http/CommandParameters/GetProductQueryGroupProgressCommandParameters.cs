//------------------------------------------------------------------------------
// <copyright file="GetProductQueryGroupProgressCommandParameters.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.Web.Http.Commands
{
    using System;
    using System.Diagnostics.Contracts;

    /// <summary>
    /// The parameters for the <see cref="GetProductQueryGroupProgressCommand"/> class.
    /// </summary>
    public class GetProductQueryGroupProgressCommandParameters
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetProductQueryGroupProgressCommandParameters"/> class.
        /// </summary>
        /// <param name="id">The unique identifier for the product query group.</param>
        /// <param name="dateTime">The date and time for the status of the product query group.</param>
        public GetProductQueryGroupProgressCommandParameters(
            Guid id,
            DateTime dateTime)
        {
            Contract.Requires(id != null);

            this.DateTime = dateTime;
            this.Id = id;
        }

        /// <summary>
        /// Gets the point in time that the progress is determined.
        /// </summary>
        public DateTime DateTime { get; private set; }

        /// <summary>
        /// Gets the unique identifier of the product query group.
        /// </summary>
        public Guid Id { get; private set; }
    }
}
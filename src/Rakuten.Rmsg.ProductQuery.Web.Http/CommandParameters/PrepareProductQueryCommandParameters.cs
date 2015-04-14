//------------------------------------------------------------------------------
// <copyright file="PrepareProductQueryCommandParameters" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.Web.Http.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;

    /// <summary>
    /// The parameters for the <see cref="PrepareProductQueryCommand"/> class.
    /// </summary>
    public class PrepareProductQueryCommandParameters
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PrepareProductQueryCommandParameters"/> class
        /// </summary>
        /// <param name="id">The unique identifier for the product query</param>
        public PrepareProductQueryCommandParameters(Guid id)
        {
            this.Id = id;
        }

        /// <summary>
        /// Gets or sets the unique identifier of the product query
        /// </summary>
        public Guid Id { get; private set; }
    }
}
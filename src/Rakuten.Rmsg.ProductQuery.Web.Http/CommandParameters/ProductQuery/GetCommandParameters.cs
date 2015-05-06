//------------------------------------------------------------------------------
// <copyright file="GetCommandParameters.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.Web.Http.Commands
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Globalization;

    /// <summary>
    /// The parameters required for obtaining an individual product query
    /// </summary>
    internal class GetCommandParameters : ProductQueryCommandParameters
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetCommandParameters"/> class
        /// </summary>
        /// <param name="id">The unique identifier for the product query</param>
        /// <param name="culture">The culture of products for the query.</param>
        public GetCommandParameters(string id, string culture)
            : base(id, culture)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GetCommandParameters"/> class
        /// </summary>
        /// <param name="id">The unique identifier for the product query</param>
        /// <param name="culture">The culture of products for the query.</param>
        public GetCommandParameters(Guid id, CultureInfo culture)
            : base(id, culture)
        {
        }
    }
}
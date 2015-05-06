//------------------------------------------------------------------------------
// <copyright file="CreateCommandParameters.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.Web.Http.Commands
{
    using System;
    using System.Globalization;

    /// <summary>
    /// The parameters required for creating a new product query.
    /// </summary>
    internal class CreateCommandParameters : ProductQueryCommandParameters
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateCommandParameters"/> class
        /// </summary>
        /// <param name="id">The unique identifier for the product query</param>
        /// <param name="culture">The culture of products for the query.</param>
        public CreateCommandParameters(Guid id, CultureInfo culture)
            : base(id, culture)
        {
        }
    }
}
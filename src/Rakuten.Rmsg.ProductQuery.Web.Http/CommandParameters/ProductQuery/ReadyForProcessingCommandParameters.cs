//------------------------------------------------------------------------------
// <copyright file="ReadyForProcessingCommandParameters.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.Web.Http.Commands
{
    using System;
using System.Globalization;

    /// <summary>
    /// The parameters required for flagging a product query as ready for processing.
    /// </summary>
    internal class ReadyForProcessingCommandParameters : ProductQueryCommandParameters
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReadyForProcessingCommandParameters"/> class
        /// </summary>
        /// <param name="id">The unique identifier for the product query</param>
        /// <param name="culture">The culture of products for the query.</param>
        public ReadyForProcessingCommandParameters(string id, string culture)
            : base(id, culture)
        {
        }
    }
}
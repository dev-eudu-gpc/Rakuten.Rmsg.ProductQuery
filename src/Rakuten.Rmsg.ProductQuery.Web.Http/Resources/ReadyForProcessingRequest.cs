//------------------------------------------------------------------------------
// <copyright file="ReadyForProcessingRequest.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.Web.Http
{
    /// <summary>
    /// Represents the the body of a request for flagging a product
    /// query as ready for processing.
    /// </summary>
    public class ReadyForProcessingRequest
    {
        /// <summary>
        /// Gets or sets the status to set the product query to.
        /// </summary>
        public string Status { get; set; }
    }
}
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InvalidStatusException.cs" company="Rakuten">
//   Copyright (c) Rakuten. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.Web.Http
{
    using System.Xml.Serialization;

    using Newtonsoft.Json;
    using Rakuten.Gpc;

    /// <summary>
    /// Represents the error raised when trying to set the status of a product query
    /// to an invalid status.
    /// </summary>
    [ApiException("http://problems.rakuten.co.uk/invalid-product-query-status", "The status supplied was invalid.")]
    internal class InvalidStatusException : ApiException
    {
        /// <summary>
        /// The message that describes this error.
        /// </summary>
        private const string Detail =
            @"The only valid status that a product query can be set to is ""submitted"".";

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidStatusException"/> class.
        /// </summary>
        public InvalidStatusException()
            : base(Detail)
        {
        }
    }
}
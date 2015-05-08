// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProductQueryNotFoundException.cs" company="Rakuten">
//   Copyright (c) Rakuten. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.Web.Http
{
    using System;
    using System.Xml.Serialization;
    using Newtonsoft.Json;
    using Rakuten.Gpc;

    /// <summary>
    /// Represents the error raised when a specified product query cannot be found.
    /// </summary>
    [ApiException("http://problems.rakuten.com/product-query-not-found", "The product query could not be found.")]
    internal class ProductQueryNotFoundException : ApiException
    {
        /// <summary>
        /// The message that describes this error.
        /// </summary>
        private const string Detail =
            @"A product query with ID '{0}' cannot be found.";

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductQueryNotFoundException"/> class.
        /// </summary>
        public ProductQueryNotFoundException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductQueryNotFoundException"/> class.
        /// </summary>
        /// <param name="id">The value for the identifier that was invalid.</param>
        public ProductQueryNotFoundException(Guid id)
            : base(string.Format(Detail, id.ToString()))
        {
            this.Id = id;
        }

        /// <summary>
        /// Gets or sets the invalid value.
        /// </summary>
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement("id")]
        public Guid Id { get; set; }
    }
}
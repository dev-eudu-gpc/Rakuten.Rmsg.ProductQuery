// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProductQueryNotFoundException.cs" company="Rakuten">
//   Copyright (c) Rakuten. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.Web.Http
{
    using System.Xml.Serialization;

    using Newtonsoft.Json;
    using Rakuten.Gpc;

    /// <summary>
    /// Represents the error raised when a specified product query cannot be found.
    /// </summary>
    [ApiException("http://problems.rakuten.co.uk/product-query-not-found", "The product query could not be found.")]
    public class ProductQueryNotFoundException : ApiException
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
        /// <param name="value">The value for the identifier that was invalid.</param>
        public ProductQueryNotFoundException(string value)
            : base(string.Format(Detail, value))
        {
            this.Value = value;
        }

        /// <summary>
        /// Gets or sets the invalid value.
        /// </summary>
        [JsonProperty("value", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement("value")]
        public string Value { get; set; }
    }
}
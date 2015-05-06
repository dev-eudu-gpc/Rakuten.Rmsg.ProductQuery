// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProductQueryCultureNotFoundException.cs" company="Rakuten">
//   Copyright (c) Rakuten. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.Web.Http
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Globalization;
    using System.Xml.Serialization;
    using Newtonsoft.Json;
    using Rakuten.Gpc;

    /// <summary>
    /// Represents the error raised when a specified product query cannot be found.
    /// </summary>
    [ApiException("http://problems.rakuten.co.uk/product-query-not-found", "The product query could not be found.")]
    internal class ProductQueryCultureNotFoundException : ApiException
    {
        /// <summary>
        /// The message that describes this error.
        /// </summary>
        private const string Detail =
            @"Failed to find a product query with identifier '{0}' and culture '{1}'";

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductQueryCultureNotFoundException"/> class.
        /// </summary>
        public ProductQueryCultureNotFoundException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductQueryCultureNotFoundException"/> class.
        /// </summary>
        /// <param name="id">The value for the identifier.</param>
        /// <param name="expectedCulture">The expected culture.</param>
        /// <param name="productQuery">The product query that was found.</param>
        public ProductQueryCultureNotFoundException(Guid id, CultureInfo expectedCulture, ProductQuery productQuery)
            : base(string.Format(Detail, id.ToString(), expectedCulture.Name))
        {
            Contract.Requires(expectedCulture != null);
            Contract.Requires(productQuery != null);
            
            this.ExpectedCulture = expectedCulture;
            this.Id = id;
            this.ProductQuery = productQuery;
        }

        /// <summary>
        /// Gets or sets the expected culture.
        /// </summary>
        [JsonProperty("expectedCulture", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement("expectedCulture")]
        public CultureInfo ExpectedCulture { get; set; }

        /// <summary>
        /// Gets or sets the invalid value.
        /// </summary>
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement("id")]
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the product query that was found.
        /// </summary>
        [JsonIgnore]
        [XmlIgnore]
        public ProductQuery ProductQuery { get; set; }
    }
}
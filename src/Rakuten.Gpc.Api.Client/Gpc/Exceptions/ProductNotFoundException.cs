// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProductNotFoundException.cs" company="Rakuten">
//   Copyright (c) Rakuten. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Rakuten.Gpc
{
    using System.Xml.Serialization;

    using Newtonsoft.Json;

    /// <summary>
    /// Represents the error raised when a search for a product via GRAN returned no product.
    /// </summary>
    [ApiException("http://problems.rakuten.co.uk/product-not-found", "The requested product was not found.")]
    public class ProductNotFoundException : ApiException
    {
        /// <summary>
        /// The format of the message that describes this error.
        /// </summary>
        private const string Detail = "A product identified by GRAN '{0}' with a culture of '{1}' was not found.";

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductNotFoundException"/> class.
        /// </summary>
        public ProductNotFoundException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductNotFoundException"/> class.
        /// </summary>
        /// <param name="gran">
        /// The GRAN that could not be located.
        /// </param>
        /// <param name="culture">
        /// The culture requested.
        /// </param>
        public ProductNotFoundException(string gran, string culture)
            : base(string.Format(Detail, gran, culture))
        {
            this.Culture = culture;
            this.Gran = gran;
        }

        /// <summary>
        /// Gets or sets the culture which was supplied in the request.
        /// </summary>
        [JsonProperty("culture", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement("culture")]
        public string Culture { get; set; }

        /// <summary>
        /// Gets or sets the GRAN which was supplied in the request that could not be found.
        /// </summary>
        [JsonProperty("gran", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement("gran")]
        public string Gran { get; set; }
    }
}

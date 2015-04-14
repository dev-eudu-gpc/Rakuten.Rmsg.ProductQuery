//------------------------------------------------------------------------------
// <copyright file="ProductImage.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
namespace Rakuten.Gpc.Resources
{
    using System.Xml.Serialization;

    using Newtonsoft.Json;

    /// <summary>
    /// Represents an image for a product
    /// </summary>
    [XmlRoot("image")]
    public class ProductImage
    {
        /// <summary>
        /// Gets or sets the alternative text for the image.
        /// </summary>
        [JsonProperty("alternateText")]
        [XmlElement("alternateText")]
        public string AlternateText { get; set; }

        /// <summary>
        /// Gets or sets the URI for the image.
        /// </summary>
        [JsonProperty("uri")]
        [XmlElement("uri")]
        public string Uri { get; set; }
    }
}
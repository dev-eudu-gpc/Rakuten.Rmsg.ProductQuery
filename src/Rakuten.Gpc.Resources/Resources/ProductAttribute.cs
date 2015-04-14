//------------------------------------------------------------------------------
// <copyright file="ProductAttribute.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
namespace Rakuten.Gpc.Resources
{
    using System.Xml.Serialization;

    using Newtonsoft.Json;

    /// <summary>
    /// Represents a product attribute.
    /// </summary>
    [XmlRoot("attribute")]
    public class ProductAttribute : Resource
    {
        /// <summary>
        /// Gets or sets the name of the attribute.
        /// </summary>
        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement("name")]
        public string Name { get; set; }
    }
}
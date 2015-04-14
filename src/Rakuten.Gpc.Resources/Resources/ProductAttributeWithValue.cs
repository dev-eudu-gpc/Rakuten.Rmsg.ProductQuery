//------------------------------------------------------------------------------
// <copyright file="ProductAttributeWithValue.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace Rakuten.Gpc.Resources
{
    using System.Collections.ObjectModel;
    using System.Xml.Serialization;

    using Newtonsoft.Json;

    /// <summary>
    /// Represents an individual product formalized attribute and the value assigned to that attribute
    /// </summary>
    public class ProductAttributeWithValue : ProductAttribute
    {
        /// <summary>
        /// Gets or sets the value for the product attribute.
        /// </summary>
        [JsonProperty("value", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement("value")]
        public string Value { get; set; }

        /// <summary>
        /// Gets or sets the values for the multi-value product attribute.
        /// </summary>
        [JsonProperty("values", NullValueHandling = NullValueHandling.Ignore)]
        [XmlArray("values")]
        [XmlArrayItem("value")]
        public Collection<string> Values { get; set; }
    }
}
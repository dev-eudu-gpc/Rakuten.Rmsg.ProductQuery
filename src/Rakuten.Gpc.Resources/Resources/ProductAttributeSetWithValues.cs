//------------------------------------------------------------------------------
// <copyright file="ProductAttributeSetWithValues.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
namespace Rakuten.Gpc.Resources
{
    using System.Collections.Generic;
    using System.Xml.Serialization;

    using Newtonsoft.Json;

    /// <summary>
    /// Represents an attribute set in the context of a product
    /// </summary>
    [XmlRoot("attributeset")]
    public class ProductAttributeSetWithValues : Resource
    {
        /// <summary>
        /// Gets or sets a list of attributes which have been applied.
        /// </summary>
        [JsonProperty("attributes", NullValueHandling = NullValueHandling.Ignore)]
        [XmlArray("attributes")]
        [XmlArrayItem("attribute")]
        public List<ProductAttributeWithValue> Attributes { get; set; }

        /// <summary>
        /// Gets or sets the name of the attribute set.
        /// </summary>
        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement("name")]
        public string Name { get; set; }
    }
}
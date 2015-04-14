//------------------------------------------------------------------------------
// <copyright file="Category.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
namespace Rakuten.Gpc.Resources
{
    using System.Collections.Generic;
    using System.Xml.Serialization;

    using Newtonsoft.Json;

    /// <summary>
    /// Represents a category.
    /// </summary>
    [XmlType("category")]
    public class Category : Resource
    {
        /// <summary>
        /// Gets or sets the culture in which the category is expressed.
        /// </summary>
        [JsonProperty("culture")]
        [XmlElement("culture")]
        public string Culture { get; set; }

        /// <summary>
        /// Gets or sets the name of the category.
        /// </summary>
        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement("name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the path segments of the category's ancestors.
        /// </summary>
        [JsonProperty("pathSegments", NullValueHandling = NullValueHandling.Ignore)]
        [XmlArray("pathSegments")]
        [XmlArrayItem("pathSegment")]
        public List<string> PathSegments { get; set; }
    }
}
//----------------------------------------------------------------------------------------------------------------------
// <copyright file="Culture.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------------------------------------------------
namespace Rakuten.Gpc.Resources
{
    using System.Xml.Serialization;

    using Newtonsoft.Json;

    /// <summary>
    /// Represents a culture
    /// </summary>
    [XmlType("culture")]
    public class Culture : Resource
    {
        /// <summary>
        /// Gets or sets the tag of the culture
        /// </summary>
        [JsonProperty("tag")]
        [XmlElement("tag")]
        public string Tag { get; set; }
    }
}
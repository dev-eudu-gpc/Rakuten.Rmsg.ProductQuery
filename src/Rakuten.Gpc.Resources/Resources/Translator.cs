//------------------------------------------------------------------------------
// <copyright file="Translator.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
namespace Rakuten.Gpc.Resources
{
    using System.Xml.Serialization;

    using Newtonsoft.Json;

    /// <summary>
    /// Represents the translator of a specific culture version of a product
    /// </summary>
    [XmlRoot("translator")]
    public class Translator : Resource
    {
        /// <summary>
        /// Gets or sets the name of the translator
        /// </summary>
        [JsonProperty("name")]
        [XmlElement("name")]
        public string Name { get; set; }
    }
}
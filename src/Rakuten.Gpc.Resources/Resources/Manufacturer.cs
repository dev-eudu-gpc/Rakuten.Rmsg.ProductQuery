//----------------------------------------------------------------------------------------------------------------------
// <copyright file="Manufacturer.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------------------------------------------------
namespace Rakuten.Gpc.Resources
{
    using System.Xml.Serialization;

    using Newtonsoft.Json;

    /// <summary>
    /// Represents a manufacturer.
    /// </summary>
    [XmlType("manufacturer")]
    public class Manufacturer : Resource
    {
        /// <summary>
        /// Gets or sets the culture in which the manufacturer is expressed.
        /// </summary>
        [JsonProperty("culture")]
        [XmlElement("culture")]
        public string Culture { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier for the manufacturer.
        /// </summary>
        [JsonProperty("id")]
        [XmlElement("id")]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the match level
        /// </summary>
        [JsonProperty("matchLevel")]
        [XmlElement("matchLevel")]
        public string MatchLevel { get; set; }

        /// <summary>
        /// Gets or sets the name of the manufacturer
        /// </summary>
        [JsonProperty("name")]
        [XmlElement("name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the phonetic name 
        /// </summary>
        [JsonProperty("phoneticName")]
        [XmlElement("phoneticName")]
        public string PhoneticName { get; set; }

        /// <summary>
        /// Gets or sets the private state. 
        /// </summary>
        [JsonProperty("privateState")]
        [XmlElement("privateState")]
        public string PrivateState { get; set; }
    }
}
//------------------------------------------------------------------------------
// <copyright file="Uploader.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
namespace Rakuten.Gpc.Resources
{
    using System.Xml.Serialization;

    using Newtonsoft.Json;

    /// <summary>
    /// Represents an uploader of product information
    /// </summary>
    [XmlRoot("uploader")]
    public class Uploader : Resource
    {
        /// <summary>
        /// Gets or sets the name of the uploader
        /// </summary>
        [JsonProperty("name")]
        [XmlElement("name")]
        public string Name { get; set; }
    }
}
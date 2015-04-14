//------------------------------------------------------------------------------
// <copyright file="DataSource.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
namespace Rakuten.Gpc.Resources
{
    using System.Xml.Serialization;

    using Newtonsoft.Json;

    /// <summary>
    /// Represents a source of product data
    /// </summary>
    [XmlRoot("dataSource")]
    public class DataSource : Resource
    {
        /// <summary>
        /// Gets or sets the name of the data source
        /// </summary>
        [JsonProperty("name")]
        [XmlElement("name")]
        public string Name { get; set; }
    }
}
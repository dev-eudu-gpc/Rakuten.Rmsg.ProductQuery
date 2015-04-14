//------------------------------------------------------------------------------
// <copyright file="Link.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
namespace Rakuten
{
    using System.Xml.Serialization;

    using Newtonsoft.Json;

    /// <summary>
    /// Represents a link to a resource.
    /// </summary>
    public class Link : ILink
    {
        /// <summary>Gets or sets the language of the resource this link points to.</summary>
        [XmlAttribute("hreflang")]
        [JsonProperty("hreflang", NullValueHandling = NullValueHandling.Ignore)]
        public string LanguageTag { get; set; }

        /// <summary>Gets or sets the media type of the representation this link points to.</summary>
        [XmlAttribute("type")]
        [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
        public string MediaType { get; set; }

        /// <summary>Gets or sets the link relation type.</summary>
        [XmlAttribute("rel")]
        [JsonIgnore]
        public string RelationType { get; set; }

        /// <summary>Gets or sets the location of the resource this link points to.</summary>
        [XmlAttribute("href")]
        [JsonProperty("href")]
        public string Target { get; set; }

        /// <summary>Gets or sets the title of this link.</summary>
        [XmlAttribute("title")]
        [JsonProperty("title", NullValueHandling = NullValueHandling.Ignore)]
        public string Title { get; set; }
    }
}
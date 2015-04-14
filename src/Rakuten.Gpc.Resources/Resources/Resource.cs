//----------------------------------------------------------------------------------------------------------------------
// <copyright file="Resource.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------------------------------------------------
namespace Rakuten
{
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Xml.Serialization;

    using Newtonsoft.Json;

    /// <summary>
    /// Represents a resource.
    /// </summary>
    public class Resource
    {
        /// <summary>
        ///  Gets or sets a collection of links associated with this resource.
        /// </summary>
        [XmlArray("links")]
        [XmlArrayItem("link")]
        [JsonProperty("links")]
        [JsonConverter(typeof(Json.EnumerableOfLinkConverter))]
        public Collection<Link> Links { get; set; }

        /// <summary>
        /// Gets a Uniform Resource Indicator (URI) for the current resource.
        /// </summary>
        /// <returns>A URI that represents the current resource.</returns>
        public Uri GetUri()
        {
            if (this.Links == null)
            {
                return null;
            }

            var link = this.Links.FirstOrDefault(l => l.RelationType == LinkRelationTypes.Canonical) ??
                this.Links.FirstOrDefault(l => l.RelationType == LinkRelationTypes.Self);

            return link == null ? null : new Uri(link.Target, UriKind.RelativeOrAbsolute);
        }
    }
}
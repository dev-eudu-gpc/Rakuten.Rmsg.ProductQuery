//------------------------------------------------------------------------------
// <copyright file="HttpProblem.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.Web.Http.Tests.Integration.Resources
{
    using System.Collections.ObjectModel;
    using Newtonsoft.Json;

    /// <summary>
    /// Represents an HTTP problem.
    /// </summary>
    public class HttpProblem
    {
        /// <summary>
        /// Gets or sets the detail.
        /// </summary>
        public string Detail { get; set; }

        /// <summary>
        /// Gets or sets a collection of links.
        /// </summary>
        [JsonProperty("links")]
        [JsonConverter(typeof(Json.EnumerableOfLinkConverter))]
        public Collection<Link> Links { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        public string Type { get; set; }
    }
}

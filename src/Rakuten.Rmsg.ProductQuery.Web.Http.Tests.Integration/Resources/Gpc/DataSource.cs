//------------------------------------------------------------------------------
// <copyright file="DataSource.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.Web.Http.Tests.Integration.Resources
{
    using Newtonsoft.Json;

    /// <summary>
    /// Represents a data source returned from the GPC core API.
    /// </summary>
    public class DataSource
    {
        /// <summary>
        /// Gets or sets the identifier of the data source.
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the culture of the expression of the data source.
        /// </summary>
        [JsonProperty("culture")]
        public string CultureCode { get; set; }

        /// <summary>
        /// Gets or sets the trust score for the data source.
        /// </summary>
        [JsonProperty("trustScore")]
        public int TrustScore { get; set; }

        /// <summary>
        /// Gets or sets the name of the data source.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the link of the data source.
        /// </summary>
        [JsonProperty("href")]
        public string HRef { get; set; }
    }
}
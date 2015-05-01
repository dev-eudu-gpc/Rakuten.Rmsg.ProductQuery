// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="DataSource.cs" company="Rakuten">
//   Copyright (c) Rakuten. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace Rakuten.Rmsg.ProductQuery.WebJob.Api
{
    using Newtonsoft.Json;

    /// <summary>
    /// Identifies the original source of a products data.
    /// </summary>
    [JsonObject(Title = "datasource")]
    public class DataSource
    {
        /// <summary>
        /// Gets or sets the culture in which this source is represented.
        /// </summary>
        [JsonProperty("culture")]
        public string CultureCode { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of this source.
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the name of this source.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the weighting given to this source against its peers.
        /// </summary>
        [JsonProperty("trustScore")]
        public int TrustScore { get; set; }
    }
}
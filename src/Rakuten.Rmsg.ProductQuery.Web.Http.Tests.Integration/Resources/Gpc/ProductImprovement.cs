//------------------------------------------------------------------------------
// <copyright file="ProductImprovement.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.Web.Http.Tests.Integration
{
    using System.Runtime.Serialization;
    using Newtonsoft.Json;

    /// <summary>
    /// Represents the resource used when calling the GPC core API to
    /// improve a product.
    /// </summary>
    public class ProductImprovement
    {
        /// <summary>
        /// Gets or sets the identifier of the product to improve.
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the improved product.
        /// </summary>
        [JsonProperty("improvedId")]
        public string ImprovedId { get; set; }

        /// <summary>
        /// Gets or sets the improvement count.
        /// </summary>
        [JsonProperty("improvementCount")]
        public int ImprovementCount { get; set; }
    }
}
//----------------------------------------------------------------------------------------------------------------------
// <copyright file="ProductAttributeSet.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.WebJob.Api
{
    using System.Collections.Generic;

    using Newtonsoft.Json;

    /// <summary>
    /// Represents a set of product attributes returned from the GPC API.
    /// </summary>
    [JsonObject(Title = "attributeSet")]
    public class ProductAttributeSet
    {
        /// <summary>
        /// Gets or sets the attributes that comprise this set.
        /// </summary>
        [JsonProperty("attributes")]
        public SortedDictionary<string, object> Attributes { get; set; }

        /// <summary>
        /// Gets or sets the name of this attribute set.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
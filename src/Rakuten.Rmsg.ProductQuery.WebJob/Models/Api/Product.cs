//----------------------------------------------------------------------------------------------------------------------
// <copyright file="Product.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.WebJob.Api
{
    using System;
    using System.Collections.Generic;

    using Newtonsoft.Json;

    /// <summary>
    /// Represents extended product information returned from the GPC API.
    /// </summary>
    [JsonObject(Title = "product")]
    public class Product
    {
        /// <summary>
        /// The default translator when creating new localized content.
        /// </summary>
        public static readonly string DefaultTranslator = "Native";

        /// <summary>
        /// Gets or sets the list of attribute sets belonging to this product.
        /// </summary>
        [JsonProperty("attributeSets")]
        public List<ProductAttributeSet> Attributes { get; set; }

        /// <summary>
        /// Gets or sets the category of this product.
        /// </summary>
        [JsonProperty("category")]
        public string Category { get; set; }

        /// <summary>
        /// Gets or sets the culture in which this product is presented.
        /// </summary>
        [JsonProperty("culture")]
        public string CultureCode { get; set; }

        /// <summary>
        /// Gets or sets the name of the data source.
        /// </summary>
        [JsonProperty("dataSource")]
        public string DataSource { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the data source.
        /// </summary>
        [JsonProperty("dataSourceId")]
        public string DataSourceId { get; set; }

        /// <summary>
        /// Gets or sets the description of this product.
        /// </summary>
        [JsonProperty("description")]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the short description of this product.
        /// </summary>
        [JsonProperty("descriptionShort")]
        public string DescriptionShort { get; set; }

        /// <summary>
        /// Gets or sets the identifier (GRAN) of this product.
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the image URL for this product.
        /// </summary>
        [JsonProperty("imageUrl")]
        public string ImageUrl { get; set; }

        /// <summary>
        /// Gets or sets the improved identifier (GRAN) of this product.
        /// </summary>
        [JsonProperty("improvedId")]
        public string ImprovedId { get; set; }

        /// <summary>
        /// Gets or sets the manufacturer of this product.
        /// </summary>
        [JsonProperty("manufacturer")]
        public string Manufacturer { get; set; }

        /// <summary>
        /// Gets or sets the name of this product.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the notes associated with this product.
        /// </summary>
        [JsonProperty("notes")]
        public List<Error> Notes { get; set; }

        /// <summary>
        /// Gets or sets the part number of this product.
        /// </summary>
        [JsonProperty("partNumber")]
        public string PartNumber { get; set; }

        /// <summary>
        /// Gets or sets the date when this product was created.
        /// </summary>
        [JsonProperty("timestampCreate")]
        public DateTime? TimestampCreate { get; set; }

        /// <summary>
        /// Gets or sets the date when this product was last updated.
        /// </summary>
        [JsonProperty("timestampUpdate")]
        public DateTime? TimestampUpdate { get; set; }

        /// <summary>
        /// Gets or sets the name of the translator of this localized content.
        /// </summary>
        [JsonProperty("translator")]
        public string Translator { get; set; }

        /// <summary>
        /// Gets or sets the name of the uploader of this product.
        /// </summary>
        [JsonProperty("uploader")]
        public string Uploader { get; set; }
    }
}
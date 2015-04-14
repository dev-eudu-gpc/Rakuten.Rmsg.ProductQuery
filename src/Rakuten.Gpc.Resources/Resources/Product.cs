//------------------------------------------------------------------------------
// <copyright file="Product.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
namespace Rakuten.Gpc.Resources
{
    using System;
    using System.Collections.Generic;
    using System.Xml.Serialization;

    using Newtonsoft.Json;

    /// <summary>
    /// Represents a product.
    /// </summary>
    [XmlType("product")]
    public class Product : Resource
    {
        /// <summary>
        /// The date on which the product was created.
        /// </summary>
        private DateTime? created;

        /// <summary>
        /// The date on which the product was updated.
        /// </summary>
        private DateTime? updated;

        /// <summary>
        /// Gets or sets a list of formalized attributes
        /// </summary>
        [JsonProperty("attributeSets")]
        [XmlArray("attributeSets")]
        [XmlArrayItem("attributeSet")]
        public List<ProductAttributeSetWithValues> AttributeSets { get; set; }

        /// <summary>
        /// Gets or sets the category to which the product belongs
        /// </summary>
        [JsonProperty("category")]
        [XmlElement("category")]
        public Category Category { get; set; }

        /// <summary>
        /// Gets or sets the date on which the product was created
        /// </summary>
        [JsonProperty("created", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement("created")]
        public DateTime? Created
        {
            get { return this.created; }
            set { this.created = value; }
        }

        /// <summary>
        /// Gets or sets the culture in which the product is expressed.
        /// </summary>
        [JsonProperty("culture")]
        [XmlElement("culture")]
        public string Culture { get; set; }

        /// <summary>
        /// Gets or sets a list of custom attributes.
        /// </summary>
        [JsonProperty("customAttributes")]
        [XmlArray("customAttributes")]
        [XmlArrayItem("attribute")]
        public List<ProductAttributeWithValue> CustomAttributes { get; set; }

        /// <summary>
        /// Gets or sets the name of the data source
        /// </summary>
        [JsonProperty("dataSource", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement("dataSource")]
        public DataSource DataSource { get; set; }

        /// <summary>
        /// Gets or sets the description of the product
        /// </summary>
        [JsonProperty("description", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement("description")]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets a collection of images for the product
        /// </summary>
        [XmlArray("images")]
        [XmlArrayItem("image")]
        [JsonProperty("images", NullValueHandling = NullValueHandling.Ignore)]
        public List<ProductImage> Images { get; set; }

        /// <summary>
        /// Gets or sets the improved Gran
        /// </summary>
        [JsonProperty("improvedGran", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement("improvedGran")]
        public string ImprovedGran { get; set; }

        /// <summary>
        /// Gets or sets the manufacturer of the product
        /// </summary>
        [JsonProperty("manufacturer", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement("manufacturer")]
        public Manufacturer Manufacturer { get; set; }

        /// <summary>
        /// Gets or sets the manufacturer's part number
        /// </summary>
        [JsonProperty("manufacturerPartNumber", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement("manufacturerPartNumber")]
        public string ManufacturerPartNumber { get; set; }

        /// <summary>
        /// Gets or sets the name of the product
        /// </summary>
        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement("name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the original Gran
        /// </summary>
        [JsonProperty("originalGran", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement("originalGran")]
        public string OriginalGran { get; set; }

        /// <summary>
        /// Gets or sets the short description of the product
        /// </summary>
        [JsonProperty("shortDescription", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement("shortDescription")]
        public string ShortDescription { get; set; }

        /// <summary>
        /// Gets or sets the translator of the current culture version of the product
        /// </summary>
        [JsonProperty("translator", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement("translator")]
        public Translator Translator { get; set; }

        /// <summary>
        /// Gets or sets when the product was last updated
        /// </summary>
        [JsonProperty("updated")]
        [XmlElement("updated")]
        public DateTime? Updated
        {
            get { return this.updated; }
            set { this.updated = value; }
        }

        /// <summary>
        /// Gets or sets the uploader of the product data
        /// </summary>
        [JsonProperty("uploader")]
        [XmlElement("uploader")]
        public Uploader Uploader { get; set; }

        /// <summary>
        /// Indicates whether to include the <see cref="Created"/> field in the serialization of the current instance.
        /// </summary>
        /// <returns>
        /// <see langword="true"/> if the <see cref="Created"/> field should be included in the serialization
        /// of the current instance; otherwise, <see langword="false"/>.
        /// </returns>
        public bool ShouldSerializeCreated()
        {
            return this.created.HasValue;
        }

        /// <summary>
        /// Indicates whether to include the <see cref="Updated"/> field in the serialization of the current instance.
        /// </summary>
        /// <returns>
        /// <see langword="true"/> if the <see cref="Updated"/> field should be included in the serialization
        /// of the current instance; otherwise, <see langword="false"/>.
        /// </returns>
        public bool ShouldSerializeUpdated()
        {
            return this.updated.HasValue;
        }
    }
}
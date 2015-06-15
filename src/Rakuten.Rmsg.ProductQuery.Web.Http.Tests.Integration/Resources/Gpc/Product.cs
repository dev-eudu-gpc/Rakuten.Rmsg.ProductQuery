//---------------------------------------------------------------------------------------------------------------------
// <copyright file="Product.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//---------------------------------------------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.Web.Http.Tests.Integration
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// A product.
    /// </summary>
    public class Product
    {
        /// <summary>
        /// Gets or sets the named key/value pair sets.
        /// </summary>
        public List<AttributeSet> AttributeSets { get; set; }

        /// <summary>
        /// Gets or sets the category assigned to the product.
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// Gets or sets the string culture code assigned to this product.
        /// </summary>
        public string Culture { get; set; }

        /// <summary>
        /// Gets or sets the primary source of data for this product.
        /// </summary>
        public string DataSource { get; set; }

        /// <summary>
        /// Gets or sets the description of this product.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the shortened description of this product.
        /// </summary>
        public string DescriptionShort { get; set; }

        /// <summary>
        /// Gets or sets the URI of this product.
        /// </summary>
        public string HRef { get; set; }

        /// <summary>
        /// Gets or sets the GRAN of the product.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the URI of the image of this product.
        /// </summary>
        public string ImageUrl { get; set; }

        /// <summary>
        /// Gets or sets the URI of the main image of this product.
        /// </summary>
        public string ImageUrlMain { get; set; }

        /// <summary>
        /// Gets or sets the GRAN of another product specified as an improvement over this.
        /// </summary>
        public string ImprovedId { get; set; }

        /// <summary>
        /// Gets or sets the assigned manufacturer of this product.
        /// </summary>
        public string Manufacturer { get; set; }

        /// <summary>
        /// Gets or sets the name of the product.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the part number of this product as assigned by the manufacturer.
        /// </summary>
        public string PartNumber { get; set; }

        /// <summary>
        /// Gets or sets the point in time at which the product was created.
        /// </summary>
        public DateTime? TimestampCreate { get; set; }

        /// <summary>
        /// Gets or sets the point in time at which the product last updated.
        /// </summary>
        public DateTime? TimestampUpdate { get; set; }

        /// <summary>
        /// Gets or sets the translator.
        /// </summary>
        public string Translator { get; set; }

        /// <summary>
        /// Gets or sets the uploader.
        /// </summary>
        public string Uploader { get; set; }
    }
}
//------------------------------------------------------------------------------
// <copyright file="ProductQueryLinks.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.Web.Http.Tests.Integration.Resources
{
    /// <summary>
    /// Represents the links in a product query.
    /// </summary>
    public class ProductQueryLinks
    {
        /// <summary>
        /// Gets or sets the enclosure link.
        /// </summary>
        public ProductQueryLink Enclosure { get; set; }

        /// <summary>
        /// Gets or sets the monitor link.
        /// </summary>
        public ProductQueryLink Monitor { get; set; }

        /// <summary>
        /// Gets or sets the self link.
        /// </summary>
        public ProductQueryLink Self { get; set; }
    }
}
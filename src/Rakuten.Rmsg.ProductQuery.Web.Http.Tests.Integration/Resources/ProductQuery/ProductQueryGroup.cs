//------------------------------------------------------------------------------
// <copyright file="ProductQueryGroup.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.Web.Http.Tests.Integration
{
    /// <summary>
    /// Represents a product query group.
    /// </summary>
    public class ProductQueryGroup
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProductQueryGroup"/> class
        /// </summary>
        public ProductQueryGroup()
        {
        }

        /// <summary>
        /// Gets or sets the identifier for the product query.
        /// </summary>
        public string Id { get; set; }
    }
}
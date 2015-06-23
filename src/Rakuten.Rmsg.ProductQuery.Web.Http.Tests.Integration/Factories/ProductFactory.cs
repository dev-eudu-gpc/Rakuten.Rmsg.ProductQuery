//------------------------------------------------------------------------------
// <copyright file="ProductFactory.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.Web.Http.Tests.Integration
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Contains methods for providing <see cref="Product"/> objects.
    /// </summary>
    public class ProductFactory
    {
        /// <summary>
        /// Creates a product with the minimum amount of data required for testing.
        /// </summary>
        /// <param name="culture">The culture for the product.</param>
        /// <returns>The new product..</returns>
        public static Product CreateMinimumProduct(
            string culture)
        {
            string id = Guid.NewGuid().ToString("D");

            return new Product
            {
                Category = "Rakuten",
                Culture = culture,
                DataSource = "GECP",
                Id = id,
                ImageUrl = "dummy image " + id,
                Name = "Integration test for rmsg product query API" + id,
                Uploader = "Developer",
                Manufacturer = "WARP Records",
                PartNumber = "WAP001x" + id,
                AttributeSets = new List<AttributeSet>
                {
                    AttributeSetFactory.CreateNewDefinedAttributeSet(
                        AttributeSetName.Gtin, 
                        new SortedDictionary<string, object>
                        {
                            { "EAN", GtinFactory.CreateEan() }
                        }),
                    AttributeSetFactory.CreateNewDefinedAttributeSet(
                        AttributeSetName.Common,
                        new SortedDictionary<string, object>
                        {
                            { "Video URL", "http://dummy.video.url/" + id }
                        }),
                    AttributeSetFactory.CreateNewDefinedAttributeSet(
                        AttributeSetName.Brand,
                        new SortedDictionary<string, object>
                        {
                            { "Brand", "Dummy brand for " + id }
                        }),
                    AttributeSetFactory.CreateImages(
                        new Uri[]
                        {
                            new Uri("http://dummy.host/image_main_" + id),
                            new Uri("http://dummy.host/image_" + Guid.NewGuid()),
                            new Uri("http://dummy.host/image_" + Guid.NewGuid()),
                            new Uri("http://dummy.host/image_" + Guid.NewGuid()),
                            new Uri("http://dummy.host/image_" + Guid.NewGuid())
                        })
                }
            };
        }
    }
}
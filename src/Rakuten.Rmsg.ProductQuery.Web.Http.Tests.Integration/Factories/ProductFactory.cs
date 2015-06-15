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
        /// Creates a product with the minimum amount of data to be considered viable by the system.
        /// </summary>
        /// <returns>A product considered to be valid by the system.</returns>
        public static Product CreateMinimumViableProduct()
        {
            string id = Guid.NewGuid().ToString("D");

            return new Product
            {
                Category = "Rakuten",
                Culture = "en-US",
                DataSource = "GECP",
                Id = id,
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
                        })
                }
            };
        }
    }
}
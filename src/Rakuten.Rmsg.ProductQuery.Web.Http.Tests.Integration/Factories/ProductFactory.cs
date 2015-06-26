//------------------------------------------------------------------------------
// <copyright file="ProductFactory.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.Web.Http.Tests.Integration
{
    using System;
    using System.Collections.Generic;
    using Rakuten.Rmsg.ProductQuery.Web.Http.Tests.Integration.Resources;

    /// <summary>
    /// Contains methods for providing <see cref="Product"/> objects.
    /// </summary>
    public class ProductFactory
    {
        /// <summary>
        /// Creates a product with the minimum amount of data required for testing.
        /// </summary>
        /// <param name="culture">The culture for the product.</param>
        /// <param name="dataSourceName">The data source.</param>
        /// <param name="identifierType">The type of the identifier.</param>
        /// <param name="identifierValue">The value of the identifier.</param>
        /// <returns>The new product..</returns>
        public static Product CreateMinimumProduct(
            string culture = "en-US",
            string dataSourceName = "GECP",
            IdentifierType identifierType = IdentifierType.Ean,
            string identifierValue = null)
        {
            string id = Guid.NewGuid().ToString("D");

            return new Product
            {
                Category = "Rakuten",
                Culture = culture,
                DataSource = dataSourceName,
                ImageUrl = "dummy image " + id,
                Name = "Integration test for rmsg product query API" + id,
                Uploader = "Developer",
                Manufacturer = "WARP Records " + id,
                PartNumber = "WAP " + id,
                AttributeSets = new List<AttributeSet>
                {
                    GetBrandAttributeSet(),
                    GetGtinAttributeSet(identifierType, identifierValue),
                    GetImagesAttributeSet(),
                    GetVideoUrlAttributeSet()
                }
            };
        }

        /// <summary>
        /// Gets an attribute set for the brand.
        /// </summary>
        /// <returns>An attribute set for the brand.</returns>
        private static AttributeSet GetBrandAttributeSet()
        {
            return AttributeSetFactory.CreateNewDefinedAttributeSet(
                AttributeSetName.Brand,
                new SortedDictionary<string, object>
                {
                    { "Brand", string.Format("Dummy brand {0}", Guid.NewGuid()) }
                });
        }

        /// <summary>
        /// Gets an attribute set containing a GTIN of the specified type
        /// optionally with the specified value.
        /// </summary>
        /// <param name="identifierType">The type of GTIN.</param>
        /// <param name="identifierValue">The value of the GTIN.</param>
        /// <returns>An attribute set containing a GTIN of the specified type.</returns>
        private static AttributeSet GetGtinAttributeSet(
            IdentifierType identifierType,
            string identifierValue = null)
        {
            switch (identifierType)
            {
                case IdentifierType.Ean:
                    return AttributeSetFactory.CreateNewDefinedAttributeSet(
                        AttributeSetName.Gtin,
                        new SortedDictionary<string, object>
                        {
                            { "EAN", identifierValue ?? GtinFactory.CreateEan() }
                        });

                case IdentifierType.Isbn:
                    return AttributeSetFactory.CreateNewDefinedAttributeSet(
                        AttributeSetName.Gtin,
                        new SortedDictionary<string, object>
                        {
                            { "ISBN", identifierValue ?? GtinFactory.CreateIsbn() }
                        });

                case IdentifierType.Jan:
                    return AttributeSetFactory.CreateNewDefinedAttributeSet(
                        AttributeSetName.Gtin,
                        new SortedDictionary<string, object>
                        {
                            { "JAN", identifierValue ?? GtinFactory.CreateJan() }
                        });

                case IdentifierType.Upc:
                    return AttributeSetFactory.CreateNewDefinedAttributeSet(
                        AttributeSetName.Gtin,
                        new SortedDictionary<string, object>
                        {
                            { "UPC", identifierValue ?? GtinFactory.CreateUpc() }
                        });
                default:
                    return null;
            }
        }

        /// <summary>
        /// Gets an attribute set containing five images.
        /// </summary>
        /// <returns>an attribute set containing five images.</returns>
        private static AttributeSet GetImagesAttributeSet()
        {
            return AttributeSetFactory.CreateImages(
                new Uri[]
                {
                    new Uri(string.Format("http://dummy.host/image_", Guid.NewGuid())),
                    new Uri(string.Format("http://dummy.host/image_", Guid.NewGuid())),
                    new Uri(string.Format("http://dummy.host/image_", Guid.NewGuid())),
                    new Uri(string.Format("http://dummy.host/image_", Guid.NewGuid())),
                    new Uri(string.Format("http://dummy.host/image_", Guid.NewGuid()))
                });
        }

        /// <summary>
        /// Gets an attribute set for the video url.
        /// </summary>
        /// <returns>An attribute set for the video url.</returns>
        private static AttributeSet GetVideoUrlAttributeSet()
        {
            return AttributeSetFactory.CreateNewDefinedAttributeSet(
                AttributeSetName.Common,
                new SortedDictionary<string, object>
                {
                    { "Video URL", string.Format("http://dummy.video.url/{0}", Guid.NewGuid()) }
                });
        }
    }
}
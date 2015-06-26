//------------------------------------------------------------------------------
// <copyright file="ProductExtensions.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.Web.Http.Tests.Integration
{
    using System;
    using System.Linq;
    using Rakuten.Rmsg.ProductQuery.Web.Http.Tests.Integration.Resources;

    /// <summary>
    /// Extension methods for the <see cref="Product"/> resource class.
    /// </summary>
    public static class ProductExtensions
    {
        /// <summary>
        /// Gets the EAN for a product.
        /// </summary>
        /// <param name="product">The product being operated upon.</param>
        /// <returns>The EAN for the product.</returns>
        public static string GetEAN(this Product product)
        {
            return product.AttributeSets
                .FirstOrDefault(set => set.Name.Equals(AttributeSetName.Gtin.ToString(), StringComparison.InvariantCultureIgnoreCase))
                .Attributes.FirstOrDefault(attribute => attribute.Key.Equals("EAN", StringComparison.InvariantCultureIgnoreCase))
                .Value.ToString();
        }

        /// <summary>
        /// Gets the brand for a product.
        /// </summary>
        /// <param name="product">The product being operated upon.</param>
        /// <returns>The brand for the product.</returns>
        public static string GetBrand(this Product product)
        {
            return product.AttributeSets
                .FirstOrDefault(set => set.Name.Equals(AttributeSetFactory.GetNameFromEnum(AttributeSetName.Brand), StringComparison.InvariantCultureIgnoreCase))
                .Attributes.FirstOrDefault(attribute => attribute.Key.Equals("Brand", StringComparison.InvariantCultureIgnoreCase))
                .Value.ToString();
        }

        /// <summary>
        /// Gets the video URL for a product.
        /// </summary>
        /// <param name="product">The product being operated upon.</param>
        /// <returns>The video URL for the product.</returns>
        public static string GetVideoUrl(this Product product)
        {
            var x = product.AttributeSets
                .FirstOrDefault(set => set.Name.Equals(AttributeSetFactory.GetNameFromEnum(AttributeSetName.Common), StringComparison.InvariantCultureIgnoreCase));

            var y = x
                .Attributes.FirstOrDefault(attribute => attribute.Key.Equals("Video URL", StringComparison.InvariantCultureIgnoreCase));

                var z = y
                .Value.ToString();

            return z;
        }

        /// <summary>
        /// Gets an image for a product.
        /// </summary>
        /// <param name="product">The product being operated upon.</param>
        /// <param name="index">The index of the image.</param>
        /// <returns>The image at the specified index for the product.</returns>
        public static string GetImageUrl(this Product product, int index)
        {
            var attributeName = index == 1 ?
                "image url main" :
                string.Format("image location {0}", index);

            return product.AttributeSets
                .FirstOrDefault(set => set.Name.Equals(AttributeSetFactory.GetNameFromEnum(AttributeSetName.Images), StringComparison.InvariantCultureIgnoreCase))
                .Attributes.FirstOrDefault(attribute => attribute.Key.Equals(attributeName, StringComparison.InvariantCultureIgnoreCase))
                .Value.ToString();
        }
    }
}

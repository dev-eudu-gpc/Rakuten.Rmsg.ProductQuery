//------------------------------------------------------------------------------
// <copyright file="ProductQueryExtensions.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.Web.Http.Tests.Integration
{
    using System;

    /// <summary>
    /// Extension methods for the <see cref="ProductQuery"/> resource class.
    /// </summary>
    public static class ProductQueryExtensions
    {
        /// <summary>
        /// Gets the blob name for the product query.
        /// </summary>
        /// <param name="productQuery">The product query being operated upon.</param>
        /// <returns>The blob name for the product query.</returns>
        public static string GetBlobName(this ProductQuery productQuery)
        {
            var enclosure = productQuery.Links.Enclosure != null ?
                new Uri(productQuery.Links.Enclosure.Href) :
                null;

            if (enclosure != null && enclosure.Segments.Length > 2)
            {
                return string.Format(
                    "{0}/{1}",
                    enclosure.Segments[enclosure.Segments.Length - 2].TrimEnd('/'),
                    enclosure.Segments[enclosure.Segments.Length - 1]);
            }

            return string.Empty;
        }
    }
}

//----------------------------------------------------------------------------------------------------------------------
// <copyright file="ProductQueryFactory.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.Web.Http.Tests
{
    using System;
    using System.Collections.ObjectModel;
    using System.Globalization;
    using System.IO;
    using System.Windows;
    using System.Windows.Media.Imaging;
    using Rakuten.Fakes;
    using Rakuten.Rmsg.ProductQuery.Web.Http.Links;

    /// <summary>
    /// Provides functionality for working with the <see cref="ProductQuery"/>for test purposes.
    /// </summary>
    public static class ProductQueryFactory
    {
        /// <summary>
        /// Creates a new <see cref="ProductQuery"/>.
        /// </summary>
        /// <param name="id">The identifier of the product query.</param>
        /// <param name="cultureName">The culture of the product query.</param>
        /// <param name="status">The status of the product query</param>
        /// <param name="blobUri">The URI for the blob of the product query.</param>
        /// <returns>A new <see cref="ProductQuery"/> object.</returns>
        public static ProductQuery Create(
            Guid id,
            string cultureName = "en-US",
            ProductQueryStatus status = ProductQueryStatus.New,
            string blobUri = null)
        {
            // Construct links
            var selfTemplate = "product-query/{0}/culture/{1}";

            var links = new Collection<Link>
            {
                new StubLink { RelationType = "self", Target = string.Format(selfTemplate, id.ToString(), cultureName) }
            };

            // Add an enclosure link if 

            // Construct and return the new object
            return new ProductQuery
            {
                CultureName = cultureName,
                Links = links,
                Status = status,
                Uri = blobUri
            };
        }
    }
}
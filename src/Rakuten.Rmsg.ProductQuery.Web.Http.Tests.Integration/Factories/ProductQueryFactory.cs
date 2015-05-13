//------------------------------------------------------------------------------
// <copyright file="ProductQueryFactory.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.Web.Http.Tests.Integration
{
    using System;

    /// <summary>
    /// Contains methods for providing <see cref="ProductQuery"/> objects.
    /// </summary>
    public class ProductQueryFactory
    {
        /// <summary>
        /// Creates a new <see cref="ProductQuery"/> object.
        /// </summary>
        /// <param name="id">The identifier of the product query.</param>
        /// <param name="culture">The culture of the product query.</param>
        /// <returns>A new instance of <see cref="ProductQuery"/>.</returns>
        public static ProductQuery Create(string id = null, string culture = "en-US")
        {
            var newId = id ?? Guid.NewGuid().ToString();

            return new ProductQuery
            {
                Culture = culture,
                Id = newId,
                Links = new ProductQueryLinks
                {
                    Self = new ProductQueryLink
                    {
                        Href = string.Format("/product-query/{0}/culture/{1}", newId, culture)
                    }
                }
            };
        }
    }
}

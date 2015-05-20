// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProductQueryAlreadySubmittedException.cs" company="Rakuten">
//   Copyright (c) Rakuten. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.Web.Http
{
    using System.Collections.ObjectModel;
    using System.Diagnostics.Contracts;
    using Newtonsoft.Json;
    using Rakuten.Gpc;
    using Rakuten.Rmsg.ProductQuery.Web.Http.Links;

    /// <summary>
    /// Represents the error raised when trying to flag a product query as ready
    /// for processing when it has already been done.
    /// </summary>
    [ApiException("http://problems.rakuten.com/product-query-already-ready-for-processing", "The product query has already been flagged as ready for processing.")]
    internal class ProductQueryAlreadySubmittedException : ApiException
    {
        /// <summary>
        /// The message that describes this error.
        /// </summary>
        private const string Detail =
            @"You attempted to flag the query at '/product-query/{0}/culture/{1}' as ready for processing but has already been flagged as such.";

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductQueryAlreadySubmittedException"/> class.
        /// </summary>
        public ProductQueryAlreadySubmittedException()
            : base(Detail)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductQueryAlreadySubmittedException"/> class.
        /// </summary>
        /// <param name="id">The identifier of the product query.</param>
        /// <param name="culture">The culture of the product query.</param>
        /// <param name="productQueryUriTemplate">A link template representing the canonical location of the resource.</param>
        public ProductQueryAlreadySubmittedException(
            string id, 
            string culture,
            IUriTemplate productQueryUriTemplate)
            : base(string.Format(Detail, id, culture))
        {
            Contract.Requires(productQueryUriTemplate != null);

            this.Links = new Collection<Link>();
            this.Links.Add(
                new ProductQueryLink("http://rels.rakuten.com/product-query", productQueryUriTemplate)
                    .ForId(id)
                    .ForCulture(culture)
                    .Expand());
        }

        /// <summary>
        /// Gets or sets a collection of links for the exception.
        /// </summary>
        [JsonProperty("links")]
        [JsonConverter(typeof(Json.EnumerableOfLinkConverter))]
        public Collection<Link> Links { get; set; }
    }
}
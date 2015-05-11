// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InvalidStatusException.cs" company="Rakuten">
//   Copyright (c) Rakuten. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.Web.Http
{
    using System.Collections.ObjectModel;
    using System.Diagnostics.Contracts;
    using System.Xml.Serialization;
    using Newtonsoft.Json;
    using Rakuten.Gpc;
    using Rakuten.Rmsg.ProductQuery.Web.Http.Links;

    /// <summary>
    /// Represents the error raised when trying to set the status of a product query
    /// to an invalid status.
    /// </summary>
    [ApiException("http://problems.rakuten.com/invalid-product-query-status", "An invalid product query status was supplied.")]
    internal class InvalidStatusException : ApiException
    {
        /// <summary>
        /// The message that describes this error.
        /// </summary>
        private const string Detail =
            @"You attempted to update the status of the query at '/product-query/{0}' to '{1}', which is an invalid status.  The only valid status to which this query can be set is 'submitted'.";

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidStatusException"/> class.
        /// </summary>
        public InvalidStatusException()
            : base(Detail)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidStatusException"/> class.
        /// </summary>
        /// <param name="id">The identifier of the product query.</param>
        /// <param name="culture">The culture of the product query.</param>
        /// <param name="suppliedStatus">The value for the status that was invalid.</param>
        /// <param name="productQueryUriTemplate">A link template representing the canonical location of the resource.</param>
        public InvalidStatusException(
            string id, 
            string culture,
            string suppliedStatus,
            IUriTemplate productQueryUriTemplate)
            : base(string.Format(Detail, id, suppliedStatus))
        {
            Contract.Requires(productQueryUriTemplate != null);

            this.Links = new Collection<Link>();
            var x = new ProductQueryLink("self", productQueryUriTemplate);
            var z = x.ForId(id).ForCulture(culture).ToLink(true);
            this.Links.Add(z);
        }

        /// <summary>
        /// Gets or sets a collection of links for the exception.
        /// </summary>
        public Collection<Link> Links { get; set; }
    }
}
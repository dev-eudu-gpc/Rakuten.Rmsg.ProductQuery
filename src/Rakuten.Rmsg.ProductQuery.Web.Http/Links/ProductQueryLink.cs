//------------------------------------------------------------------------------
// <copyright file="ProductQueryLink.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.Web.Http.Links
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Web;

    /// <summary>
    /// Represents a link to a product query.
    /// </summary>
    [LinkRelation(Name = "http://rels.rakuten.com/rmsg/product-query")]
    public class ProductQueryLink : LinkTemplate
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProductQueryLink"/> class.
        /// </summary>
        /// <param name="target">The location of the product query this link points to.</param>
        public ProductQueryLink(IUriTemplate target) : base(target)
        {
            Contract.Requires(target != null);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductQueryLink"/> class.
        /// </summary>
        /// <param name="relationType">The link relation type.</param>
        /// <param name="target">The location of the product query this link points to.</param>
        public ProductQueryLink(string relationType, IUriTemplate target) : base(relationType, target)
        {
            Contract.Requires(target != null);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductQueryLink"/> class.
        /// </summary>
        /// <param name="relationType">The link relation type.</param>
        /// <param name="target">The location of the product query this link points to.</param>
        /// <param name="attributes">The attributes of the product query this link points to.</param>
        public ProductQueryLink(string relationType, IUriTemplate target, ITargetAttributes attributes)
            : base(relationType, target, attributes)
        {
            Contract.Requires(target != null);
        }

        /// <summary>
        /// Gets a link to a product query with the specified unique identifier.
        /// </summary>
        /// <param name="id">The identifier of the product query.</param>
        /// <returns>A link to a product query with the specified unique identifier.</returns>
        public ProductQueryLink ForId(string id)
        {
            var resolvedTemplate = this.Target.Bind("id", id);

            Contract.Assume(resolvedTemplate != null);

            return new ProductQueryLink(this.RelationType, resolvedTemplate, this.TargetAttributes);
        }
    }
}
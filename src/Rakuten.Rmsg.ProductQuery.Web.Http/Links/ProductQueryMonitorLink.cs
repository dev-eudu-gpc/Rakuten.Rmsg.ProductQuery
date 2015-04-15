//------------------------------------------------------------------------------
// <copyright file="ProductQueryMonitorLink.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.Web.Http.Links
{
    using System.Diagnostics.Contracts;

    /// <summary>
    /// Represents a link to a product query.
    /// </summary>
    [LinkRelation(Name = "http://rels.rakuten.com/rmsg/product-query-monitor")]
    public class ProductQueryMonitorLink : LinkTemplate
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProductQueryMonitorLink"/> class.
        /// </summary>
        /// <param name="target">The location of the product query monitor this link points to.</param>
        public ProductQueryMonitorLink(IUriTemplate target) : base(target)
        {
            Contract.Requires(target != null);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductQueryMonitorLink"/> class.
        /// </summary>
        /// <param name="relationType">The link relation type.</param>
        /// <param name="target">The location of the product query this link points to.</param>
        public ProductQueryMonitorLink(string relationType, IUriTemplate target) : base(relationType, target)
        {
            Contract.Requires(target != null);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductQueryMonitorLink"/> class.
        /// </summary>
        /// <param name="relationType">The link relation type.</param>
        /// <param name="target">The location of the product query this link points to.</param>
        /// <param name="attributes">The attributes of the product query this link points to.</param>
        public ProductQueryMonitorLink(string relationType, IUriTemplate target, ITargetAttributes attributes)
            : base(relationType, target, attributes)
        {
            Contract.Requires(target != null);
        }

        /// <summary>
        /// Gets a link to a product query with the specified unique identifier.
        /// </summary>
        /// <param name="id">The identifier of the product query.</param>
        /// <returns>A link to a product query with the specified unique identifier.</returns>
        public ProductQueryMonitorLink ForId(string id)
        {
            var resolvedTemplate = this.Target.Bind("id", id);

            Contract.Assume(resolvedTemplate != null);

            return new ProductQueryMonitorLink(this.RelationType, resolvedTemplate, this.TargetAttributes);
        }
    }
}
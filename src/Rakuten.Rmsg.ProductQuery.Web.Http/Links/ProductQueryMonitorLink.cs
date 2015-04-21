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
        /// Gets a link to a product query group status for the specified day.
        /// </summary>
        /// <param name="day">The day of the month.</param>
        /// <returns>A link to a product query group status.</returns>
        public ProductQueryMonitorLink ForDay(string day)
        {
            var resolvedTemplate = this.Target.Bind("day", day);

            Contract.Assume(resolvedTemplate != null);

            return new ProductQueryMonitorLink(this.RelationType, resolvedTemplate, this.TargetAttributes);
        }

        /// <summary>
        /// Gets a link to a product query with the specified unique identifier.
        /// </summary>
        /// <param name="id">The identifier of the product query.</param>
        /// <returns>A link to a product query group status.</returns>
        public ProductQueryMonitorLink ForId(string id)
        {
            var resolvedTemplate = this.Target.Bind("id", id);

            Contract.Assume(resolvedTemplate != null);

            return new ProductQueryMonitorLink(this.RelationType, resolvedTemplate, this.TargetAttributes);
        }

        /// <summary>
        /// Gets a link to a product query group status for the specified month.
        /// </summary>
        /// <param name="month">The month number.</param>
        /// <returns>A link to a product query group status.</returns>
        public ProductQueryMonitorLink ForMonth(string month)
        {
            var resolvedTemplate = this.Target.Bind("month", month);

            Contract.Assume(resolvedTemplate != null);

            return new ProductQueryMonitorLink(this.RelationType, resolvedTemplate, this.TargetAttributes);
        }

        /// <summary>
        /// Gets a link to a product query group status for the specified time.
        /// </summary>
        /// <param name="time">The time of day in hours and minutes.</param>
        /// <returns>A link to a product query group status.</returns>
        public ProductQueryMonitorLink ForTime(string time)
        {
            var resolvedTemplate = this.Target.Bind("time", time);

            Contract.Assume(resolvedTemplate != null);

            return new ProductQueryMonitorLink(this.RelationType, resolvedTemplate, this.TargetAttributes);
        }

        /// <summary>
        /// Gets a link to a product query group status for the specified year.
        /// </summary>
        /// <param name="year">The year.</param>
        /// <returns>A link to a product query group status.</returns>
        public ProductQueryMonitorLink ForYear(string year)
        {
            var resolvedTemplate = this.Target.Bind("year", year);

            Contract.Assume(resolvedTemplate != null);

            return new ProductQueryMonitorLink(this.RelationType, resolvedTemplate, this.TargetAttributes);
        }
    }
}
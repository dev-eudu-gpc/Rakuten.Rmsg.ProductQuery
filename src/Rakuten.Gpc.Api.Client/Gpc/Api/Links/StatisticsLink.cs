// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="StatisticsLink.cs" company="Rakuten">
//   Copyright (c) Rakuten. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------
namespace Rakuten.Gpc.Api.Links
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Globalization;

    /// <summary>
    /// Represents a link to catalog statistics.
    /// </summary>
    [LinkRelation(Name = "http://rels.rakuten.com/catalog-statistics")]
    public class StatisticsLink : LinkTemplate
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StatisticsLink"/> class.
        /// </summary>
        /// <param name="target">The location of the product this link points to.</param>
        public StatisticsLink(IUriTemplate target) : base(target)
        {
            Contract.Requires(target != null);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StatisticsLink"/> class.
        /// </summary>
        /// <param name="relationType">The link relation type.</param>
        /// <param name="target">The location of the product this link points to.</param>
        /// <param name="attributes">The attributes of the product this link points to.</param>
        public StatisticsLink(string relationType, IUriTemplate target, ITargetAttributes attributes)
            : base(relationType, target, attributes)
        {
            Contract.Requires(target != null);
        }

        /// <summary>
        /// Gets a link to statistics for the specified culture.
        /// </summary>
        /// <param name="culture">The culture for which to retrieve statistics.</param>
        /// <returns>A link to statistics in the specified culture.</returns>
        public StatisticsLink ForCulture(CultureInfo culture)
        {
            Contract.Ensures(Contract.Result<StatisticsLink>() != null);

            var resolvedTemplate = this.Target.Bind("culture", culture == null ? null : culture.Name);
            Contract.Assume(resolvedTemplate != null);

            return new StatisticsLink(
                this.RelationType,
                resolvedTemplate,
                this.TargetAttributes.WithLanguageTag(culture));
        }

        /// <summary>
        /// Gets a link to statistics for events of a specified type.
        /// </summary>
        /// <param name="type">The type of event for which to retrieve statistics.</param>
        /// <returns>A link to statistics for events of the specified type.</returns>
        public StatisticsLink ForEventType(string type)
        {
            Contract.Ensures(Contract.Result<StatisticsLink>() != null);

            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            if (string.IsNullOrWhiteSpace(type))
            {
                throw new ArgumentException("type");
            }

            var resolvedTemplate = this.Target.Bind("event", type);
            Contract.Assume(resolvedTemplate != null);

            return new StatisticsLink(this.RelationType, resolvedTemplate, this.TargetAttributes);
        }

        /// <summary>
        /// Gets a link to statistics for a specified entity.
        /// </summary>
        /// <param name="name">The name of the entity for which to retrieve statistics.</param>
        /// <returns>A link to statistics for the specified entity.</returns>
        public StatisticsLink ForEntity(string name)
        {
            Contract.Ensures(Contract.Result<StatisticsLink>() != null);

            if (name == null)
            {
                throw new ArgumentNullException("name");
            }

            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("name");
            }

            var resolvedTemplate = this.Target.Bind("entity", name);
            Contract.Assume(resolvedTemplate != null);

            return new StatisticsLink(this.RelationType, resolvedTemplate, this.TargetAttributes);
        }

        /// <summary>
        /// Gets a link to statistics for a specified date range.</summary>
        /// <param name="fromDate">The date from which the data of the statistic should be retrieved.</param>
        /// <returns>A link to statistics for the specified date range.</returns>
        public StatisticsLink From(DateTime fromDate)
        {
            Contract.Ensures(Contract.Result<StatisticsLink>() != null);

            if (fromDate == null)
            {
                throw new ArgumentNullException("fromDate");
            }

            var resolvedTemplate = this.Target.Bind("from", fromDate.ToUniversalTime().ToString("o"));
            Contract.Assume(resolvedTemplate != null);

            return new StatisticsLink(this.RelationType, resolvedTemplate, this.TargetAttributes);
        }

        /// <summary>
        /// Gets a link to statistics for a specified date range.
        /// </summary>
        /// <param name="toDate">The date at which point the data for the statistic should stop.</param>
        /// <returns>A link to statistics for the specified date range.</returns>
        public StatisticsLink To(DateTime toDate)
        {
            Contract.Ensures(Contract.Result<StatisticsLink>() != null);

            if (toDate == null)
            {
                throw new ArgumentNullException("toDate");
            }

            var resolvedTemplate = this.Target.Bind("to", toDate.ToUniversalTime().ToString("o"));
            Contract.Assume(resolvedTemplate != null);

            return new StatisticsLink(this.RelationType, resolvedTemplate, this.TargetAttributes);
        }
    }
}
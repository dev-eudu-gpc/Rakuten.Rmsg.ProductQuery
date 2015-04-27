// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="DataSourcesLink.cs" company="Rakuten">
//   Copyright (c) Rakuten. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.WebJob.Linking
{
    using System.Diagnostics.Contracts;
    using System.Globalization;

    using Rakuten.Rmsg.ProductQuery.WebJob.Api;

    /// <summary>
    /// Represents a link template that will return a collection of <see cref="DataSource"/>s.
    /// </summary>
    [LinkRelation(Name = "http://rels.rakuten.com/data-sources")]
    internal class DataSourcesLink : LinkTemplate
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DataSourcesLink"/> class.
        /// </summary>
        /// <param name="target">A <see cref="IUriTemplate"/> that defines the link structure.</param>
        public DataSourcesLink(IUriTemplate target)
            : base(target)
        {
            Contract.Requires(target != null);
        }

        /// <summary>
        /// Constructs a link that defines a collection of <see cref="DataSource"/>s in a specific culture.
        /// </summary>
        /// <param name="culture">The culture of the $$datasources$$.</param>
        /// <returns>A link to a collection of <see cref="DataSource"/>s defined in the specified culture.</returns>
        public ProductLink ForCulture(CultureInfo culture)
        {
            var resolvedTemplate = this.Target.Bind("culture", culture == null ? null : culture.Name);
            Contract.Assume(resolvedTemplate != null);

            return new ProductLink(resolvedTemplate);
        }
    }
}
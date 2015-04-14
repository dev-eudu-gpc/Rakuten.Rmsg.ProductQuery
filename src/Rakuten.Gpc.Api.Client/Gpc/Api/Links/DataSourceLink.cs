// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="DataSourceLink.cs" company="Rakuten">
//   Copyright (c) Rakuten. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------
namespace Rakuten.Gpc.Api.Links
{
    using System.Diagnostics.Contracts;

    /// <summary>
    /// Represents a link to a data source.
    /// </summary>
    [LinkRelation(Name = "http://rels.rakuten.com/data-source")]
    public class DataSourceLink : LinkTemplate
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DataSourceLink"/> class.
        /// </summary>
        /// <param name="target">The location of the data source this link points to.</param>
        public DataSourceLink(IUriTemplate target) : base(target)
        {
            Contract.Requires(target != null);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataSourceLink"/> class.
        /// </summary>
        /// <param name="relationType">The link relation type.</param>
        /// <param name="target">The location of the data source this link points to.</param>
        public DataSourceLink(string relationType, IUriTemplate target)
            : base(relationType, target)
        {
            Contract.Requires(target != null);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataSourceLink"/> class.
        /// </summary>
        /// <param name="relationType">The link relation type.</param>
        /// <param name="target">The location of the data source this link points to.</param>
        /// <param name="attributes">The attributes of the data source this link points to.</param>
        public DataSourceLink(string relationType, IUriTemplate target, ITargetAttributes attributes)
            : base(relationType, target, attributes)
        {
            Contract.Requires(target != null);
        }

        /// <summary>
        /// Gets a link to a data source with the specified unique identifier.
        /// </summary>
        /// <param name="id">The identifier of the data source.</param>
        /// <returns>A link to a data source with the specified unique identifier.</returns>
        public DataSourceLink ForId(string id)
        {
            var resolvedTemplate = this.Target.Bind("id", id);
            Contract.Assume(resolvedTemplate != null);

            return new DataSourceLink(this.RelationType, resolvedTemplate, this.TargetAttributes);
        }
    }
}
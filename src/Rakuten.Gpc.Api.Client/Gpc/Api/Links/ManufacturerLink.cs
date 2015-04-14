// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="ManufacturerLink.cs" company="Rakuten">
//   Copyright (c) Rakuten. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------
namespace Rakuten.Gpc.Api.Links
{
    using System.Diagnostics.Contracts;
    using System.Globalization;

    /// <summary>
    /// Represents a link to a manufacturer.
    /// </summary>
    [LinkRelation(Name = "http://rels.rakuten.com/manufacturer")]
    public class ManufacturerLink : LinkTemplate
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ManufacturerLink"/> class.
        /// </summary>
        /// <param name="target">The location of the manufacturer this link points to.</param>
        public ManufacturerLink(IUriTemplate target) : base(target)
        {
            Contract.Requires(target != null);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ManufacturerLink"/> class.
        /// </summary>
        /// <param name="relationType">The link relation type.</param>
        /// <param name="target">The location of the manufacturer this link points to.</param>
        public ManufacturerLink(string relationType, IUriTemplate target) : base(relationType, target)
        {
            Contract.Requires(target != null);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ManufacturerLink"/> class.
        /// </summary>
        /// <param name="relationType">The link relation type.</param>
        /// <param name="target">The location of the manufacturer this link points to.</param>
        /// <param name="attributes">The attributes of the manufacturer this link points to.</param>
        public ManufacturerLink(string relationType, IUriTemplate target, ITargetAttributes attributes)
            : base(relationType, target, attributes)
        {
            Contract.Requires(target != null);
        }

        /// <summary>
        /// Gets a link to a manufacturer in the specified culture.
        /// </summary>
        /// <param name="culture">The culture of the manufacturer.</param>
        /// <returns>A link to a manufacturer in the specified culture.</returns>
        public ManufacturerLink ForCulture(CultureInfo culture)
        {
            var resolvedTemplate = this.Target.Bind("culture", culture == null ? null : culture.Name);
            Contract.Assume(resolvedTemplate != null);

            return new ManufacturerLink(
                this.RelationType,
                resolvedTemplate,
                this.TargetAttributes.WithLanguageTag(culture));
        }

        /// <summary>
        /// Gets a link to a manufacturer with the specified unique identifier.
        /// </summary>
        /// <param name="id">The identifier of the manufacturer.</param>
        /// <returns>A link to a manufacturer with the specified unique identifier.</returns>
        public ManufacturerLink ForId(string id)
        {
            var resolvedTemplate = this.Target.Bind("id", id);
            Contract.Assume(resolvedTemplate != null);

            return new ManufacturerLink(this.RelationType, resolvedTemplate, this.TargetAttributes);
        }

        /// <summary>
        /// Gets a link to a manufacturer with the specified alias.
        /// </summary>
        /// <param name="alias">The alias of the manufacturer.</param>
        /// <returns>A link to a manufacturer with the specified alias.</returns>
        public ManufacturerLink ForAlias(string alias)
        {
            var resolvedTemplate = this.Target.Bind("alias", alias);
            Contract.Assume(resolvedTemplate != null);

            return new ManufacturerLink(this.RelationType, resolvedTemplate, this.TargetAttributes);
        }
    }
}
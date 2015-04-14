// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="CultureLink.cs" company="Rakuten">
//   Copyright (c) Rakuten. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------
namespace Rakuten.Gpc.Api.Links
{
    using System.Diagnostics.Contracts;
    using System.Globalization;

    /// <summary>
    /// Represents a link to a culture.
    /// </summary>
    [LinkRelation(Name = "http://rels.rakuten.com/culture")]
    public class CultureLink : LinkTemplate
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CultureLink"/> class.
        /// </summary>
        /// <param name="target">The location of the culture this link points to.</param>
        public CultureLink(IUriTemplate target) : base(target)
        {
            Contract.Requires(target != null);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CultureLink"/> class.
        /// </summary>
        /// <param name="relationType">The link relation type.</param>
        /// <param name="target">The location of the culture this link points to.</param>
        public CultureLink(string relationType, IUriTemplate target) : base(relationType, target)
        {
            Contract.Requires(target != null);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CultureLink"/> class.
        /// </summary>
        /// <param name="relationType">The link relation type.</param>
        /// <param name="target">The location of the culture this link points to.</param>
        /// <param name="attributes">The attributes of the culture this link points to.</param>
        public CultureLink(string relationType, IUriTemplate target, ITargetAttributes attributes)
            : base(relationType, target, attributes)
        {
            Contract.Requires(target != null);
        }

        /// <summary>
        /// Gets a link to a specified culture.</summary>
        /// <param name="culture">The culture code (language tag).</param>
        /// <returns>A link to the specified culture.</returns>
        public CultureLink ForCulture(CultureInfo culture)
        {
            var resolvedTemplate = this.Target.Bind("culture", culture == null ? null : culture.Name);
            Contract.Assume(resolvedTemplate != null);

            return new CultureLink(
                this.RelationType,
                resolvedTemplate,
                this.TargetAttributes.WithLanguageTag(culture));
        }
    }
}
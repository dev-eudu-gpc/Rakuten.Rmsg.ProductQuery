// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="CategoryLink.cs" company="Rakuten">
//   Copyright (c) Rakuten. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------
namespace Rakuten.Gpc.Api.Links
{
    using System.Diagnostics.Contracts;
    using System.Globalization;

    /// <summary>
    /// Represents a link to a category.
    /// </summary>
    [LinkRelation(Name = "http://rels.rakuten.com/product-category")]
    public class CategoryLink : LinkTemplate
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CategoryLink"/> class.
        /// </summary>
        /// <param name="target">The location of the category this link points to.</param>
        public CategoryLink(IUriTemplate target) : base(target)
        {
            Contract.Requires(target != null);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CategoryLink"/> class.
        /// </summary>
        /// <param name="relationType">The link relation type.</param>
        /// <param name="target">The location of the category this link points to.</param>
        public CategoryLink(string relationType, IUriTemplate target) : base(relationType, target)
        {
            Contract.Requires(target != null);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CategoryLink"/> class.
        /// </summary>
        /// <param name="relationType">The link relation type.</param>
        /// <param name="target">The location of the category this link points to.</param>
        /// <param name="attributes">The attributes of the category this link points to.</param>
        public CategoryLink(string relationType, IUriTemplate target, ITargetAttributes attributes)
            : base(relationType, target, attributes)
        {
            Contract.Requires(target != null);
        }

        /// <summary>
        /// Gets a link to a category with the specified unique identifier.
        /// </summary>
        /// <param name="id">The identifier of the category.</param>
        /// <returns>A link to a category with the specified unique identifier.</returns>
        public CategoryLink ForId(string id)
        {
            var resolvedTemplate = this.Target.Bind("id", id);
            Contract.Assume(resolvedTemplate != null);

            return new CategoryLink(this.RelationType, resolvedTemplate, this.TargetAttributes);
        }

        /// <summary>
        /// Gets a link to a category in the specified culture.
        /// </summary>
        /// <param name="culture">The culture of the category.</param>
        /// <returns>A link to a category in the specified culture.</returns>
        public CategoryLink ForCulture(CultureInfo culture)
        {
            var resolvedTemplate = this.Target.Bind("culture", culture == null ? null : culture.Name);
            Contract.Assume(resolvedTemplate != null);

            return new CategoryLink(
                this.RelationType,
                resolvedTemplate,
                this.TargetAttributes.WithLanguageTag(culture));
        }
    }
}
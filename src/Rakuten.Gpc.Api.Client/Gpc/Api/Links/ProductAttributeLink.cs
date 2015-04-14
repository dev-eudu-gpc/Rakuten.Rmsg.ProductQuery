// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="ProductAttributeLink.cs" company="Rakuten">
//   Copyright (c) Rakuten. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------
namespace Rakuten.Gpc.Api.Links
{
    using System.Diagnostics.Contracts;
    using System.Globalization;

    /// <summary>
    /// Represents a link to a product.
    /// </summary>
    [LinkRelation(Name = "http://rels.rakuten.com/product-attribute")]
    public class ProductAttributeLink : LinkTemplate
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProductAttributeLink"/> class.
        /// </summary>
        /// <param name="target">The location of the product attribute set this link points to.</param>
        public ProductAttributeLink(IUriTemplate target) : base(target)
        {
            Contract.Requires(target != null);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductAttributeLink"/> class.
        /// </summary>
        /// <param name="relationType">The link relation type.</param>
        /// <param name="target">The location of the product attribute set this link points to.</param>
        public ProductAttributeLink(string relationType, IUriTemplate target) : base(relationType, target)
        {
            Contract.Requires(target != null);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductAttributeLink"/> class.
        /// </summary>
        /// <param name="relationType">The link relation type.</param>
        /// <param name="target">The location of the product attribute set this link points to.</param>
        /// <param name="attributes">The attributes of the product attribute set this link points to.</param>
        public ProductAttributeLink(string relationType, IUriTemplate target, ITargetAttributes attributes)
            : base(relationType, target, attributes)
        {
            Contract.Requires(target != null);
        }

        /// <summary>
        /// Gets a link to a product attribute set with the specified unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the product attribute set.</param>
        /// <returns>A link to a product attribute set with the specified unique identifier.</returns>
        public ProductAttributeLink ForId(string id)
        {
            var resolvedTemplate = this.Target.Bind("id", id);
            Contract.Assume(resolvedTemplate != null);

            return new ProductAttributeLink(this.RelationType, resolvedTemplate, this.TargetAttributes);
        }

        /// <summary>
        /// Gets a link to a product in the specified culture.
        /// </summary>
        /// <param name="culture">The culture of the product.</param>
        /// <returns>A link to a product in the specified culture.</returns>
        public ProductAttributeLink ForCulture(CultureInfo culture)
        {
            var resolvedTemplate = this.Target.Bind("culture", culture == null ? null : culture.Name);
            Contract.Assume(resolvedTemplate != null);

            return new ProductAttributeLink(
                this.RelationType,
                resolvedTemplate,
                this.TargetAttributes.WithLanguageTag(culture));
        }
    }
}
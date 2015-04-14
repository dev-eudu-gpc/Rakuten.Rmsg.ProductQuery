// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="ProductLink.cs" company="Rakuten">
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
    [LinkRelation(Name = "http://rels.rakuten.com/product")]
    public class ProductLink : LinkTemplate
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProductLink"/> class.
        /// </summary>
        /// <param name="target">The location of the product this link points to.</param>
        public ProductLink(IUriTemplate target) : base(target)
        {
            Contract.Requires(target != null);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductLink"/> class.
        /// </summary>
        /// <param name="relationType">The link relation type.</param>
        /// <param name="target">The location of the product this link points to.</param>
        /// <param name="attributes">The attributes of the product this link points to.</param>
        public ProductLink(string relationType, IUriTemplate target, ITargetAttributes attributes)
            : base(relationType, target, attributes)
        {
            Contract.Requires(target != null);
        }

        /// <summary>
        /// Gets a link to a product with the specified Global Rakuten Article Number (GRAN).
        /// </summary>
        /// <param name="gran">The Global Rakuten Article Number (GRAN) of the product.</param>
        /// <returns>A link to a product with the specified Global Rakuten Article Number (GRAN).</returns>
        public ProductLink ForId(string gran)
        {
            var resolvedTemplate = this.Target.Bind("gran", gran);
            Contract.Assume(resolvedTemplate != null);

            return new ProductLink(this.RelationType, resolvedTemplate, this.TargetAttributes);
        }

        /// <summary>
        /// Gets a link to a product in the specified culture.
        /// </summary>
        /// <param name="culture">The culture of the product.</param>
        /// <returns>A link to a product in the specified culture.</returns>
        public ProductLink ForCulture(CultureInfo culture)
        {
            var resolvedTemplate = this.Target.Bind("culture", culture == null ? null : culture.Name);
            Contract.Assume(resolvedTemplate != null);

            return new ProductLink(
                this.RelationType,
                resolvedTemplate,
                this.TargetAttributes.WithLanguageTag(culture));
        }
    }
}
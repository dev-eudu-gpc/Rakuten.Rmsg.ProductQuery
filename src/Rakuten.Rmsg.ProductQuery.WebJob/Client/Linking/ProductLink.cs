// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="ProductLink.cs" company="Rakuten">
//   Copyright (c) Rakuten. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.WebJob.Linking
{
    using System.Diagnostics.Contracts;
    using System.Globalization;

    /// <summary>
    /// Represents a URI to an endpoint that will perform a product search.
    /// </summary>
    [LinkRelation(Name = "http://rels.rakuten.com/product")]
    internal class ProductLink : LinkTemplate
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProductLink"/> class.
        /// </summary>
        /// <param name="target">The location of the product query this link points to.</param>
        public ProductLink(IUriTemplate target)
            : base(target)
        {
            Contract.Requires(target != null);
        }

        /// <summary>
        /// Constructs a link to a product within a specified culture.
        /// </summary>
        /// <param name="culture">The culture of the product.</param>
        /// <returns>A link to a product defined in the specified culture.</returns>
        public ProductLink ForCulture(CultureInfo culture)
        {
            var resolvedTemplate = this.Target.Bind("culture", culture == null ? null : culture.Name);
            Contract.Assume(resolvedTemplate != null);

            return new ProductLink(resolvedTemplate);
        }

        /// <summary>
        /// Constructs a link to a product.
        /// </summary>
        /// <param name="gran">The GRAN (Global $Rakuten$ Article Number) of the product.</param>
        /// <returns>A link to a product.</returns>
        public ProductLink ForGran(string gran)
        {
            var resolvedTemplate = this.Target.Bind("gran", gran);
            Contract.Assume(resolvedTemplate != null);

            return new ProductLink(resolvedTemplate);
        }
    }
}
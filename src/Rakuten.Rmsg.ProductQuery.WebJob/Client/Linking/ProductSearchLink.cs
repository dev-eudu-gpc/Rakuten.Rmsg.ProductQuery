// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="ProductSearchLink.cs" company="Rakuten">
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
    [LinkRelation(Name = "http://rels.rakuten.com/product-search")]
    internal class ProductSearchLink : LinkTemplate
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProductSearchLink"/> class.
        /// </summary>
        /// <param name="target">The location of the product query this link points to.</param>
        public ProductSearchLink(IUriTemplate target)
            : base(target)
        {
            Contract.Requires(target != null);
        }

        /// <summary>
        /// Constructs a product search link to search for a product within a specified culture.
        /// </summary>
        /// <param name="culture">The culture of the product.</param>
        /// <returns>A link to perform a product search for products within the specified culture.</returns>
        public ProductSearchLink ForCulture(CultureInfo culture)
        {
            var resolvedTemplate = this.Target.Bind("culture", culture == null ? null : culture.Name);
            Contract.Assume(resolvedTemplate != null);

            return new ProductSearchLink(resolvedTemplate);
        }

        /// <summary>
        /// Constructs a product search link to search for a product skipping the defined number of records.
        /// </summary>
        /// <param name="count">The number of records to skip from the top of the results.</param>
        /// <returns>
        /// A link to perform a product search for products that skips a specified number of products.
        /// </returns>
        public ProductSearchLink Skipping(int count)
        {
            var resolvedTemplate = this.Target.Bind("skip", count.ToString());
            Contract.Assume(resolvedTemplate != null);

            return new ProductSearchLink(resolvedTemplate);
        }

        /// <summary>
        /// Constructs a product search link to search for a product returning only the defined number of records.
        /// </summary>
        /// <param name="count">The number of records to return.</param>
        /// <returns>
        /// A link to perform a product search for products that returns a specified number of products.
        /// </returns>
        public ProductSearchLink Taking(int count)
        {
            var resolvedTemplate = this.Target.Bind("top", count.ToString());
            Contract.Assume(resolvedTemplate != null);

            return new ProductSearchLink(resolvedTemplate);
        }

        /// <summary>
        /// Constructs a product search link to search for products with a matching EAN.
        /// </summary>
        /// <param name="value">The value to match upon.</param>
        /// <returns>A link to perform a product search matching upon an EAN.</returns>
        public ProductSearchLink WithEanFilter(string value)
        {
            var filter = string.Format("EAN eq '{0}'", value);

            var resolvedTemplate = this.Target.Bind("filter", filter);
            Contract.Assume(resolvedTemplate != null);

            return new ProductSearchLink(resolvedTemplate);
        }

        /// <summary>
        /// Constructs a product search link to search for products across all GTINs.
        /// </summary>
        /// <param name="value">The value to match upon.</param>
        /// <returns>A link to perform a product search matching upon all GTINs.</returns>
        public ProductSearchLink WithGtinFilter(string value)
        {
            var filter = string.Format("(ISBN eq '{0}' or EAN eq '{0}' or JAN eq '{0}' or UPC eq '{0}')", value);

            var resolvedTemplate = this.Target.Bind("filter", filter);
            Contract.Assume(resolvedTemplate != null);

            return new ProductSearchLink(resolvedTemplate);
        }

        /// <summary>
        /// Constructs a product search link to search for products with a matching ISBN.
        /// </summary>
        /// <param name="value">The value to match upon.</param>
        /// <returns>A link to perform a product search matching upon an ISBN.</returns>
        public ProductSearchLink WithIsbnFilter(string value)
        {
            var filter = string.Format("ISBN eq '{0}'", value);

            var resolvedTemplate = this.Target.Bind("filter", filter);
            Contract.Assume(resolvedTemplate != null);

            return new ProductSearchLink(resolvedTemplate);
        }

        /// <summary>
        /// Constructs a product search link to search for products with a matching JAN.
        /// </summary>
        /// <param name="value">The value to match upon.</param>
        /// <returns>A link to perform a product search matching upon an JAN.</returns>
        public ProductSearchLink WithJanFilter(string value)
        {
            var filter = string.Format("JAN eq '{0}'", value);

            var resolvedTemplate = this.Target.Bind("filter", filter);
            Contract.Assume(resolvedTemplate != null);

            return new ProductSearchLink(resolvedTemplate);
        }

        /// <summary>
        /// Constructs a product search link to search for products with a matching UPC.
        /// </summary>
        /// <param name="value">The value to match upon.</param>
        /// <returns>A link to perform a product search matching upon an UPC.</returns>
        public ProductSearchLink WithUpcFilter(string value)
        {
            var filter = string.Format("UPC eq '{0}'", value);

            var resolvedTemplate = this.Target.Bind("filter", filter);
            Contract.Assume(resolvedTemplate != null);

            return new ProductSearchLink(resolvedTemplate);
        }
    }
}
//------------------------------------------------------------------------------
// <copyright file="ProductQueryCommandParameters.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.Web.Http.Commands
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Globalization;

    /// <summary>
    /// Base class for parameters required for product query related commands.
    /// </summary>
    internal abstract class ProductQueryCommandParameters
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProductQueryCommandParameters"/> class.
        /// </summary>
        /// <param name="id">The unique identifier of the product query.</param>
        /// <param name="culture">The culture of products for the query.</param>
        public ProductQueryCommandParameters(string id, string culture)
        {
            Contract.Requires(id != null);

            // Try and parse the Id
            Guid parsedId;

            if (!Guid.TryParse(id, out parsedId))
            {
                throw new InvalidGuidException("product query identifier", id);
            }

            this.Id = parsedId;

            // Try and parse the culture
            CultureInfo cultureInfo = null;
            try
            {
                cultureInfo = new CultureInfo(culture);
            }
            catch (CultureNotFoundException)
            {
                throw new InvalidCultureException("culture", culture);
            }

            this.Culture = cultureInfo;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductQueryCommandParameters"/> class.
        /// </summary>
        /// <param name="id">The unique identifier of the product query.</param>
        /// <param name="culture">The culture of products for the query.</param>
        public ProductQueryCommandParameters(Guid id, CultureInfo culture)
        {
            Contract.Requires(culture != null);

            this.Culture = culture;
            this.Id = id;
        }

        /// <summary>
        /// Gets the culture for products in the query.
        /// </summary>
        public CultureInfo Culture { get; private set; }

        /// <summary>
        /// Gets the unique identifier of the product query.
        /// </summary>
        public Guid Id { get; private set; }
    }
}
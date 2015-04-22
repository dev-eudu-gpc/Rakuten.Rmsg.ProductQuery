//------------------------------------------------------------------------------
// <copyright file="CreateProgressImageCommandParameters.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.Web.Http.Commands
{
    using System.Diagnostics.Contracts;
    using System.Linq;

    /// <summary>
    /// The parameters required for creating an image for the progress of product
    /// queries within a product query group.
    /// </summary>
    public class CreateProgressImageCommandParameters
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateProgressImageCommandParameters"/> class.
        /// </summary>
        /// <param name="progressMap">The progress map to create an image for.</param>
        public CreateProgressImageCommandParameters(IQueryable<ProductQueryProgress> progressMap)
        {
            Contract.Requires(progressMap != null);

            this.ProgressMap = progressMap;
        }

        /// <summary>
        /// Gets the progress map to create an image for.
        /// </summary>
        public IQueryable<ProductQueryProgress> ProgressMap { get; private set; }
    }
}
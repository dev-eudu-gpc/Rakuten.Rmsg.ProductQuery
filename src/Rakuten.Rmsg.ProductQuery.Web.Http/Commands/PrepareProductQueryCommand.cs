//------------------------------------------------------------------------------
// <copyright file="Filename.extension" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.Web.Http.Commands
{
    using System;
    using System.Collections.ObjectModel;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web;
    using Rakuten.Rmsg.ProductQuery.Web.Http.Links;

    /// <summary>
    /// Represents a command for preparing a product query
    /// </summary>
    public class PrepareProductQueryCommand : AsyncCommand<PrepareProductQueryCommandParameters, ProductQuery>
    {
        /// <summary>
        /// A link representing the canonical location of the resource.
        /// </summary>
        private readonly ProductQueryLink selfLink;

        /// <summary>
        /// Initializes a new instance of the <see cref="PrepareProductQueryCommand"/> class
        /// </summary>
        /// <param name="productQueryUriTemplate">A link template representing the canonical location of the resource.</param>
        public PrepareProductQueryCommand(IUriTemplate productQueryUriTemplate)
        {
            Contract.Requires(productQueryUriTemplate != null);

            this.selfLink = new ProductQueryLink(productQueryUriTemplate);
        }

        /// <summary>
        /// Prepares a product query.
        /// </summary>
        /// <param name="parameters">The input parameters enabling the product query to be uniquely identified</param>
        /// <returns></returns>
        public override Task<ProductQuery> ExecuteAsync(PrepareProductQueryCommandParameters parameters)
        {
            Contract.Requires(parameters != null);

            var self = this.selfLink
                .ForId(parameters.Id.ToString())
                .ToLink(resolveTemplate: true);

            return Task.Run(() =>
                {
                    var z = new ProductQuery
                    {
                        Links = new Collection<Link> { self },
                        Status = "new"
                    };

                    return z;
                });
        }
    }
}
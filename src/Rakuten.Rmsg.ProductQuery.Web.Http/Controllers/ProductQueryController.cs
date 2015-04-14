//------------------------------------------------------------------------------
// <copyright file="ProductQueryController.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.Web.Http
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.Http;
    using Rakuten.Rmsg.ProductQuery.Web.Http.Commands;

    /// <summary>
    /// Defines endpoints for creating and obtaining information for product queries
    /// </summary>
    public class ProductQueryController : ApiController
    {
        /// <summary>
        /// A command that can prepare a new product query.
        /// </summary>
        private readonly ICommand<PrepareProductQueryCommandParameters, Task<ProductQuery>> prepareCommand;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductQueryController"/> class
        /// </summary>
        /// <param name="prepareCommand">A command that can prepare a new product query</param>
        public ProductQueryController(
            ICommand<PrepareProductQueryCommandParameters, Task<ProductQuery>> prepareCommand)
        {
            Contract.Requires(prepareCommand != null);

            this.prepareCommand = prepareCommand;
        }
        /// <summary>
        /// Prepares a new product query
        /// </summary>
        /// <param name="id">The unique identifier for the product query</param>
        /// <returns>A representation of the new product query</returns>
        [Route("product-query/{id}")]
        public Task<ProductQuery> Put (Guid id)
        {
            Contract.Assume (this.prepareCommand != null);

            var parameters = new PrepareProductQueryCommandParameters(id);

            var x = this.prepareCommand.Execute(parameters);

            return x;
        }
    }
}
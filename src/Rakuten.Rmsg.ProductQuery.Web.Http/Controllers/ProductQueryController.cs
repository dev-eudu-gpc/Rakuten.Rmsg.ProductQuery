//------------------------------------------------------------------------------
// <copyright file="ProductQueryController.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.Web.Http
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Net;
    using System.Threading.Tasks;
    using System.Web.Http;
    using System.Web.Http.Results;
    using Rakuten.Rmsg.ProductQuery.Web.Http.Commands;

    /// <summary>
    /// Defines endpoints for creating and obtaining information for product queries
    /// </summary>
    public class ProductQueryController : ApiController
    {
        /// <summary>
        /// A command that can get a product query from a database.
        /// </summary>
        private readonly ICommand<GetProductQueryCommandParameters, Task<ProductQuery>> getProductQueryCommand;

        /// <summary>
        /// A command that can prepare a new product query.
        /// </summary>
        private readonly ICommand<CreateProductQueryCommandParameters, Task<ProductQuery>> createProductQueryCommand;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductQueryController"/> class
        /// </summary>
        /// <param name="getProductQueryCommand">A command that gets a specified product query.</param>
        /// <param name="createProductQueryCommand">A command that creates a new product query.</param>
        public ProductQueryController(
            ICommand<GetProductQueryCommandParameters, Task<ProductQuery>> getProductQueryCommand,
            ICommand<CreateProductQueryCommandParameters, Task<ProductQuery>> createProductQueryCommand)
        {
            Contract.Requires(getProductQueryCommand != null);
            Contract.Requires(createProductQueryCommand != null);

            this.getProductQueryCommand = getProductQueryCommand;
            this.createProductQueryCommand = createProductQueryCommand;
        }

        /// <summary>
        /// Prepares a new product query
        /// </summary>
        /// <param name="id">The unique identifier for the product query</param>
        /// <returns>A representation of the new product query</returns>
        [Route("product-query/{id}")]
        public async Task<IHttpActionResult> Get(Guid id)
        {
            Contract.Assume(this.getProductQueryCommand != null);
            Contract.Assume(this.createProductQueryCommand != null);

            // Try and get the product query
            ProductQuery query = await this.getProductQueryCommand.Execute(
                new GetProductQueryCommandParameters(id));

            if (query == null)
            {
                // Create new product query
                var parameters = new CreateProductQueryCommandParameters(id);

                return new NegotiatedContentResult<ProductQuery>(
                    HttpStatusCode.Created,
                    await this.createProductQueryCommand.Execute(parameters),
                    this);
            }
            else
            {
                // Query already exists so return it
                return new NegotiatedContentResult<ProductQuery>(HttpStatusCode.OK, query, this);
            }
        }
    }
}
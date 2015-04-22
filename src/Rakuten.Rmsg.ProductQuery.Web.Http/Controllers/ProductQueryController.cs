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
    using Rakuten.Gpc.Api;
    using Rakuten.Rmsg.ProductQuery.Web.Http.Commands;
    using Rakuten.Rmsg.ProductQuery.Web.Http.Links;

    /// <summary>
    /// Defines endpoints for creating and obtaining information for product queries.
    /// </summary>
    public class ProductQueryController : ApiController
    {
        /// <summary>
        /// A command that can prepare a new product query.
        /// </summary>
        private readonly ICommand<CreateProductQueryCommandParameters, Task<ProductQuery>> createProductQueryCommand;

        /// <summary>
        /// A command that can get a product query from a database.
        /// </summary>
        private readonly ICommand<GetProductQueryCommandParameters, Task<ProductQuery>> getProductQueryCommand;

        /// <summary>
        /// A command that can make a product query ready for processing.
        /// </summary>
        private readonly ICommand<ReadyForProcessingCommandParameters, Task<ProductQuery>> readyForProcessingCommand;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductQueryController"/> class.
        /// </summary>
        /// <param name="getProductQueryCommand">A command that gets a specified product query.</param>
        /// <param name="createProductQueryCommand">A command that creates a new product query.</param>
        /// <param name="readyForProcessingCommand">A command that can make a product query ready for processing.</param>
        public ProductQueryController(
            ICommand<GetProductQueryCommandParameters, Task<ProductQuery>> getProductQueryCommand,
            ICommand<CreateProductQueryCommandParameters, Task<ProductQuery>> createProductQueryCommand,
            ICommand<ReadyForProcessingCommandParameters, Task<ProductQuery>> readyForProcessingCommand)
        {
            Contract.Requires(createProductQueryCommand != null);
            Contract.Requires(getProductQueryCommand != null);
            Contract.Requires(readyForProcessingCommand != null);

            this.createProductQueryCommand = createProductQueryCommand;
            this.getProductQueryCommand = getProductQueryCommand;
            this.readyForProcessingCommand = readyForProcessingCommand;
        }

        /// <summary>
        /// Accepts PUT requests for both preparing a new product query and
        /// servicing existing product queries.
        /// </summary>
        /// <param name="id">The unique identifier for the product query.</param>
        /// <param name="source">A new representation of the product query.</param>
        /// <returns>A representation of the new product query.</returns>
        [Route("product-query/{id}")]
        public async Task<IHttpActionResult> PutAsync(Guid id, ProductQuery source)
        {
            if (source == null)
            {
                return await this.CreateAsync(id);
            }
            else
            {
                return await this.ReadyForProcessingAsync(id, source);
            }
        }

        /// <summary>
        /// Creates a new product query.
        /// </summary>
        /// <param name="id">The unique identifier for the product query.</param>
        /// <returns>A representation of the new product query.</returns>
        private async Task<IHttpActionResult> CreateAsync(Guid id)
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

        /// <summary>
        /// Makes a product query ready for processing.
        /// </summary>
        /// <param name="id">The unique identifier of the product query.</param>
        /// <param name="source">The status of the product query.  This must be "submitted".</param>
        /// <returns>A full representation of the product query.</returns>
        private async Task<IHttpActionResult> ReadyForProcessingAsync(Guid id, ProductQuery source)
        {
            // Ensure that the requested status is "submitted"
            if (source.Status != ProductQueryStatus.Submitted)
            {
                throw new ValidationFailedException(new InvalidStatusException());
            }

            // Call the command
            try
            {
                ProductQuery productQuery = await this.readyForProcessingCommand.Execute(
                    new ReadyForProcessingCommandParameters(id));

                return new NegotiatedContentResult<ProductQuery>(
                    HttpStatusCode.Accepted,
                    productQuery,
                    this);
            }
            catch (ProductQueryNotFoundException ex)
            {
                throw new ObjectNotFoundException(ex);
            }
        }
    }
}
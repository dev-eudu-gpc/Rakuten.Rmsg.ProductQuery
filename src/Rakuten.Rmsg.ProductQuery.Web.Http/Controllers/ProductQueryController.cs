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
        private readonly ICommand<CreateCommandParameters, Task<ProductQuery>> createProductQueryCommand;

        /// <summary>
        /// A command that can get a product query from a database.
        /// </summary>
        private readonly ICommand<GetCommandParameters, Task<ProductQuery>> getProductQueryCommand;

        /// <summary>
        /// A command that can make a product query ready for processing.
        /// </summary>
        private readonly ICommand<ReadyForProcessingCommandParameters, Task<ProductQuery>> readyForProcessingCommand;

        /// <summary>
        /// A link template representing the canonical link of a product query.
        /// </summary>
        private readonly IUriTemplate productQueryUriTemplate;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductQueryController"/> class.
        /// </summary>
        /// <param name="getProductQueryCommand">A command that gets a specified product query.</param>
        /// <param name="createProductQueryCommand">A command that creates a new product query.</param>
        /// <param name="readyForProcessingCommand">A command that can make a product query ready for processing.</param>
        /// <param name="productQueryUriTemplate">A link template representing the canonical location of the resource.</param>
        internal ProductQueryController(
            ICommand<GetCommandParameters, Task<ProductQuery>> getProductQueryCommand,
            ICommand<CreateCommandParameters, Task<ProductQuery>> createProductQueryCommand,
            ICommand<ReadyForProcessingCommandParameters, Task<ProductQuery>> readyForProcessingCommand,
            IUriTemplate productQueryUriTemplate)
        {
            Contract.Requires(createProductQueryCommand != null);
            Contract.Requires(getProductQueryCommand != null);
            Contract.Requires(productQueryUriTemplate != null);
            Contract.Requires(readyForProcessingCommand != null);

            this.createProductQueryCommand = createProductQueryCommand;
            this.getProductQueryCommand = getProductQueryCommand;
            this.productQueryUriTemplate = productQueryUriTemplate;
            this.readyForProcessingCommand = readyForProcessingCommand;
        }

        /// <summary>
        /// Accepts PUT requests for both preparing a new product query and
        /// servicing existing product queries.
        /// </summary>
        /// <param name="id">The unique identifier for the product query.</param>
        /// <param name="culture">The culture of products for the query.</param>
        /// <param name="source">A new representation of the product query.</param>
        /// <returns>A representation of the new product query.</returns>
        [Route("product-query/{id}/culture/{culture}")]
        public async Task<IHttpActionResult> PutAsync(
            string id,
            string culture,
            ReadyForProcessingRequest source)
        {
            if (source == null)
            {
                return await this.CreateAsync(id, culture);
            }
            else
            {
                return await this.ReadyForProcessingAsync(id, culture, source);
            }
        }

        /// <summary>
        /// Creates a new product query.
        /// </summary>
        /// <param name="id">The unique identifier for the product query.</param>
        /// <param name="culture">The culture of products for the query.</param>
        /// <returns>A representation of the new product query.</returns>
        private async Task<IHttpActionResult> CreateAsync(string id, string culture)
        {
            Contract.Assume(this.getProductQueryCommand != null);
            Contract.Assume(this.createProductQueryCommand != null);

            // Initialize
            IHttpActionResult result = null;

            // Create the parameters for getting the product query.
            GetCommandParameters getCommandParameters = null;

            try
            {
                getCommandParameters = new GetCommandParameters(id, culture);
            }
            catch (InvalidGuidException guidEx)
            {
                throw new BadRequestException(guidEx);
            }
            catch (InvalidCultureException cultureEx)
            {
                throw new BadRequestException(cultureEx);
            }

            // Try and get the product query
            ProductQuery query = await this.getProductQueryCommand.Execute(getCommandParameters);

            if (query == null)
            {
                // Query does not exist so create a new one
                try
                {
                    var productQuery = await this.createProductQueryCommand.Execute(
                        new CreateCommandParameters(getCommandParameters.Id, getCommandParameters.Culture));

                    result = new CreatedNegotiatedContentResult<ProductQuery>(
                        productQuery.GetUri(),
                        productQuery,
                        this);
                }
                catch
                {
                    throw new InternalServerException(null);
                }
            }
            else
            {
                // Check culture
                if (query.CultureName.Equals(getCommandParameters.Culture.Name, StringComparison.InvariantCultureIgnoreCase))
                {
                    // Query exists and is in the same culture as that specified
                    // so just return the existing query
                    result = new OkNegotiatedContentResult<ProductQuery>(query, this);
                }
                else
                {
                    // Query exists but is in a different culture to that specified
                    // so redirect the user to the correct URI
                    result = new SeeOtherResult(query.GetUri(), this.Request);
                }
            }

            return result;
        }

        /// <summary>
        /// Makes a product query ready for processing.
        /// </summary>
        /// <param name="id">The unique identifier of the product query.</param>
        /// <param name="culture">The culture of products for the query.</param>
        /// <param name="source">The status of the product query.  This must be "submitted".</param>
        /// <returns>A full representation of the product query.</returns>
        private async Task<IHttpActionResult> ReadyForProcessingAsync(
            string id,
            string culture,
            ReadyForProcessingRequest source)
        {
            // Ensure that the requested status is "submitted"
            if (!source.Status.Equals("submitted", StringComparison.InvariantCultureIgnoreCase))
            {
                throw new ValidationFailedException(new InvalidStatusException(
                    id, culture, source.Status, this.productQueryUriTemplate));
            }

            IHttpActionResult result = null;
            try
            {
                // Execute the command
                ProductQuery productQuery = await this.readyForProcessingCommand.Execute(
                    new ReadyForProcessingCommandParameters(id, culture));

                // Construct and return the response
                result = new AcceptedNegotiatedContentResult<ProductQuery>(productQuery, this);
            }
            catch (ProductQueryCultureNotFoundException cultureException)
            {
                result = new SeeOtherResult(cultureException.ProductQuery.GetUri(), this.Request);
            }
            catch (ProductQueryNotFoundException notFoundException)
            {
                throw new ObjectNotFoundException(notFoundException);
            }
            catch (InvalidGuidException guidEx)
            {
                throw new BadRequestException(guidEx);
            }
            catch (InvalidCultureException cultureEx)
            {
                throw new BadRequestException(cultureEx);
            }

            return result;
        }
    }
}
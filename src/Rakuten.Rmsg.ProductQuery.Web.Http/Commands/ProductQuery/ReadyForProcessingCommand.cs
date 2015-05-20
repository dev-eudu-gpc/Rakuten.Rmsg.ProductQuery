//------------------------------------------------------------------------------
// <copyright file="ReadyForProcessingCommand.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.Web.Http.Commands
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;
    using Rakuten.Rmsg.ProductQuery.Configuration;

    /// <summary>
    /// Represents a command for making a product query ready for processing.
    /// </summary>
    internal class ReadyForProcessingCommand : AsyncCommand<ReadyForProcessingCommandParameters, ProductQuery>
    {
        /// <summary>
        /// The context under which this instance is operating.
        /// </summary>
        private readonly IApiContext context;

        /// <summary>
        /// A command that dispatches a product query message to the queue.
        /// </summary>
        private readonly ICommand<DispatchMessageCommandParameters, Task> dispatchMessageCommand;

        /// <summary>
        /// A command that can get a product query from a database.
        /// </summary>
        private readonly ICommand<GetCommandParameters, Task<ProductQuery>> getProductQueryCommand;

        /// <summary>
        /// A link template representing the canonical link of a product query.
        /// </summary>
        private readonly IUriTemplate productQueryUriTemplate;

        /// <summary>
        /// A command that updates the status of a product query in the database.
        /// </summary>
        private readonly ICommand<UpdateStatusDatabaseCommandParameters, Task> updateProductQueryStatusDatabaseCommand;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReadyForProcessingCommand"/> class
        /// </summary>
        /// <param name="context">The context in which this instance is running.</param>
        /// <param name="dispatchMessageCommand">A command that dispatches a product query message to the queue.</param>
        /// <param name="getProductQueryCommand">A command for getting a product query.</param>
        /// <param name="updateProductQueryStatusDatabaseCommand">A command that updates the status of a product query in the database.</param>
        /// <param name="productQueryUriTemplate">A link template representing the canonical location of the resource.</param>
        public ReadyForProcessingCommand(
            IApiContext context,
            ICommand<DispatchMessageCommandParameters, Task> dispatchMessageCommand,
            ICommand<GetCommandParameters, Task<ProductQuery>> getProductQueryCommand,
            ICommand<UpdateStatusDatabaseCommandParameters, Task> updateProductQueryStatusDatabaseCommand,
            IUriTemplate productQueryUriTemplate)
        {
            Contract.Requires(context != null);
            Contract.Requires(dispatchMessageCommand != null);
            Contract.Requires(getProductQueryCommand != null);
            Contract.Requires(productQueryUriTemplate != null);
            Contract.Requires(updateProductQueryStatusDatabaseCommand != null);

            this.context = context;
            this.dispatchMessageCommand = dispatchMessageCommand;
            this.getProductQueryCommand = getProductQueryCommand;
            this.productQueryUriTemplate = productQueryUriTemplate;
            this.updateProductQueryStatusDatabaseCommand = updateProductQueryStatusDatabaseCommand;
        }

        /// <summary>
        /// Makes a product query ready for processing.
        /// </summary>
        /// <param name="parameters">The input parameters enabling the product query to be uniquely identified</param>
        /// <returns>A task that does the work.</returns>
        public override async Task<ProductQuery> ExecuteAsync(ReadyForProcessingCommandParameters parameters)
        {
            Contract.Assume(parameters != null);

            // Initialize
            var dateCreated = DateTime.Now;

            // Try and get the product query
            ProductQuery productQuery = await this.getProductQueryCommand.Execute(
                new GetCommandParameters(parameters.Id, parameters.Culture));

            // Ensure the product query was found
            if (productQuery == null)
            {
                throw new ProductQueryNotFoundException(parameters.Id);
            }

            // Ensure the product query is in the specified culture
            if (!productQuery.CultureName.Equals(parameters.Culture.Name, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new ProductQueryCultureNotFoundException(parameters.Id, parameters.Culture, productQuery);
            }

            if (productQuery.Status == ProductQueryStatus.New)
            {
                // Send a message to the queue.
                var blobLink = productQuery.Links.First(link => link.RelationType.Equals("enclosure", StringComparison.InvariantCultureIgnoreCase));
                await this.dispatchMessageCommand.Execute(new DispatchMessageCommandParameters(
                    parameters.Id, parameters.Culture.Name, blobLink));

                // Update the status of the product query in the database.
                await this.updateProductQueryStatusDatabaseCommand.Execute(
                    new UpdateStatusDatabaseCommandParameters(parameters.Id, ProductQueryStatus.Submitted));

                // Update the status in the product query object rather than get
                // it again from the database as we know that that is the only change
                productQuery.Status = ProductQueryStatus.Submitted;
            }
            else
            {
                throw new ProductQueryAlreadySubmittedException(
                    parameters.Id.ToString(),
                    parameters.Culture.Name,
                    this.productQueryUriTemplate);
            }

            return productQuery;
        }
    }
}
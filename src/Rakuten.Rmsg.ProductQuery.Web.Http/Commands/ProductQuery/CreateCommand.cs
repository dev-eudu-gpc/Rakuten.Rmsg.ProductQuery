//------------------------------------------------------------------------------
// <copyright file="CreateCommand.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.Web.Http.Commands
{
    using System;
    using System.Collections.ObjectModel;
    using System.Diagnostics.Contracts;
    using System.Threading.Tasks;
    using Rakuten.Rmsg.ProductQuery.Configuration;
    using Rakuten.Rmsg.ProductQuery.Web.Http.EntityModels;
    using Rakuten.Rmsg.ProductQuery.Web.Http.Links;

    /// <summary>
    /// Represents a command for creating a new product query.
    /// </summary>
    internal class CreateCommand : AsyncCommand<CreateCommandParameters, ProductQuery>
    {
        /// <summary>
        /// A link representing the canonical location of the blob in Azure storage.
        /// </summary>
        private readonly AzureBlobLink azureBlobLink;

        /// <summary>
        /// The context under which this instance is operating.
        /// </summary>
        private readonly IApiContext context;

        /// <summary>
        /// A command for creating a blob in storage for the product query file.
        /// </summary>
        private readonly ICommand<CreateStorageBlobCommandParameters, Task<Uri>> createStorageBlobCommand;

        /// <summary>
        /// A command that can create an entry in a database for the product query.
        /// </summary>
        private readonly ICommand<CreateDatabaseCommandParameters, Task<rmsgProductQuery>> createProductQueryDatabaseCommand;

        /// <summary>
        /// A link representing the canonical location of the monitor for the resource.
        /// </summary>
        private readonly ProductQueryMonitorLink monitorLink;

        /// <summary>
        /// A link representing the canonical location of the resource.
        /// </summary>
        private readonly ProductQueryLink selfLink;

        /// <summary>
        /// A command for updating a product query's uri.
        /// </summary>
        private readonly ICommand<UpdateUriDatabaseCommandParameters, Task> updateProductQueryUriCommand;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateCommand"/> class
        /// </summary>
        /// <param name="context">The context in which this instance is running.</param>
        /// <param name="productQueryUriTemplate">A link template representing the canonical location of the resource.</param>
        /// <param name="azureBlobUriTemplate">A link template representing the canonical location of the blob in Azure storage.</param>
        /// <param name="monitorUriTemplate">A link template representing the canonical location of the monitor for the resource.</param>
        /// <param name="createProductQueryDatabaseCommand">A command for creating an entry in a database for the product query.</param>
        /// <param name="createStorageBlobCommand">A command for creating a blob in storage for the product query file</param>
        /// <param name="updateProductQueryUriCommand">A command for updating a product query's uri</param>
        public CreateCommand(
            IApiContext context,
            IUriTemplate productQueryUriTemplate,
            IUriTemplate azureBlobUriTemplate,
            IUriTemplate monitorUriTemplate,
            ICommand<CreateDatabaseCommandParameters, Task<rmsgProductQuery>> createProductQueryDatabaseCommand,
            ICommand<CreateStorageBlobCommandParameters, Task<Uri>> createStorageBlobCommand,
            ICommand<UpdateUriDatabaseCommandParameters, Task> updateProductQueryUriCommand)
        {
            Contract.Requires(context != null);
            Contract.Requires(productQueryUriTemplate != null);
            Contract.Requires(azureBlobUriTemplate != null);
            Contract.Requires(createProductQueryDatabaseCommand != null);
            Contract.Requires(createStorageBlobCommand != null);
            Contract.Requires(updateProductQueryUriCommand != null);

            this.azureBlobLink = new AzureBlobLink("enclosure", azureBlobUriTemplate);
            this.context = context;
            this.createStorageBlobCommand = createStorageBlobCommand;
            this.createProductQueryDatabaseCommand = createProductQueryDatabaseCommand;
            this.monitorLink = new ProductQueryMonitorLink("monitor", monitorUriTemplate, new TargetAttributes(null, "image/png", null));
            this.selfLink = new ProductQueryLink(productQueryUriTemplate);
            this.updateProductQueryUriCommand = updateProductQueryUriCommand;
        }

        /// <summary>
        /// Creates a new product query.
        /// </summary>
        /// <param name="parameters">The input parameters enabling the product query to be uniquely identified</param>
        /// <returns>A task that does the work.</returns>
        public override async Task<ProductQuery> ExecuteAsync(CreateCommandParameters parameters)
        {
            // Initialize
            var dateCreated = DateTime.Now;

            // Create an entry for the query in the database
            rmsgProductQuery newProductQuery = await this.createProductQueryDatabaseCommand.Execute(
                new CreateDatabaseCommandParameters(parameters.Id, parameters.Culture, dateCreated));

            //// TODO: [WB 15-Apr-2015] Catch azure exceptions and update database accordingly

            // Create blob in storage
            Uri blobUri = await this.createStorageBlobCommand.Execute(
                new CreateStorageBlobCommandParameters(dateCreated, parameters.Id));

            // Update the database with the blob Uri
            await this.updateProductQueryUriCommand.Execute(
                new UpdateUriDatabaseCommandParameters(parameters.Id, blobUri));

            // Construct and return a new product query object
            return new ProductQuery
            {
                DateCreated = dateCreated,
                Links = new Collection<Link> 
                {
                    this.selfLink
                        .ForId(parameters.Id.ToString())
                        .ForCulture(parameters.Culture.Name)
                        .Expand(),
                    this.azureBlobLink.ForId(blobUri.ToString()).Expand(),
                    this.monitorLink.ForId(newProductQuery.rmsgProductQueryGroupID.ToString())
                },
                Status = ProductQueryStatus.New
            };
        }
    }
}
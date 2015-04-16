//------------------------------------------------------------------------------
// <copyright file="CreateProductQueryCommand.cs" company="Rakuten">
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
    using Rakuten.Rmsg.ProductQuery.Web.Http.Links;
    using Rakuten.WindowsAzure.Storage;

    /// <summary>
    /// Represents a command for preparing a product query
    /// </summary>
    public class CreateProductQueryCommand : AsyncCommand<CreateProductQueryCommandParameters, ProductQuery>
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
        private readonly ICommand<CreateProductQueryDatabaseCommandParameters, Task> createProductQueryDatabaseCommand;

        /// <summary>
        /// A link representing the canonical location of the resource.
        /// </summary>
        private readonly ProductQueryLink selfLink;

        /// <summary>
        /// The object for interacting with storage.
        /// </summary>
        private readonly IStorage storage;

        /// <summary>
        /// A command for updating a product query's uri.
        /// </summary>
        private readonly ICommand<UpdateProductQueryUriCommandParameters, Task> updateProductQueryUriCommand;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateProductQueryCommand"/> class
        /// </summary>
        /// <param name="storage">A means to interact with storage for the product query</param>
        /// <param name="context">The context in which this instance is running.</param>
        /// <param name="productQueryUriTemplate">A link template representing the canonical location of the resource.</param>
        /// <param name="azureBlobUriTemplate">A link template representing the canonical location of the blob in Azure storage.</param>
        /// <param name="createProductQueryDatabaseCommand">A command for creating an entry in a database for the product query.</param>
        /// <param name="createStorageBlobCommand">A command for creating a blob in storage for the product query file</param>
        /// <param name="updateProductQueryUriCommand">A command for updating a product query's uri</param>
        public CreateProductQueryCommand(
            IStorage storage,
            IApiContext context,
            IUriTemplate productQueryUriTemplate,
            IUriTemplate azureBlobUriTemplate,
            ICommand<CreateProductQueryDatabaseCommandParameters, Task> createProductQueryDatabaseCommand,
            ICommand<CreateStorageBlobCommandParameters, Task<Uri>> createStorageBlobCommand,
            ICommand<UpdateProductQueryUriCommandParameters, Task> updateProductQueryUriCommand)
        {
            Contract.Requires(storage != null);
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
            this.selfLink = new ProductQueryLink(productQueryUriTemplate);
            this.storage = storage;
            this.updateProductQueryUriCommand = updateProductQueryUriCommand;
        }

        /// <summary>
        /// Creates a new product query.
        /// </summary>
        /// <param name="parameters">The input parameters enabling the product query to be uniquely identified</param>
        /// <returns>A task that does the work.</returns>
        public override async Task<ProductQuery> ExecuteAsync(CreateProductQueryCommandParameters parameters)
        {
            Contract.Requires(parameters != null);

            // Initialize
            var dateCreated = DateTime.Now;

            // Create an entry for the query in the database
            await this.createProductQueryDatabaseCommand.Execute(
                new CreateProductQueryDatabaseCommandParameters(parameters.Id, dateCreated));

            // TODO: [WB 15-Apr-2015] Catch azure exceptions and update database accordingly

            // Create blob in storage
            Uri blobUri = await this.createStorageBlobCommand.Execute(
                new CreateStorageBlobCommandParameters(dateCreated, parameters.Id));

            // Update the database with the blob Uri
            await this.updateProductQueryUriCommand.Execute(
                new UpdateProductQueryUriCommandParameters(parameters.Id, blobUri));

            // Construct and return a new product query object
            return new ProductQuery
            {
                Links = new Collection<Link> 
                {
                    this.selfLink.ForId(parameters.Id.ToString()).ToLink(true),
                    this.azureBlobLink.ForId(blobUri.ToString()).ToLink(true)
                },
                Status = "new"
            };
        }
    }
}
//------------------------------------------------------------------------------
// <copyright file="GetCommand.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.Web.Http.Commands
{
    using System.Collections.ObjectModel;
    using System.Diagnostics.Contracts;
    using System.Threading.Tasks;
    using Rakuten.Rmsg.ProductQuery.Configuration;
    using Rakuten.Rmsg.ProductQuery.Web.Http.Links;
    using Rakuten.WindowsAzure.Storage;

    /// <summary>
    /// Represents a command for obtaining a product query.
    /// </summary>
    internal class GetCommand : AsyncCommand<GetCommandParameters, ProductQuery>
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
        /// A command that can get a product query from a database.
        /// </summary>
        private readonly ICommand<GetDatabaseCommandParameters, Task<ProductQuery>> getDatabaseCommand;

        /// <summary>
        /// A link representing the canonical location of the monitor for the resource.
        /// </summary>
        private readonly ProductQueryMonitorLink monitorLink;

        /// <summary>
        /// A link representing the canonical location of the resource.
        /// </summary>
        ////private readonly ProductQueryLink selfLink;
        private readonly ProductQueryLink selfLink;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetCommand"/> class
        /// </summary>
        /// <param name="context">The context in which this instance is running.</param>
        /// <param name="productQueryUriTemplate">A link template representing the canonical location of the resource.</param>
        /// <param name="azureBlobUriTemplate">A link template representing the canonical location of the blob in Azure storage.</param>
        /// <param name="monitorUriTemplate">A link template representing the canonical location of the monitor for the resource.</param>
        /// <param name="getDatabaseCommand">A command that gets product query data from the database.</param>
        public GetCommand(
            IApiContext context,
            IUriTemplate productQueryUriTemplate,
            IUriTemplate azureBlobUriTemplate,
            IUriTemplate monitorUriTemplate,
            ICommand<GetDatabaseCommandParameters, Task<ProductQuery>> getDatabaseCommand)
        {
            Contract.Requires(context != null);
            Contract.Requires(productQueryUriTemplate != null);
            Contract.Requires(azureBlobUriTemplate != null);
            Contract.Requires(monitorUriTemplate != null);
            Contract.Requires(getDatabaseCommand != null);

            this.azureBlobLink = new AzureBlobLink("enclosure", azureBlobUriTemplate);
            this.context = context;
            this.getDatabaseCommand = getDatabaseCommand;
            this.monitorLink = new ProductQueryMonitorLink("monitor", monitorUriTemplate, new TargetAttributes(null, "image/png", null));
            this.selfLink = new ProductQueryLink(productQueryUriTemplate);
        }

        /// <summary>
        /// Gets a product query object.
        /// </summary>
        /// <param name="parameters">The input parameters enabling the product query to be uniquely identified</param>
        /// <returns>A task the gets a product query object.</returns>
        public override async Task<ProductQuery> ExecuteAsync(GetCommandParameters parameters)
        {
            // Get the product query from the database
            ProductQuery productQuery = await this.getDatabaseCommand.Execute(
                new GetDatabaseCommandParameters(parameters.Id));

            if (productQuery != null)
            {
                // Add links
                productQuery.Links = new Collection<Link> 
                {
                    this.selfLink
                        .ForId(parameters.Id.ToString())
                        .ForCulture(parameters.Culture.Name)
                        .ToLink(true),
                    this.monitorLink
                        .ForId(productQuery.GroupId.ToString())
                        .ToLink(true)
                };

                if (!string.IsNullOrEmpty(productQuery.Uri))
                {
                    productQuery.Links.Add(this.azureBlobLink.ForId(productQuery.Uri).ToLink(true));
                }
            }

            return productQuery;
        }
    }
}
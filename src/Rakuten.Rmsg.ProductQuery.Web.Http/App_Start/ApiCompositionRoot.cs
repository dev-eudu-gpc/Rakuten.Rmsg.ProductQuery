//----------------------------------------------------------------------------------------------------------------------
// <copyright file="ApiCompositionRoot.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.Web.Http
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Net.Http;
    using System.Web.Http.Controllers;
    using System.Web.Http.Dispatcher;
    using System.Web.Routing;
    using System.Web.SessionState;

    using Rakuten.Rmsg.ProductQuery.Configuration;
    using Rakuten.Rmsg.ProductQuery.Web.Http.Commands;
    using Rakuten.Rmsg.ProductQuery.Web.Http.EntityModels;
    using Rakuten.WindowsAzure.Storage;

    /// <summary>
    /// Constructs the application's object graph.
    /// </summary>
    public class ApiCompositionRoot : IHttpControllerActivator
    {
        /// <summary>
        /// The context of the current instance.
        /// </summary>
        private readonly IApiContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiCompositionRoot" /> class.
        /// </summary>
        /// <param name="context">The context of the current instance.</param>
        internal ApiCompositionRoot(IApiContext context)
        {
            Contract.Requires(context != null);

            this.context = context;
        }

        /// <summary>
        /// Create a new controller based on the parameters provided, allows DI.
        /// </summary>
        /// <param name="request">The HTTP request.</param>
        /// <param name="controllerDescriptor">The controller descriptor.</param>
        /// <param name="controllerType">The type of the controller.</param>
        /// <returns>A new controller based on the parameters provided.</returns>
        public IHttpController Create(
            HttpRequestMessage request,
            HttpControllerDescriptor controllerDescriptor,
            Type controllerType)
        {
            IHttpController controller = null;
            ProductQueryDbContext databaseContext = null;

            // Initialize common collection of Uri templates
            var uriTemplates = new
            {
                AzureBlob = new Rakuten.UriTemplate("{id}"),
                ProductQuery = new Rakuten.UriTemplate("/product-query/{id}"),
                ProductQueryMonitorLink = new Rakuten.UriTemplate("/product-query-group/{id}/status/{year}/{month}/{day}/{time}")
            };

            // Create appropriate controller based on the controller name
            switch (controllerDescriptor.ControllerName)
            {
                case "ProductQuery":
                    databaseContext = new ProductQueryDbContext();
                    var storage = new AzureStorage();

                    // Database commands
                    var createDatabaseCommand = new CreateProductQueryDatabaseCommand(this.context, databaseContext);
                    var getDatabaseCommand = new GetProductQueryDatabaseCommand(this.context, databaseContext);
                    var updateProductQueryStatusDatabaseCommand = new UpdateProductQueryStatusDatabaseCommand(this.context, databaseContext);
                    var updateProductQueryUriDatabaseCommand = new UpdateProductQueryUriCommand(this.context, databaseContext);                   

                    // Storage and messaging commands
                    var createStorageBlobCommand = new CreateStorageBlobCommand(this.context, storage);
                    var dispatchMessageCommand = new DispatchMessageCommand(this.context);

                    // Macro commands
                    var createCommand = new CreateProductQueryCommand(
                        this.context,
                        uriTemplates.ProductQuery,
                        uriTemplates.AzureBlob,
                        createDatabaseCommand,
                        createStorageBlobCommand,
                        updateProductQueryUriDatabaseCommand);

                    var getCommand = new GetProductQueryCommand(
                        this.context,
                        uriTemplates.ProductQuery,
                        uriTemplates.AzureBlob,
                        uriTemplates.ProductQueryMonitorLink,
                        getDatabaseCommand);

                    var readyForProcessingCommand = new ReadyForProcessingCommand(
                        this.context,
                        dispatchMessageCommand,
                        getCommand,
                        updateProductQueryStatusDatabaseCommand);

                    controller = new ProductQueryController(
                        getCommand,
                        createCommand,
                        readyForProcessingCommand);

                    break;
                case "ProductQueryGroup":
                    databaseContext = new ProductQueryDbContext();

                    // Commands
                    var getProductQueryGroupProgressDatabaseCommand = new GetProductQueryGroupProgressDatabaseCommand(databaseContext);
                    var createProgressMapImageCommand = new CreateProductQueryGroupProgressImageCommand(this.context);

                    // Macro commands
                    var getProductQueryGroupProgressCommand = new GetProductQueryGroupProgressCommand(
                        this.context,
                        uriTemplates.ProductQueryMonitorLink,
                        createProgressMapImageCommand,
                        getProductQueryGroupProgressDatabaseCommand);

                    controller = new ProductQueryGroupController(
                        getProductQueryGroupProgressCommand,
                        uriTemplates.ProductQueryMonitorLink);

                    break;
            }

            return controller;
        }

        /// <summary>
        /// Gets a named controller's session behavior
        /// </summary>
        /// <param name="requestContext">The context of the request</param>
        /// <param name="controllerName">The name of the controller</param>
        /// <returns>The session behavior of the named controller</returns>
        public SessionStateBehavior GetControllerSessionBehavior(RequestContext requestContext, string controllerName)
        {
            return SessionStateBehavior.Default;
        }

        /// <summary>
        /// Disposes of a controller if possible
        /// </summary>
        /// <param name="controller">The controller to dispose</param>
        public void ReleaseController(IHttpController controller)
        {
            var disposable = controller as IDisposable;
            if (disposable != null)
            {
                disposable.Dispose();
            }
        }
    }
}
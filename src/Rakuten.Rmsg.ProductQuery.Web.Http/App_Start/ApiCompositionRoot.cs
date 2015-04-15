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
    using Rakuten.Rmsg.ProductQuery.Web.Http.Commands;
    using Rakuten.Rmsg.ProductQuery.Web.Http.Configuration;
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

            // Initialize common collection of Uri templates
            var uriTemplates = new
            {
                AzureBlob = new UriTemplate("{id}"),
                ProductQuery = new UriTemplate("/product-query/{id}"),
                ProductQueryMonitorLink = new UriTemplate("/product-query-group/{id}/status/{year}/{month}/{day}/{time}")
            };

            // Create appropriate controller based on the controller name
            switch (controllerDescriptor.ControllerName)
            {
                case "ProductQuery":
                    var databaseContext = new ProductQueryDbContext();
                    var storage = new AzureStorage();

                    var createDatabaseCommand = new CreateProductQueryDatabaseCommand(this.context, databaseContext);
                    var updateUriDatabaseCommand = new UpdateProductQueryUriCommand(this.context, databaseContext);                   
                    var getDatabaseCommand = new GetProductQueryDatabaseCommand(this.context, databaseContext);
                    var createStorageBlobCommand = new CreateStorageBlobCommand(storage, this.context);

                    var createCommand = new CreateProductQueryCommand(
                        storage,
                        this.context,
                        uriTemplates.ProductQuery,
                        uriTemplates.AzureBlob,
                        createDatabaseCommand,
                        createStorageBlobCommand,
                        updateUriDatabaseCommand);
                    var getCommand = new GetProductQueryCommand(
                        storage,
                        this.context,
                        uriTemplates.ProductQuery,
                        uriTemplates.AzureBlob,
                        uriTemplates.ProductQueryMonitorLink,
                        getDatabaseCommand);

                    controller = new ProductQueryController(getCommand, createCommand);

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
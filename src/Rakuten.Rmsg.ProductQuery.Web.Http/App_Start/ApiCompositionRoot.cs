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

    using Rakuten.Rmsg.ProductQuery.Web.Http.EntityModels;

    /// <summary>
    /// Constructs the application's object graph.
    /// </summary>
    public class ApiCompositionRoot : IHttpControllerActivator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApiCompositionRoot" /> class.
        /// </summary>
        /// A delegate that will create a client for connecting to the current GPC instance.
        /// </param>
        internal ApiCompositionRoot()
        {
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

            ProductQueryDbContext databaseContext;

            // Create appropriate controller based on the controller name
            switch (controllerDescriptor.ControllerName)
            {
                case "ProductQuery":
                    databaseContext = new ProductQueryDbContext();

                    controller = new ProductQueryController();

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
// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="RouteConfig.cs" company="Rakuten">
//   Copyright (c) Rakuten. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.Web.Http
{
    using System.Web.Http;

    /// <summary>
    /// The route config.
    /// </summary>
    public class RouteConfig
    {
        /// <summary>
        /// Registers routes into a collection.
        /// </summary>
        /// <param name="configuration">The instance upon which to register the application's routes.</param>
        public static void RegisterRoutes(HttpConfiguration configuration)
        {
            // Web API routes 
            configuration.MapHttpAttributeRoutes();
        }
    }
}
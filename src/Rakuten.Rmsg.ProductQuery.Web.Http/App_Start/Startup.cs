// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="Startup.cs" company="Rakuten">
//   Copyright (c) Rakuten. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.Web.Http
{
    using System.Diagnostics.Contracts;
    using System.Net.Http.Formatting;
    using System.Web.Http;
    using System.Web.Http.Dispatcher;
    using System.Web.Http.ExceptionHandling;

    using Owin;

    using Rakuten.Gpc.Api.Web.Http.ExceptionHandling;
    using Rakuten.Net.Http.Formatting;
    using Rakuten.Reflection.Emit;
    using Rakuten.Rmsg.ProductQuery.Configuration;
    using Rakuten.Web.Http;
    using Rakuten.Web.Http.ExceptionHandling;
    using Rakuten.Web.Http.Results;

    /// <summary>
    /// Configures the current application.
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Configure the current application.
        /// </summary>
        /// <param name="app">The current application.</param>
        public void Configuration(IAppBuilder app)
        {
            Contract.Requires(app != null);

            // Get a new configuration instance.
            var configuration = new HttpConfiguration();

            // Register API components: filters, routes, bundles, binders.
            RouteConfig.RegisterRoutes(configuration);

            // Formatters
            ConfigureFormatters(configuration);

            // Exception handling
            configuration.Services.Replace(typeof(IExceptionHandler), GetExceptionHandler());

            // Get the context for the current instance.
            var context = GetContext();

            // Replace the default controller activator with the ApiCompositionRoot activator.
            configuration.Services.Replace(
                typeof(IHttpControllerActivator),
                new ApiCompositionRoot(context));

            // Configure the OWIN pipeline with a Web API endpoint using the specified configuration.
            app.UseWebApi(configuration);
        }

        /// <summary>
        /// Configures the <see cref="MediaTypeFormatter"/>s on the specified <see cref="HttpConfiguration"/> instance.
        /// </summary>
        /// <param name="configuration">The instance upon which the MediaTypeFormatters should be configured.</param>
        private static void ConfigureFormatters(HttpConfiguration configuration)
        {
            Contract.Requires(configuration != null);

            var formatters = configuration.Formatters;
            formatters.Clear();
            formatters.Add(new ResourceJsonMediaTypeFormatter());
            formatters.Add(new ResourceXmlMediaTypeFormatter());
        }

        /// <summary>
        /// Gets a context for the current instance
        /// </summary>
        /// <returns>A context for the current instance.</returns>
        private static IApiContext GetContext()
        {
            return new ApiContextFactory(new AppSettingsConfigurationSource()).Create();
        }

        /// <summary>
        /// Creates a new <see cref="IExceptionHandler"/> instance for returning custom errors to the client.
        /// </summary>
        /// <returns>A new <see cref="IExceptionHandler"/> instance for returning custom errors to the client.</returns>
        private static IExceptionHandler GetExceptionHandler()
        {
            var typeFactory = new ProblemTypeFactory(
                new ProblemBuilder(
                    new AssemblyBuilderFactory(),
                    new ModuleBuilderFactory(),
                    new TypeBuilderFactory(),
                    new ConstructorBuilderFactory(),
                    new FieldBuilderFactory(),
                    new PropertyBuilderFactory(),
                    new MethodBuilderFactory(),
                    new XmlTypeAttributeFactory(),
                    new XmlElementAttributeFactory(),
                    new JsonPropertyAttributeFactory()));

            var activator = new ProblemActivator(new AttributeApiExceptionMetadataProvider());
            var resultFactory = new NegotiatedContentResultFactory();

            return
                new Rakuten.Web.Http.ExceptionHandling.ExceptionHandler(
                        new WrappedApiExceptionProcessor(
                            typeFactory,
                            activator,
                            resultFactory,
                            new HttpStatusCodeMapper()),
                        new ApiExceptionProcessor(typeFactory, activator, resultFactory),
                        new DefaultExceptionProcessor(typeFactory, activator, resultFactory));
        }
    }
}
// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="DefaultExceptionProcessor.cs" company="Rakuten">
//   Copyright (c) Rakuten. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------
namespace Rakuten.Gpc.Api.Web.Http.ExceptionHandling
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Net.Http;
    using System.Net.Http.Formatting;
    using System.Web.Http;
    using System.Web.Http.Results;

    using Rakuten.Reflection.Emit;
    using Rakuten.Web.Http.ExceptionHandling;
    using Rakuten.Web.Http.Results;

    /// <summary>
    /// Processes exception instances.
    /// </summary>
    internal class DefaultExceptionProcessor : ExceptionProcessor
    {
        /// <summary>
        /// Instantiates and populates types.
        /// </summary>
        private readonly ProblemActivator activator;

        /// <summary>
        /// An instance that will create serializable problem types from exceptions.
        /// </summary>
        private readonly ProblemTypeFactory problemTypeFactory;

        /// <summary>
        /// Creates <see cref="NegotiatedContentResult{T}"/> instances.
        /// </summary>
        private readonly NegotiatedContentResultFactory resultFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultExceptionProcessor"/> class.
        /// </summary>
        /// <param name="problemTypeFactory">
        /// An instance with will convert exceptions to serializable types.
        /// </param>
        /// <param name="activator">
        /// The activator that will instantiate and populate problem types.
        /// </param>
        /// <param name="resultFactory">
        /// The instance through which <see cref="NegotiatedContentResult{T}"/> instances can be created.
        /// </param>
        public DefaultExceptionProcessor(
            ProblemTypeFactory problemTypeFactory, 
            ProblemActivator activator, 
            NegotiatedContentResultFactory resultFactory)
        {
            Contract.Requires(problemTypeFactory != null);
            Contract.Requires(activator != null);
            Contract.Requires(resultFactory != null);

            this.problemTypeFactory = problemTypeFactory;
            this.activator = activator;
            this.resultFactory = resultFactory;
        }

        /// <summary>
        /// Determines whether the specified exception can be processed by this instance.
        /// </summary>
        /// <param name="exception">
        /// The exception that requires processing.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if the specified exception can be processed by this instance, otherwise; 
        /// <see langword="false"/>.
        /// </returns>
        public override bool CanProcess(Exception exception)
        {
            Contract.Requires(exception != null);

            return true;
        }

        /// <summary>
        /// Processes the specified exception into a <see cref="IHttpActionResult"/>.
        /// </summary>
        /// <param name="exception">The exception that requires processing.</param>
        /// <param name="negotiator">The <see cref="IContentNegotiator"/> instance.</param>
        /// <param name="request">The original <see cref="HttpRequestMessage"/>.</param>
        /// <param name="formatters">The collection of <see cref="MediaTypeFormatter"/>s.</param>
        /// <returns>A <see cref="IHttpActionResult"/> instance that represents the specified exception.</returns>
        public override IHttpActionResult Process(
            Exception exception,
            IContentNegotiator negotiator,
            HttpRequestMessage request,
            IEnumerable<MediaTypeFormatter> formatters)
        {
            Contract.Requires(exception != null);
            Contract.Requires(negotiator != null);
            Contract.Requires(request != null);
            Contract.Requires(formatters != null);

            var apiException = new ApiException(exception.Message);

            Type type;

            if (!ExceptionProcessor.TryCreateProblemType(apiException, this.problemTypeFactory, out type))
            {
                return null;
            }

            object problemInstance = ExceptionProcessor.BuildProblem(
                type,
                apiException,
                this.problemTypeFactory,
                this.activator);

            // Create a new IHttpActionResult for the response.
            return this.resultFactory.Create(
                type,
                problemInstance,
                negotiator,
                request,
                formatters);
        }
    }
}
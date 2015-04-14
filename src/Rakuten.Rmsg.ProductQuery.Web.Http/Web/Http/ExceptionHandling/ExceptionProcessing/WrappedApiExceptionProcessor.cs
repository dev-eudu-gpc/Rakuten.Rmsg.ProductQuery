// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="WrappedApiExceptionProcessor.cs" company="Rakuten">
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
    using Rakuten.Web.Http;
    using Rakuten.Web.Http.ExceptionHandling;
    using Rakuten.Web.Http.Results;

    /// <summary>
    /// Processes <see cref="ApiException"/>s that have been wrapped by an outer exception.
    /// </summary>
    public class WrappedApiExceptionProcessor : ExceptionProcessor
    {
        /// <summary>
        /// Instantiates and populates types.
        /// </summary>
        private readonly ProblemActivator activator;

        /// <summary>
        /// Returns a HTTP status code from an exception.
        /// </summary>
        private readonly HttpStatusCodeMapper mapper;

        /// <summary>
        /// An instance that will create serializable problem types from exceptions.
        /// </summary>
        private readonly ProblemTypeFactory problemTypeFactory;

        /// <summary>
        /// Creates <see cref="NegotiatedContentResult{T}"/> instances.
        /// </summary>
        private readonly NegotiatedContentResultFactory resultFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="WrappedApiExceptionProcessor"/> class.
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
        /// <param name="mapper">
        /// Maps an exception instance to a HTTP status code.
        /// </param>
        public WrappedApiExceptionProcessor(
            ProblemTypeFactory problemTypeFactory, 
            ProblemActivator activator, 
            NegotiatedContentResultFactory resultFactory, 
            HttpStatusCodeMapper mapper)
        {
            Contract.Requires(problemTypeFactory != null);
            Contract.Requires(activator != null);
            Contract.Requires(resultFactory != null);
            Contract.Requires(mapper != null);

            this.problemTypeFactory = problemTypeFactory;
            this.activator = activator;
            this.resultFactory = resultFactory;
            this.mapper = mapper;
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

            return exception.InnerException is IApiException;
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

            Type type;

            if (!ExceptionProcessor.TryCreateProblemType(exception.InnerException, this.problemTypeFactory, out type))
            {
                return null;
            }

            object problemInstance = ExceptionProcessor.BuildProblem(
                type, 
                exception.InnerException, 
                this.problemTypeFactory, 
                this.activator);

            // Create a new IHttpActionResult for the response.
            return this.resultFactory.Create(
                type, 
                problemInstance, 
                negotiator, 
                request, 
                formatters, 
                this.mapper.Map(exception));
        }
    }
}
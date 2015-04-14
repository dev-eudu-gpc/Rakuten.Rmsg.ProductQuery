// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExceptionHandler.cs" company="Rakuten">
//   Copyright (c) Rakuten. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Rakuten.Web.Http.ExceptionHandling
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Net.Http.Formatting;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Web.Http;
    using System.Web.Http.ExceptionHandling;

    using Rakuten.Gpc.Api.Web.Http.ExceptionHandling;
    using Rakuten.Threading.Tasks;

    /// <summary>
    /// Provides an implementation for handling exceptions within the Web API.
    /// </summary>
    internal class ExceptionHandler : IExceptionHandler
    {
        /// <summary>
        /// A collection of <see cref="IExceptionProcessor"/> that can handle the exceptions.
        /// </summary>
        private readonly IExceptionProcessor[] processors; 

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionHandler"/> class.
        /// </summary>
        /// <param name="processors">
        /// The collection of <see cref="IExceptionProcessor"/>s that this handler should use to process the handled 
        /// exceptions.
        /// </param>
        public ExceptionHandler(params IExceptionProcessor[] processors)
        {
            if (processors == null)
            {
                throw new ArgumentNullException("processors");
            }

            this.processors = processors;
        }

        /// <summary>
        /// Handles the thrown exception by mapping it to an API exception if possible;
        /// otherwise the standard error response is returned.
        /// </summary>
        /// <param name="context">
        /// The context of the exception that includes the exception thrown.
        /// </param>
        /// <param name="cancellationToken">
        /// The cancellation token.
        /// </param>
        /// <returns>
        /// A <see cref="Task"/>.
        /// </returns>
        public Task HandleAsync(ExceptionHandlerContext context, CancellationToken cancellationToken)
        {
            Handle(
                context,
                this.processors);

            return TaskHelpers.Completed();
        }

        /// <summary>
        /// Handles the thrown exception by mapping it to an API exception if possible; 
        /// otherwise the standard error response is returned.
        /// </summary>
        /// <param name="context">
        /// The context of the exception that includes the exception thrown.
        /// </param>
        /// <param name="processors">The collection of <see cref="IExceptionProcessor"/>s.</param>
        private static void Handle(
            ExceptionHandlerContext context,
            IEnumerable<IExceptionProcessor> processors)
        {
            Contract.Requires(context != null);
            Contract.Requires(context.ExceptionContext != null);

            Contract.Requires(processors != null);

            var exceptionContext = context.ExceptionContext;

            if (exceptionContext.CatchBlock == ExceptionCatchBlocks.IExceptionFilter)
            {
                // The exception filter stage propagates unhandled exceptions by default
                // (when no filter handles the exception).
                return;
            }

            var request = exceptionContext.Request;

            if (request == null)
            {
                throw new ArgumentException("context.Request must not be null.");
            }

            IContentNegotiator negotiator = context.RequestContext.Configuration.Services.GetContentNegotiator();
            Contract.Assume(negotiator != null);

            IEnumerable<MediaTypeFormatter> formatters = context.RequestContext.Configuration.Formatters;
            Contract.Assume(formatters != null);

            // This will be the outer exception from which we determine the HTTP status code to set on the response.
            Exception exception = exceptionContext.Exception;

            foreach (IExceptionProcessor processor in processors)
            {
                if (processor.CanProcess(exception))
                {
                    context.Result = processor.Process(exception, negotiator, request, formatters);

                    return;
                }
            }
        }
    }
}
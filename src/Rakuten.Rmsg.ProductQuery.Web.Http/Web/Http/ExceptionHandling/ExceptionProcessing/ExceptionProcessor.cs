// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="ExceptionProcessor.cs" company="Rakuten">
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

    using Rakuten.Reflection.Emit;
    using Rakuten.Web.Http.ExceptionHandling;

    /// <summary>
    /// A base implementation providing common functionality when processing exceptions.
    /// </summary>
    public abstract class ExceptionProcessor : IExceptionProcessor
    {
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
        public abstract bool CanProcess(System.Exception exception);

        /// <summary>
        /// Processes the specified exception into a <see cref="IHttpActionResult"/>.
        /// </summary>
        /// <param name="exception">The exception that requires processing.</param>
        /// <param name="negotiator">The <see cref="IContentNegotiator"/> instance.</param>
        /// <param name="request">The original <see cref="HttpRequestMessage"/>.</param>
        /// <param name="formatters">The collection of <see cref="MediaTypeFormatter"/>s.</param>
        /// <returns>A <see cref="IHttpActionResult"/> instance that represents the specified exception.</returns>
        public abstract IHttpActionResult Process(
            Exception exception,
            IContentNegotiator negotiator,
            HttpRequestMessage request,
            IEnumerable<MediaTypeFormatter> formatters);

        /// <summary>
        /// Constructs a problem from the given type populating it with the data from the specified exception.
        /// </summary>
        /// <param name="type">
        /// The type from which an instance should be created.
        /// </param>
        /// <param name="exception">
        /// The instance from which data should be copied.
        /// </param>
        /// <param name="factory">
        /// The factory through which problem types should be created.
        /// </param>
        /// <param name="activator">
        /// The activator that will instantiate and populate problem types.
        /// </param>
        /// <returns>
        /// An instance which can be serialized into a HTTP problem instance.
        /// </returns>
        protected static object BuildProblem(
            Type type,
            Exception exception,
            ProblemTypeFactory factory,
            ProblemActivator activator)
        {
            Contract.Requires(type != null);
            Contract.Requires(exception != null);
            Contract.Requires(factory != null);
            Contract.Requires(activator != null);

            object[] innerInstances = null;

            var aggregateException = exception as AggregateException;

            if (aggregateException != null)
            {
                var innerExceptions = new List<object>();

                foreach (Exception innerException in aggregateException.InnerExceptions)
                {
                    Contract.Assume(innerException != null);

                    Type innerType;
                    if (TryCreateProblemType(innerException, factory, out innerType))
                    {
                        // TODO: [MM, 25/02] Should this be recursive?
                        object innerInstance = activator.CreateInstance(innerType, innerException);

                        innerExceptions.Add(innerInstance);
                    }
                }

                innerInstances = innerExceptions.ToArray();
            }

            // Create an instance of the dynamic type and populate the properties from the exception.
            return activator.CreateInstance(type, exception, innerInstances);
        }

        /// <summary>
        /// Attempts to create a type from the specified exception.
        /// </summary>
        /// <param name="exception">
        /// The exception from which a type should be created.
        /// </param>
        /// <param name="factory">
        /// The <see cref="ProblemTypeFactory"/> instance to use to create the type.
        /// </param>
        /// <param name="type">
        /// When this method returns, contains the generated type for the exception passed in 
        /// <paramref name="exception"/>. The creation fails if <paramref name="exception"/> is null or not an instance 
        /// of <see cref="IApiException"/>. This parameter is passed uninitialized.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if a type of <paramref name="exception"/> was created; otherwise, 
        /// <see langword="false"/>.
        /// </returns>
        protected static bool TryCreateProblemType(Exception exception, ProblemTypeFactory factory, out Type type)
        {
            Contract.Requires(factory != null);

            type = null;

            if (exception == null)
            {
                return false;
            }

            var apiException = exception as IApiException;

            if (apiException != null)
            {
                type = factory.Create(apiException);
                return true;
            }

            return false;
        }
    }
}
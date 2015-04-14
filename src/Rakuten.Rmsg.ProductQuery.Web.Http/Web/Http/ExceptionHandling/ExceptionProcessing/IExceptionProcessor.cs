// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="IExceptionProcessor.cs" company="Rakuten">
//   Copyright (c) Rakuten. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------
namespace Rakuten.Gpc.Api.Web.Http.ExceptionHandling
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Net.Http.Formatting;
    using System.Web.Http;

    /// <summary>
    /// Defines an instance that can process an exception.
    /// </summary>
    internal interface IExceptionProcessor
    {
        /// <summary>
        /// Determines whether the specified exception can be processed by this instance.
        /// </summary>
        /// <param name="exception">The exception that requires processing.</param>
        /// <returns>
        /// <see langword="true"/> if the specified exception can be processed by this instance, otherwise; 
        /// <see langword="false"/>.
        /// </returns>
        bool CanProcess(Exception exception);

        /// <summary>
        /// Processes the specified exception into a <see cref="IHttpActionResult"/>.
        /// </summary>
        /// <param name="exception">The exception that requires processing.</param>
        /// <param name="negotiator">The <see cref="IContentNegotiator"/> instance.</param>
        /// <param name="request">The original <see cref="HttpRequestMessage"/>.</param>
        /// <param name="formatters">The collection of <see cref="MediaTypeFormatter"/>s.</param>
        /// <returns>A <see cref="IHttpActionResult"/> instance that represents the specified exception.</returns>
        IHttpActionResult Process(
            Exception exception,
            IContentNegotiator negotiator,
            HttpRequestMessage request,
            IEnumerable<MediaTypeFormatter> formatters);
    }
}
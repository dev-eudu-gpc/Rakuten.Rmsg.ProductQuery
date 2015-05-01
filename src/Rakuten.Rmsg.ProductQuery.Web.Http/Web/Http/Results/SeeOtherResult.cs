﻿//------------------------------------------------------------------------------
// <copyright file="SeeOtherResult.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
namespace System.Web.Http.Results
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Represents an HTTP result for the status code 303 See Other.
    /// </summary>
    public class SeeOtherResult : IHttpActionResult
    {
        /// <summary>
        /// The URI for the location header.
        /// </summary>
        private readonly Uri location;

        /// <summary>
        /// The request message from which to obtain the dependencies needed for execution.
        /// </summary>
        private readonly HttpRequestMessage message;

        /// <summary>
        /// The minimum time the user-agent is asked wait before issuing the redirected request.
        /// </summary>
        private readonly int retryAfter;

        /// <summary>
        /// Initializes a new instance of the <see cref="SeeOtherResult"/> class.
        /// </summary>
        /// <param name="location">The URI for the Location header.</param>
        /// <param name="message">The request message from which to obtain the dependencies needed for execution.</param>
        public SeeOtherResult(Uri location, HttpRequestMessage message)
            : this(location, 0, message)
        {
            Contract.Requires(location != null);
            Contract.Requires(message != null);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SeeOtherResult"/> class.
        /// </summary>
        /// <param name="location">The URI for the Location header.</param>
        /// <param name="retryAfter">The minimum time, in seconds, that the user-agent is asked wait before issuing the redirected request.</param>
        /// <param name="message">The request message from which to obtain the dependencies needed for execution.</param>
        public SeeOtherResult(Uri location, int retryAfter, HttpRequestMessage message)
        {
            Contract.Requires(message != null);
            Contract.Requires(location != null);

            this.message = message;
            this.location = location;
            this.retryAfter = retryAfter;
        }

        /// <summary>
        /// Creates http response message.
        /// </summary>
        /// <returns>An http response message with a status code of 303 See Other.</returns>
        public HttpResponseMessage Execute()
        {
            var response = this.message.CreateResponse(HttpStatusCode.SeeOther);

            response.Headers.Location = this.location;
            response.Headers.RetryAfter = new RetryConditionHeaderValue(new TimeSpan(0, 0, this.retryAfter));

            return response;
        }

        /// <summary>
        /// Creates http response message.
        /// </summary>
        /// <param name="cancellationToken">A token for cancellation</param>
        /// <returns>An http response message with a status code of 303 See Other.</returns>
        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            return Task.FromResult(this.Execute());
        }
    }
}
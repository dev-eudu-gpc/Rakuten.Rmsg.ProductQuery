// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="ApiClient.cs" company="Rakuten">
//   Copyright (c) Rakuten. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------
namespace Rakuten.Net.Http
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Net.Http;
    using System.Threading.Tasks;

    using Rakuten.Gpc.Api.Client;

    /// <summary>
    /// Handles requests to HTTP endpoints and processes responses.
    /// </summary>
    public class ApiClient : IDisposable
    {
        /// <summary>
        /// The current context in which HTTP requests will be made.
        /// </summary>
        private readonly IApiClientContext context;

        /// <summary>
        /// Handles validating and reading of responses from HTTP requests.
        /// </summary>
        private readonly IHttpResponseHandler handler;

        /// <summary>
        /// Constructs URLs from a specified set of parameters.
        /// </summary>
        private readonly IUriBuilderMediator mediator;

        /// <summary>
        /// Handles manufacturing and sending of requests to HTTP resources.
        /// </summary>
        private readonly IHttpRequestHandler requestor;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiClient"/> class.
        /// </summary>
        /// <param name="context">The context in which the client should operate.</param>
        /// <param name="requestor">The <see cref="IHttpRequestHandler"/> to handle requests to HTTP endpoints.</param>
        /// <param name="handler">
        /// The <see cref="IHttpResponseHandler"/> to validate and read responses from HTTP requests.
        /// </param>
        public ApiClient(
            IApiClientContext context,
            IHttpRequestHandler requestor,
            IHttpResponseHandler handler)
        {
            Contract.Requires(context != null);
            Contract.Requires(requestor != null);
            Contract.Requires(handler != null);

            this.context = context;
            this.requestor = requestor;
            this.handler = handler;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiClient"/> class.
        /// </summary>
        /// <param name="context">
        /// The context in which the client should operate.
        /// </param>
        /// <param name="mediator">
        /// The <see cref="IUriBuilderMediator"/> implementation to build URLs.
        /// </param>
        /// <param name="requestor">
        /// The <see cref="IHttpRequestHandler"/> to handle requests to HTTP endpoints.
        /// </param>
        /// <param name="handler">
        /// The <see cref="IHttpResponseHandler"/> to validate and read responses from HTTP requests.
        /// </param>
        public ApiClient(
            IApiClientContext context,
            IUriBuilderMediator mediator,
            IHttpRequestHandler requestor,
            IHttpResponseHandler handler)
            : this(context, requestor, handler)
        {
            Contract.Requires(context != null);
            Contract.Requires(mediator != null);
            Contract.Requires(requestor != null);
            Contract.Requires(handler != null);

            this.mediator = mediator;
        }

        /// <summary>
        /// Releases all resources used by the current instance of the <see cref="ApiClient"/> class.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Creates and issues a GET request to the specified URI.
        /// </summary>
        /// <typeparam name="T">The type of the response body.</typeparam>
        /// <param name="requestUri">The URI the request is sent to.</param>
        /// <returns>A <see cref="Task"/> that will issue the request, validate then read the response.</returns>
        public Task<T> GetAsync<T>(Uri requestUri)
        {
            Contract.Requires(requestUri != null);

            return this.GetAsyncWithoutValidation<T>(requestUri);
        }

        /// <summary>
        /// Creates and issues a GET request using the parameters provided.
        /// </summary>
        /// <typeparam name="T">The type of the response body.</typeparam>
        /// <param name="parameters">The parameters required to build the URI.</param>
        /// <returns>A <see cref="Task"/> that will issue the request, validate then read the response.</returns>
        public Task<T> GetAsync<T>(IUriParameters parameters)
        {
            Contract.Requires(parameters != null);

            if (this.mediator == null)
            {
                throw new InvalidOperationException();
            }

            Uri requestUri = this.mediator.Build(this.context, parameters);
            Contract.Assume(requestUri != null);

            return this.GetAsync<T>(requestUri);
        }

        /// <summary>
        /// Creates an absolute URI from the specified relative URI.
        /// </summary>
        /// <param name="relativeLink">The URI the request is sent to.</param>
        /// <returns>An absolute URI the request should be sent to.</returns>
        public Link MakeAbsoluteLink(Link relativeLink)
        {
            return relativeLink == null || string.IsNullOrWhiteSpace(relativeLink.Target) ?
                null :
                new Link
                {
                    LanguageTag = relativeLink.LanguageTag,
                    MediaType = relativeLink.MediaType,
                    RelationType = relativeLink.RelationType,
                    Target = this.MakeAbsoluteUri(
                        new Uri(relativeLink.Target, UriKind.RelativeOrAbsolute)).ToString(),
                    Title = relativeLink.Title
                };
        }

        /// <summary>
        /// Creates an absolute URI from the specified relative URI.
        /// </summary>
        /// <param name="relativeLinks">The URI the request is sent to.</param>
        /// <returns>An absolute URI the request should be sent to.</returns>
        public IEnumerable<Link> MakeAbsoluteLinks(IEnumerable<Link> relativeLinks)
        {
            return relativeLinks == null ? null : relativeLinks.Select(this.MakeAbsoluteLink);
        }

        /// <summary>
        /// Creates an absolute URI from the specified relative URI.
        /// </summary>
        /// <param name="relativeUri">The URI the request is sent to.</param>
        /// <returns>An absolute URI the request should be sent to.</returns>
        public Uri MakeAbsoluteUri(Uri relativeUri)
        {
            return this.context.MakeAbsoluteUri(relativeUri);
        }

        /// <summary>
        /// Creates and issues a POST request to the specified URI.
        /// </summary>
        /// <typeparam name="TOut">The type of the response body.</typeparam>
        /// <param name="requestUri">The URI the request is sent to.</param>
        /// <returns>A <see cref="Task"/> that will issue the request, validate then read the response.</returns>
        public Task<TOut> PostAsync<TOut>(Uri requestUri)
        {
            Contract.Requires(requestUri != null);

            return this.PostAsyncWithoutValidation<TOut>(requestUri);
        }

        /// <summary>
        /// Creates and issues a POST request using the parameters provided.
        /// </summary>
        /// <typeparam name="TOut">The type of the response body.</typeparam>
        /// <param name="parameters">The parameters required to build the URI.</param>
        /// <returns>A <see cref="Task"/> that will issue the request, validate then read the response.</returns>
        public Task<TOut> PostAsync<TOut>(IUriParameters parameters)
        {
            Contract.Requires(parameters != null);

            if (this.mediator == null)
            {
                throw new InvalidOperationException();
            }

            Uri requestUri = this.mediator.Build(this.context, parameters);
            Contract.Assume(requestUri != null);

            return this.PostAsyncWithoutValidation<TOut>(requestUri);
        }

        /// <summary>
        /// Creates and issues a POST request to the specified URI.
        /// </summary>
        /// <typeparam name="TIn">The type of the request body.</typeparam>
        /// <typeparam name="TOut">The type of the response body.</typeparam>
        /// <param name="requestUri">The URI the request is sent to.</param>
        /// <param name="content">The object to be sent in the request body.</param>
        /// <returns>A <see cref="Task"/> that will issue the request, validate then read the response.</returns>
        public Task<TOut> PostAsync<TIn, TOut>(Uri requestUri, TIn content)
        {
            Contract.Requires(requestUri != null);

            return this.PostAsyncWithoutValidation<TIn, TOut>(requestUri, content);
        }

        /// <summary>
        /// Creates and issues a POST request to the specified URI.
        /// </summary>
        /// <typeparam name="TIn">The type of the request body.</typeparam>
        /// <typeparam name="TOut">The type of the response body.</typeparam>
        /// <param name="parameters">The parameters required to build the URI.</param>
        /// <param name="content">The object to be sent in the request body.</param>
        /// <returns>A <see cref="Task"/> that will issue the request, validate then read the response.</returns>
        public Task<TOut> PostAsync<TIn, TOut>(IUriParameters parameters, TIn content)
        {
            Contract.Requires(parameters != null);

            if (this.mediator == null)
            {
                throw new InvalidOperationException();
            }

            Uri requestUri = this.mediator.Build(this.context, parameters);
            Contract.Assume(requestUri != null);

            return this.PostAsyncWithoutValidation<TIn, TOut>(requestUri, content);
        }

        /// <summary>
        /// Creates and issues a POST request to the specified URI.
        /// </summary>
        /// <typeparam name="TOut">The type of the response body.</typeparam>
        /// <param name="parameters">The parameters required to build the URI.</param>
        /// <param name="content">The object to be sent in the request body.</param>
        /// <returns>A <see cref="Task"/> that will issue the request, validate then read the response.</returns>
        public Task<TOut> PostAsync<TOut>(IUriParameters parameters, MultipartFormDataContent content)
        {
            Contract.Requires(parameters != null);
            Contract.Requires(content != null);

            return this.PostAsync<MultipartFormDataContent, TOut>(parameters, content);
        }

        /// <summary>
        /// Creates and issues a PUT request to the specified URI.
        /// </summary>
        /// <typeparam name="TIn">The type of the request body.</typeparam>
        /// <typeparam name="TOut">The type of the response body.</typeparam>
        /// <param name="requestUri">The URI the request is sent to.</param>
        /// <param name="content">The object to be sent in the request body.</param>
        /// <returns>A <see cref="Task"/> that will issue the request, validate then read the response.</returns>
        public Task<TOut> PutAsync<TIn, TOut>(Uri requestUri, TIn content)
        {
            Contract.Requires(requestUri != null);

            return this.PutAsyncWithoutValidation<TIn, TOut>(requestUri, content);
        }

        /// <summary>
        /// Creates and issues a PUT request to the specified URI.
        /// </summary>
        /// <typeparam name="TIn">The type of the request body.</typeparam>
        /// <typeparam name="TOut">The type of the response body.</typeparam>
        /// <param name="parameters">The parameters required to build the URI.</param>
        /// <param name="content">The object to be sent in the request body.</param>
        /// <returns>A <see cref="Task"/> that will issue the request, validate then read the response.</returns>
        public Task<TOut> PutAsync<TIn, TOut>(IUriParameters parameters, TIn content)
        {
            Contract.Requires(parameters != null);

            if (this.mediator == null)
            {
                throw new InvalidOperationException();
            }

            Uri requestUri = this.mediator.Build(this.context, parameters);
            Contract.Assume(requestUri != null);

            return this.PutAsyncWithoutValidation<TIn, TOut>(requestUri, content);
        }

        /// <summary>
        /// Releases the unmanaged resources used by the <see cref="HttpRequestHandler"/>
        /// and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing">
        /// <see langword="true"/> to release both managed and unmanaged resources;
        /// <see langword="false"/> to release only unmanaged resources.
        /// </param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                var disposable = this.requestor as IDisposable;
                if (disposable != null)
                {
                    disposable.Dispose();
                }

                // ReSharper disable once SuspiciousTypeConversion.Global
                // We may inject handlers from outside the solution and need to ensure they are released correctly.
                disposable = this.handler as IDisposable;
                if (disposable != null)
                {
                    disposable.Dispose();
                }
            }
        }

        /// <summary>
        /// Creates and issues a GET request to the specified URI.
        /// </summary>
        /// <typeparam name="T">The type of the response body.</typeparam>
        /// <param name="requestUri">The URI the request is sent to.</param>
        /// <returns>A <see cref="Task"/> that will issue the request, validate then read the response.</returns>
        private async Task<T> GetAsyncWithoutValidation<T>(Uri requestUri)
        {
            var response = await this.requestor.GetAsync(requestUri).ConfigureAwait(false);

            return await this.handler.ReadAsync<T>(response).ConfigureAwait(false);
        }

        /// <summary>
        /// Creates and issues a POST request to the specified URI.
        /// </summary>
        /// <typeparam name="TOut">The type of the response body.</typeparam>
        /// <param name="requestUri">The URI the request is sent to.</param>
        /// <returns>A <see cref="Task"/> that will issue the request, validate then read the response.</returns>
        private async Task<TOut> PostAsyncWithoutValidation<TOut>(Uri requestUri)
        {
            var response = await this.requestor.PostAsync(requestUri).ConfigureAwait(false);

            return await this.handler.ReadAsync<TOut>(response).ConfigureAwait(false);
        }

        /// <summary>
        /// Creates and issues a POST request to the specified URI.
        /// </summary>
        /// <typeparam name="TIn">The type of the request body.</typeparam>
        /// <typeparam name="TOut">The type of the response body.</typeparam>
        /// <param name="requestUri">The URI the request is sent to.</param>
        /// <param name="content">The object to be sent in the request body.</param>
        /// <returns>A <see cref="Task"/> that will issue the request, validate then read the response.</returns>
        private async Task<TOut> PostAsyncWithoutValidation<TIn, TOut>(Uri requestUri, TIn content)
        {
            var response = await this.requestor.PostAsync(requestUri, content).ConfigureAwait(false);

            return await this.handler.ReadAsync<TOut>(response).ConfigureAwait(false);
        }

        /// <summary>
        /// Creates and issues a PUT request to the specified URI.
        /// </summary>
        /// <typeparam name="TIn">The type of the request body.</typeparam>
        /// <typeparam name="TOut">The type of the response body.</typeparam>
        /// <param name="requestUri">The URI the request is sent to.</param>
        /// <param name="content">The object to be sent in the request body.</param>
        /// <returns>A <see cref="Task"/> that will issue the request, validate then read the response.</returns>
        private async Task<TOut> PutAsyncWithoutValidation<TIn, TOut>(Uri requestUri, TIn content)
        {
            var response = await this.requestor.PutAsync(requestUri, content).ConfigureAwait(false);

            return await this.handler.ReadAsync<TOut>(response).ConfigureAwait(false);
        }
    }
}
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HttpRequestHandler.cs" company="Rakuten">
//   Copyright (c) Rakuten. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Rakuten.Net.Http
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;

    using Rakuten.Gpc.Api.Client;

    /// <summary>
    /// Performs requests to HTTP endpoints.
    /// </summary>
    public class HttpRequestHandler : IHttpRequestHandler, IDisposable
    {
        /// <summary>
        /// The standard value for the Accept header on requests.
        /// </summary>
        private const string StandardAcceptHeaderValue = "application/json";

        /// <summary>
        /// A delegate that will asynchronously generate and return an authorization header.
        /// </summary>
        private readonly Func<Task<AuthenticationHeaderValue>> getAuthenticationHeader; 

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpRequestHandler"/> class.
        /// </summary>
        /// <param name="context">The context in which HTTP requests should be made.</param>
        /// <param name="client">The client using which HTTP requests should be made.</param>
        /// <param name="getAuthenticationHeader">A delegate to asynchronously generate an authorization header.</param>
        public HttpRequestHandler(
            IApiClientContext context, 
            HttpClient client, 
            Func<Task<AuthenticationHeaderValue>> getAuthenticationHeader)
        {
            Contract.Requires(context != null);
            Contract.Requires(client != null);
            Contract.Requires(getAuthenticationHeader != null);

            client.BaseAddress = context.BaseAddress;

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue(StandardAcceptHeaderValue));

            this.Client = client;
            this.getAuthenticationHeader = getAuthenticationHeader;
        }

        /// <summary>
        /// Gets or sets the client using which HTTP requests should be made.
        /// </summary>
        private HttpClient Client { get; set; }

        /// <summary>
        /// Releases all resources used by the current instance of the <see cref="HttpRequestHandler"/> class.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Issues a HTTP request to the specified address using the GET method.
        /// </summary>
        /// <param name="requestUri">The URI the request is sent to.</param>
        /// <returns>A <see cref="Task"/> that will return the response.</returns>
        public Task<HttpResponseMessage> GetAsync(Uri requestUri)
        {
            if (requestUri == null)
            {
                throw new ArgumentNullException("requestUri");
            }

            return this.GetAsyncWithoutValidation(this.MakeAbsoluteUri(requestUri));
        }

        /// <summary>
        /// Issues a HTTP request to the specified address using the POST method.
        /// </summary>
        /// <param name="requestUri">The URI the request is sent to.</param>
        /// <returns>A <see cref="Task"/> that will return the response.</returns>
        public Task<HttpResponseMessage> PostAsync(Uri requestUri)
        {
            if (requestUri == null)
            {
                throw new ArgumentNullException("requestUri");
            }

            return this.PostAsyncWithoutValidation(this.MakeAbsoluteUri(requestUri));
        }

        /// <summary>
        /// Issues a HTTP request to the specified address using the POST method.
        /// </summary>
        /// <typeparam name="T">The type of the request body.</typeparam> 
        /// <param name="requestUri">The URI the request is sent to.</param>
        /// <param name="content">The object to be encoded to the request body.</param>
        /// <returns>A <see cref="Task"/> that will return the response.</returns>
        public Task<HttpResponseMessage> PostAsync<T>(Uri requestUri, T content)
        {
            if (requestUri == null)
            {
                throw new ArgumentNullException("requestUri");
            }

            return this.PostAsyncWithoutValidation(this.MakeAbsoluteUri(requestUri), content);
        }

        /// <summary>
        /// Issues a HTTP request to the specified address using the PUT method.
        /// </summary>
        /// <typeparam name="T">The type of the request body.</typeparam> 
        /// <param name="requestUri">The URI the request is sent to.</param>
        /// <param name="content">The object to be encoded to the request body.</param>
        /// <returns>A <see cref="Task"/> that will return the response.</returns>
        public Task<HttpResponseMessage> PutAsync<T>(Uri requestUri, T content)
        {
            if (requestUri == null)
            {
                throw new ArgumentNullException("requestUri");
            }
            
            return this.PutAsyncWithoutValidation(this.MakeAbsoluteUri(requestUri), content);
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
                HttpClient client = this.Client;
                if (client != null)
                {
                    client.Dispose();
                }
            }
        }

        /// <summary>
        /// Issues a HTTP request to the specified address using the GET method.
        /// </summary>
        /// <param name="requestUri">The URI the request is sent to.</param>
        /// <returns>A <see cref="Task"/> that will return the response.</returns>
        private async Task<HttpResponseMessage> GetAsyncWithoutValidation(Uri requestUri)
        {
            this.Client.DefaultRequestHeaders.Authorization = await this.getAuthenticationHeader();

            return await this.Client.GetAsync(requestUri);
        }

        /// <summary>
        /// Creates an absolute URI from the specified request URI.
        /// </summary>
        /// <param name="requestUri">The URI the request is sent to.</param>
        /// <returns>An absolute URI the request should be sent to.</returns>
        private Uri MakeAbsoluteUri(Uri requestUri)
        {
            Contract.Requires(requestUri != null);

            if (requestUri.IsAbsoluteUri)
            {
                if (!requestUri.Host.Equals(this.Client.BaseAddress.Host, StringComparison.OrdinalIgnoreCase))
                {
                    throw new InvalidOperationException();
                }
            }
            else
            {
                requestUri = new Uri(this.Client.BaseAddress, requestUri);
            }

            return requestUri;
        }

        /// <summary>
        /// Issues a HTTP request to the specified address using the POST method.
        /// </summary>
        /// <param name="requestUri">The URI the request is sent to.</param>
        /// <returns>A <see cref="Task"/> that will return the response.</returns>
        private async Task<HttpResponseMessage> PostAsyncWithoutValidation(Uri requestUri)
        {
            this.Client.DefaultRequestHeaders.Authorization = await this.getAuthenticationHeader();

            return await this.Client.SendAsync(new HttpRequestMessage(HttpMethod.Post, requestUri));
        }

        /// <summary>
        /// Issues a HTTP request to the specified address using the POST method.
        /// </summary>
        /// <typeparam name="T">The type of the request body.</typeparam> 
        /// <param name="requestUri">The URI the request is sent to.</param>
        /// <param name="content">The object to be encoded to the request body.</param>
        /// <returns>A <see cref="Task"/> that will return the response.</returns>
        private async Task<HttpResponseMessage> PostAsyncWithoutValidation<T>(Uri requestUri, T content)
        {
            this.Client.DefaultRequestHeaders.Authorization = await this.getAuthenticationHeader();

            var httpContent = content as HttpContent;

            return await(httpContent == null ?
                this.Client.PostAsJsonAsync(requestUri, content) :
                this.Client.PostAsync(requestUri, httpContent));
        }

        /// <summary>
        /// Issues a HTTP request to the specified address using the PUT method.
        /// </summary>
        /// <typeparam name="T">The type of the request body.</typeparam> 
        /// <param name="requestUri">The URI the request is sent to.</param>
        /// <param name="content">The object to be encoded to the request body.</param>
        /// <returns>A <see cref="Task"/> that will return the response.</returns>
        private async Task<HttpResponseMessage> PutAsyncWithoutValidation<T>(Uri requestUri, T content)
        {
            this.Client.DefaultRequestHeaders.Authorization = await this.getAuthenticationHeader();

            var httpContent = content as HttpContent;

            return await(httpContent == null ?
                this.Client.PutAsJsonAsync(requestUri, content) :
                this.Client.PutAsync(requestUri, httpContent));
        }
    }
}
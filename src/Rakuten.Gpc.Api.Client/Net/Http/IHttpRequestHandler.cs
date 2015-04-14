// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="IHttpRequestHandler.cs" company="Rakuten">
//   Copyright (c) Rakuten. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------
namespace Rakuten.Net.Http
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines an object that performs requests to HTTP endpoints.
    /// </summary>
    public interface IHttpRequestHandler
    {
        /// <summary>
        /// Issues a HTTP request to the specified address using the GET method.
        /// </summary>
        /// <param name="requestUri">The URI the request is sent to.</param>
        /// <returns>A <see cref="Task"/> that will return the response.</returns>
        Task<HttpResponseMessage> GetAsync(Uri requestUri);

        /// <summary>
        /// Issues a HTTP request to the specified address using the POST method.
        /// </summary>
        /// <param name="requestUri">The URI the request is sent to.</param>
        /// <returns>A <see cref="Task"/> that will return the response.</returns>
        Task<HttpResponseMessage> PostAsync(Uri requestUri);

        /// <summary>
        /// Issues a HTTP request to the specified address using the POST method.
        /// </summary>
        /// <typeparam name="T">The type of the request body.</typeparam> 
        /// <param name="requestUri">The URI the request is sent to.</param>
        /// <param name="content">The object to be sent in the request body.</param>
        /// <returns>A <see cref="Task"/> that will return the response.</returns>
        Task<HttpResponseMessage> PostAsync<T>(Uri requestUri, T content);

        /// <summary>
        /// Issues a HTTP request to the specified address using the PUT method.
        /// </summary>
        /// <typeparam name="T">The type of the request body.</typeparam> 
        /// <param name="requestUri">The URI the request is sent to.</param>
        /// <param name="content">The object to be sent in the request body.</param>
        /// <returns>A <see cref="Task"/> that will return the response.</returns>
        Task<HttpResponseMessage> PutAsync<T>(Uri requestUri, T content);
    }
}
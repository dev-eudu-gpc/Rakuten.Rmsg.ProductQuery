// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="IHttpResponseHandler.cs" company="Rakuten">
//   Copyright (c) Rakuten. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------
namespace Rakuten.Net.Http
{
    using System.Net.Http;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines an object that reads responses to HTTP requests.
    /// </summary>
    public interface IHttpResponseHandler
    {
        /// <summary>
        /// Validates and extracts a model from a response from a HTTP request.
        /// </summary>
        /// <typeparam name="T">The type of model to be extracted from the response.</typeparam>
        /// <param name="response">The response to the HTTP request.</param>
        /// <returns>A <see cref="Task"/> that will validate and extract a model from a response.</returns>
        Task<T> ReadAsync<T>(HttpResponseMessage response);
    }
}

// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="IHttpResponseValidator.cs" company="Rakuten">
//   Copyright (c) Rakuten. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------
namespace Rakuten.Net.Http
{
    using System.Net.Http;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines an object the validates responses from requests made over HTTP.
    /// </summary>
    public interface IHttpResponseValidator
    {
        /// <summary>
        /// Validates the specified <see cref="HttpResponseMessage"/>.
        /// </summary>
        /// <param name="response">The <see cref="HttpResponseMessage"/> to validate.</param>
        /// <returns>A <see cref="Task"/> that will validate the specified response.</returns>
        Task ValidateAsync(HttpResponseMessage response);
    }
}

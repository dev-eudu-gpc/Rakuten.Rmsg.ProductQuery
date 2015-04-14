// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="HttpResponseValidator.cs" company="Rakuten">
//   Copyright (c) Rakuten. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------
namespace Rakuten.Net.Http
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;

    using Rakuten.Gpc.Api.Client;

    /// <summary>
    /// Validates responses from requests made over HTTP.
    /// </summary>
    public class HttpResponseValidator : IHttpResponseValidator
    {
        /// <summary>
        /// Locates and throws exceptions for non-successful HTTP requests.
        /// </summary>
        private readonly IExceptionRegister register;

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpResponseValidator"/> class.
        /// </summary>
        /// <param name="register">
        /// The register that will locate and throw exceptions for a specified <see cref="HttpResponseMessage"/>.
        /// </param>
        public HttpResponseValidator(IExceptionRegister register)
        {
            if (register == null)
            {
                throw new ArgumentNullException("register");
            }

            this.register = register;
        }

        /// <summary>
        /// Validates the specified <see cref="HttpResponseMessage"/>.
        /// </summary>
        /// <param name="response">The <see cref="HttpResponseMessage"/> to validate.</param>
        /// <returns>A <see cref="Task"/> that will validate the specified response.</returns>
        public Task ValidateAsync(HttpResponseMessage response)
        {
            if (response == null)
            {
                throw new ArgumentNullException("response");
            }

            return response.IsSuccessStatusCode ? Task.FromResult(true) : this.RaiseError(response);
        }

        /// <summary>
        /// Validates the specified <see cref="HttpResponseMessage"/>.
        /// </summary>
        /// <param name="response">The <see cref="HttpResponseMessage"/> to validate.</param>
        /// <returns>A <see cref="Task"/> that will validate the specified response.</returns>
        private async Task RaiseError(HttpResponseMessage response)
        {
            string content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            throw this.register.GetException(content) ?? new HttpRequestException(content);
        }
    }
}
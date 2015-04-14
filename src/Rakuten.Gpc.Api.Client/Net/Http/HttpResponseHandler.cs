// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HttpResponseHandler.cs" company="Rakuten">
//   Copyright (c) Rakuten. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Rakuten.Net.Http
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Net.Http.Formatting;
    using System.Threading.Tasks;

    /// <summary>
    /// Validates and extracts models from responses to HTTP requests.
    /// </summary>
    public class HttpResponseHandler : IHttpResponseHandler
    {
        /// <summary>
        /// List of <see cref="MediaTypeFormatter"/> that will be used for services
        /// </summary>
        private readonly List<MediaTypeFormatter> formatters;

        /// <summary>
        /// The <see cref="IHttpResponseValidator"/> that will validate the response.
        /// </summary>
        private readonly IHttpResponseValidator validator;

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpResponseHandler"/> class.
        /// </summary>
        /// <param name="validator">
        /// A <see cref="IHttpResponseValidator"/> that will validate the response.
        /// </param>
        public HttpResponseHandler(IHttpResponseValidator validator)
        {
            if (validator == null)
            {
                throw new ArgumentNullException("validator");
            }

            this.formatters = new List<MediaTypeFormatter> { new JsonMediaTypeFormatter() };
            this.validator = validator;
        }

        /// <summary>
        /// Validates and extracts a model from a response from a HTTP request.
        /// </summary>
        /// <typeparam name="T">
        /// The type of model to be extracted from the response.
        /// </typeparam>
        /// <param name="response">
        /// The response to the HTTP request.
        /// </param>
        /// <returns>
        /// A <see cref="Task"/> that will validate and extract a model from a response.
        /// </returns>
        public Task<T> ReadAsync<T>(HttpResponseMessage response)
        {
            if (response == null)
            {
                throw new ArgumentNullException("response");
            }

            return this.ReadAsyncWithoutValidation<T>(response);
        }

        /// <summary>
        /// Validates and extracts a model from a response from a HTTP request.
        /// </summary>
        /// <typeparam name="T">
        /// The type of model to be extracted from the response.
        /// </typeparam>
        /// <param name="response">
        /// The response to the HTTP request.
        /// </param>
        /// <returns>
        /// A <see cref="Task"/> that will validate and extract a model from a response.
        /// </returns>
        private async Task<T> ReadAsyncWithoutValidation<T>(HttpResponseMessage response)
        {
            await this.validator.ValidateAsync(response).ConfigureAwait(false);

            return await response.Content.ReadAsAsync<T>(this.formatters).ConfigureAwait(false);
        }
    }
}

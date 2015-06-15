//------------------------------------------------------------------------------
// <copyright file="GpcApiClient.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.Web.Http.Tests.Integration
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading.Tasks;
    using Rakuten.Rmsg.ProductQuery.Configuration;

    /// <summary>
    /// Provides methods for making calls to the Product Query API
    /// </summary>
    public class GpcApiClient
    {
        /// <summary>
        /// The username to be supplied in HTTP requests.
        /// </summary>
        private const string GpcApiUsername = "Rakuten";

        /// <summary>
        /// The password to be supplied in HTTP requests.
        /// </summary>
        private const string GpcApiPassword = "197869d4-c221-4d4f-a5ad-5ddbf7aa92fd";

        /// <summary>
        /// The accept header to be sent in HTTP requests.
        /// </summary>
        private static readonly MediaTypeWithQualityHeaderValue AcceptHeader =
            new MediaTypeWithQualityHeaderValue("application/json");

        /// <summary>
        /// The context for the API.
        /// </summary>
        private readonly IApiContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="GpcApiClient"/> class
        /// </summary>
        /// <param name="apiContext">A context for the API.</param>
        public GpcApiClient(IApiContext apiContext)
        {
            Contract.Requires(apiContext != null);

            this.context = apiContext;
        }

        /// <summary>
        /// Makes a call to create a new product.
        /// </summary>
        /// <param name="source">The product to create.</param>
        /// <returns>The <see cref="HttpResponseMessage"/> from the API call.</returns>
        public async Task<HttpResponseMessage> CreateProduct(Product source)
        {
            // Create the HTTP client
            var client = this.CreateNewHttpClient();

            // Construct the URI
            var uri = string.Format(
                "/v1/product",
                source.Id,
                source.Culture);

            // Make the call
            return await client.PostAsJsonAsync<Product>(uri, source);
        }

        /// <summary>
        /// Constructs and configures a <see cref="HttpClient"/> instance.
        /// </summary>
        /// <returns>A configured <see cref="HttpClient"/> instance.</returns>
        private HttpClient CreateNewHttpClient()
        {
            var handler = new HttpClientHandler { AllowAutoRedirect = false };

            var client = new HttpClient(handler)
            {
                BaseAddress = this.context.GpcCoreApiBaseAddress
            };

            client.DefaultRequestHeaders.Accept.Add(AcceptHeader);

            string credentials = Convert.ToBase64String(
                Encoding.Default.GetBytes(
                    string.Format("{0}:{1}", GpcApiUsername, GpcApiPassword)));

            var authorizationHeader = new AuthenticationHeaderValue("Basic", credentials);

            client.DefaultRequestHeaders.Authorization = authorizationHeader;

            return client;
        }
    }
}
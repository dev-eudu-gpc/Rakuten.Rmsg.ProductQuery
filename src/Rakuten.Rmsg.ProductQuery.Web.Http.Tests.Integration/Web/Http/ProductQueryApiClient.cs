//------------------------------------------------------------------------------
// <copyright file="ProductQueryApiClient.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.Web.Http.Tests.Integration
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Rakuten.Rmsg.ProductQuery.Configuration;
    using Rakuten.Rmsg.ProductQuery.Web.Http.Tests.Integration.Resources;

    /// <summary>
    /// Provides methods for making calls to the Product Query API
    /// </summary>
    public class ProductQueryApiClient
    {
        /// <summary>
        /// The context for the Product Query API.
        /// </summary>
        private readonly IApiContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductQueryApiClient"/> class
        /// </summary>
        /// <param name="apiContext">A context for the API.</param>
        public ProductQueryApiClient(IApiContext apiContext)
        {
            Contract.Requires(apiContext != null);

            this.context = apiContext;
        }

        /// <summary>
        /// Makes a call to the Product Query API to submit a new product query.
        /// </summary>
        /// <param name="source">The product query to create.</param>
        /// <returns>The <see cref="HttpResponseMessage"/> from the API call.</returns>
        public async Task<HttpResponseMessage> SubmitNewProductQuery(ProductQuery source)
        {
            // Create the HTTP client
            var handler = new HttpClientHandler { AllowAutoRedirect = false };
            var client = new HttpClient(handler) { BaseAddress = this.context.ProductQueryApiBaseAddress };

            // Construct the URI
            var uri = string.Format(
                "/product-query/{0}/culture/{1}",
                source.Id,
                source.Culture);

            // Make the call
            return await client.PutAsJsonAsync<string>(uri, string.Empty);
        }

        /// <summary>
        /// Makes a call to the Product Query API to flag a product query
        /// as ready for processing.
        /// </summary>
        /// <param name="source">The product query to flag as ready for processing.</param>
        /// <returns>The <see cref="HttpResponseMessage"/> from the API call.</returns>
        public async Task<HttpResponseMessage> FlagAsReadyForProcessing(ProductQuery source)
        {
            // Create the HTTP client
            var handler = new HttpClientHandler { AllowAutoRedirect = false };
            var client = new HttpClient(handler) { BaseAddress = this.context.ProductQueryApiBaseAddress };

            // Construct the URI
            var uri = string.Format(
                "/product-query/{0}/culture/{1}",
                source.Id,
                source.Culture);

            // Make the call
            return await client.PutAsJsonAsync<ProductQuery>(uri, source);
        }

        /// <summary>
        /// Makes a call to the Product Query API to get the status of a given
        /// product query group.
        /// </summary>
        /// <param name="productQueryGroupId">The identifier of the product query group.</param>
        /// <param name="year">The year of the point in time.</param>
        /// <param name="month">The month of the point in time.</param>
        /// <param name="day">The day of the point in time.</param>
        /// <param name="hour">The hour of the point in time.</param>
        /// <param name="minute">The minute of the point in time.</param>
        /// <returns>The <see cref="HttpResponseMessage"/> from the API call.</returns>
        public async Task<HttpResponseMessage> GetProductQueryGroupStatus(
            string productQueryGroupId,
            string year,
            string month,
            string day,
            string hour,
            string minute)
        {
            // Create the HTTP client
            var handler = new HttpClientHandler { AllowAutoRedirect = false };
            var client = new HttpClient(handler) { BaseAddress = this.context.ProductQueryApiBaseAddress };

            // Construct the URI
            var uri = string.Format(
                "/product-query-group/{0}/status/{1}/{2}/{3}/{4}/{5}",
                productQueryGroupId,
                year,
                month,
                day,
                hour,
                minute);

            // Make the call
            return await client.GetAsync(uri);
        }
    }
}

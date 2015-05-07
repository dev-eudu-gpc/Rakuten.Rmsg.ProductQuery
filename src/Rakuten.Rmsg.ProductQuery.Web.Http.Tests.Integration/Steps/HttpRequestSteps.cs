//------------------------------------------------------------------------------
// <copyright file="HTtpRequestSteps.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.Web.Http.Tests.Integration
{
    using System.Diagnostics.Contracts;
    using System.Net.Http;
    using Rakuten.Rmsg.ProductQuery.Configuration;
    using TechTalk.SpecFlow;

    /// <summary>
    /// Steps for making HTTP requests to the Product Query API.
    /// </summary>
    [Binding]
    public class HttpRequestSteps
    {
        /// <summary>
        /// The context under which this instance is operating.
        /// </summary>
        private readonly IApiContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpRequestSteps"/> class
        /// </summary>
        /// <param name="apiContext">A context for the API.</param>
        public HttpRequestSteps(IApiContext apiContext)
        {
            Contract.Requires(apiContext != null);

            this.context = apiContext;
        }

        /// <summary>
        /// Makes an HTTP request to the API for submitting a new product query
        /// and stores it in the current scenario context.
        /// </summary>
        [Given(@"a request is made to submit the new product query")]
        [When(@"a request is made to submit the new product query")]
        [When(@"a request is made to submit the new product query again")]
        public void ARequestIsMadeToSubmitTheNewProductQuery()
        {
            // TODO: Centralise URI template

            // Get the details of the new product query
            var productQuery = ScenarioStorage.NewProductQuery;

            // Create the HTTP client
            var client = new HttpClient(new HttpClientHandler { AllowAutoRedirect = false })
            {
                BaseAddress = this.context.BaseAddress
            };

            // Construct the URI
            var uri = string.Format(
                "/product-query/{0}/culture/{1}",
                productQuery.Id,
                productQuery.Culture);

            // Make the call
            HttpResponseMessage response = client.PutAsJsonAsync<string>(uri, string.Empty).Result;

            // Store the request and response
            ScenarioStorage.HttpResponseMessage = response;
        }
    }
}
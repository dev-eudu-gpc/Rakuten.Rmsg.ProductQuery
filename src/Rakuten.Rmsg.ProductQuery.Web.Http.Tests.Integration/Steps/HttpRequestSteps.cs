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
        private readonly ProductQueryApiClient apiClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpRequestSteps"/> class
        /// </summary>
        /// <param name="apiClient">A context for the API.</param>
        public HttpRequestSteps(ProductQueryApiClient apiClient)
        {
            Contract.Requires(apiClient != null);

            this.apiClient = apiClient;
        }

        /// <summary>
        /// Makes an HTTP request to the API for submitting a new product query
        /// and stores it in scenario storage.
        /// </summary>
        [Given(@"a request has been made to submit the new product query")]
        [When(@"a request is made to submit the new product query")]
        [When(@"a request is made to submit the new product query again")]
        public void ARequestIsMadeToSubmitTheNewProductQuery()
        {
            ScenarioStorage.HttpResponseMessage =
                this.apiClient
                    .SubmitNewProductQuery(ScenarioStorage.NewProductQuery)
                    .Result;
        }

        /// <summary>
        ///  Makes an HTTP request to the API to flag a product query as ready for processing
        ///  using a given value for the status and stores the response in scenario storage.
        /// </summary>
        /// <param name="status">The status to include in the request body.</param>
        [When(@"a request is made to flag the product query as ready for processing with a status of (.*)")]
        public void WhenARequestIsMadeToFlagTheProductQueryAsReadyForProcessingWithAStatusOf(string status)
        {
            // Get the product query to use in the call and set 
            // its status to "submitted"
            ProductQuery productQuery = ScenarioStorage.NewProductQuery;
            productQuery.Status = status;

            ScenarioStorage.HttpResponseMessage =
                this.apiClient
                    .FlagAsReadyForProcessing(productQuery)
                    .Result;
        }
    }
}
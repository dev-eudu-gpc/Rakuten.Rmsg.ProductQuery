//------------------------------------------------------------------------------
// <copyright file="HTtpRequestSteps.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.Web.Http.Tests.Integration
{
    using System;
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
        /// A client that provides access to the product query API.
        /// </summary>
        private readonly ProductQueryApiClient apiClient;

        /// <summary>
        /// The context under which this instance is operating.
        /// </summary>
        private readonly IApiContext apiContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpRequestSteps"/> class
        /// </summary>
        /// <param name="apiClient">A client that provides access to the product query API.</param>
        /// <param name="apiContext">A context for the API.</param>
        public HttpRequestSteps(
            ProductQueryApiClient apiClient,
            IApiContext apiContext)
        {
            Contract.Requires(apiClient != null);
            Contract.Requires(apiContext != null);

            this.apiClient = apiClient;
            this.apiContext = apiContext;
        }

        /// <summary>
        /// Makes an HTTP request to the API for submitting a new product query
        /// and stores it in scenario storage.
        /// </summary>
        [Given(@"a request has been made to submit the new product query")]
        [When(@"a request is made to submit the new product query")]
        [When(@"a request is made to submit the new product query again")]
        public void WhenARequestIsMadeToSubmitTheNewProductQuery()
        {
            ScenarioStorage.HttpResponseMessage = this.apiClient.SubmitNewProductQuery(ScenarioStorage.NewProductQuery).Result;
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

            ScenarioStorage.HttpResponseMessage = this.apiClient.FlagAsReadyForProcessing(productQuery).Result;
        }

        /// <summary>
        /// Makes an HTTP request to the API to get the status of a product query
        /// using the prepared parameters from scenario storage.
        /// </summary>
        [When(@"a request is made to get the status of a product query group using the prepared request")]
        public void WhenARequestIsMadeToGetTheStatusOfAProductQueryGroupUsingThePreparedRequest()
        {
            // Get the request parameters from scenario storage
            var p = ScenarioStorage.ProductQueryMonitorRequest;

            // Make the call
            ScenarioStorage.HttpResponseMessage = 
                this.apiClient
                    .GetProductQueryGroupStatus(p.Id, p.Year, p.Month, p.Day, p.Hour, p.Minute)
                    .Result;
        }
    }
}
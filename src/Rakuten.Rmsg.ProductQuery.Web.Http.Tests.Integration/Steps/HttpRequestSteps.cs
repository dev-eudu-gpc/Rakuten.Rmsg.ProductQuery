//------------------------------------------------------------------------------
// <copyright file="HTtpRequestSteps.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.Web.Http.Tests.Integration.Steps
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using Rakuten.Rmsg.ProductQuery.Configuration;
    using Rakuten.Rmsg.ProductQuery.Web.Http.Tests.Integration.Resources;
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
        /// An object for storing and retrieving information from the scenario context.
        /// </summary>
        private readonly ScenarioStorage scenarioStorage;

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpRequestSteps"/> class
        /// </summary>
        /// <param name="apiClient">A client that provides access to the product query API.</param>
        /// <param name="apiContext">A context for the API.</param>
        /// <param name="scenarioStorage">An object for sharing information between steps.</param>
        public HttpRequestSteps(
            ProductQueryApiClient apiClient,
            IApiContext apiContext,
            ScenarioStorage scenarioStorage)
        {
            Contract.Requires(apiClient != null);
            Contract.Requires(apiContext != null);
            Contract.Requires(scenarioStorage != null);

            this.apiClient = apiClient;
            this.apiContext = apiContext;
            this.scenarioStorage = scenarioStorage;
        }

        /// <summary>
        ///  Makes an HTTP request to the API to flag a product query as ready for processing
        ///  using a given value for the status and stores the response in scenario storage.
        /// </summary>
        /// <param name="status">The status to include in the request body.</param>
        [Given(@"a request is made to flag the product query as ready for processing with a status of (.*)")]
        [When(@"a request is made to flag the product query as ready for processing with a status of (.*)")]
        public void WhenARequestIsMadeToFlagTheProductQueryAsReadyForProcessingWithAStatusOf(string status)
        {
            // Get the product query to use in the call and set its status to "submitted"
            var productQuery = this.scenarioStorage.Creation.SourceProductQuery;
            productQuery.Status = status;

            // Call the API
            var response = this.apiClient
                .FlagAsReadyForProcessing(productQuery)
                .Result;

            // Store the pertinent details for subsequent steps
            this.scenarioStorage.ReadyForProcessing.ResponseMessage = response;
            this.scenarioStorage.LastResponse = response;
        }

        /// <summary>
        /// Makes an HTTP request to the API to get the status of a product query
        /// using the prepared parameters from scenario storage.
        /// </summary>
        [When(@"a request is made to get the status of the product query group of the new product query")]
        public void WhenARequestIsMadeToGetTheStatusOfAProductQueryGroupOfTheNewProductQuery()
        {
            using (var database = new ProductQueryDbContext())
            {
                var group = database.rmsgProductQueries
                    .Single(q => q.rmsgProductQueryID == this.scenarioStorage.Creation.SourceProductQuery.IdAsGuid)
                    .rmsgProductQueryGroup;

                // Get the request parameters from scenario storage
                var request = new ProductQueryMonitorRequest(
                    group.rmsgProductQueryGroupID.ToString(),
                    DateTime.UtcNow.AddSeconds(0 - this.apiContext.ProgressMapIntervalInSeconds));

                // Call the API
                var response = this.apiClient
                    .GetProductQueryGroupStatus(request.Id, request.Year, request.Month, request.Day, request.Hour, request.Minute)
                    .Result;

                // Store the pertinent details for subsequent steps
                this.scenarioStorage.Monitor.SourceRequest = request;
                this.scenarioStorage.Monitor.ResponseMessage = response;
                this.scenarioStorage.LastResponse = response;
            }
        }

        /// <summary>
        /// Makes a call to the API to get the status of the product query group for
        /// the new product query but using a date that is too recent.
        /// </summary>
        [When(@"a request is made to get the status of a product query group using a date/time that is too recent")]
        public void WhenARequestIsMadeToGetTheStatusOfAProductQueryGroupUsingADateTimeThatIsTooRecent()
        {
            using (var database = new ProductQueryDbContext())
            {
                // Get the group of the product query that was created
                var group = database.rmsgProductQueries
                    .Single(q => q.rmsgProductQueryID == this.scenarioStorage.Creation.SourceProductQuery.IdAsGuid)
                    .rmsgProductQueryGroup;

                // Get the request parameters from scenario storage
                var request = new ProductQueryMonitorRequest(
                    group.rmsgProductQueryGroupID.ToString(),
                    DateTime.UtcNow.AddSeconds(60));

                // Call the API
                var response = this.apiClient
                    .GetProductQueryGroupStatus(request.Id, request.Year, request.Month, request.Day, request.Hour, request.Minute)
                    .Result;

                // Store pertinent details for subsequent steps
                this.scenarioStorage.Monitor.SourceRequest = request;
                this.scenarioStorage.Monitor.ResponseMessage = response;
                this.scenarioStorage.LastResponse = response;
            }
        }

        /// <summary>
        /// Makes a request to the API to get the status of a product query group that does not exist.
        /// </summary>
        [When(@"a request is made to get the status of a product query group which does not exist")]
        public void WhenARequestIsMadeToGetTheStatusOfAProductQueryGroupWhichDoesNotExist()
        {
            using (var database = new ProductQueryDbContext())
            {
                // Get the request parameters from scenario storage
                var request = new ProductQueryMonitorRequest(
                    Guid.NewGuid().ToString(),
                    DateTime.UtcNow.AddSeconds(0 - this.apiContext.ProgressMapIntervalInSeconds));

                // Call the API
                var response = this.apiClient
                    .GetProductQueryGroupStatus(request.Id, request.Year, request.Month, request.Day, request.Hour, request.Minute)
                    .Result;

                // Store pertinent details for subsequent steps
                this.scenarioStorage.Monitor.SourceRequest = request;
                this.scenarioStorage.Monitor.ResponseMessage = response;
                this.scenarioStorage.LastResponse = response;
            }
        }

        /// <summary>
        /// Makes a request to the API to get the status of a product query group
        /// using an identifier that is not a GUID.
        /// </summary>
        [When(@"a request is made to get the status of a product query group with an identifier that is not a GUID")]
        public void WhenARequestIsMadeToGetTheStatusOfAProductQueryGroupWithAnIdentifierThatIsNotAGUID()
        {
            using (var database = new ProductQueryDbContext())
            {
                // Get the request parameters from scenario storage
                var request = new ProductQueryMonitorRequest(
                    "i am not a guid",
                    DateTime.UtcNow.AddSeconds(0 - this.apiContext.ProgressMapIntervalInSeconds));

                // Call the API
                var response = this.apiClient
                    .GetProductQueryGroupStatus(request.Id, request.Year, request.Month, request.Day, request.Hour, request.Minute)
                    .Result;

                // Store pertinent details for subsequent steps
                this.scenarioStorage.Monitor.SourceRequest = request;
                this.scenarioStorage.Monitor.ResponseMessage = response;
                this.scenarioStorage.LastResponse = response;
            }
        }

        /// <summary>
        /// Makes a call to the API to get the status of the product query group for
        /// the new product query but using an invalid date.
        /// </summary>
        [When(@"a request is made to get the status of the product query group using an invalid date/time")]
        public void WhenARequestIsMadeToGetTheStatusOfTheProductQueryGroupUsingAnInvalidDateTime()
        {
            using (var database = new ProductQueryDbContext())
            {
                // Get the group for the product query
                var group = database.rmsgProductQueries
                    .Single(q => q.rmsgProductQueryID == this.scenarioStorage.Creation.SourceProductQuery.IdAsGuid)
                    .rmsgProductQueryGroup;

                // Create the request
                var request = new ProductQueryMonitorRequest
                {
                    Id = group.rmsgProductQueryGroupID.ToString(),
                    Year = "zz",
                    Month = "07",
                    Day = "17",
                    Hour = "01",
                    Minute = "02"
                };

                // Call the API
                var response = this.apiClient
                    .GetProductQueryGroupStatus(request.Id, request.Year, request.Month, request.Day, request.Hour, request.Minute)
                    .Result;

                // Store pertinent details for subsequent steps
                this.scenarioStorage.Monitor.SourceRequest = request;
                this.scenarioStorage.Monitor.ResponseMessage = response;
                this.scenarioStorage.LastResponse = response;
            }
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
            // Call the API
            var response = this.apiClient
                .SubmitNewProductQuery(this.scenarioStorage.Creation.SourceProductQuery)
                .Result;

            // Store the pertinent details for subsequent steps
            this.scenarioStorage.Creation.ResponseMessage = response;
            this.scenarioStorage.LastResponse = response;
        }
    }
}
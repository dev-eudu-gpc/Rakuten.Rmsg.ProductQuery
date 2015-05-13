//------------------------------------------------------------------------------
// <copyright file="ProductQueryMonitorSteps.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.Web.Http.Tests.Integration.Steps
{
    using System;
    using System.Diagnostics.Contracts;
    using Rakuten.Rmsg.ProductQuery.Configuration;
    using TechTalk.SpecFlow;

    /// <summary>
    /// Contains steps pertaining to the product query monitor end point.
    /// </summary>
    [Binding]
    public class ProductQueryMonitorSteps
    {
        /// <summary>
        /// A context for the product query API.
        /// </summary>
        private readonly IApiContext apiContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductQueryMonitorSteps"/> class
        /// </summary>
        /// <param name="apiContext">A context for the product query API.</param>
        public ProductQueryMonitorSteps(IApiContext apiContext)
        {
            Contract.Requires(apiContext != null);

            this.apiContext = apiContext;
        }

        /// <summary>
        /// Prepares parameters for requesting the status of a product query group
        /// that does not exist.
        /// </summary>
        [Given(@"a product query monitor request for a non-existent product query group has been prepared")]
        public void GivenAProductQueryMonitorRequestForANon_ExistentProductQueryGroupHasBeenPrepared()
        {
            ScenarioStorage.ProductQueryMonitorRequest = 
                new ProductQueryMonitorRequest(
                    Guid.NewGuid().ToString(),
                    DateTime.UtcNow.AddSeconds(0 - this.apiContext.ProgressMapIntervalInSeconds));
        }

        /// <summary>
        /// Prepares parameters for requesting the status of a product query group
        /// using an identifier that is not a GUID.
        /// </summary>
        [Given(@"a product query monitor request with an identifier that is not a GUID has been prepared")]
        public void GivenAProductQueryMonitorRequestWithAnIdentifierThatIsNotAGUIDHasBeenPrepared()
        {
            ScenarioStorage.ProductQueryMonitorRequest = 
                new ProductQueryMonitorRequest(
                    "i-am-not-a-guid",
                    DateTime.UtcNow.AddSeconds(0 - this.apiContext.ProgressMapIntervalInSeconds));
        }

        /// <summary>
        /// Prepares parameters for requesting the status of a product query group
        /// using a date time that is too recent.
        /// </summary>
        [Given(@"a product query monitor request with a date/time that is too recent has been prepared")]
        public void GivenAProductQueryMonitorRequestWithADateTimeThatIsTooRecentHasBeenPrepared()
        {
            ScenarioStorage.ProductQueryMonitorRequest =
                new ProductQueryMonitorRequest(Guid.NewGuid().ToString(), DateTime.UtcNow.AddSeconds(60));
        }
        
        /// <summary>
        /// Prepares parameters for requesting the status of a product query group
        /// using a point in time that is not a valid date/time.
        /// </summary>
        [Given(@"a product query monitor request with an invalid date/time has been prepared")]
        public void GivenAProductQueryMonitorRequestWithAnInvalidDateTimeHasBeenPrepared()
        {
            ScenarioStorage.ProductQueryMonitorRequest = new ProductQueryMonitorRequest
            {
                Id = Guid.NewGuid().ToString(),
                Year = "zz",
                Month = "07",
                Day = "17",
                Hour = "01",
                Minute = "02"
            };
        }

        /// <summary>
        /// Prepares parameters for requesting the status of the product query group
        /// for the new product query.
        /// </summary>
        [Given(@"a product query monitor request for the new product query has been prepared")]
        public void GivenAProductQueryMonitorRequestForTheNewProductQueryHasBeenPrepared()
        {
            ScenarioStorage.ProductQueryMonitorRequest =
                new ProductQueryMonitorRequest(
                    ScenarioStorage.ProductQueryGroupActual.rmsgProductQueryGroupID.ToString(),
                    DateTime.UtcNow.AddSeconds(0 - this.apiContext.ProgressMapIntervalInSeconds));
        }
    }
}

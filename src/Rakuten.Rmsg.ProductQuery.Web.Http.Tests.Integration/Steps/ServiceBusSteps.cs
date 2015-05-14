//------------------------------------------------------------------------------
// <copyright file="ServiceBusSteps.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.Web.Http.Tests.Integration.Steps
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Rakuten.Rmsg.ProductQuery.Configuration;
    using TechTalk.SpecFlow;

    /// <summary>
    /// Contains steps pertaining to the Azure Service Bus.
    /// </summary>
    [Binding]
    public class ServiceBusSteps
    {
        /// <summary>
        /// The context under which this instance is operating.
        /// </summary>
        private readonly IApiContext apiContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceBusSteps"/> class
        /// </summary>
        /// <param name="apiContext">A context for the API.</param>
        public ServiceBusSteps(IApiContext apiContext)
        {
            Contract.Requires(apiContext != null);

            this.apiContext = apiContext;
        }

        /// <summary>
        /// Ensures that the web job is not running.
        /// </summary>
        [Given(@"the web job is not running")]
        public void GivenTheWebJobIsNotRunning()
        {
            // TODO: [WB 11-May-2015] Actually implement this
            if (this.apiContext.BaseAddress.AbsoluteUri.StartsWith("http://localhost"))
            {
            }
            else
            {
            }
        }
    }
}

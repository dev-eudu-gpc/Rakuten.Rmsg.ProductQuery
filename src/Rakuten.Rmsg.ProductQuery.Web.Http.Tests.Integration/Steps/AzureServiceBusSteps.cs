//------------------------------------------------------------------------------
// <copyright file="AzureServiceBusSteps.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.Web.Http.Tests.Integration
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
    public class AzureServiceBusSteps
    {
        /// <summary>
        /// The context under which this instance is operating.
        /// </summary>
        private readonly IApiContext apiContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="AzureServiceBusSteps"/> class
        /// </summary>
        /// <param name="apiContext">A context for the API.</param>
        public AzureServiceBusSteps(IApiContext apiContext)
        {
            Contract.Requires(apiContext != null);

            this.apiContext = apiContext;
        }
    }
}

//------------------------------------------------------------------------------
// <copyright file="ProductQueryMonitorSteps.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.Web.Http.Tests.Integration.Steps
{
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
    }
}

// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="BeforeScenarioHooks.cs" company="Rakuten">
//   Copyright (c) Rakuten. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.Web.Http.Tests.Integration
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;
    using BoDi;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Rakuten.Rmsg.ProductQuery.Configuration;
    using TechTalk.SpecFlow;

    /// <summary>
    /// Constructs the object graph for the test run.
    /// </summary>
    [Binding]
    public class BeforeScenarioHooks
    {
        /// <summary>
        /// The <see cref="IObjectContainer"/> instances used for dependency resolution.
        /// </summary>
        private readonly IObjectContainer container;

        /// <summary>
        /// Initializes a new instance of the <see cref="BeforeScenarioHooks"/> class with the specified 
        /// <see cref="IObjectContainer"/> instance.
        /// </summary>
        /// <param name="container">The <see cref="IObjectContainer"/> instance to populate with dependencies.</param>
        public BeforeScenarioHooks(IObjectContainer container)
        {
            if (container == null)
            {
                throw new ArgumentNullException("container");
            }

            IApiContext apiContext = new ApiContextFactory(new AppSettingsConfigurationSource()).Create();
            IStorage azureStorage = new AzureStorage();
            ProductQueryApiClient apiClient = new ProductQueryApiClient(apiContext);

            container.RegisterInstanceAs(apiContext);
            container.RegisterInstanceAs(azureStorage);
            container.RegisterInstanceAs(apiClient);

            this.container = container;
        }

        /// <summary>
        /// Initializes the dependencies of all scenarios
        /// </summary>
        [BeforeScenario]
        public void BeforeScenario()
        {
            this.container.Resolve<IApiContext>();
            this.container.Resolve<IStorage>();
        }

        /// <summary>
        /// Initializes dependencies of all scenarios that require the GPC core API to be running.
        /// </summary>
        [BeforeScenario("GpcCoreApi")]
        public void BeforeScenarioGpcCoreApi()
        {
            IApiContext apiContext = this.container.Resolve<IApiContext>();

            // Ensure that the GPC core API is available, if not inform the user
            // and bail out of the test run.
            var isAvailable = IsWebsiteAvailableAsync(apiContext.GpcCoreApiBaseAddress).Result;
            var message = string.Format(
                    "The GPC core API did not response for URI {0}.  Please ensure that web site is started.",
                    apiContext.GpcCoreApiBaseAddress);
            
            Assert.IsTrue(isAvailable, message);
        }

        /// <summary>
        /// Initializes dependencies of all scenarios that require the web job to be running.
        /// </summary>
        [BeforeScenario("WebJob")]
        public void BeforeScenarioWebJob()
        {
            IApiContext apiContext = this.container.Resolve<IApiContext>();

            var webJobClient = new WebJobClient();

            this.container.RegisterInstanceAs(webJobClient);
        }

        /// <summary>
        /// Verifies that the specified website is available by making a HTTP request to the 
        /// <paramref name="address"/>.
        /// </summary>
        /// <param name="address">
        /// The URI to which a request should be made to check availability.
        /// </param>
        /// <returns>
        /// A <see cref="Task"/> that will determine if the website is available.
        /// </returns>
        private static async Task<bool> IsWebsiteAvailableAsync(Uri address)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    await client.GetAsync(address.ToString());
                }
                catch (HttpRequestException)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
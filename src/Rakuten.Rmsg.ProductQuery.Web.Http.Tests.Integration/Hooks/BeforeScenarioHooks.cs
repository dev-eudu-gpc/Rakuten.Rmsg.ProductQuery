﻿// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="BeforeScenarioHooks.cs" company="Rakuten">
//   Copyright (c) Rakuten. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.Web.Http.Tests.Integration
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;
    using BoDi;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Newtonsoft.Json;
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
            GpcApiClient gpcApiClient = new GpcApiClient(apiContext);

            container.RegisterInstanceAs(apiContext);
            container.RegisterInstanceAs(azureStorage);
            container.RegisterInstanceAs(apiClient);
            container.RegisterInstanceAs(gpcApiClient);

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

            // Log the time at which the scenario began.
            ScenarioStorage.ScenarioStartTime = DateTime.UtcNow;
        }

        /// <summary>
        /// Initializes dependencies of all scenarios that require the GPC core API to be running.
        /// </summary>
        [BeforeScenario("GpcCoreApi")]
        public void BeforeScenarioGpcCoreApi()
        {
            // Get a reference to the context under which we're running
            IApiContext apiContext = this.container.Resolve<IApiContext>();

            // Ensure that the GPC core API is available, if not inform the user
            // and bail out of the test run.
            var isAvailable = IsWebsiteAvailableAsync(apiContext.GpcCoreApiBaseAddress).Result;
            var message = string.Format(
                    "The GPC core API did not response for URI {0}.  Please ensure that web site is started.",
                    apiContext.GpcCoreApiBaseAddress);
            
            Assert.IsTrue(isAvailable, message);

            // Get a list of all data sources
            var gpcApiClient = this.container.Resolve<GpcApiClient>();
            var response = gpcApiClient.GetDataSources().Result;
            var content = response.Content.ReadAsStringAsync().Result;
            var dataSources = JsonConvert.DeserializeObject<List<DataSource>>(content);

            // Register the list in the container
            this.container.RegisterInstanceAs(dataSources);
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
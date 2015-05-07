﻿// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="BeforeScenarioHooks.cs" company="Rakuten">
//   Copyright (c) Rakuten. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.Web.Http.Tests.Integration
{
    using System;
    using BoDi;
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

            // TODO: Connection string from config
            IApiContext apiContext = new ApiContextFactory(new AppSettingsConfigurationSource()).Create();
            IStorage azureStorage = new AzureStorage();
            var databaseContext = new ProductQueryDbContext();

            container.RegisterInstanceAs(apiContext);
            container.RegisterInstanceAs(databaseContext);
            container.RegisterInstanceAs(azureStorage);

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
    }
}
//------------------------------------------------------------------------------
// <copyright file="DatabaseSteps.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.Web.Http.Tests.Integration
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using Rakuten.Rmsg.ProductQuery.Configuration;
    using TechTalk.SpecFlow;

    // TODO: Connection string from config

    /// <summary>
    /// Steps for working with the database.
    /// </summary>
    [Binding]
    public class DatabaseSteps
    {
        /// <summary>
        /// The context under which this instance is operating.
        /// </summary>
        private readonly IApiContext apiContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseSteps"/> class
        /// </summary>
        /// <param name="apiContext">A context for the API.</param>
        public DatabaseSteps(IApiContext apiContext)
        {
            Contract.Requires(apiContext != null);

            this.apiContext = apiContext;
        }

        /// <summary>
        /// Ensures that a product query group that is not full exists in the database.
        /// The product query group is stored in scenario context.
        /// </summary>
        [Given(@"a sparse product query group exists")]
        public void GivenASparseProductQueryGroupExists()
        {
            using (var databaseContext = new ProductQueryDbContext())
            {
                // Get a query group that is not yet full
                var queryGroup = databaseContext.rmsgProductQueryGroups
                    .Where(q => q.count < this.apiContext.MaximumQueriesPerGroup)
                    .FirstOrDefault();

                // If there are no empty query groups then create a new one
                if (queryGroup == null)
                {
                    queryGroup = databaseContext.rmsgProductQueryGroups.Add(
                        new rmsgProductQueryGroup
                        {
                            rmsgProductQueryGroupID = Guid.NewGuid()
                        });
                }

                // Store the product query group in scenario context
                ScenarioStorage.SparseProductQueryGroup = queryGroup;
            }
        }

        /// <summary>
        /// Retrieves the product query group from the database using the
        /// identifier of the sparse product query group stored in 
        /// scenario context.
        /// </summary>
        [When(@"the sparse product query group is retrieved from the database")]
        public void WhenTheProductQueryGroupIsRetrievedFromTheDatabase()
        {
            using (var databaseContext = new ProductQueryDbContext())
            {
                // Get the details of the new product query from the current scenario context
                var source = ScenarioStorage.SparseProductQueryGroup;

                // Get the product query from the database, construct a new product query 
                // resource and dump it in scenario storage.
                ScenarioStorage.ProductQueryGroupFromDatabase =
                    databaseContext.rmsgProductQueryGroups
                        .Where(q => q.rmsgProductQueryGroupID == source.rmsgProductQueryGroupID)
                        .Single();
            }
        }

        /// <summary>
        /// Retrieves the product query group from the database for the 
        /// new product query stored in scenario context.
        /// </summary>
        [When(@"the new product query group for the new product query is retrieved from the database")]
        public void WhenTheNewProductQueryGroupForTheNewProductQueryIsRetrievedFromTheDatabase()
        {
            using (var databaseContext = new ProductQueryDbContext())
            {
                // Get the details of the new product query from the current scenario context
                var source = ScenarioStorage.ProductQueryFromDatabase;

                // Get the product query from the database, construct a new product query 
                // resource and dump it in scenario storage.
                ScenarioStorage.ProductQueryGroupFromDatabase =
                    databaseContext.rmsgProductQueryGroups
                        .Where(q => q.rmsgProductQueryGroupID == source.rmsgProductQueryGroupID)
                        .Single();
            }
        }

        /// <summary>
        /// Retrieves the product query from the database and stores it in
        /// the current scenario context.
        /// </summary>
        [When(@"the product query is retrieved from the database")]
        [Then(@"the product query can be retrieved from the database")]
        public void WhenTheProductQueryIsRetrievedFromTheDatabase()
        {
            using (var databaseContext = new ProductQueryDbContext())
            {
                // Get the details of the new product query from the current scenario context
                var source = ScenarioStorage.NewProductQuery;

                // Get the product query from the database, construct a new product query 
                // resource and dump it in scenario storage.
                ScenarioStorage.ProductQueryFromDatabase =
                    databaseContext.rmsgProductQueries
                        .Where(q => q.rmsgProductQueryID == source.IdAsGuid && q.culture == source.Culture)
                        .Single();
            }
        }

        /// <summary>
        /// Ensures that there are no empty or sparse product query groups in the database
        /// </summary>
        [Given(@"there are no empty or sparse product query groups in the database")]
        public void GivenThereAreNoEmptyOrSparseProductQueryGroupsInTheDatabase()
        {
            using (var databaseContext = new ProductQueryDbContext())
            {
                // Update the count of all empty or sparse product query groups
                // to ensure they appear full
                foreach (var group in databaseContext.rmsgProductQueryGroups.Where(q => q.count < this.apiContext.MaximumQueriesPerGroup))
                {
                    group.count = (short)this.apiContext.MaximumQueriesPerGroup;
                }

                databaseContext.SaveChanges();
            }
        }
    }
}

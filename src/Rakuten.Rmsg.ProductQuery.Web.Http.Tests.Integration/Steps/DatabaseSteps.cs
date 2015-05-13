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
        /// Ensures that there is only one empty product query group in the database
        /// and stores it in scenario storage.  A new product query group is created if necessary.
        /// </summary>
        [Given(@"only one empty product query group exists")]
        public void GivenOnlyOneEmptyProductQueryGroupExists()
        {
            using (var databaseContext = new ProductQueryDbContext())
            {
                // First, ensure there are no empty product query groups
                foreach (var group in databaseContext.rmsgProductQueryGroups.Where(q => q.count < this.apiContext.MaximumQueriesPerGroup))
                {
                    group.count = (short)this.apiContext.MaximumQueriesPerGroup;
                }

                // Now create an empty product query group
                ScenarioStorage.ProductQueryGroupExpected = databaseContext.rmsgProductQueryGroups.Add(
                    new rmsgProductQueryGroup
                    {
                        rmsgProductQueryGroupID = Guid.NewGuid(),
                        count = 0
                    });

                databaseContext.SaveChanges();
            }
        }

        /// <summary>
        /// Ensures that only one product query group that has members but 
        /// is not full exists in the database.
        /// The product query group is stored in scenario context.
        /// </summary>
        [Given(@"only one sparse product query group exists")]
        public void GivenOnlyOneSparseProductQueryGroupExists()
        {
            using (var databaseContext = new ProductQueryDbContext())
            {
                // First, ensure there are no empty product query groups
                foreach (var group in databaseContext.rmsgProductQueryGroups.Where(q => q.count < this.apiContext.MaximumQueriesPerGroup))
                {
                    group.count = (short)this.apiContext.MaximumQueriesPerGroup;
                }

                // Now create a new sparse product query group
                ScenarioStorage.ProductQueryGroupExpected = databaseContext.rmsgProductQueryGroups.Add(
                    new rmsgProductQueryGroup
                    {
                        rmsgProductQueryGroupID = Guid.NewGuid(),
                        count = 1
                    });

                databaseContext.SaveChanges();
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

        /// <summary>
        /// Retrieves the product query group from the database for the 
        /// new product query stored in scenario context.
        /// </summary>
        [Given(@"the product query group for the new product query has been retrieved from the database")]
        [When(@"the product query group for the new product query is retrieved from the database")]
        public void WhenTheProductQueryGroupForTheNewProductQueryIsRetrievedFromTheDatabase()
        {
            using (var databaseContext = new ProductQueryDbContext())
            {
                // Get the details of the new product query from the current scenario context
                ////var source = ScenarioStorage.ProductQueryFromDatabase;
                var productQueryId = Guid.Parse(ScenarioStorage.NewProductQuery.Id);

                // Get the product query group from the database and dump it in scenario storage.
                ScenarioStorage.ProductQueryGroupActual =
                    databaseContext.rmsgProductQueryGroups
                        .Join(
                            databaseContext.rmsgProductQueries.Where<rmsgProductQuery>(query => query.rmsgProductQueryID == productQueryId),
                            group => group.rmsgProductQueryGroupID,
                            query => query.rmsgProductQueryGroupID,
                            (group, query) => group)
                        .Single();

                ////ScenarioStorage.ProductQueryGroupActual =
                ////    databaseContext.rmsgProductQueryGroups
                ////        .Where(q => q.rmsgProductQueryGroupID == source.rmsgProductQueryGroupID)
                ////        .Single();
            }
        }

        /// <summary>
        /// Retrieves the product query group from the database for the 
        /// product query that is stored in scenario storage and puts it
        /// in scenario storage.
        /// </summary>
        [When(@"the product query group is retrieved from the database")]
        public void WhenTheProductQueryGroupIsRetrievedFromTheDatabase()
        {
            using (var databaseContext = new ProductQueryDbContext())
            {
                // Get the product query group from the database and dump it in scenario storage.
                ScenarioStorage.ProductQueryGroupActual =
                    databaseContext.rmsgProductQueryGroups
                        .Where(q => q.rmsgProductQueryGroupID == ScenarioStorage.ProductQueryGroupExpected.rmsgProductQueryGroupID)
                        .Single();
            }
        }

        /// <summary>
        /// Retrieves the product query from the database and stores it in
        /// the current scenario context.
        /// </summary>
        [Given(@"the product query has been retrieved from the database")]
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
    }
}

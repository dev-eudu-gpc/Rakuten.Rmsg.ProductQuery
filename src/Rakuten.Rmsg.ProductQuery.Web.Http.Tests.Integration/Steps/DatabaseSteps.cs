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
    using System.Threading;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
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
            using (var database = new ProductQueryDbContext())
            {
                // First, ensure there are no empty product query groups
                foreach (var group in database.rmsgProductQueryGroups.Where(q => q.count < this.apiContext.MaximumQueriesPerGroup))
                {
                    group.count = (short)this.apiContext.MaximumQueriesPerGroup;
                }

                // Now create an empty product query group
                ScenarioStorage.ProductQueryGroupExpected = database.rmsgProductQueryGroups.Add(
                    new rmsgProductQueryGroup
                    {
                        rmsgProductQueryGroupID = Guid.NewGuid(),
                        count = 0
                    });

                database.SaveChanges();
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
            using (var database = new ProductQueryDbContext())
            {
                // First, ensure there are no empty product query groups
                foreach (var group in database.rmsgProductQueryGroups.Where(q => q.count < this.apiContext.MaximumQueriesPerGroup))
                {
                    group.count = (short)this.apiContext.MaximumQueriesPerGroup;
                }

                // Now create a new sparse product query group
                ScenarioStorage.ProductQueryGroupExpected = database.rmsgProductQueryGroups.Add(
                    new rmsgProductQueryGroup
                    {
                        rmsgProductQueryGroupID = Guid.NewGuid(),
                        count = 1
                    });

                database.SaveChanges();
            }
        }

        /// <summary>
        /// Ensures that there are no empty or sparse product query groups in the database
        /// </summary>
        [Given(@"there are no empty or sparse product query groups in the database")]
        public void GivenThereAreNoEmptyOrSparseProductQueryGroupsInTheDatabase()
        {
            using (var database = new ProductQueryDbContext())
            {
                // Update the count of all empty or sparse product query groups
                // to ensure they appear full
                foreach (var group in database.rmsgProductQueryGroups.Where(q => q.count < this.apiContext.MaximumQueriesPerGroup))
                {
                    group.count = (short)this.apiContext.MaximumQueriesPerGroup;
                }

                database.SaveChanges();
            }
        }

        /// <summary>
        /// Verifies that the items in the database do not have a GRAN.
        /// </summary>
        [Then(@"the items in the database do not have a GRAN")]
        public void ThenTheItemsInTheDatabaseDoNotHaveAGRAN()
        {
            using (var database = new ProductQueryDbContext())
            {
                var itemsWithGran = database.rmsgProductQueryItems
                    .Where(i =>
                        i.rmsgProductQueryID == ScenarioStorage.NewProductQuery.IdAsGuid &&
                        i.gran != null);

                Assert.IsTrue(itemsWithGran.Count() == 0, "One or more items have a GRAN but should not have.");
            }
        }

        /// <summary>
        /// Verifies that all items for the product query have a completed date
        /// and that the date is newer than the time at which the scenario started.
        /// </summary>
        [Then(@"the items in the database have a valid completed date")]
        public void ThenTheItemsInTheDatabaseHaveAValidCompletedDate()
        {
            using (var database = new ProductQueryDbContext())
            {
                var items = database.rmsgProductQueryItems
                    .Where(item => item.rmsgProductQueryID == ScenarioStorage.NewProductQuery.IdAsGuid);

                foreach (var item in items)
                {
                    Assert.IsNotNull(
                        item.dateCompleted,
                        string.Format("The item with GTIN {0} should have a completed date but hasn't", item.gtin));

                    var message = string.Format(
                            "The item with GTIN {0} has a completed date of {1} which is earlier than the scenario start date of {2}",
                            item.gtin,
                            item.dateCompleted,
                            ScenarioStorage.ScenarioStartTime);

                    Assert.IsTrue(item.dateCompleted > ScenarioStorage.ScenarioStartTime, message);
                }
            }
        }

        /// <summary>
        /// Verifies that all items for the product query have the correct GRAN.
        /// </summary>
        [Then(@"the items in the database have the correct GRAN")]
        public void ThenTheItemsInTheDatabaseHaveTheCorrectGRAN()
        {
            using (var database = new ProductQueryDbContext())
            {
                var databaseItems = database.rmsgProductQueryItems
                    .Where(i => i.rmsgProductQueryID == ScenarioStorage.NewProductQuery.IdAsGuid)
                    .OrderBy(i => i.gtin)
                    .ToDictionary(i => i.gtin, i => i.gran);
  
                var sourceItems = ScenarioStorage.Products
                    .OrderBy(p => p.GetEAN())
                    .ToDictionary(p => p.GetEAN(), p => p.Id);

                CollectionAssert.AreEqual(sourceItems, databaseItems);
            }
        }

        /// <summary>
        /// Verifies that the product query items in the database
        /// match those in the product query file.
        /// </summary>
        [Then(@"the items in the database match the items in the file")]
        public void ThenTheItemsInTheDatabaseMatchTheItemsInTheFile()
        {
            // Get the product query from scenario storage
            var productQuery = ScenarioStorage.NewProductQuery;
            var expectedEans = ScenarioStorage.Items.Select(item => item.GtinValue).ToList();

            // Assert
            using (var database = new ProductQueryDbContext())
            {
                var databaseEans = database.rmsgProductQueryItems
                    .Where(i => i.rmsgProductQueryID == productQuery.IdAsGuid)
                    .Select(i => i.gtin)
                    .ToList();

                CollectionAssert.AreEqual(expectedEans, databaseEans);
            }
        }

        /// <summary>
        /// Verifies that the items in the database match the valid items in the
        /// file and that only those items exist for the product query.
        /// </summary>
        [Then(@"the items in the database match the valid items in the file")]
        public void ThenTheItemsInTheDatabaseMatchTheValidItemsInTheFile()
        {
            // Get the product query from scenario storage
            var productQuery = ScenarioStorage.NewProductQuery;
            var expectedEans = ScenarioStorage.Items
                .Where(item => !string.IsNullOrWhiteSpace(item.GtinValue))
                .Select(item => item.GtinValue).ToList();

            // Assert
            using (var database = new ProductQueryDbContext())
            {
                var databaseEans = database.rmsgProductQueryItems
                    .Where(i => i.rmsgProductQueryID == productQuery.IdAsGuid)
                    .Select(i => i.gtin)
                    .ToList();

                CollectionAssert.AreEqual(expectedEans, databaseEans);
            }
        }

        /// <summary>
        /// Verifies that there are no items for the product query in the database.
        /// </summary>
        [Then(@"there are no items for the product query in the database")]
        public void ThenThereAreNoItemsForTheProductQueryInTheDatabase()
        {
            using (var database = new ProductQueryDbContext())
            {
                var hasItems = database.rmsgProductQueryItems
                        .Any(i => i.rmsgProductQueryID == ScenarioStorage.NewProductQuery.IdAsGuid);

                Assert.IsFalse(hasItems, "There are items for the product query in the database when none were expected.");
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
            using (var database = new ProductQueryDbContext())
            {
                // Get the details of the new product query from the current scenario context
                ////var source = ScenarioStorage.ProductQueryFromDatabase;
                var productQueryId = Guid.Parse(ScenarioStorage.NewProductQuery.Id);

                // Get the product query group from the database and dump it in scenario storage.
                ScenarioStorage.ProductQueryGroupActual =
                    database.rmsgProductQueryGroups
                        .Join(
                            database.rmsgProductQueries.Where<rmsgProductQuery>(query => query.rmsgProductQueryID == productQueryId),
                            group => group.rmsgProductQueryGroupID,
                            query => query.rmsgProductQueryGroupID,
                            (group, query) => group)
                        .Single();
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
            using (var database = new ProductQueryDbContext())
            {
                // Get the product query group from the database and dump it in scenario storage.
                ScenarioStorage.ProductQueryGroupActual =
                    database.rmsgProductQueryGroups
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
            using (var database = new ProductQueryDbContext())
            {
                // Get the details of the new product query from the current scenario context
                var source = ScenarioStorage.NewProductQuery;

                // Get the product query from the database, construct a new product query
                // resource and dump it in scenario storage.
                ScenarioStorage.ProductQueryFromDatabase =
                    database.rmsgProductQueries
                        .Where(q => q.rmsgProductQueryID == source.IdAsGuid && q.culture == source.Culture)
                        .Single();
            }
        }

        /// <summary>
        /// Waits for the product query to move to the specified status.
        /// </summary>
        /// <param name="expectedStatus">The status to wait for.</param>
        [When(@"the status of the product query is (.*)")]
        public void WhenTheStatusOfTheProductQueryIs(string expectedStatus)
        {
            var productQuery = ScenarioStorage.NewProductQuery;
            var isCompleted = false;

            using (var database = new ProductQueryDbContext())
            {
                // Check the status every 5 seconds for a maximum of 12 times
                for (int i = 0; i < 12; i++)
                {
                    isCompleted = database.rmsgProductQueries
                        .Any(q => q.rmsgProductQueryID == productQuery.IdAsGuid
                            && q.rmsgProductQueryStatusID == (int)ProductQueryStatus.Completed);

                    if (isCompleted)
                    {
                        break;
                    }

                    Thread.Sleep(5000);
                }

                Assert.IsTrue(isCompleted);
            }
        }
    }
}
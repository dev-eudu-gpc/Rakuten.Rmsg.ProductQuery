//------------------------------------------------------------------------------
// <copyright file="DatabaseSteps.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.Web.Http.Tests.Integration.Steps
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Threading;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Rakuten.Rmsg.ProductQuery.Configuration;
    using Rakuten.Rmsg.ProductQuery.Web.Http.Tests.Integration.Resources;
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
        /// An object for storing and retrieving information from the scenario context.
        /// </summary>
        private readonly ScenarioStorage scenarioStorage;

        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseSteps"/> class
        /// </summary>
        /// <param name="apiContext">A context for the API.</param>
        /// <param name="scenarioStorage">An object for sharing information between steps.</param>
        public DatabaseSteps(
            IApiContext apiContext,
            ScenarioStorage scenarioStorage)
        {
            Contract.Requires(apiContext != null);
            Contract.Requires(scenarioStorage != null);

            this.apiContext = apiContext;
            this.scenarioStorage = scenarioStorage;
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
                var emptyGroup = database.rmsgProductQueryGroups.Add(
                    new rmsgProductQueryGroup
                    {
                        rmsgProductQueryGroupID = Guid.NewGuid(),
                        count = 0
                    });

                database.SaveChanges();

                // Store pertinent details for subsequent steps
                this.scenarioStorage.Creation.ExpectedGroup = emptyGroup;
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
                var sparseGroup = database.rmsgProductQueryGroups.Add(
                    new rmsgProductQueryGroup
                    {
                        rmsgProductQueryGroupID = Guid.NewGuid(),
                        count = 1
                    });

                database.SaveChanges();

                // Store pertinent details for subsequent steps
                this.scenarioStorage.Creation.ExpectedGroup = sparseGroup;
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
                        i.rmsgProductQueryID == this.scenarioStorage.Creation.SourceProductQuery.IdAsGuid &&
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
                    .Where(item => item.rmsgProductQueryID == this.scenarioStorage.Creation.SourceProductQuery.IdAsGuid);

                foreach (var item in items)
                {
                    Assert.IsNotNull(
                        item.dateCompleted,
                        string.Format("The item with GTIN {0} should have a completed date but hasn't", item.gtin));

                    var message = string.Format(
                            "The item with GTIN {0} has a completed date of {1} which is earlier than the scenario start date of {2}",
                            item.gtin,
                            item.dateCompleted,
                            this.scenarioStorage.StartTime);

                    Assert.IsTrue(item.dateCompleted > this.scenarioStorage.StartTime, message);
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
                    .Where(item => item.rmsgProductQueryID == this.scenarioStorage.Creation.SourceProductQuery.IdAsGuid)
                    .OrderBy(item => item.gtin)
                    .ToDictionary(item => item.gtin, i => i.gran);

                var sourceItems = new Dictionary<string, string>
                {
                    { this.scenarioStorage.Gpc.Product.GetEAN(), this.scenarioStorage.Gpc.Product.Id }
                };

                CollectionAssert.AreEqual(sourceItems, databaseItems);
            }
        }

        /// <summary>
        /// Verifies that the valid items in the product query file
        /// can be found in the database and only those items are in the
        /// database.
        /// </summary>
        [Then(@"the valid items in the file can be found in the database")]
        public void ThenTheItemsInTheDatabaseMatchTheValidItemsInTheFile()
        {
            // Arrange
            var productQuery = this.scenarioStorage.Creation.SourceProductQuery;
            var expectedGtins = this.scenarioStorage.Files.SourceItems
                .Where(item => !string.IsNullOrWhiteSpace(item.GtinValue))
                .Select(item => item.GtinValue).ToList();

            // Assert
            using (var database = new ProductQueryDbContext())
            {
                var databaseGtins = database.rmsgProductQueryItems
                    .Where(i => i.rmsgProductQueryID == productQuery.IdAsGuid)
                    .Select(i => i.gtin)
                    .ToList();

                CollectionAssert.AreEqual(expectedGtins, databaseGtins);
            }
        }

        /// <summary>
        /// Verifies that the items in the product query file can be found in the database.
        /// </summary>
        [Then(@"the items in the file can be found in the database")]
        public void ThenTheItemsInTheFileCanBeFoundInTheDatabase()
        {
            // Arrange
            var productQuery = this.scenarioStorage.Creation.SourceProductQuery;
            var expectedGtins = this.scenarioStorage.Files.SourceItems.Select(item => item.GtinValue).ToList();

            // Assert
            using (var database = new ProductQueryDbContext())
            {
                var actualGtins = database.rmsgProductQueryItems
                    .Where(i => i.rmsgProductQueryID == productQuery.IdAsGuid)
                    .Select(i => i.gtin)
                    .ToList();

                CollectionAssert.AreEqual(expectedGtins, actualGtins);
            }
        }

        /// <summary>
        /// Verifies that the product query in the response body
        /// has the same enclosure link as that found in the database.
        /// </summary>
        [Then(@"the product query in the response body has the correct enclosure link")]
        public void ThenTheProductQueryInTheResponseBodyHasTheCorrectEnclosureLink()
        {
            using (var database = new ProductQueryDbContext())
            {
                // Arrange
                var productQuery = this.scenarioStorage.Creation.ResponseProductQuery;
                var databaseEntity = database.rmsgProductQueries
                    .Single(q => q.rmsgProductQueryID == this.scenarioStorage.Creation.SourceProductQuery.IdAsGuid);

                // Assert
                Assert.IsNotNull(productQuery.Links.Enclosure);
                Assert.AreEqual(databaseEntity.uri, productQuery.Links.Enclosure.Href, true);
            }
        }

        /// <summary>
        /// Verifies that the product query in the response has the correct monitor link.
        /// </summary>
        [Then(@"the product query in the response body has the correct monitor link")]
        public void ThenTheProductQueryInTheResponseBodyHasTheCorrectMonitorLink()
        {
            using (var database = new ProductQueryDbContext())
            {
                // Arrange
                var productQuery = this.scenarioStorage.Creation.ResponseProductQuery;
                var group = database.rmsgProductQueries
                    .Single(q => q.rmsgProductQueryID == this.scenarioStorage.Creation.SourceProductQuery.IdAsGuid)
                    .rmsgProductQueryGroup;

                var expectedUri = string.Format(
                    "/product-query-group/{0}/status/{{year}}/{{month}}/{{day}}/{{hour}}/{{minute}}",
                    group.rmsgProductQueryGroupID);

                // Assert
                Assert.IsNotNull(productQuery.Links.Monitor);
                Assert.AreEqual(expectedUri, productQuery.Links.Monitor.Href);
            }
        }

        /// <summary>
        /// Ensures that the product query in the response body has the
        /// same date created as that found in the database.
        /// </summary>
        [Then(@"the product query in the response body has the same created date as that in the database")]
        public void ThenTheProductQueryInTheResponseBodyHasTheSameCreatedDateAsThatInTheDatabase()
        {
            using (var database = new ProductQueryDbContext())
            {
                // Arrange
                ProductQuery productQuery = this.scenarioStorage.Creation.ResponseProductQuery;
                var databaseEntity = database.rmsgProductQueries
                    .Single(q => q.rmsgProductQueryID == this.scenarioStorage.Creation.SourceProductQuery.IdAsGuid);

                // Assert
                Assert.AreEqual(databaseEntity.dateCreated.Year, productQuery.Year);
                Assert.AreEqual(databaseEntity.dateCreated.Month, productQuery.Month);
                Assert.AreEqual(databaseEntity.dateCreated.Day, productQuery.Day);
                Assert.AreEqual(databaseEntity.dateCreated.Hour, productQuery.Hour);
                Assert.AreEqual(databaseEntity.dateCreated.Minute, productQuery.Minute);
            }
        }

        /// <summary>
        /// Verifies that the status of the product query in the response is the
        /// same as the status of the product query in the database
        /// </summary>
        [Then(@"the product query in the response body has the same status as that in the database")]
        public void ThenTheProductQueryInTheResponseBodyHasTheSameStatusAsThatInTheDatabase()
        {
            //// TODO: [WB 26-Jun-2015] Really ought to be able to get the id from the response rather than the source.

            using (var database = new ProductQueryDbContext())
            {
                // Arrange
                var productQuery = this.scenarioStorage.Creation.ResponseProductQuery;
                var databaseEntity = database.rmsgProductQueries
                    .Single(q => q.rmsgProductQueryID == this.scenarioStorage.Creation.SourceProductQuery.IdAsGuid);

                ProductQueryStatus parsedStatus;
                Enum.TryParse<ProductQueryStatus>(productQuery.Status, out parsedStatus);

                Assert.IsNotNull(parsedStatus, string.Format("The status in the response is '{0}' which is not a valid status.", productQuery.Status));
                Assert.AreEqual((ProductQueryStatus)databaseEntity.rmsgProductQueryStatusID, parsedStatus);
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
                        .Any(i => i.rmsgProductQueryID == this.scenarioStorage.Creation.SourceProductQuery.IdAsGuid);

                Assert.IsFalse(hasItems, "There are items for the product query in the database when none were expected.");
            }
        }

        /// <summary>
        /// Waits for the product query to move to the specified status.
        /// </summary>
        /// <param name="expectedStatus">The status to wait for.</param>
        [When(@"the status of the product query becomes (.*)")]
        public void WhenTheStatusOfTheProductQueryBecomes(string expectedStatus)
        {
            var productQuery = this.scenarioStorage.Creation.SourceProductQuery;
            var isCompleted = false;

            using (var database = new ProductQueryDbContext())
            {
                // Check the status every 5 seconds for a maximum of 12 times
                for (int i = 0; i < 12; i++)
                {
                    isCompleted = database.rmsgProductQueries
                        .Any(q => 
                            q.rmsgProductQueryID == productQuery.IdAsGuid &&
                            q.rmsgProductQueryStatusID == (int)ProductQueryStatus.Completed);

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
//------------------------------------------------------------------------------
// <copyright file="ProductQueryGroupSteps.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.Web.Http.Tests.Integration.Steps
{
    using System.Diagnostics.Contracts;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Rakuten.Rmsg.ProductQuery.Configuration;
    using TechTalk.SpecFlow;

    /// <summary>
    /// Steps for working with product query groups.
    /// </summary>
    [Binding]
    public class ProductQueryGroupSteps
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
        /// Initializes a new instance of the <see cref="ProductQueryGroupSteps"/> class
        /// </summary>
        /// <param name="apiContext">A context for the API.</param>
        /// <param name="scenarioStorage">An object for sharing information between steps.</param>
        public ProductQueryGroupSteps(
            IApiContext apiContext,
            ScenarioStorage scenarioStorage)
        {
            Contract.Requires(apiContext != null);
            Contract.Requires(scenarioStorage != null);

            this.apiContext = apiContext;
            this.scenarioStorage = scenarioStorage;
        }

        /// <summary>
        /// Verifies that the count property of the product query group that was
        /// retrieved from the database matches the expected count.
        /// </summary>
        /// <param name="expectedCount">The expected value for the count property.</param>
        [Then(@"the count of product queries in the new product query group is (.*)")]
        public void ThenTheCountOfProductQueriesInTheNewProductQueryGroupIs(int expectedCount)
        {
            using (var database = new ProductQueryDbContext())
            {
                var group = database.rmsgProductQueries
                    .Single(q => q.rmsgProductQueryID == this.scenarioStorage.Creation.SourceProductQuery.IdAsGuid)
                    .rmsgProductQueryGroup;

                Assert.AreEqual(expectedCount, group.count);
            }
        }

        /// <summary>
        /// Verifies that the count property of the product query group that was
        /// retrieved from the database is 1 greater than the count property of the
        /// product query group in scenario context.
        /// </summary>
        [Then(@"the count of product queries in the product query group has been incremented by 1")]
        public void ThenTheCountOfProductQueriesInTheProductQueryGroupHasBeenIncrementedBy1()
        {
            using (var database = new ProductQueryDbContext())
            {
                var expectedGroup = this.scenarioStorage.Creation.ExpectedGroup;
                var actualGroup = database.rmsgProductQueries
                    .Single(q => q.rmsgProductQueryID == this.scenarioStorage.Creation.SourceProductQuery.IdAsGuid)
                    .rmsgProductQueryGroup;

                Assert.AreEqual(expectedGroup.count + 1, actualGroup.count);
            }
        }

        /// <summary>
        /// Verifies that the index property of the product query that was
        /// retrieved from the database matches the expected index.
        /// </summary>
        /// <param name="expectedIndex">The expected value for the count property.</param>
        [Then(@"the index of the product query from the database is (.*)")]
        public void ThenTheIndexOfTheProductQueryFromTheDatabaseIs(int expectedIndex)
        {
            using (var database = new ProductQueryDbContext())
            {
                var databaseEntity = database.rmsgProductQueries
                    .Single(q => q.rmsgProductQueryID == this.scenarioStorage.Creation.SourceProductQuery.IdAsGuid);

                Assert.AreEqual(expectedIndex, databaseEntity.index);
            }
        }

        /// <summary>
        /// Verifies that the index of the new product query matches the count of the
        /// product query group retrieved from the database.
        /// </summary>
        [Then(@"the index of the product query from the database matches the incremented count of the product query group")]
        public void ThenTheIndexOfTheProductQueryFromTheDatabaseMatchesTheIncrementedCountOfTheProductQueryGroup()
        {
            using (var database = new ProductQueryDbContext())
            {
                var databaseEntity = database.rmsgProductQueries
                    .Single(q => q.rmsgProductQueryID == this.scenarioStorage.Creation.SourceProductQuery.IdAsGuid);

                Assert.AreEqual(databaseEntity.rmsgProductQueryGroup.count, databaseEntity.index);
            }
        }

        /// <summary>
        /// Verifies that the product query group assigned to the product query in 
        /// scenario storage is the expected group.
        /// </summary>
        [Then(@"the product query group assigned to the new product query is the correct one")]
        public void ThenTheProductQueryGroupAssignedToTheNewProductQueryIsTheCorrectOne()
        {
            using (var database = new ProductQueryDbContext())
            {
                var databaseEntity = database.rmsgProductQueries
                    .Single(q => q.rmsgProductQueryID == this.scenarioStorage.Creation.SourceProductQuery.IdAsGuid);

                Assert.AreEqual(
                    this.scenarioStorage.Creation.ExpectedGroup.rmsgProductQueryGroupID,
                    databaseEntity.rmsgProductQueryGroupID);
            }
        }
    }
}

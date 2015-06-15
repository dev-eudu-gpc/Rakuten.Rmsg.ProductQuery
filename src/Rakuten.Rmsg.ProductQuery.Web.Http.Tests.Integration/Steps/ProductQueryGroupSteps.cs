//------------------------------------------------------------------------------
// <copyright file="ProductQueryGroupSteps.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.Web.Http.Tests.Integration
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using TechTalk.SpecFlow;

    /// <summary>
    /// Steps for working with product query groups.
    /// </summary>
    [Binding]
    public class ProductQueryGroupSteps
    {
        /// <summary>
        /// Verifies that the count property of the product query group that was
        /// retrieved from the database matches the expected count.
        /// </summary>
        /// <param name="expectedCount">The expected value for the count property.</param>
        [Then(@"the count of product queries in the new product query group is (.*)")]
        public void ThenTheCountOfProductQueriesInTheNewProductQueryGroupIs(int expectedCount)
        {
            Assert.AreEqual(expectedCount, ScenarioStorage.ProductQueryGroupActual.count);
        }

        /// <summary>
        /// Verifies that the count property of the product query group that was
        /// retrieved from the database is 1 greater than the count property of the
        /// product query group in scenario context.
        /// </summary>
        [Then(@"the count of product queries in the product query group has been incremented by 1")]
        public void ThenTheCountOfProductQueriesInTheProductQueryGroupHasBeenIncrementedBy1()
        {
            Assert.AreEqual(
                ScenarioStorage.ProductQueryGroupExpected.count + 1,
                ScenarioStorage.ProductQueryGroupActual.count);
        }

        /// <summary>
        /// Verifies that the index property of the product query that was
        /// retrieved from the database matches the expected index.
        /// </summary>
        /// <param name="expectedIndex">The expected value for the count property.</param>
        [Then(@"the index of the product query from the database is (.*)")]
        public void ThenTheIndexOfTheProductQueryFromTheDatabaseIs(int expectedIndex)
        {
            Assert.AreEqual(expectedIndex, ScenarioStorage.ProductQueryFromDatabase.index);
        }

        /// <summary>
        /// Verifies that the index of the new product query matches the count of the
        /// product query group retrieved from the database.
        /// </summary>
        [Then(@"the index of the product query from the database matches the incremented count of the product query group")]
        public void ThenTheIndexOfTheProductQueryFromTheDatabaseMatchesTheIncrementedCountOfTheProductQueryGroup()
        {
            Assert.AreEqual(
                ScenarioStorage.ProductQueryGroupActual.count,
                ScenarioStorage.ProductQueryFromDatabase.index);
        }

        /// <summary>
        /// Verifies that the product query group assigned to the product query in 
        /// scenario storage is the expected group.
        /// </summary>
        [Then(@"the product query group assigned to the new product query is the correct one")]
        public void ThenTheProductQueryGroupAssignedToTheNewProductQueryIsTheCorrectOne()
        {
            // Assert
            Assert.AreEqual(
                ScenarioStorage.ProductQueryGroupExpected.rmsgProductQueryGroupID,
                ScenarioStorage.ProductQueryFromDatabase.rmsgProductQueryGroupID);
        }
    }
}

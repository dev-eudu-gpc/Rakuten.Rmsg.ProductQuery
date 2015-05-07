//------------------------------------------------------------------------------
// <copyright file="ProductQueryGroupSteps.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.Web.Http.Tests.Integration.Steps
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using TechTalk.SpecFlow;

    /// <summary>
    /// Steps for working with product query groups.
    /// </summary>
    [Binding]
    public class ProductQueryGroupSteps
    {
        /// <summary>
        /// Verifies that the identifier of the product query group that was retrieved
        /// from the database matches the identifier of the sparse product query group
        /// in scenario context.
        /// </summary>
        [Then(@"the product query group identifier of the product query from the database matches the identifier of the sparse product query group")]
        public void ThenTheProductQueryGroupIdentifierOfTheProductQueryFromTheDatabaseMatchesTheIdentifierOfTheSparseProductQueryGroup()
        {
            Assert.AreEqual(
                ScenarioStorage.SparseProductQueryGroup.rmsgProductQueryGroupID,
                ScenarioStorage.ProductQueryGroupFromDatabase.rmsgProductQueryGroupID);
        }

        /// <summary>
        /// Verifies that the count property of the product query group that was
        /// retrieved from the database is 1 greater than the count property of the
        /// sparse product query group in scenario context.
        /// </summary>
        [Then(@"the count of product queries in the sparse product query group has been incremented by 1")]
        public void ThenTheCountOfProductQueriesInTheSparseProductQueryGroupHasBeenIncrementedBy1()
        {
            Assert.AreEqual(
                ScenarioStorage.SparseProductQueryGroup.count + 1,
                ScenarioStorage.ProductQueryGroupFromDatabase.count);
        }

        /// <summary>
        /// Verifies that the index of the new product query matches the count of the
        /// product query group retrieved from the database.
        /// </summary>
        [Then(@"the index of the product query from the database matches the incremented count of the sparse product query group")]
        public void ThenTheIndexOfTheProductQueryFromTheDatabaseMatchesTheIncrementedCountOfTheSparseProductQueryGroup()
        {
            Assert.AreEqual(
                ScenarioStorage.ProductQueryGroupFromDatabase.count,
                ScenarioStorage.ProductQueryFromDatabase.index);
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
        /// Verifies that the count property of the product query group that was
        /// retrieved from the database matches the expected count.
        /// </summary>
        /// <param name="expectedCount">The expected value for the count property.</param>
        [Then(@"the count of product queries in the new product query group is (.*)")]
        public void ThenTheCountOfProductQueriesInTheNewProductQueryGroupIs(int expectedCount)
        {
            Assert.AreEqual(expectedCount, ScenarioStorage.ProductQueryGroupFromDatabase.count);
        }
    }
}

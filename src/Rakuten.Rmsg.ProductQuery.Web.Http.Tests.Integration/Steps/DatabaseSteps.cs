//------------------------------------------------------------------------------
// <copyright file="DatabaseSteps.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.Web.Http.Tests.Integration
{
    using System.Diagnostics.Contracts;
    using System.Linq;
    using Rakuten.Rmsg.ProductQuery.Configuration;
    using TechTalk.SpecFlow;

    /// <summary>
    /// Steps for working with the database.
    /// </summary>
    [Binding]
    public class DatabaseSteps
    {
        /// <summary>
        /// The context under which this instance is operating.
        /// </summary>
        private readonly IApiContext context;

        /// <summary>
        /// The context in which to perform database operations.
        /// </summary>
        private readonly ProductQueryDbContext databaseContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseSteps"/> class
        /// </summary>
        /// <param name="apiContext">A context for the API.</param>
        /// <param name="databaseContext">A context in which to perform database operations.</param>
        public DatabaseSteps(
            IApiContext apiContext,
            ProductQueryDbContext databaseContext)
        {
            Contract.Requires(apiContext != null);
            Contract.Requires(databaseContext != null);

            this.context = apiContext;
            this.databaseContext = databaseContext;
        }

        /// <summary>
        /// Retrieves the product query from the database and stores it in
        /// the current scenario context.
        /// </summary>
        [When(@"the product query is retrieved from the database")]
        [Then(@"the product query can be retrieved from the database")]
        public void WhenTheProductQueryIsRetrievedFromTheDatabase()
        {
            // Get the details of the new product query from the current scenario context
            var source = ScenarioStorage.NewProductQuery;

            // Get the product query from the database, construct a new product query 
            // resource and dump it in scenario storage.
            ScenarioStorage.ProductQueryFromDatabase = 
                this.databaseContext.rmsgProductQueries
                    .Where(q => q.rmsgProductQueryID == source.IdAsGuid && q.culture == source.Culture)
                    .Single();
        }
    }
}

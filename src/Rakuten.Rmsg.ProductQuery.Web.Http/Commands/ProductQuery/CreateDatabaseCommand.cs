//------------------------------------------------------------------------------
// <copyright file="CreateDatabaseCommand.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.Web.Http.Commands
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Threading.Tasks;

    using Rakuten.Rmsg.ProductQuery.Configuration;
    using Rakuten.Rmsg.ProductQuery.Web.Http.EntityModels;

    /// <summary>
    /// Represents a command for inserting a new product query into the database.
    /// </summary>
    public class CreateDatabaseCommand : AsyncCommandAction<CreateDatabaseCommandParameters>
    {
        /// <summary>
        /// The context under which this instance is operating.
        /// </summary>
        private readonly IApiContext apiContext;

        /// <summary>
        /// The context in which to perform database operations.
        /// </summary>
        private readonly ProductQueryDbContext databaseContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateDatabaseCommand"/> class
        /// </summary>
        /// <param name="apiContext">The context in which this instance is running.</param>
        /// <param name="databaseContext">A context in which to perform database operations.</param>
        public CreateDatabaseCommand(
            IApiContext apiContext,
            ProductQueryDbContext databaseContext)
        {
            Contract.Requires(apiContext != null);
            Contract.Requires(databaseContext != null);

            this.apiContext = apiContext;
            this.databaseContext = databaseContext;
        }

        /// <summary>
        /// Creates the necessary entries in the database for a new product query
        /// </summary>
        /// <param name="parameters">The necessary parameters to enter the product query into the database</param>
        /// <returns>A task that does the work.</returns>
        public override Task ExecuteAsync(CreateDatabaseCommandParameters parameters)
        {
            Contract.Requires(parameters != null);

            return Task.Run(() =>
            {
                // Get a query group that is not yet full
                var queryGroup = this.databaseContext.rmsgProductQueryGroups
                    .Where(q => q.count < this.apiContext.MaximumQueriesPerGroup)
                    .FirstOrDefault();

                // If there are no empty query groups then create a new one
                if (queryGroup == null)
                {
                    queryGroup = this.databaseContext.rmsgProductQueryGroups.Add(
                        new rmsgProductQueryGroup
                        { 
                            rmsgProductQueryGroupID = Guid.NewGuid()
                        });
                }

                // Create the query
                // TODO: Take a different approach to the status id
                this.databaseContext.rmsgProductQueries.Add(
                    new rmsgProductQuery
                    {
                        dateCreated = parameters.DateCreated,
                        rmsgProductQueryID = parameters.Id,
                        rmsgProductQueryGroupID = queryGroup.rmsgProductQueryGroupID,
                        index = ++queryGroup.count,
                        rmsgProductQueryStatusID = this.databaseContext.rmsgProductQueryStatus.First(s => s.name == "new").rmsgProductQueryStatusID
                    });

                // Submit the changes to the database
                this.databaseContext.SaveChanges();
            });
        }
    }
}
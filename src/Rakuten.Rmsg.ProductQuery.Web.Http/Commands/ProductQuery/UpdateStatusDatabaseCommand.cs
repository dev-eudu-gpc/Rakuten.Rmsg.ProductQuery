//------------------------------------------------------------------------------
// <copyright file="UpdateStatusDatabaseCommand.cs" company="Rakuten">
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
    /// Represents a command for updating the status of a product query in the database.
    /// </summary>
    public class UpdateStatusDatabaseCommand : AsyncCommandAction<UpdateStatusDatabaseCommandParameters>
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
        /// Initializes a new instance of the <see cref="UpdateStatusDatabaseCommand"/> class
        /// </summary>
        /// <param name="apiContext">The context in which this instance is running.</param>
        /// <param name="databaseContext">A context in which to perform database operations.</param>
        public UpdateStatusDatabaseCommand(
            IApiContext apiContext,
            ProductQueryDbContext databaseContext)
        {
            Contract.Requires(apiContext != null);
            Contract.Requires(databaseContext != null);

            this.apiContext = apiContext;
            this.databaseContext = databaseContext;
        }

        /// <summary>
        /// Updates the URI for a specified product query.
        /// </summary>
        /// <param name="parameters">The necessary parameters to identify and update the product query in the database</param>
        /// <returns>A task that does the work.</returns>
        public override Task ExecuteAsync(UpdateStatusDatabaseCommandParameters parameters)
        {
            Contract.Requires(parameters != null);

            return Task.Run(() =>
            {
                // Update the product query's Uri column
                var query = this.databaseContext.rmsgProductQueries
                    .Where(q => q.rmsgProductQueryID == parameters.Id)
                    .FirstOrDefault();

                if (query != null)
                {
                    // TODO: [WB 15-Apr-2015] Probably a better way of doing this
                    query.rmsgProductQueryStatusID = this.databaseContext.rmsgProductQueryStatus
                        .Where(s => s.name.Equals(parameters.NewStatus, StringComparison.InvariantCultureIgnoreCase))
                        .FirstOrDefault()
                        .rmsgProductQueryStatusID;
                }
                else
                {
                    // TODO: [WB 15-Apr-2015] Implement sad path
                }

                // Submit the changes to the database
                this.databaseContext.SaveChanges();
            });
        }
    }
}
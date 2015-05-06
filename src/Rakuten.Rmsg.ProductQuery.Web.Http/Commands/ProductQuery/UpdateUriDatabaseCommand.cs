﻿//------------------------------------------------------------------------------
// <copyright file="UpdateUriDatabaseCommand.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.Web.Http.Commands
{
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Threading.Tasks;

    using Rakuten.Rmsg.ProductQuery.Configuration;
    using Rakuten.Rmsg.ProductQuery.Web.Http.EntityModels;

    /// <summary>
    /// Represents a command for updating the blob URI for a product query in the database.
    /// </summary>
    internal class UpdateUriDatabaseCommand : AsyncCommandAction<UpdateUriDatabaseCommandParameters>
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
        /// Initializes a new instance of the <see cref="UpdateUriDatabaseCommand"/> class
        /// </summary>
        /// <param name="apiContext">The context in which this instance is running.</param>
        /// <param name="databaseContext">A context in which to perform database operations.</param>
        public UpdateUriDatabaseCommand(
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
        public override Task ExecuteAsync(UpdateUriDatabaseCommandParameters parameters)
        {
            return Task.Run(() =>
            {
                // Update the product query's Uri column
                var query = this.databaseContext.rmsgProductQueries
                    .Where(q => q.rmsgProductQueryID == parameters.Id)
                    .FirstOrDefault();

                if (query != null)
                {
                    query.uri = parameters.Uri.ToString();
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
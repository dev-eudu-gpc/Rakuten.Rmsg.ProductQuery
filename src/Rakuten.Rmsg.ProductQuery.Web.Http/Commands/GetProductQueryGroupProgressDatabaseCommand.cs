//------------------------------------------------------------------------------
// <copyright file="GetProductQueryGroupProgressDatabaseCommand.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.Web.Http.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Threading.Tasks;
    using Rakuten.Rmsg.ProductQuery.Configuration;
    using Rakuten.Rmsg.ProductQuery.Web.Http.EntityModels;

    /// <summary>
    /// A command for obtaining the progress of all product queries in
    /// a given product query group.
    /// </summary>
    public class GetProductQueryGroupProgressDatabaseCommand : AsyncCommand<GetProductQueryGroupProgressDatabaseCommandParameters, IQueryable<ProductQueryProgress>>
    {
        /// <summary>
        /// The context in which to perform database operations.
        /// </summary>
        private readonly ProductQueryDbContext databaseContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetProductQueryGroupProgressDatabaseCommand"/> class
        /// </summary>
        /// <param name="databaseContext">A context in which to perform database operations.</param>
        public GetProductQueryGroupProgressDatabaseCommand(
            ProductQueryDbContext databaseContext)
        {
            Contract.Requires(databaseContext != null);

            this.databaseContext = databaseContext;
        }

        /// <summary>
        /// Constructs a progress map according to the input parameters.
        /// </summary>
        /// <param name="parameters">The parameters required to build the progress map.</param>
        /// <returns>A collection of progress objects for each product query in the group.</returns>
        public override Task<IQueryable<ProductQueryProgress>> ExecuteAsync(GetProductQueryGroupProgressDatabaseCommandParameters parameters)
        {
            Contract.Requires(parameters != null);

            return Task.Run(() =>
            {
                // Get the statistics for all product queries in the group
                var progressMap = this.databaseContext.rmsgProductQueries
                    .Where(query => query.rmsgProductQueryGroupID == parameters.Id)
                    .GroupJoin(
                        this.databaseContext.rmsgProductQueryItems,
                        query => query.rmsgProductQueryID,
                        item => item.rmsgProductQueryID,
                        (query, items) => new ProductQueryProgress
                        {
                            Index = query.index,
                            Status = (ProductQueryStatus)query.rmsgProductQueryStatusID,
                            ItemCount = items.Count(),
                            CompletedItemCount = items.Count(item => item.dateCompleted <= parameters.Datetime)
                        });

                 return progressMap;
            });
        }
    }
}
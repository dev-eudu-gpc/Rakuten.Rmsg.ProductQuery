//------------------------------------------------------------------------------
// <copyright file="GetProgressDatabaseCommand.cs" company="Rakuten">
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
    /// A command for obtaining the progress of all product queries in
    /// a given product query group.
    /// </summary>
    internal class GetProgressDatabaseCommand : AsyncCommand<GetProgressDatabaseCommandParameters, IQueryable<ProductQueryProgress>>
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
        /// Initializes a new instance of the <see cref="GetProgressDatabaseCommand"/> class
        /// </summary>
        /// <param name="context">The context in which this instance is running.</param>
        /// <param name="databaseContext">A context in which to perform database operations.</param>
        public GetProgressDatabaseCommand(
            IApiContext context,
            ProductQueryDbContext databaseContext)
        {
            Contract.Requires(context != null);
            Contract.Requires(databaseContext != null);

            this.context = context;
            this.databaseContext = databaseContext;
        }

        /// <summary>
        /// Constructs a progress map according to the input parameters.
        /// </summary>
        /// <param name="parameters">The parameters required to build the progress map.</param>
        /// <returns>A collection of progress objects for each product query in the group.</returns>
        public override Task<IQueryable<ProductQueryProgress>> ExecuteAsync(GetProgressDatabaseCommandParameters parameters)
        {
            return Task.Run(() =>
            {
                // Ensure the product query group exists
                if (!this.databaseContext
                        .rmsgProductQueryGroups
                        .Any<rmsgProductQueryGroup>(q => q.rmsgProductQueryGroupID == parameters.Id))
                {
                    throw new ProductQueryGroupNotFoundException(parameters.Id);
                }

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
                            CompletedItemCount = items.Count(item => item.dateCompleted <= parameters.Datetime),
                            ProportionOfTimeAllocatedForFinalization = this.context.ProportionOfTimeAllocatedForFinalization
                        });

                 return progressMap;
            });
        }
    }
}
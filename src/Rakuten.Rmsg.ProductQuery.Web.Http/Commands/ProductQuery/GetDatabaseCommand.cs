//------------------------------------------------------------------------------
// <copyright file="GetDatabaseCommand.cs" company="Rakuten">
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
    /// Represents a command for obtaining a product query from the database.
    /// </summary>
    public class GetDatabaseCommand : AsyncCommand<GetDatabaseCommandParameters, ProductQuery>
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
        /// Initializes a new instance of the <see cref="GetDatabaseCommand"/> class
        /// </summary>
        /// <param name="apiContext">The context in which this instance is running.</param>
        /// <param name="databaseContext">A context in which to perform database operations.</param>
        public GetDatabaseCommand(
            IApiContext apiContext,
            ProductQueryDbContext databaseContext)
        {
            Contract.Requires(apiContext != null);
            Contract.Requires(databaseContext != null);

            this.apiContext = apiContext;
            this.databaseContext = databaseContext;
        }

        /// <summary>
        /// Gets a specified product query from the database
        /// </summary>
        /// <param name="parameters">The necessary parameters to uniquely identify the product query.</param>
        /// <returns>A task that does the work.</returns>
        public override Task<ProductQuery> ExecuteAsync(GetDatabaseCommandParameters parameters)
        {
            Contract.Requires(parameters != null);

            return Task.Run(() =>
            {
                // Determine if the product query already exists
                var query = this.databaseContext.rmsgProductQueries
                    .Where(q => q.rmsgProductQueryID == parameters.Id)
                    .FirstOrDefault();

                return query != null ?
                    new ProductQuery
                    {
                        DateCreated = query.dateCreated,
                        Index = query.index,
                        GroupId = query.rmsgProductQueryGroupID,
                        Status = (ProductQueryStatus)query.rmsgProductQueryStatusID,
                        Uri  = query.uri
                    }
                    : null;
            });
        }
    }
}
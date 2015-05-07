// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="GetDataSourcesCommand.cs" company="Rakuten">
//   Copyright (c) Rakuten. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.WebJob
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Threading.Tasks;

    using Rakuten.Net.Http;
    using Rakuten.Rmsg.ProductQuery.WebJob.Api;
    using Rakuten.Rmsg.ProductQuery.WebJob.Linking;

    /// <summary>
    /// Retrieves a collection of <see cref="DataSource"/>s.
    /// </summary>
    internal class GetDataSourcesCommand
    {
        /// <summary>
        /// Fetches a collection of <see cref="DataSource"/>s.
        /// </summary>
        /// <param name="link">
        /// A <see cref="LinkTemplate"/> that specifies the location to retrieve a collection of data sources.
        /// </param>
        /// <param name="getClient">
        /// A <see cref="Func{TResult}"/> that will generate a <see cref="ApiClient"/> instance.
        /// </param>
        /// <returns>A <see cref="Task"/> the represents the asynchronous operation.</returns>
        public static async Task<IEnumerable<DataSource>> Execute(
            DataSourcesLink link, 
            Func<ApiClient> getClient)
        {
            Contract.Requires(link != null);

            var client = getClient();
            Contract.Assume(client != null);

            var uri = link.ToUri();

            return await client.GetAsync<IEnumerable<DataSource>>(uri);
        }
    }
}
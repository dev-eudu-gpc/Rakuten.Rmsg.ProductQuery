//------------------------------------------------------------------------------
// <copyright file="DataSourceExtensions.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.Web.Http.Tests.Integration
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Rakuten.Rmsg.ProductQuery.Web.Http.Tests.Integration.Resources;

    /// <summary>
    /// Extension methods for the <see cref="DataSource"/> class.
    /// </summary>
    public static class DataSourceExtensions
    {
        /// <summary>
        /// Returns the data source with the highest trust score for the specified culture.
        /// </summary>
        /// <param name="dataSources">The list of data sources being operated upon.</param>
        /// <param name="culture">The culture.</param>
        /// <returns>The data source with the highest trust score.</returns>
        public static DataSource GetHighestTrustScore(this List<DataSource> dataSources, string culture)
        {
            return dataSources
                .Where(ds => ds.CultureCode.Equals(culture, StringComparison.InvariantCultureIgnoreCase))
                .OrderByDescending(ds => ds.TrustScore)
                .First();
        }

        /// <summary>
        /// Returns the data source with the lowest trust score for the specified culture.
        /// </summary>
        /// <param name="dataSources">The list of data sources being operated upon.</param>
        /// <param name="culture">The culture.</param>
        /// <returns>The data source with the highest trust score.</returns>
        public static DataSource GetLowestTrustScore(this List<DataSource> dataSources, string culture)
        {
            return dataSources
                .Where(ds => ds.CultureCode.Equals(culture, StringComparison.InvariantCultureIgnoreCase))
                .OrderBy(ds => ds.TrustScore)
                .First();
        }
    }
}

// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="ProductQueryContext.cs" company="Rakuten">
//   Copyright (c) Rakuten. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.WebJob
{
    using System.Data.Entity;

    /// <summary>
    /// The product query context.
    /// </summary>
    public class ProductQueryContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProductQueryContext"/> class.
        /// </summary>
        public ProductQueryContext()
            : base("Rakuten.Rmsg.ProductQuery.SqlServer")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductQueryContext"/> class using the specified 
        /// connection string.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public ProductQueryContext(string connectionString)
            : base(connectionString)
        {
        }

        /// <summary>
        /// Gets or sets the groups containing the received product queries.
        /// </summary>
        public DbSet<ProductQueryGroup> ProductQueryGroups { get; set; }

        /// <summary>
        /// Gets or sets the collection of individual product identifiers whose information has been requested.
        /// </summary>
        public DbSet<ProductQueryItem> ProductQueryItems { get; set; }

        /// <summary>
        /// Gets or sets the collection of received product queries.
        /// </summary>
        public DbSet<ProductQuery> ProductQueries { get; set; }

        /// <summary>
        /// Gets or sets the collection of status that define a queries progress.
        /// </summary>
        public DbSet<ProductQueryStatus> ProductQueryStatus { get; set; }
    }
}
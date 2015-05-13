// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="ProductQueryItem.cs" company="Rakuten">
//   Copyright (c) Rakuten. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.WebJob
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// Represents a request for product information for a specific GTIN.
    /// </summary>
    [Table("rmsgProductQueryItem")]
    public class ProductQueryItem
    {
        /// <summary>
        /// Gets or sets the point in time at which a matching product was located.
        /// </summary>
        [Column("dateCompleted")]
        public DateTime Completed { get; set; }

        /// <summary>
        /// Gets or sets the GRAN of the product that matches this request.
        /// </summary>
        [Column("gran")]
        public string Gran { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the product whose information has been requested.
        /// </summary>
        [Key]
        [Column("gtin", Order = 1)]
        public string Gtin { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of this request to which this relates.
        /// </summary>
        [Key]
        [ForeignKey("ProductQuery")]
        [Column("rmsgProductQueryID", Order = 0)]
        public Guid ProductQueryId { get; set; }

        /// <summary>
        /// Gets or sets the request to which this relates.
        /// </summary>
        public virtual ProductQuery ProductQuery { get; set; }
    }
}
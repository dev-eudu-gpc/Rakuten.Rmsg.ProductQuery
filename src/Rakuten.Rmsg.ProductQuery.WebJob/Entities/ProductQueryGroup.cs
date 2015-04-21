// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="ProductQueryGroup.cs" company="Rakuten">
//   Copyright (c) Rakuten. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace Rakuten.Rmsg.ProductQuery.WebJob.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// Represents a collection of requests received for product information.
    /// </summary>
    [Table("rmsgProductQueryGroup")]
    public class ProductQueryGroup
    {
        /// <summary>
        /// Gets or sets the unique identifier for this collection.
        /// </summary>
        [Key]
        [ForeignKey("ProductQueries")]
        [Column("rmsgProductQueryGroupID")]
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the number of requests within this group.
        /// </summary>
        [Column("count")]
        public int Count { get; set; }

        /// <summary>
        /// Gets or sets the collection of requests within this group.
        /// </summary>
        public virtual ICollection<ProductQuery> ProductQueries { get; set; }
    }
}
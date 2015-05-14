// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="ProductQueryStatus.cs" company="Rakuten">
//   Copyright (c) Rakuten. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.WebJob
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// Defines the status of a request for product information.
    /// </summary>
    [Table("rmsgProductQueryStatus")]
    public class ProductQueryStatus
    {
        /// <summary>
        /// Gets or sets the unique identifier of this status.
        /// </summary>
        [Key]
        [Column("rmsgProductQueryStatusID")]
        public byte Id { get; set; }

        /// <summary>
        /// Gets or sets this status textual representation.
        /// </summary>
        [Column("name")]
        public string Name { get; set; }
    }
}
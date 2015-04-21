// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="ProductQuery.cs" company="Rakuten">
//   Copyright (c) Rakuten. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.WebJob.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// Details a request for product information.
    /// </summary>
    [Table("rmsgProductQuery")]
    public class ProductQuery
    {
        /// <summary>
        /// Gets or sets the point in time at which the request was received.
        /// </summary>
        [Column("dateCreated")]
        public DateTime Created { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the collection to which this request belongs.
        /// </summary>
        [ForeignKey("Group")]
        [Column("rmsgProductQueryGroupID")]
        public Guid GroupId { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of this request.
        /// </summary>
        [Key]
        [Column("rmsgProductQueryID")]
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the position of this request within its collection.
        /// </summary>
        [Column("index")]
        public short Index { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of this requests status.
        /// </summary>
        [ForeignKey("Status")]
        [Column("rmsgProductQueryStatusID")]
        public byte StatusId { get; set; }

        /// <summary>
        /// Gets or sets the Uniform Resource Indicator (URI) of the request data.
        /// </summary>
        [Column("uri")]
        public string Uri { get; set; }

        /// <summary>
        /// Gets or sets the collection to which this request belongs.
        /// </summary>
        public virtual ProductQueryGroup Group { get; set; }

        /// <summary>
        /// Gets or sets the current status of this request.
        /// </summary>
        public virtual ProductQueryStatus Status { get; set; }
    }
}
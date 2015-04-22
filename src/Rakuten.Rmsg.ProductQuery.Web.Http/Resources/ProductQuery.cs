//------------------------------------------------------------------------------
// <copyright file="ProductQuery.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.Web.Http
{
    using System;
    using System.Xml.Serialization;
    using Newtonsoft.Json;

    /// <summary>
    /// Represents a product query
    /// </summary>
    public class ProductQuery : Resource
    {
        /// <summary>
        /// Gets or sets the date on which the product query was created.
        /// </summary>
        [JsonIgnore]
        [XmlIgnore]
        public DateTime? DateCreated { get; set; }

        /// <summary>
        /// Gets the day of the month on which the product query was created.
        /// </summary>
        public string Day 
        {
            get
            {
                return this.DateCreated == null ?
                    null :
                    this.DateCreated.Value.Day.ToString("00");
            }
        }

        /// <summary>
        /// Gets or sets the unique identifier of the product query group that the product query belongs to.
        /// </summary>
        [JsonIgnore]
        [XmlIgnore]
        public Guid GroupId { get; set; }

        /// <summary>
        /// Gets or sets the index of the product query within the product query group.
        /// </summary>
        public int? Index { get; set; }

        /// <summary>
        /// Gets the month on which the product query was created.
        /// </summary>
        public string Month 
        {
            get
            {
                return this.DateCreated == null ?
                    null :
                    this.DateCreated.Value.Month.ToString("00");
            }
        }

        /// <summary>
        /// Gets or sets the status of the product query
        /// </summary>
        public ProductQueryStatus Status { get; set; }

        /// <summary>
        /// Gets the time at which the product query was created.
        /// </summary>
        public string Time 
        { 
            get 
            {
                return this.DateCreated == null ?
                    null :
                    this.DateCreated.Value.ToString("HHmm"); 
            } 
        }

        /// <summary>
        /// Gets or sets the URI for the storage blob.
        /// </summary>
        [JsonIgnore]
        [XmlIgnore]
        public string Uri { get; set; }

        /// <summary>
        /// Gets the year in which the product query was created.
        /// </summary>
        public string Year 
        {
            get
            {
                return this.DateCreated == null ?
                    null :
                    this.DateCreated.Value.Year.ToString();
            }
        }
    }
}
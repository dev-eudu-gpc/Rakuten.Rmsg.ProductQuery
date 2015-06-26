//------------------------------------------------------------------------------
// <copyright file="ProductQuery.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.Web.Http.Tests.Integration.Resources
{
    using System;
    using Newtonsoft.Json;

    /// <summary>
    /// Represents a product query.
    /// </summary>
    public class ProductQuery
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProductQuery"/> class
        /// </summary>
        public ProductQuery()
        {
        }

        /// <summary>
        /// Gets or sets the culture for the product query.
        /// </summary>
        [JsonIgnore]
        public string Culture { get; set; }

        /// <summary>
        /// Gets or sets the date on which the product query was created.
        /// </summary>
        [JsonIgnore]
        public DateTime? DateCreated
        {
            get
            {
                return new DateTime(this.Year, this.Month, this.Day, this.Hour, this.Minute, 0);
            }

            set
            {
                this.Minute = value.Value.Minute;
                this.Hour = value.Value.Hour;
                this.Day = value.Value.Day;
                this.Month = value.Value.Month;
                this.Year = value.Value.Year;
            }
        }

        /// <summary>
        /// Gets or sets the created day
        /// </summary>
        public int Day { get; set; }

        /// <summary>
        /// Gets or sets the created hour
        /// </summary>
        public int Hour { get; set; }

        /// <summary>
        /// Gets or sets the identifier for the product query.
        /// </summary>
        [JsonIgnore]
        public string Id { get; set; }

        /// <summary>
        /// Gets the identifier of the product query as a GUID.
        /// </summary>
        [JsonIgnore]
        public Guid IdAsGuid
        {
            get
            {
                return Guid.Parse(this.Id);
            }
        }

        /// <summary>
        /// Gets or sets the links for the product query.
        /// </summary>
        public ProductQueryLinks Links { get; set; }

        /// <summary>
        /// Gets or sets the created minute
        /// </summary>
        public int Minute { get; set; }

        /// <summary>
        /// Gets or sets the created month
        /// </summary>
        public int Month { get; set; }

        /// <summary>
        /// Gets or sets the status of the product query.
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Gets or sets the created year
        /// </summary>
        public int Year { get; set; }
    }
}

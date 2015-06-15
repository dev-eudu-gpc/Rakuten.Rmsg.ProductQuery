//------------------------------------------------------------------------------
// <copyright file="ProductQueryMonitorRequest.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.Web.Http.Tests.Integration
{
    using System;
    using TechTalk.SpecFlow;

    /// <summary>
    /// Parameters for making a request to the product query group status end point.
    /// </summary>
    [Binding]
    public class ProductQueryMonitorRequest
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProductQueryMonitorRequest"/> class
        /// </summary>
        public ProductQueryMonitorRequest()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductQueryMonitorRequest"/> class
        /// </summary>
        /// <param name="id">The identifier of the product query group.</param>
        /// <param name="pointInTime">The point in time for the URI for the request.</param>
        public ProductQueryMonitorRequest(string id, DateTime pointInTime)
        {
            this.Id = id;
            this.Year = pointInTime.Year.ToString();
            this.Month = pointInTime.Month.ToString();
            this.Day = pointInTime.Day.ToString();
            this.Hour = pointInTime.Hour.ToString();
            this.Minute = pointInTime.Minute.ToString();
        }

        /// <summary>
        /// Gets or sets the identifier of the product query group
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the day of the month on which the product query was created.
        /// </summary>
        public string Day { get; set; }

        /// <summary>
        /// Gets or sets the month on which the product query was created.
        /// </summary>
        public string Month { get; set; }

        /// <summary>
        /// Gets or sets the hour at which the product query was created.
        /// </summary>
        public string Hour { get; set; }

        /// <summary>
        /// Gets or sets the minute at which the product query was created.
        /// </summary>
        public string Minute { get; set; }

        /// <summary>
        /// Gets or sets the year in which the product query was created.
        /// </summary>
        public string Year { get; set; }
    }
}

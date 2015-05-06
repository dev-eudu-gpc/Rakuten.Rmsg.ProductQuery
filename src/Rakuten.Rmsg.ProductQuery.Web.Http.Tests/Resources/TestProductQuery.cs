//------------------------------------------------------------------------------
// <copyright file="TestProductQuery.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.Web.Http
{
    using System;
    using System.Globalization;
    using System.Xml.Serialization;
    using Newtonsoft.Json;

    /// <summary>
    /// Represents a product query for de-serializing into to avoid using
    /// the resource from the API.
    /// </summary>
    public class TestProductQuery : Resource
    {
        /// <summary>
        /// Gets or sets the day of the month on which the product query was created.
        /// </summary>
        public string Day { get; set; }

        /// <summary>
        /// Gets or sets the index of the product query within the product query group.
        /// </summary>
        public int? Index { get; set; }

        /// <summary>
        /// Gets or sets the month on which the product query was created.
        /// </summary>
        public string Month { get; set; }

        /// <summary>
        /// Gets or sets the status of the product query
        /// </summary>
        public string Status { get; set; }

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
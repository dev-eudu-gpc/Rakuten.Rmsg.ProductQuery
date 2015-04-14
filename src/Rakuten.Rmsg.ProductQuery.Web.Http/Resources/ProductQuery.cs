//------------------------------------------------------------------------------
// <copyright file="ProductQuery" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.Web.Http
{
    using System;
    using System.Collections.ObjectModel;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Web;
    using Rakuten.Rmsg.ProductQuery.Web.Http.Links;

    /// <summary>
    /// Represents a product query
    /// </summary>
    public class ProductQuery : Resource
    {
        /// <summary>
        /// Gets or sets the status of the product query
        /// </summary>
        public string Status { get; set; }
    }
}
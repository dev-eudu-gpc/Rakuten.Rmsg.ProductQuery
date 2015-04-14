//------------------------------------------------------------------------------
// <copyright file="ProductQueryController.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.Web.Http
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.Http;

    /// <summary>
    /// Defines endpoints for creating and obtaining information for product queries
    /// </summary>
    public class ProductQueryController : ApiController
    {
        /// <summary>
        /// Prepares a new product query
        /// </summary>
        /// <param name="id">The unique identifier for the product query</param>
        /// <returns>A representation of the new product query</returns>
        [HttpGet, Route("product-query/{id}")]
        public ProductQuery Prepare (Guid id)
        {
            return new ProductQuery()
            {
                Status = "new"
            };
        }
    }
}
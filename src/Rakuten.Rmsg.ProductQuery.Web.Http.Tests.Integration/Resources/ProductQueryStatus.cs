//------------------------------------------------------------------------------
// <copyright file="ProductQueryStatus.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.Web.Http.Tests.Integration
{
    /// <summary>
    /// Product query status.
    /// </summary>
    public enum ProductQueryStatus
    {
        /// <summary>
        /// The product query has been created but not yet submitted for processing.
        /// </summary>
        New = 1,

        /// <summary>
        /// The product query has been submitted and is awaiting processing or 
        /// is currently being processed.
        /// </summary>
        Submitted = 2,

        /// <summary>
        /// Processing of the product query has completed.
        /// </summary>
        Completed = 3
    }
}
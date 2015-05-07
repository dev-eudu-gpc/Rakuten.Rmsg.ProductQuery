//------------------------------------------------------------------------------
// <copyright file="HttpProblem.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.Web.Http.Tests.Integration
{
    /// <summary>
    /// Represents an HTTP problem.
    /// </summary>
    public class HttpProblem
    {
        /// <summary>
        /// Gets or sets the detail.
        /// </summary>
        public string Detail { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        public string Type { get; set; }
    }
}

// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="ApiClientContext.cs" company="Rakuten">
//   Copyright (c) Rakuten. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.WebJob
{
    using System;
    using System.Diagnostics.Contracts;

    using Rakuten.Gpc.Api.Client;
    using Rakuten.Rmsg.ProductQuery.Configuration;

    /// <summary>
    /// The context in which the API client should operate.
    /// </summary>
    internal class ApiClientContext : IApiClientContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApiClientContext"/> class.
        /// </summary>
        /// <param name="context">Environmental settings for this instance.</param>
        public ApiClientContext(IApiContext context)
        {
            Contract.Requires(context != null);
            Contract.Requires(context.AuthenticationToken != null);

            this.AuthorizationToken = context.AuthenticationToken;
            this.BaseAddress = context.BaseAddress;
        }

        /// <summary>
        /// Gets the authorization token for the GPC API.
        /// </summary>
        public string AuthorizationToken { get; private set; }

        /// <summary>
        /// Gets the base URI for API requests.
        /// </summary>
        public Uri BaseAddress { get; private set; }
    }
}
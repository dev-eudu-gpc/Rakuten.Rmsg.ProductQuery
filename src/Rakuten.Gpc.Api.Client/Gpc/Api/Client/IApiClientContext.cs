// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="IApiClientContext.cs" company="Rakuten">
//   Copyright (c) Rakuten. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------
namespace Rakuten.Gpc.Api.Client
{
    using System;

    /// <summary>
    /// Defines an object that represents the context in which an API client should operate.
    /// </summary>
    public interface IApiClientContext
    {
        /// <summary>
        /// Gets the authorization token for API requests.
        /// </summary>
        string AuthorizationToken { get; }

        /// <summary>
        /// Gets the scheme (protocol), host and (optionally) the port number to be used for all requests.
        /// </summary>
        Uri BaseAddress { get; }
    }
}
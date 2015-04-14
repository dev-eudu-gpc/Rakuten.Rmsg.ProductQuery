// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="ApiClientContextExtensions.cs" company="Rakuten">
//   Copyright (c) Rakuten. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------
namespace Rakuten.Gpc.Api.Client
{
    using System;

    /// <summary>
    /// Provides useful extensions to the <see cref="IApiClientContext"/> interface.
    /// </summary>
    public static class ApiClientContextExtensions
    {
        /// <summary>
        /// Creates an absolute URI from the specified relative URI.
        /// </summary>
        /// <param name="context">The context in which the client is operating.</param>
        /// <param name="relativeUri">The URI the request is sent to.</param>
        /// <returns>An absolute URI the request should be sent to.</returns>
        public static Uri MakeAbsoluteUri(this IApiClientContext context, Uri relativeUri)
        {
            if (relativeUri == null || relativeUri.IsAbsoluteUri)
            {
                return relativeUri;
            }

            if (context == null || context.BaseAddress == null || !context.BaseAddress.IsAbsoluteUri)
            {
                throw new InvalidOperationException();
            }

            return new Uri(context.BaseAddress, relativeUri);
        }
    }
}
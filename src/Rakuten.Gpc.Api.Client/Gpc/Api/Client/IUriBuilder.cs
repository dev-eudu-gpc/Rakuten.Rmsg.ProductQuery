// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="IUriBuilder.cs" company="Rakuten">
//   Copyright (c) Rakuten. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------
namespace Rakuten.Gpc.Api.Client
{
    using System;

    /// <summary>
    /// Defines an object that builds a URI.
    /// </summary>
    public interface IUriBuilder
    {
        /// <summary>
        /// Builds a URI using the parameters within the specified context.
        /// </summary>
        /// <param name="context">The context in which the URI should be constructed.</param>
        /// <param name="parameters">The parameters required to build the URI.</param>
        /// <returns>A URI built using the specified parameters.</returns>
        Uri Build(IApiClientContext context, IUriParameters parameters);
    }
}
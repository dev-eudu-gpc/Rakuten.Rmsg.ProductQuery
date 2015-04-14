// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="IUriBuilder{T}.cs" company="Rakuten">
//   Copyright (c) Rakuten. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------
namespace Rakuten.Gpc.Api.Client
{
    using System;

    /// <summary>
    /// Defines an object that builds a URI.
    /// </summary>
    /// <typeparam name="T">The type of parameters required to build the URI.</typeparam>
    public interface IUriBuilder<in T> : IUriBuilder where T : IUriParameters
    {
        /// <summary>
        /// Builds a URI using the parameters within the specified context.
        /// </summary>
        /// <param name="context">The context in which the URI should be constructed.</param>
        /// <param name="parameters">The parameters required to build the URI.</param>
        /// <returns>A URI built using the specified parameters.</returns>
        Uri Build(IApiClientContext context, T parameters);
    }
}
// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="UriBuilder{T}.cs" company="Rakuten">
//   Copyright (c) Rakuten. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------
namespace Rakuten.Gpc.Api.Client
{
    using System;

    /// <summary>
    /// Defines an object that constructs a URI.
    /// </summary>
    /// <typeparam name="T">The type of parameters required to construct the URI.</typeparam>
    public abstract class UriBuilder<T> : IUriBuilder<T> where T : IUriParameters
    {
        /// <summary>
        /// Builds a URI using the parameters within the specified context.
        /// </summary>
        /// <param name="context">The context in which the URI should be constructed.</param>
        /// <param name="parameters">The parameters required to build the URI.</param>
        /// <returns>A URI built using the specified parameters.</returns>
        public abstract Uri Build(IApiClientContext context, T parameters);

        /// <summary>
        /// Builds a URI using the parameters within the specified context.
        /// </summary>
        /// <param name="context">The context in which the URI should be constructed.</param>
        /// <param name="parameters">The parameters required to build the URI.</param>
        /// <returns>A URI built using the specified parameters.</returns>
        Uri IUriBuilder.Build(IApiClientContext context, IUriParameters parameters)
        {
            // Validation
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            if (context.BaseAddress == null)
            {
                throw new ArgumentException("context");
            }

            if (!(parameters is T))
            {
                throw new ArgumentException("Unsupported URI parameters implementation.", "parameters");
            }

            return this.Build(context, (T)parameters);
        }
    }
}
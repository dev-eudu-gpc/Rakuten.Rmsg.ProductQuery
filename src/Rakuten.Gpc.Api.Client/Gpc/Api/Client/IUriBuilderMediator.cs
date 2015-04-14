// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IUriBuilderMediator.cs" company="Rakuten">
//   Copyright (c) Rakuten. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Rakuten.Gpc.Api.Client
{
    using System;

    /// <summary>
    /// Defines an object that will locate a <see cref="IUriBuilder"/> from the given <see cref="IUriParameters"/>.
    /// </summary>
    public interface IUriBuilderMediator
    {
        /// <summary>
        /// Locates and calls a URI builder using the given <see cref="IUriParameters"/>.
        /// </summary>
        /// <typeparam name="T">
        /// The type of parameters from which an <see cref="IUriBuilder"/> should be located.
        /// </typeparam>
        /// <param name="context">The context in which URLs should be built.</param>
        /// <param name="parameters">The parameters required to build the URI.</param>
        /// <returns>A URI built using the specified parameters.</returns>
        Uri Build<T>(IApiClientContext context, T parameters) where T : IUriParameters;
    }
}
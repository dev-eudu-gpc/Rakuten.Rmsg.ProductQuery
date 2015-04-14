// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="IExceptionRegister.cs" company="Rakuten">
//   Copyright (c) Rakuten. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------
namespace Rakuten.Gpc.Api.Client
{
    using System;

    /// <summary>
    /// Defines an object that will locate and throw an exception for the specified message.
    /// </summary>
    public interface IExceptionRegister
    {
        /// <summary>
        /// Locates and throws an exception for the specified message.
        /// </summary>
        /// <param name="message">The <see cref="System.String"/> for which to locate and throw an exception.</param>
        /// <returns>
        /// An instance of <see cref="Exception"/> if an exception was located for the message;
        /// otherwise, <see langword="null"/>.
        /// </returns>
        Exception GetException(string message);
    }
}
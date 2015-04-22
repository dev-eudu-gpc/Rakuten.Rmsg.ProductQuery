// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="ExceptionRegister.cs" company="Rakuten">
//   Copyright (c) Rakuten. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.WebJob
{
    using System;

    using Rakuten.Gpc.Api.Client;

    /// <summary>
    /// Locates a specific exception for the given response body.
    /// </summary>
    public class ExceptionRegister : IExceptionRegister
    {
        /// <summary>
        /// Attempts to locate a higher level exception for the given response body.
        /// </summary>
        /// <param name="message">
        /// The body of the response received from the HTTP request.
        /// </param>
        /// <returns>
        /// A <see cref="Exception"/> implementation to throw populated with specified details from the response.
        /// </returns>
        public Exception GetException(string message)
        {
            return new Exception(message);
        }
    }
}
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ApiException.cs" company="Rakuten">
//   Copyright (c) Rakuten. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Rakuten.Gpc
{
    using System;

    /// <summary>
    /// Represents an error raised when an error was returned from the core services.
    /// </summary>
    [ApiException("http://problems.rakuten.co.uk/problem", "There was an error when handling the request.")]
    public class ApiException : Exception, IApiException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApiException"/> class.
        /// </summary>
        public ApiException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiException"/> class.
        /// </summary>
        /// <param name="message">
        /// The message that describes the error.
        /// </param>
        public ApiException(string message)
            : base(message)
        {
        }
    }
}

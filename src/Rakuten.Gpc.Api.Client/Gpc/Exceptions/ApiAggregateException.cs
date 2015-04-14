// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ApiAggregateException.cs" company="Rakuten">
//   Copyright (c) Rakuten. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Rakuten.Gpc
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Represents an error raised when multiple errors were returned from the core services.
    /// </summary>
    [ApiException("http://problems.rakuten.co.uk/aggregate-problem", "There were errors when handling the request.")]
    public class ApiAggregateException : AggregateException, IApiException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApiAggregateException"/> class.
        /// </summary>
        public ApiAggregateException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiAggregateException"/> class.
        /// </summary>
        /// <param name="message">
        /// The message that describes the error.
        /// </param>
        public ApiAggregateException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiAggregateException"/> class.
        /// </summary>
        /// <param name="message">
        /// The message that describes the error.
        /// </param>
        /// <param name="innerExceptions">
        /// The exceptions that are the cause of this exception.
        /// </param>
        public ApiAggregateException(string message, IEnumerable<Exception> innerExceptions)
            : base(message, innerExceptions)
        {
        }
    }
}

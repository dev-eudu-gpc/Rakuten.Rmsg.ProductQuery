// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="ValidationFailedException.cs" company="Rakuten">
//   Copyright (c) Rakuten. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------
namespace Rakuten.Gpc.Api
{
    using System;

    /// <summary>
    /// Represents the error raised when validation of an entity was not successful.
    /// </summary>
    public class ValidationFailedException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationFailedException"/> class
        /// </summary>
        /// <param name="innerException">
        /// The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if 
        /// no inner exception is specified. 
        /// </param>
        public ValidationFailedException(Exception innerException)
            : base(string.Empty, innerException)
        {
        }
    }
}
// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="BadRequestException.cs" company="Rakuten">
//   Copyright (c) Rakuten. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------
namespace Rakuten.Gpc.Api
{
    using System;

    /// <summary>
    /// Represents the error raised when an invalid parameter is passed to a method.
    /// </summary>
    public class BadRequestException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BadRequestException"/> class
        /// </summary>
        /// <param name="innerException">
        /// The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if 
        /// no inner exception is specified. 
        /// </param>
        public BadRequestException(Exception innerException)
            : base(string.Empty, innerException)
        {
        }
    }
}
// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="InternalServerException.cs" company="Rakuten">
//   Copyright (c) Rakuten. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------
namespace Rakuten.Gpc.Api
{
    using System;

    /// <summary>
    /// Represents the error raised when an un-identified exception was handled and raised.
    /// </summary>
    public class InternalServerException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InternalServerException"/> class
        /// </summary>
        /// <param name="innerException">
        /// The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if 
        /// no inner exception is specified. 
        /// </param>
        public InternalServerException(Exception innerException)
            : base(string.Empty, innerException)
        {
        }
    }
}
// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="ObjectNotFoundException.cs" company="Rakuten">
//   Copyright (c) Rakuten. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------
namespace Rakuten.Gpc.Api
{
    using System;

    /// <summary>
    /// Represents the error raised when an identified entity could not be located.
    /// </summary>
    public class ObjectNotFoundException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectNotFoundException"/> class
        /// </summary>
        /// <param name="innerException">
        /// The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if 
        /// no inner exception is specified. 
        /// </param>
        public ObjectNotFoundException(Exception innerException)
            : base(string.Empty, innerException)
        {
        }
    }
}
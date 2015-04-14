// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="ConstraintViolationException.cs" company="Rakuten">
//   Copyright (c) Rakuten. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------
namespace Rakuten.Gpc.Api
{
    using System;

    /// <summary>
    /// Represents the error raised when an entity already exists.
    /// </summary>
    public class ConstraintViolationException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConstraintViolationException"/> class.
        /// </summary>
        /// <param name="innerException">
        /// The exception that is the cause of the current exception,
        /// or <see langword="null"/> if no inner exception is specified.
        /// </param>
        public ConstraintViolationException(Exception innerException) : base(string.Empty, innerException)
        {
        }
    }
}
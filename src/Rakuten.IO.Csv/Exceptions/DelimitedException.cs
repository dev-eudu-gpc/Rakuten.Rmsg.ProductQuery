// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DelimitedException.cs" company="Rakuten">
//   Copyright (c) Rakuten. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------
namespace Rakuten.IO.Delimited
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// Describes an error that can occur when processing delimited files.
    /// </summary>
    public class DelimitedException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DelimitedException" /> class.
        /// </summary>
        public DelimitedException()
            : base(ExceptionMessages.ExceptionMessage)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DelimitedException"/> class.
        /// </summary>
        /// <param name="innerException">
        /// The inner exception.
        /// </param>
        public DelimitedException(Exception innerException)
            : base(ExceptionMessages.ExceptionMessage, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DelimitedException"/> class.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        public DelimitedException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DelimitedException"/> class.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <param name="innerException">
        /// The inner exception.
        /// </param>
        public DelimitedException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DelimitedException"/> class.
        /// </summary>
        /// <param name="info">
        /// The info.
        /// </param>
        /// <param name="context">
        /// The context.
        /// </param>
        public DelimitedException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
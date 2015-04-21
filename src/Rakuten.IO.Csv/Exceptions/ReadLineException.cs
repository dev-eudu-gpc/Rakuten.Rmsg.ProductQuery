// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReadLineException.cs" company="Rakuten">
//   Copyright (c) Rakuten. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------
namespace Rakuten.IO.Delimited
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// Describes an error that occurred whilst reading a line of a delimited file.
    /// </summary>
    public class ReadLineException : DelimitedException
    {
        /// <summary>
        /// The line.
        /// </summary>
        public readonly long Line;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReadLineException"/> class.
        /// </summary>
        /// <param name="line">
        /// The line number.
        /// </param>
        public ReadLineException(long line)
            : base(ErrorMessage(line))
        {
            this.Line = line;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReadLineException"/> class.
        /// </summary>
        /// <param name="line">
        /// The line number.
        /// </param>
        /// <param name="innerException">
        /// The inner exception.
        /// </param>
        public ReadLineException(long line, Exception innerException)
            : base(ErrorMessage(line), innerException)
        {
            this.Line = line;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReadLineException"/> class.
        /// </summary>
        /// <param name="info">
        /// The info.
        /// </param>
        /// <param name="context">
        /// The context.
        /// </param>
        public ReadLineException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        /// <summary>
        /// The error message.
        /// </summary>
        /// <param name="line">
        /// The line.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        private static string ErrorMessage(long line)
        {
            return string.Format(ExceptionMessages.ReadLineExceptionMessage, line);
        }
    }
}
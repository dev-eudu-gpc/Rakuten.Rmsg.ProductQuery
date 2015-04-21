// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HeaderMissingException.cs" company="Rakuten">
//   Copyright (c) Rakuten. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------
namespace Rakuten.IO.Delimited
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// Describes an error that occurs when a field has not had a header specified.
    /// </summary>
    public class HeaderMissingException : DelimitedException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HeaderMissingException"/> class.
        /// </summary>
        /// <param name="header">
        /// The header.
        /// </param>
        public HeaderMissingException(string header)
            : base(ErrorMessage(header))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HeaderMissingException"/> class.
        /// </summary>
        /// <param name="header">
        /// The header.
        /// </param>
        /// <param name="innerException">
        /// The inner exception.
        /// </param>
        public HeaderMissingException(string header, Exception innerException)
            : base(ErrorMessage(header), innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HeaderMissingException"/> class.
        /// </summary>
        /// <param name="info">
        /// The info.
        /// </param>
        /// <param name="context">
        /// The context.
        /// </param>
        public HeaderMissingException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        /// <summary>
        /// The error message.
        /// </summary>
        /// <param name="header">
        /// The header.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        private static string ErrorMessage(string header)
        {
            return string.Format(ExceptionMessages.HeaderMissingExceptionMessage, header);
        }
    }
}
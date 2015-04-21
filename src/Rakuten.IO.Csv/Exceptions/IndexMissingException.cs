// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IndexMissingException.cs" company="Rakuten">
//   Copyright (c) Rakuten. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------
namespace Rakuten.IO.Delimited
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// Describes a error that occurs when a field has not had an index specified.
    /// </summary>
    public class IndexMissingException : DelimitedException
    {
        /// <summary>
        /// The line.
        /// </summary>
        public readonly string PropertyName;

        /// <summary>
        /// Initializes a new instance of the <see cref="IndexMissingException"/> class.
        /// </summary>
        /// <param name="propertyName">
        /// The property.
        /// </param>
        public IndexMissingException(string propertyName)
            : base(ErrorMessage(propertyName))
        {
            this.PropertyName = propertyName;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IndexMissingException"/> class.
        /// </summary>
        /// <param name="propertyName">
        /// The line number.
        /// </param>
        /// <param name="innerException">
        /// The inner exception.
        /// </param>
        public IndexMissingException(string propertyName, Exception innerException)
            : base(ErrorMessage(propertyName), innerException)
        {
            this.PropertyName = propertyName;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IndexMissingException"/> class.
        /// </summary>
        /// <param name="info">
        /// The info.
        /// </param>
        /// <param name="context">
        /// The context.
        /// </param>
        public IndexMissingException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        /// <summary>
        /// The error message.
        /// </summary>
        /// <param name="propertyName">
        /// The property.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        private static string ErrorMessage(string propertyName)
        {
            return string.Format(ExceptionMessages.IndexMissingExceptionMessage, propertyName);
        }
    }
}
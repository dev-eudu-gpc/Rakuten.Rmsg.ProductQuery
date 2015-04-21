// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InvalidEnumValueException.cs" company="Rakuten">
//   Copyright (c) Rakuten. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------
namespace Rakuten.IO.Delimited
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// Describes an error when a value from a file could not be mapped to the defined <see cref="Enum"/>.
    /// </summary>
    public class InvalidEnumValueException : DelimitedException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidEnumValueException"/> class.
        /// </summary>
        /// <param name="stringValue">
        /// The string Value.
        /// </param>
        /// <param name="enumType">
        /// The enum Type.
        /// </param>
        public InvalidEnumValueException(
            string stringValue, 
            Type enumType)
            : base(ErrorMessage(stringValue, enumType))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidEnumValueException"/> class.
        /// </summary>
        /// <param name="stringValue">
        /// The string Value.
        /// </param>
        /// <param name="enumType">
        /// The enum Type.
        /// </param>
        /// <param name="innerException">
        /// The inner exception.
        /// </param>
        public InvalidEnumValueException(
            string stringValue, 
            Type enumType, 
            Exception innerException)
            : base(
                ErrorMessage(stringValue, enumType), 
                innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidEnumValueException"/> class.
        /// </summary>
        /// <param name="info">
        /// The info.
        /// </param>
        /// <param name="context">
        /// The context.
        /// </param>
        public InvalidEnumValueException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        /// <summary>
        /// The error message.
        /// </summary>
        /// <param name="stringValue">
        /// The string value.
        /// </param>
        /// <param name="enumType">
        /// The enum type.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        private static string ErrorMessage(
            string stringValue, 
            Type enumType)
        {
            return string.Format(
                ExceptionMessages.InvalidEnumValueExceptionMessage, 
                stringValue, 
                enumType);
        }
    }
}
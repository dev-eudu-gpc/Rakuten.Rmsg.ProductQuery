// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InvalidDefaultException.cs" company="Rakuten">
//   Copyright (c) Rakuten. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------
namespace Rakuten.IO.Delimited
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// Describes an error that occurs when an invalid default value for a field has been specified.
    /// </summary>
    public class InvalidDefaultException : DelimitedException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidDefaultException"/> class.
        /// </summary>
        /// <param name="defaultValue">
        /// The default Value.
        /// </param>
        /// <param name="propertyName">
        /// The property Name.
        /// </param>
        /// <param name="type">
        /// The type.
        /// </param>
        public InvalidDefaultException(
            object defaultValue, 
            string propertyName, 
            Type type)
            : base(ErrorMessage(defaultValue, propertyName, type))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidDefaultException"/> class.
        /// </summary>
        /// <param name="defaultValue">
        /// The default Value.
        /// </param>
        /// <param name="propertyName">
        /// The property Name.
        /// </param>
        /// <param name="type">
        /// The type.
        /// </param>
        /// <param name="innerException">
        /// The inner exception.
        /// </param>
        public InvalidDefaultException(
            object defaultValue, 
            string propertyName, 
            Type type, 
            Exception innerException)
            : base(
                ErrorMessage(defaultValue, propertyName, type), 
                innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidDefaultException"/> class.
        /// </summary>
        /// <param name="info">
        /// The info.
        /// </param>
        /// <param name="context">
        /// The context.
        /// </param>
        public InvalidDefaultException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        /// <summary>
        /// The error message.
        /// </summary>
        /// <param name="defaultValue">
        /// The default value.
        /// </param>
        /// <param name="propertyName">
        /// The property name.
        /// </param>
        /// <param name="type">
        /// The type.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        private static string ErrorMessage(
            object defaultValue, 
            string propertyName, 
            Type type)
        {
            return string.Format(
                ExceptionMessages.InvalidDefaultExceptionMessage, 
                defaultValue ?? "<null>", 
                propertyName, 
                type);
        }
    }
}
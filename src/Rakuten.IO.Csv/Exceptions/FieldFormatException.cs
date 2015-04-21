// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FieldFormatException.cs" company="Rakuten">
//   Copyright (c) Rakuten. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------
namespace Rakuten.IO.Delimited
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// Describes an error that can occur when a field is found to have an invalid format.
    /// </summary>
    public class FieldFormatException : DelimitedException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FieldFormatException"/> class.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <param name="propertyType">
        /// The property Type.
        /// </param>
        /// <param name="fieldName">
        /// The field Name.
        /// </param>
        /// <param name="fieldIndex">
        /// The field Index.
        /// </param>
        /// <param name="propertyName">
        /// The property Name.
        /// </param>
        public FieldFormatException(
            object value, 
            Type propertyType, 
            string fieldName, 
            int fieldIndex, 
            string propertyName)
            : base(ErrorMessage(value, propertyType, fieldName, fieldIndex, propertyName))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FieldFormatException"/> class.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <param name="propertyType">
        /// The property Type.
        /// </param>
        /// <param name="fieldName">
        /// The field Name.
        /// </param>
        /// <param name="fieldIndex">
        /// The field Index.
        /// </param>
        /// <param name="propertyName">
        /// The property Name.
        /// </param>
        /// <param name="innerException">
        /// The inner exception.
        /// </param>
        public FieldFormatException(
            object value, 
            Type propertyType, 
            string fieldName, 
            int fieldIndex, 
            string propertyName, 
            Exception innerException)
            : base(
                ErrorMessage(value, propertyType, fieldName, fieldIndex, propertyName), 
                innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FieldFormatException"/> class.
        /// </summary>
        /// <param name="info">
        /// The info.
        /// </param>
        /// <param name="context">
        /// The context.
        /// </param>
        public FieldFormatException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        /// <summary>
        /// The error message.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <param name="propertyType">
        /// The property type.
        /// </param>
        /// <param name="fieldName">
        /// The field name.
        /// </param>
        /// <param name="fieldIndex">
        /// The field index.
        /// </param>
        /// <param name="propertyName">
        /// The property name.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        private static string ErrorMessage(
            object value, 
            Type propertyType, 
            string fieldName, 
            int fieldIndex, 
            string propertyName)
        {
            return string.Format(
                ExceptionMessages.FieldFormatExceptionMessage, 
                value, 
                propertyType, 
                fieldName, 
                fieldIndex, 
                propertyName);
        }
    }
}
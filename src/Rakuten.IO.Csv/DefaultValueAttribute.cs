// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="DefaultValueAttribute.cs" company="Rakuten">
//   Copyright (c) Rakuten. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------
namespace Rakuten.IO.Delimited
{
    using System;
    using System.ComponentModel;

    /// <summary>
    /// An attribute that allows the default value of a field to be specified.
    /// </summary>
    public sealed class DefaultValueAttribute : System.ComponentModel.DefaultValueAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultValueAttribute"/> class.
        /// </summary>
        /// <param name="type">A <see cref="Type"/> that represents the type to convert the value to.</param>
        /// <param name="value">
        /// A <see cref="string"/> that can be converted to the type using the <see cref="TypeConverter"/> for the type 
        /// and the U.S. English culture.
        /// </param>
        public DefaultValueAttribute(Type type, string value)
            : base(type, value)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultValueAttribute"/> class.
        /// </summary>
        /// <param name="value">A Unicode character that is the default value.</param>
        public DefaultValueAttribute(char value)
            : base(value)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultValueAttribute"/> class.
        /// </summary>
        /// <param name="value">A 8-bit unsigned integer that is the default value.</param>
        public DefaultValueAttribute(byte value)
            : base(value)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultValueAttribute"/> class.
        /// </summary>
        /// <param name="value">A 16-bit signed integer that is the default value.</param>
        public DefaultValueAttribute(short value)
            : base(value)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultValueAttribute"/> class.
        /// </summary>
        /// <param name="value">A 32-bit signed integer that is the default value.</param>
        public DefaultValueAttribute(int value)
            : base(value)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultValueAttribute"/> class.
        /// </summary>
        /// <param name="value">A 64-bit signed integer that is the default value.</param>
        public DefaultValueAttribute(long value)
            : base(value)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultValueAttribute"/> class.
        /// </summary>
        /// <param name="value">A single-precision floating point number that is the default value.</param>
        public DefaultValueAttribute(float value)
            : base(value)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultValueAttribute"/> class.
        /// </summary>
        /// <param name="value">A double-precision floating point number that is the default value.</param>
        public DefaultValueAttribute(double value)
            : base(value)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultValueAttribute"/> class.
        /// </summary>
        /// <param name="value">A <see cref="bool"/> that is the default value.</param>
        public DefaultValueAttribute(bool value)
            : base(value)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultValueAttribute"/> class.
        /// </summary>
        /// <param name="value">A <see cref="string"/> that is the default value.</param>
        public DefaultValueAttribute(string value)
            : base(value)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultValueAttribute"/> class.
        /// </summary>
        /// <param name="value">A <see cref="object"/> that represents the default value.</param>
        public DefaultValueAttribute(object value)
            : base(value)
        {
        }
    }
}
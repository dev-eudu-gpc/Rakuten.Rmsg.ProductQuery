// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HeaderAttribute.cs" company="Rakuten">
//   Copyright (c) Rakuten. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------
namespace Rakuten.IO.Delimited
{
    using System;

    /// <summary>
    /// Allows the header of a field to be specified.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public sealed class HeaderAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HeaderAttribute"/> class.
        /// </summary>
        /// <param name="value">The field header value.</param>
        public HeaderAttribute(string value)
        {
            this.Value = value;
        }

        /// <summary>
        /// Gets or sets the field header value.
        /// </summary>
        public string Value { get; set; }
    }
}
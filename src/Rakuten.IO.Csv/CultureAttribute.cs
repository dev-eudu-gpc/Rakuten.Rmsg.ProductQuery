// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="CultureAttribute.cs" company="Rakuten">
//   Copyright (c) Rakuten. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------
namespace Rakuten.IO.Delimited
{
    using System;
    using System.Globalization;

    /// <summary>
    /// An attribute that can be applied to specify the culture to be used during serialization.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public sealed class CultureAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CultureAttribute"/> class.
        /// </summary>
        /// <param name="culture">The culture to be used during serialization.</param>
        public CultureAttribute(string culture)
        {
            this.Culture = new CultureInfo(culture);
        }

        /// <summary>
        /// Gets or sets the culture of the serialization.
        /// </summary>
        public CultureInfo Culture { get; set; }
    }
}
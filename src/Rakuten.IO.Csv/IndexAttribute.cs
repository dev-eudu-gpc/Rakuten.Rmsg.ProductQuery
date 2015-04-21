// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IndexAttribute.cs" company="Rakuten">
//   Copyright (c) Rakuten. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------
namespace Rakuten.IO.Delimited
{
    using System;

    /// <summary>
    /// Allows the index of a field to be specified.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public sealed class IndexAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IndexAttribute"/> class.
        /// </summary>
        /// <param name="index">The index of the field.</param>
        public IndexAttribute(int index)
        {
            this.Index = index;
        }

        /// <summary>
        /// Gets or sets the index of the field
        /// </summary>
        public int Index { get; set; }
    }
}
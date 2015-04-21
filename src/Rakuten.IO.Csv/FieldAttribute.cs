// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="FieldAttribute.cs" company="Rakuten">
//   Copyright (c) Rakuten. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------
namespace Rakuten.IO.Delimited
{
    using System;

    /// <summary>
    /// Defines an attribute that can be applied to a property to describe the operations that should be applied during
    /// serialization.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public sealed class FieldAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FieldAttribute"/> class.
        /// </summary>
        /// <param name="operations">The operations to be applied to this field during serialization.</param>
        public FieldAttribute(FieldOperations operations)
        {
            this.Operations = operations;
        }

        /// <summary>
        /// Gets or sets the operations that are to be performed on this field.
        /// </summary>
        internal FieldOperations Operations { get; set; }
    }
}
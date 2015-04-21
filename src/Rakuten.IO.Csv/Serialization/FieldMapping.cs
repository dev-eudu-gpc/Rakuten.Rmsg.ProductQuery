// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FieldMapping.cs" company="Rakuten">
//   Copyright (c) Rakuten. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------
namespace Rakuten.IO.Delimited.Serialization
{
    using System;
    using System.Globalization;

    /// <summary>
    /// Stores information required to map a particular field from a CSV to an object
    /// </summary>
    internal class FieldMapping
    {
        /// <summary>
        /// Gets or sets the name of the property on the mapping class
        /// </summary>
        internal string PropertyName { get; set; }

        /// <summary>
        /// Gets or sets the name of the field header
        /// </summary>
        internal string FieldName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the field is optional or not
        /// </summary>
        internal bool Optional { get; set; }

        /// <summary>
        /// Gets or sets the type of the property this field is to map to.
        /// </summary>
        internal Type PropertyType { get; set; }

        /// <summary>
        /// Gets or sets the type to use for mapping the field to or from a property. In the case of <see cref="Enum"/>s
        /// or <see cref="Nullable"/> Types, this will be the underlying type of the Property.
        /// </summary>
        internal Type MappingType { get; set; }

        /// <summary>
        /// Gets or sets the format provider for the field
        /// </summary>
        internal CultureInfo Culture { get; set; }

        /// <summary>
        /// Gets or sets the field direction.
        /// </summary>
        internal FieldOperations Operations { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="Enum"/> type.
        /// </summary>
        internal Type EnumType { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether is string type.
        /// </summary>
        internal bool IsStringType { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether is <see cref="Enum"/> type.
        /// </summary>
        internal bool IsEnumType { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether has <see cref="Enum"/> converter.
        /// </summary>
        internal bool HasEnumConverter { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether has index.
        /// </summary>
        internal bool HasIndex { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="Enum"/> converter.
        /// </summary>
        internal EnumConverterAttribute EnumConverter { get; set; }

        /// <summary>
        /// Gets or sets the field index.
        /// </summary>
        internal int FieldIndex { get; set; }

        /// <summary>
        /// Gets or sets the default value.
        /// </summary>
        internal object DefaultValue { get; set; }
    }
}
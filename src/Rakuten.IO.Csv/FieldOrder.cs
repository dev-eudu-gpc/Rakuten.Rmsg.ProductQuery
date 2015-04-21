// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FieldOrder.cs" company="Rakuten">
//   Copyright (c) Rakuten. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------
namespace Rakuten.IO.Delimited
{
    /// <summary>
    /// Identifies in which order fields should be written.
    /// </summary>
    public enum FieldOrder
    {
        /// <summary>
        /// Fields will be written in the order the appear in the class
        /// </summary>
        OrderByClass,

        /// <summary>
        /// Fields will be written in alphabetical order of the property name
        /// </summary>
        OrderByProperty,

        /// <summary>
        /// Fields will be written in alphabetical order of the field name
        /// </summary>
        OrderByFieldName,

        /// <summary>
        /// Fields will be written in the order specified by the index attribute
        /// </summary>
        OrderByIndex
    }
}
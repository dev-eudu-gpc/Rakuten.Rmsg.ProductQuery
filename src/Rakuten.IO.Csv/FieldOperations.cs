// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="FieldOperations.cs" company="Rakuten">
//   Copyright (c) Rakuten. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------
namespace Rakuten.IO.Delimited
{
    using System;

    /// <summary>
    /// Specifies the operations to perform on a given field
    /// </summary>
    [Flags]
    public enum FieldOperations
    {
        /// <summary>
        /// Read this field, but don't write it
        /// </summary>
        Read = 1, 

        /// <summary>
        /// Write this field, but don't read it
        /// </summary>
        Write = 2, 

        /// <summary>
        /// Read and write this field (this is the default operation)
        /// </summary>
        ReadWrite = 3, 

        /// <summary>
        /// Ignore this field (don't read or write)
        /// </summary>
        Ignore = 4
    }
}
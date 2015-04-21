// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="DefaultStringsToNullAttribute.cs" company="Rakuten">
//   Copyright (c) Rakuten. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------
namespace Rakuten.IO.Delimited
{
    using System;

    /// <summary>
    /// Specifies that string types should be defaulted to null where the field is empty in the file, and where there is
    /// no default value specified on the property via a <see cref="DefaultValueAttribute"/>.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public sealed class DefaultStringsToNullAttribute : Attribute
    {
    }
}
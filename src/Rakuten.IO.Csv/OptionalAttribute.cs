// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OptionalAttribute.cs" company="Rakuten">
//   Copyright (c) Rakuten. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------
namespace Rakuten.IO.Delimited
{
    using System;

    /// <summary>
    /// Indicates that this column is optional for read operations.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public sealed class OptionalAttribute : Attribute
    {
    }
}
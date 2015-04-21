// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TypeExtensions.cs" company="Rakuten">
//   Copyright (c) Rakuten. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------
namespace Rakuten.IO.Delimited
{
    using System;

    /// <summary>
    /// A set of extensions to the <see cref="Type" /> type
    /// </summary>
    public static class TypeExtensions
    {
        /// <summary>
        /// The default value.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>The <see cref="object"/>.</returns>
        public static object DefaultValue(this Type type)
        {
            return type.IsValueType
                       ? Activator.CreateInstance(type)
                       : null;
        }

        /// <summary>
        /// The get type or nullable underlying type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>The <see cref="Type"/>.</returns>
        public static Type GetTypeOrNullableUnderlyingType(this Type type)
        {
            return Nullable.GetUnderlyingType(type) ?? type;
        }

        /// <summary>
        /// The is nullable type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public static bool IsNullableType(this Type type)
        {
            return !type.IsValueType || Nullable.GetUnderlyingType(type) != null;
        }
    }
}
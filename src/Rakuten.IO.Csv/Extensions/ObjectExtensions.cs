// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="ObjectExtensions.cs" company="Rakuten">
//   Copyright (c) Rakuten. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------
namespace Rakuten.IO.Delimited
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// A set of extensions to the o<see cref="object" /> type.
    /// </summary>
    public static class ObjectExtensions
    {
        /// <summary>
        /// Returns an object converted to the specified type, or null if the source object is null
        /// </summary>
        /// <param name="sourceObj">The object to convert.</param>
        /// <param name="conversionType">The type to convert the specified instance to.</param>
        /// <param name="formatProvider">The format Provider.</param>
        /// <returns>A <paramref name="conversionType"/> instance of conversion was successful.</returns>
        public static object AsType(
            this object sourceObj, 
            Type conversionType, 
            IFormatProvider formatProvider = null)
        {
            return 
                sourceObj == null ? 
                null :
                formatProvider == null ? 
                    Convert.ChangeType(sourceObj, conversionType) :
                    Convert.ChangeType(sourceObj, conversionType, formatProvider);
        }

        /// <summary>
        /// Returns an object cast to an <see cref="Enum"/> type, or null if the source object is null
        /// </summary>
        /// <param name="sourceObj">The object to convert.</param>
        /// <param name="enumType">The <see cref="Enum"/> type to convert the instance to.</param>
        /// <param name="underlyingType">The underlying type.</param>
        /// <returns>A <paramref name="underlyingType"/> instance of conversion was successful.</returns>
        public static object AsEnum(this object sourceObj, Type enumType, Type underlyingType)
        {
            return Enum.ToObject(
                enumType, 
                sourceObj.AsType(underlyingType));
        }

        /// <summary>
        /// Returns null if source is null, otherwise it returns the result of the func
        /// </summary>
        /// <param name="source">
        /// The source object.
        /// </param>
        /// <param name="func">
        /// A function on the source object
        /// </param>
        /// <typeparam name="TIn">
        /// The type of the source object
        /// </typeparam>
        /// <typeparam name="TResult">
        /// The type of the return object
        /// </typeparam>
        /// <returns>
        /// The <see cref="TResult"/>.
        /// </returns>
        public static TResult IfNotNull<TIn, TResult>(
            this TIn source,
            Func<TIn, TResult> func) where TIn : class
        {
            return source == null ? default(TResult) : func(source);
        }

        /// <summary>
        /// Returns null if source is null, otherwise it returns the result of the func
        /// </summary>
        /// <param name="source">
        /// The source object.
        /// </param>
        /// <param name="func">
        /// A function on the source object
        /// </param>
        /// <typeparam name="TIn">
        /// The underlying type of the nullable
        /// </typeparam>
        /// <typeparam name="TResult">
        /// The type of the return object
        /// </typeparam>
        /// <returns>
        /// The <see cref="TResult"/>.
        /// </returns>
        public static TResult IfNotNull<TIn, TResult>(
            this TIn? source,
            Func<TIn?, TResult> func) where TIn : struct
        {
            return source == null ? default(TResult) : func(source);
        }

        /// <summary>
        /// Wraps this object instance into an <see cref="IEnumerable{T}"/>
        /// </summary>
        /// <param name="item">
        /// The item.
        /// </param>
        /// <typeparam name="T">
        /// The type of the item
        /// </typeparam>
        /// <returns>
        /// An <see cref="IEnumerable{T}"/>.
        /// </returns>
        public static IEnumerable<T> Yield<T>(this T item)
        {
            // ReSharper disable once CompareNonConstrainedGenericWithNull
            // Ok in this instance, since if T is a value type, this will never be true anyway
            if (item == null)
            {
                yield break;
            }

            yield return item;
        }
    }
}
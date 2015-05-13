//----------------------------------------------------------------------------------------------------------------------
// <copyright file="EnumeratorExtensions.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------------------------------------------------
// ReSharper disable once CheckNamespace
// Extensions should exist in the same namespace as the class being extended.
namespace System.Collections.Generic
{
    using System;
    using System.Diagnostics.Contracts;

    /// <summary>
    /// Provides useful extensions to the <see cref="IEnumerator{T}"/> interface.
    /// </summary>
    public static class EnumeratorExtensions
    {
        /// <summary>
        /// Reads the next element from the enumerated collection.
        /// </summary>
        /// <typeparam name="T">The type of object being enumerated.</typeparam>
        /// <param name="stream">The current enumerator.</param>
        /// <returns>The next element in the collection.</returns>
        /// <exception cref="InvalidOperationException">
        /// The enumerator is already at the end of the collection.
        /// </exception>
        public static T Read<T>(this IEnumerator<T> stream)
        {
            Contract.Requires(stream != null);

            if (!stream.MoveNext())
            {
                throw new InvalidOperationException("Stream terminated unexpectedly.");
            }

            return stream.Current;
        }
    }
}
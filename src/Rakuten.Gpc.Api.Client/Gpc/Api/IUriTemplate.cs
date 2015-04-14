//----------------------------------------------------------------------------------------------------------------------
// <copyright file="IUriTemplate.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------------------------------------------------
namespace Rakuten
{
    using System;
    using System.Linq;

    /// <summary>
    /// Exposes method for manipulating a Uniform Resource Indicator (URI) template, as defined by RFC 6570.
    /// </summary>
    public interface IUriTemplate
    {
        /// <summary>
        /// Creates a new URI template with the specified variable bound to a specified value.
        /// </summary>
        /// <param name="name">The name of the variable to bind.</param>
        /// <param name="value">The value of the variable.</param>
        /// <returns>A new URI template with the specified variable bound to the specified value.</returns>
        IUriTemplate Bind(string name, string value);

        /// <summary>
        /// Attempts to match a <see cref="Uri"/> to a <see cref="IUriTemplate"/>.
        /// </summary>
        /// <param name="candidate">The <see cref="Uri"/> to match against the template.</param>
        /// <returns>A collection of key/value pairs representing bound variables and their values.</returns>
        ILookup<string, string> Match(Uri candidate);

        /// <summary>
        /// Returns a string that represents the current URI template.
        /// </summary>
        /// <param name="resolveTemplate">
        /// <see langword="true"/> if the string representation should be resolved, with no unspecified parameters;
        /// otherwise, <see langword="false"/>.
        /// </param>
        /// <returns>A string that represents the current URI template.</returns>
        string ToString(bool resolveTemplate);
    }
}
//----------------------------------------------------------------------------------------------------------------------
// <copyright file="IUriTemplate.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------------------------------------------------
namespace Rakuten
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;

    /// <summary>
    /// Exposes method for manipulating a Uniform Resource Indicator (URI) template, as defined by RFC 6570.
    /// </summary>
    [ContractClass(typeof(UriTemplateContract))]
    public interface IUriTemplate
    {
        /// <summary>
        /// Gets the text representation of this URI template.
        /// </summary>
        string Text { get; }

        /// <summary>
        /// Gets a collection of bound variables and their values.
        /// </summary>
        ILookup<string, string> Values { get; }

        /// <summary>
        /// Creates a new URI template with the specified variable bound to a specified value.
        /// </summary>
        /// <param name="name">The name of the variable to bind.</param>
        /// <param name="value">The value of the variable.</param>
        /// <returns>A new URI template with the specified variable bound to the specified value.</returns>
        IUriTemplate Bind(string name, string value);

        /// <summary>
        /// Creates a new URI template with the specified variable bound to a set of specified values.
        /// </summary>
        /// <param name="name">The name of the variable to bind.</param>
        /// <param name="values">The values of the variable.</param>
        /// <returns>A new URI template with the specified variable bound to the specified values.</returns>
        IUriTemplate Bind(string name, IEnumerable<string> values);

        /// <summary>
        /// Creates a new URI template by partially expanding this template.
        /// </summary>
        /// <returns>A new URI template containing only the unbound variables from this template.</returns>
        IUriTemplate CreateTemplate();

        /// <summary>
        /// Expands the URI template.
        /// </summary>
        /// <returns>A <see cref="Uri"/> produced by expanding the URI template.</returns>
        Uri Expand();

        /// <summary>
        /// Expands the URI template.
        /// </summary>
        /// <param name="values">The values of variables to use when expanding the template.</param>
        /// <returns>A <see cref="Uri"/> produced by expanding the URI template.</returns>
        Uri Expand(ILookup<string, string> values);

        /// <summary>
        /// Creates a new URI template with the variables bound from a specified URI.
        /// </summary>
        /// <param name="uri">The URI from which to extract variable values.</param>
        /// <param name="result">
        /// When this method succeeds, contains a new URI template with the variables parsed from the specified URI.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if the specified URI is a match for the current template;
        /// otherwise, <see langword="false"/>.
        /// </returns>
        bool TryBind(Uri uri, out IUriTemplate result);
    }
}
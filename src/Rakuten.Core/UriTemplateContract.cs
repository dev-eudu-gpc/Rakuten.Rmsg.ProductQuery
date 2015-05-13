//----------------------------------------------------------------------------------------------------------------------
// <copyright file="UriTemplateContract.cs" company="Rakuten">
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
    [ContractClassFor(typeof(IUriTemplate))]
    internal abstract class UriTemplateContract : IUriTemplate
    {
        /// <summary>
        /// Gets the text representation of this URI template.
        /// </summary>
        public string Text
        {
            get
            {
                Contract.Ensures(Contract.Result<string>() != null);
                return default(string);
            }
        }

        /// <summary>
        /// Gets a collection of bound variables and their values.
        /// </summary>
        public ILookup<string, string> Values
        {
            get
            {
                Contract.Ensures(Contract.Result<ILookup<string, string>>() != null);
                return default(ILookup<string, string>);
            }
        }

        /// <summary>
        /// Creates a new URI template with the specified variable bound to a specified value.
        /// </summary>
        /// <param name="name">The name of the variable to bind.</param>
        /// <param name="value">The value of the variable.</param>
        /// <returns>A new URI template with the specified variable bound to the specified value.</returns>
        public IUriTemplate Bind(string name, string value)
        {
            Contract.Ensures(Contract.Result<IUriTemplate>() != null);
            return default(IUriTemplate);
        }

        /// <summary>
        /// Creates a new URI template with the specified variable bound to a specified value.
        /// </summary>
        /// <param name="name">The name of the variable to bind.</param>
        /// <param name="values">The values of the variable.</param>
        /// <returns>A new URI template with the specified variable bound to the specified value.</returns>
        public IUriTemplate Bind(string name, IEnumerable<string> values)
        {
            Contract.Ensures(Contract.Result<IUriTemplate>() != null);
            return default(IUriTemplate);
        }

        /// <summary>
        /// Creates a new URI template by partially expanding this template.
        /// </summary>
        /// <returns>A new URI template containing only the unbound variables from this template.</returns>
        public IUriTemplate CreateTemplate()
        {
            Contract.Ensures(Contract.Result<IUriTemplate>() != null);
            return default(IUriTemplate);
        }

        /// <summary>
        /// Expands the URI template.
        /// </summary>
        /// <returns>A <see cref="Uri"/> produced by expanding the URI template.</returns>
        public Uri Expand()
        {
            Contract.Ensures(Contract.Result<Uri>() != null);
            return default(Uri);
        }

        /// <summary>
        /// Expands the URI template.
        /// </summary>
        /// <param name="values">The values of variables to use when expanding the template.</param>
        /// <returns>A <see cref="Uri"/> produced by expanding the URI template.</returns>
        public Uri Expand(ILookup<string, string> values)
        {
            Contract.Ensures(Contract.Result<Uri>() != null);
            return default(Uri);
        }

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
        public bool TryBind(Uri uri, out IUriTemplate result)
        {
            Contract.Ensures(Contract.ValueAtReturn(out result) != null == Contract.Result<bool>());
            return default(bool);
        }
    }
}
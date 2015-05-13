//----------------------------------------------------------------------------------------------------------------------
// <copyright file="UriTemplate.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------------------------------------------------
namespace Rakuten
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Represents a Uniform Resource Indicator (URI) template, as defined by RFC 6570.
    /// </summary>
    public partial class UriTemplate : IUriTemplate
    {
        /// <summary>
        /// An empty collection of bound variables.
        /// </summary>
        private static readonly ILookup<string, string> NoBoundVariables = Enumerable.Empty<string>()
            .ToLookup(s => s, s => s);

        /// <summary>
        /// Initializes a new instance of the <see cref="UriTemplate"/> class.
        /// </summary>
        /// <param name="templateString">A URI template.</param>
        public UriTemplate(string templateString)
        {
            IEnumerator<Token> tokens = null;
            try
            {
                tokens = Lexer.Lex(templateString).GetEnumerator();
                this.Components = Parser.Parse(tokens).ToImmutableArray();
            }
            finally
            {
                if (tokens != null)
                {
                    tokens.Dispose();
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UriTemplate"/> class.
        /// </summary>
        /// <param name="components">The components of the URI template.</param>
        private UriTemplate(IEnumerable<IComponent> components)
        {
            Contract.Requires(components != null);

            this.Components = components.ToImmutableArray();
        }

        /// <summary>
        /// Gets the text representation of this URI template.
        /// </summary>
        string IUriTemplate.Text
        {
            get { return this.ToString(); }
        }

        /// <summary>
        /// Gets a collection of bound variables and their values.
        /// </summary>
        public ILookup<string, string> Values
        {
            get { return NoBoundVariables; }
        }

        /// <summary>
        /// Gets or sets the components of this URI template.
        /// </summary>
        private ImmutableArray<IComponent> Components { get; set; }

        /// <summary>
        /// Creates a new URI template with the specified variable bound to a specified value.
        /// </summary>
        /// <param name="name">The name of the variable to bind.</param>
        /// <param name="value">The value of the variable.</param>
        /// <returns>A new URI template with the specified variable bound to the specified value.</returns>
        public IUriTemplate Bind(string name, string value)
        {
            return this.Bind(name, Enumerable.Repeat(value, 1));
        }

        /// <summary>
        /// Creates a new URI template with the specified variable bound to a set of specified values.
        /// </summary>
        /// <param name="name">The name of the variable to bind.</param>
        /// <param name="values">The values of the variable.</param>
        /// <returns>A new URI template with the specified variable bound to the specified values.</returns>
        public IUriTemplate Bind(string name, IEnumerable<string> values)
        {
            return new BoundUriTemplate(this, name, values);
        }

        /// <summary>
        /// Creates a new URI template by partially expanding this template.
        /// </summary>
        /// <returns>A new URI template containing only the unbound variables from this template.</returns>
        public IUriTemplate CreateTemplate()
        {
            return this;
        }

        /// <summary>
        /// Expands the URI template.
        /// </summary>
        /// <returns>A <see cref="Uri"/> produced by expanding the URI template.</returns>
        public Uri Expand()
        {
            return this.Expand(NoBoundVariables);
        }

        /// <summary>
        /// Expands the URI template.
        /// </summary>
        /// <param name="values">The values of variables to use when expanding the template.</param>
        /// <returns>A <see cref="Uri"/> produced by expanding the URI template.</returns>
        public Uri Expand(ILookup<string, string> values)
        {
            var expansion = this.Components.Aggregate(
                new StringBuilder(),
                (buffer, component) => buffer.Append(component.Expand(values))).ToString();

            return new Uri(expansion, UriKind.RelativeOrAbsolute);
        }

        /// <summary>
        /// Gets a string that represents the URI template.
        /// </summary>
        /// <returns>A string that represents the URI template.</returns>
        public override string ToString()
        {
            return this.Components.Aggregate(
                new StringBuilder(),
                (buffer, component) => buffer.Append(component)).ToString();
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
            result = null;

            var candidate = uri == null ? string.Empty : uri.ToString();

            if (this.Components.Length == 0 != string.IsNullOrEmpty(candidate))
            {
                return false;
            }

            var candidateIndex = 0;
            var componentIndex = 0;
            var variables = new List<KeyValuePair<string, string>>();

            var queryComponentsIndex = candidate.IndexOf('?');
            if (queryComponentsIndex == -1)
            {
                queryComponentsIndex = candidate.Length;
            }

            // Process the path segments
            while (candidateIndex < queryComponentsIndex && componentIndex < this.Components.Length)
            {
                string componentText;
                var component = this.Components[componentIndex++];
                var expression = component as Expression;
                if (expression == null)
                {
                    componentText = component.ToString();
                    if (string.CompareOrdinal(candidate, candidateIndex, componentText, 0, componentText.Length) != 0)
                    {
                        return false;
                    }
                }
                else
                {
                    var nextIndex = candidate.IndexOf('/', candidateIndex);
                    componentText = nextIndex == -1 ?
                        candidate.Substring(candidateIndex) :
                        candidate.Substring(candidateIndex, nextIndex - candidateIndex);

                    variables.AddRange(
                        componentText.Split(',')
                            .Select(value => new KeyValuePair<string, string>(expression.Varspecs[0].Name, value)));
                }

                candidateIndex += componentText.Length;
            }

            // Process the query components
            while (candidateIndex < candidate.Length && componentIndex < this.Components.Length)
            {
                string componentText;
                var component = this.Components[componentIndex++];
                var expression = component as Expression;
                if (expression == null)
                {
                    componentText = component.ToString();
                    if (string.CompareOrdinal(candidate, candidateIndex, componentText, 0, componentText.Length) != 0)
                    {
                        return false;
                    }

                    candidateIndex += componentText.Length;
                }
                else
                {
                    var varspec = expression.Varspecs[0];
                    if (expression.Operator == null)
                    {
                        // Match up to the next query component.
                        var nextIndex = candidate.IndexOf('&', candidateIndex);
                        var value = nextIndex == -1 ?
                            candidate.Substring(candidateIndex) :
                            candidate.Substring(candidateIndex, nextIndex - candidateIndex);

                        variables.AddRange(
                            value.Split(',').Select(v => new KeyValuePair<string, string>(varspec.Name, v)));

                        candidateIndex += value.Length;
                    }
                    else
                    {
                        componentText = expression.Operator + varspec.Name + "=";
                        for (var varspecIndex = 0;;)
                        {
                            while (string.CompareOrdinal(
                                candidate,
                                candidateIndex,
                                componentText,
                                0,
                                componentText.Length) == 0)
                            {
                                candidateIndex += componentText.Length;
                                var nextIndex = candidate.IndexOf('&', candidateIndex);
                                var value = nextIndex == -1 ?
                                    candidate.Substring(candidateIndex) :
                                    candidate.Substring(candidateIndex, nextIndex - candidateIndex);
                                candidateIndex += value.Length;

                                if (varspec.IsExploded)
                                {
                                    variables.Add(new KeyValuePair<string, string>(varspec.Name, value));
                                    componentText = '&' + varspec.Name + "=";
                                }
                                else
                                {
                                    variables.AddRange(
                                        value.Split(',')
                                            .Select(v => new KeyValuePair<string, string>(varspec.Name, v)));
                                    break;
                                }
                            }

                            if (++varspecIndex == expression.Varspecs.Length)
                            {
                                break;
                            }

                            varspec = expression.Varspecs[varspecIndex];
                            componentText = "&" + varspec.Name + "=";
                        }
                    }
                }
            }

            result = new BoundUriTemplate(
                this,
                variables.ToLookup(variable => variable.Key, variable => variable.Value));

            return true;
        }

        /// <summary>
        /// Determines whether a variable features in the <see cref="UriTemplate"/>.
        /// </summary>
        /// <param name="name">The name of the variable.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="name"/> is featured in the <see cref="UriTemplate"/>;
        /// otherwise, <see langword="false"/>.
        /// </returns>
        protected bool ContainsVariable(string name)
        {
            return this.Components.OfType<Expression>()
                .SelectMany(e => e.Varspecs)
                .Any(v => v.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }
    }
}
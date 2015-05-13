//----------------------------------------------------------------------------------------------------------------------
// <copyright file="UriTemplate.BoundUriTemplate.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------------------------------------------------
namespace Rakuten
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Represents a Uniform Resource Indicator (URI) template, as defined by RFC 6570.
    /// </summary>
    public partial class UriTemplate
    {
        /// <summary>
        /// Represents a Uniform Resource Indicator (URI) template (as defined by RFC 6570),
        /// for which values have been supplied for the variables.
        /// </summary>
        private sealed class BoundUriTemplate : IUriTemplate
        {
            /// <summary>
            /// The base URI template to which variables have been bound.
            /// </summary>
            private readonly UriTemplate baseTemplate;

            /// <summary>
            /// The currently bound variables and their values.
            /// </summary>
            private readonly ILookup<string, string> boundValues;

            /// <summary>
            /// Initializes a new instance of the <see cref="BoundUriTemplate"/> class.
            /// </summary>
            /// <param name="template">The URI template to bind.</param>
            /// <param name="name">The name of the variable to bind.</param>
            /// <param name="values">The values of the bound variable.</param>
            public BoundUriTemplate(UriTemplate template, string name, IEnumerable<string> values)
                : this(template, values.ToLookup(value => name, value => value))
            {
                Contract.Requires(template != null);
                Contract.Requires(values != null);
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="BoundUriTemplate"/> class.
            /// </summary>
            /// <param name="template">The URI template to bind.</param>
            /// <param name="boundValues">The variables to bind and their values.</param>
            public BoundUriTemplate(UriTemplate template, ILookup<string, string> boundValues)
            {
                Contract.Requires(template != null);
                Contract.Requires(boundValues != null);

                this.baseTemplate = template;
                this.boundValues = boundValues;
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="BoundUriTemplate"/> class.
            /// </summary>
            /// <param name="template">The URI template to bind.</param>
            /// <param name="name">The name of the variable to bind.</param>
            /// <param name="values">The values of the bound variable.</param>
            private BoundUriTemplate(BoundUriTemplate template, string name, IEnumerable<string> values)
            {
                Contract.Requires(template != null);
                Contract.Requires(template.boundValues != null);
                Contract.Requires(template.baseTemplate != null);

                var variables = from variable in template.boundValues
                                from value in variable
                                select new KeyValuePair<string, string>(variable.Key, value);

                if (template.boundValues.Contains(name))
                {
                    variables = variables.Where(
                        variable => !variable.Key.Equals(name, StringComparison.OrdinalIgnoreCase));
                }

                if (values != null && template.baseTemplate.ContainsVariable(name))
                {
                    variables = variables.Concat(
                        values
                            .Where(value => value != null)
                            .Select(value => new KeyValuePair<string, string>(name, value)));
                }

                this.baseTemplate = template.baseTemplate;
                this.boundValues = variables.ToLookup(variable => variable.Key, variable => variable.Value);
            }

            /// <summary>
            /// Gets the text representation of this URI template.
            /// </summary>
            public string Text
            {
                get { return this.baseTemplate.ToString(); }
            }

            /// <summary>
            /// Gets a collection of bound variables and their values.
            /// </summary>
            public ILookup<string, string> Values
            {
                get { return this.boundValues; }
            }

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
            /// Creates a new URI template with the specified variable bound to a specified value.
            /// </summary>
            /// <param name="name">The name of the variable to bind.</param>
            /// <param name="values">The values of the variable.</param>
            /// <returns>A new URI template with the specified variable bound to the specified value.</returns>
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
                return new UriTemplate(this.CreateComponents());
            }

            /// <summary>
            /// Expands the URI template.
            /// </summary>
            /// <returns>A <see cref="Uri"/> produced by expanding the URI template.</returns>
            public Uri Expand()
            {
                return this.baseTemplate.Expand(this.boundValues);
            }

            /// <summary>
            /// Expands the URI template.
            /// </summary>
            /// <param name="values">The values of variables to use when expanding the template.</param>
            /// <returns>A <see cref="Uri"/> produced by expanding the URI template.</returns>
            public Uri Expand(ILookup<string, string> values)
            {
                return this.baseTemplate.Expand(values);
            }

            /// <summary>
            /// Gets a string that represents the URI template.
            /// </summary>
            /// <returns>A string that represents the URI template.</returns>
            public override string ToString()
            {
                return this.Text;
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
            bool IUriTemplate.TryBind(Uri uri, out IUriTemplate result)
            {
                return this.baseTemplate.TryBind(uri, out result);
            }

            /// <summary>
            /// Creates a collection of components by partially expanding this template.
            /// </summary>
            /// <returns>A collection of components containing only the unbound variables from this template.</returns>
            private IEnumerable<IComponent> CreateComponents()
            {
                var text = new StringBuilder();
                foreach (var component in this.baseTemplate.Components)
                {
                    var expression = component as Expression;
                    if (expression == null)
                    {
                        text.Append(component);
                    }
                    else if (expression.Varspecs.All(varspec => this.boundValues.Contains(varspec.Name)))
                    {
                        text.Append(expression.Expand(this.boundValues));
                    }
                    else if (!expression.Varspecs.Any(varspec => this.boundValues.Contains(varspec.Name)))
                    {
                        if (text.Length != 0)
                        {
                            yield return new Literal(text.ToString());
                            text.Clear();
                        }

                        yield return expression;
                    }
                    else if (expression.Operator == null)
                    {
                        throw new InvalidOperationException(UriTemplateErrorMessages.MissingOperator);
                    }
                    else
                    {
                        Varspec varspec;
                        var index = 0;
                        if (expression.Operator == '?')
                        {
                            varspec = expression.Varspecs[0];
                            if (!this.boundValues.Contains(varspec.Name))
                            {
                                // We can't create new templates from expressions such as {?a,b,c}
                                // unless the first parameter is bound to a value.
                                throw new InvalidOperationException(UriTemplateErrorMessages.FirstValueMissing);
                            }

                            text.Append(expression.Operator).Append(varspec.Expand(expression.Operator, this.boundValues));

                            if (expression.Varspecs.Length == 1)
                            {
                                yield return new Literal(text.ToString());
                                yield break;
                            }

                            index = 1;
                        }

                        while (index < expression.Varspecs.Length)
                        {
                            for (varspec = expression.Varspecs[index]; this.boundValues.Contains(varspec.Name);)
                            {
                                text.Append("&").Append(varspec.Expand(expression.Operator, this.boundValues));
                                if (++index == expression.Varspecs.Length)
                                {
                                    yield return new Literal(text.ToString());
                                    yield break;
                                }

                                varspec = expression.Varspecs[index];
                            }

                            yield return new Literal(text.ToString());
                            text.Clear();

                            var varspecs = new List<Varspec>();
                            do
                            {
                                varspecs.Add(varspec);
                                if (++index == expression.Varspecs.Length)
                                {
                                    yield return new Expression('&', varspecs);
                                    yield break;
                                }

                                varspec = expression.Varspecs[index];
                            }
                            while (!this.boundValues.Contains(varspec.Name));

                            yield return new Expression('&', varspecs);
                            varspecs.Clear();
                        }
                    }
                }

                if (text.Length != 0)
                {
                    yield return new Literal(text.ToString());
                }
            }
        }
    }
}
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
    using System.Web;

    /// <summary>
    /// Represents a Uniform Resource Indicator (URI) template, as defined by RFC 6570.
    /// </summary>
    public class UriTemplate : IUriTemplate
    {
        /// <summary>
        /// An empty collection of bound variables.
        /// </summary>
        private static readonly ILookup<string, string> NoBoundVariables =
            Enumerable.Empty<string>().ToLookup(s => s, s => s);

        /// <summary>
        /// The currently bound variables and their values.
        /// </summary>
        private readonly ILookup<string, string> boundVariables;

        /// <summary>
        /// The path segments that make up the current URI template.
        /// </summary>
        private readonly ImmutableArray<IUriTemplateComponent> pathSegments;

        /// <summary>
        /// The mandatory query string parameters that are part of the current URI template.
        /// </summary>
        private readonly ImmutableArray<KeyValuePair<string, IVarSpec>> mandatoryQueryStringParameters;

        /// <summary>
        /// The optional query string parameters that may be supplied with the current URI template.
        /// </summary>
        private readonly ImmutableArray<KeyValuePair<string, IVarSpec>> optionalQueryStringParameters;

        /// <summary>
        /// Initializes a new instance of the <see cref="UriTemplate"/> class.
        /// </summary>
        /// <param name="pathSegments">The paths segments that make up the URI template.</param>
        /// <param name="mandatoryQueryStringParameters">
        /// The mandatory query string parameters that are part of the URI template.
        /// </param>
        /// <param name="optionalQueryStringParameters">
        /// The optional query string parameters that are part of the URI template.
        /// </param>
        public UriTemplate(
            IEnumerable<IUriTemplateComponent> pathSegments,
            IEnumerable<KeyValuePair<string, IVarSpec>> mandatoryQueryStringParameters,
            IEnumerable<KeyValuePair<string, IVarSpec>> optionalQueryStringParameters)
            : this(pathSegments, mandatoryQueryStringParameters, optionalQueryStringParameters, NoBoundVariables)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UriTemplate"/> class with the specified value.
        /// </summary>
        /// <param name="value">The URI template.</param>
        public UriTemplate(string value)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(value));

            var parsedPathSegments = new List<IUriTemplateComponent>();
            var parsedMandatoryQueryStringParameters = new List<KeyValuePair<string, IVarSpec>>();
            var parsedOptionalQueryStringParameters = new List<KeyValuePair<string, IVarSpec>>();

            var isQueryString = false;
            var isOptional = false;

            int startPosition = 0;

            int i;
            string text = null;

            for (i = 0; i < value.Length;)
            {
                var c = value[i];
                if (c == '{')
                {
                    if (++i == value.Length)
                    {
                        throw UnsupportedUriTemplateError();
                    }

                    startPosition++;

                    // Varspec
                    if (startPosition != i)
                    {
                        // There are only two cases in which varspecs may be adjacent:
                        //   {path}{?param}
                        //   {path}?param1={param1}{&param2}
                        if (!(((c = value[i]) == '&' && isQueryString && !isOptional) || (!isQueryString && c == '?')) ||
                            ++i == value.Length)
                        {
                            throw UnsupportedUriTemplateError();
                        }

                        if (!isQueryString)
                        {
                            parsedPathSegments.Add(new Literal(value.Substring(startPosition - 1, i - startPosition - 1)));
                            isQueryString = true;
                        }

                        isOptional = true;
                        startPosition = i;
                    }

                    bool isExploded = false;
                    for (c = value[i];; c = value[i])
                    {
                        if (c == '*')
                        {
                            if (++i == value.Length)
                            {
                                throw UnsupportedUriTemplateError();
                            }

                            isExploded = true;
                            c = value[i];
                        }

                        if (c == '}')
                        {
                            int length = isExploded ? i - startPosition - 1 : i - startPosition;
                            string variableName = value.Substring(startPosition, length);

                            if (isQueryString)
                            {
                                if (isOptional)
                                {
                                    parsedOptionalQueryStringParameters.Add(
                                        new KeyValuePair<string, IVarSpec>(variableName, new VarSpec(variableName, isExploded)));
                                }
                                else
                                {
                                    parsedMandatoryQueryStringParameters.Add(
                                        new KeyValuePair<string, IVarSpec>(text, new VarSpec(variableName, isExploded)));
                                }

                                startPosition = i++;
                            }
                            else
                            {
                                bool hasTrailingSlash = ++i != value.Length;

                                if (hasTrailingSlash)
                                {
                                    switch (value[i])
                                    {
                                        case '?':
                                            isQueryString = true;
                                            hasTrailingSlash = false;
                                            break;
                                        case '/':
                                            break;
                                        default:
                                            throw UnsupportedUriTemplateError();
                                    }

                                    startPosition = ++i;
                                }
                                else
                                {
                                    startPosition = i++;
                                }

                                parsedPathSegments.Add(new VariablePathSegment(variableName, isExploded, hasTrailingSlash));
                            }

                            break;
                        }

                        if (c == ',')
                        {
                            if (!isQueryString || !isOptional)
                            {
                                throw UnsupportedUriTemplateError();
                            }

                            int length = isExploded ? i - startPosition - 1 : i - startPosition;

                            string variableName = value.Substring(startPosition, length);

                            parsedOptionalQueryStringParameters.Add(
                                new KeyValuePair<string, IVarSpec>(variableName, new VarSpec(variableName, isExploded)));

                            if ((startPosition = ++i) == value.Length)
                            {
                                throw UnsupportedUriTemplateError();
                            }

                            continue;
                        }

                        if (++i == value.Length || isExploded)
                        {
                            throw UnsupportedUriTemplateError();
                        }
                    }
                }
                else if (c == '?')
                {
                    if (isQueryString)
                    {
                        throw UnsupportedUriTemplateError();
                    }

                    if (startPosition != i)
                    {
                        parsedPathSegments.Add(new Literal(value.Substring(startPosition, i - startPosition)));
                    }

                    isQueryString = true;
                    startPosition = ++i;
                }
                else
                {
                    // Literal
                    if (isQueryString)
                    {
                        if (isOptional)
                        {
                            throw UnsupportedUriTemplateError();
                        }

                        if (c == '&')
                        {
                            if ((startPosition = ++i) == value.Length)
                            {
                                throw UnsupportedUriTemplateError();
                            }

                            continue;
                        }

                        // Read up to equals into name
                        for (; c != '='; c = value[i])
                        {
                            if (++i == value.Length)
                            {
                                throw UnsupportedUriTemplateError();
                            }
                        }

                        text = value.Substring(startPosition, i - startPosition);

                        if ((startPosition = ++i) == value.Length || value[i] != '{')
                        {
                            throw UnsupportedUriTemplateError();
                        }

                        continue;
                    }

                    if (c == '/')
                    {
                        parsedPathSegments.Add(new Literal(value.Substring(startPosition, ++i - startPosition)));
                        startPosition = i;
                    }
                    else if (++i == value.Length)
                    {
                        text = value.Substring(startPosition);
                        parsedPathSegments.Add(new Literal(text));
                        break;
                    }
                }
            }

            this.boundVariables = NoBoundVariables;
            this.mandatoryQueryStringParameters = parsedMandatoryQueryStringParameters.ToImmutableArray();
            this.optionalQueryStringParameters = parsedOptionalQueryStringParameters.ToImmutableArray();
            this.pathSegments = parsedPathSegments.ToImmutableArray();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UriTemplate" /> class.
        /// </summary>
        /// <param name="pathSegments">The paths segments that make up the URI template.</param>
        /// <param name="mandatoryQueryStringParameters">
        /// The mandatory query string parameters that are part of the URI template.
        /// </param>
        /// <param name="optionalQueryStringParameters">
        /// The optional query string parameters that are part of the URI template.
        /// </param>
        /// <param name="boundVariables">The currently bound variables and their values.</param>
        private UriTemplate(
            IEnumerable<IUriTemplateComponent> pathSegments,
            IEnumerable<KeyValuePair<string, IVarSpec>> mandatoryQueryStringParameters,
            IEnumerable<KeyValuePair<string, IVarSpec>> optionalQueryStringParameters,
            ILookup<string, string> boundVariables)
        {
            this.boundVariables = boundVariables;
            this.mandatoryQueryStringParameters = ToImmutableArray(mandatoryQueryStringParameters);
            this.optionalQueryStringParameters = ToImmutableArray(optionalQueryStringParameters);
            this.pathSegments = ToImmutableArray(pathSegments);
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
        /// Creates a new URI template with the specified variable bound to the specified values.
        /// </summary>
        /// <param name="name">The name of the variable to bind.</param>
        /// <param name="values">The values of the variable.</param>
        /// <returns>A new URI template with the specified variable bound to the specified values.</returns>
        public IUriTemplate Bind(string name, IEnumerable<string> values)
        {
            var variables = from variable in this.boundVariables
                            from value in variable
                            select new KeyValuePair<string, string>(variable.Key, value);

            if (this.boundVariables.Contains(name))
            {
                variables = variables.Where(v => v.Key != name);

                if (values != null)
                {
                    variables = variables.Concat(
                        values.Where(v => v != null).Select(v => new KeyValuePair<string, string>(name, v)));
                }
            }
            else if (!this.ContainsVariable(name))
            {
                throw new ArgumentException(
                    message: string.Format("Name '{0}' must be defined within the URI template", name),
                    paramName: "name");
            }
            else
            {
                if (values == null)
                {
                    return this;
                }

                variables = variables.Concat(
                    values.Where(v => v != null).Select(v => new KeyValuePair<string, string>(name, v)));
            }

            return new UriTemplate(
                this.pathSegments,
                this.mandatoryQueryStringParameters,
                this.optionalQueryStringParameters,
                variables.ToLookup(v => v.Key, v => v.Value));
        }

        /// <summary>
        /// Determines whether a variable is defined in the <see cref="UriTemplate"/>.
        /// </summary>
        /// <param name="name">The name of the variable.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="name"/> is defined in the <see cref="UriTemplate"/>;
        /// otherwise, <see langword="false"/>.
        /// </returns>
        public bool ContainsVariable(string name)
        {
            return this.pathSegments.Any(s => { var v = s as IVarSpec; return v != null && v.Name == name; }) ||
                   this.mandatoryQueryStringParameters.Any(q => q.Value.Name == name) ||
                   this.optionalQueryStringParameters.Any(q => q.Value.Name == name);
        }

        /// <summary>
        /// Attempts to match a <see cref="Uri"/> to a <see cref="IUriTemplate"/>.
        /// </summary>
        /// <param name="candidate">The <see cref="Uri"/> to match against the template.</param>
        /// <returns>A collection of key/value pairs representing bound variables and their values.</returns>
        public ILookup<string, string> Match(Uri candidate)
        {
            try
            {
                return this.MatchComponents(candidate).ToLookup(v => v.Key, v => v.Value);
            }
            catch (UriTemplateNotMatchedException)
            {
                return null;
            }
        }

        /// <summary>
        /// Gets a representation of the template without attempting to resolve any bound variables.
        /// </summary>
        /// <returns>
        /// A string that represents the current URI template (without any attempt to resolve bound variables).
        /// </returns>
        public string GetTemplateString()
        {
            var buffer = new StringBuilder();

            foreach (var segment in this.pathSegments)
            {
                buffer.Append(segment.Text);
            }

            bool hasMandatoryParameters = this.mandatoryQueryStringParameters.Length != 0;
            bool hasOptionalParameters = this.optionalQueryStringParameters.Length != 0;

            if (hasMandatoryParameters)
            {
                buffer.Append("?");
                buffer.Append(
                    string.Join(
                        "&",
                        this.mandatoryQueryStringParameters.Select(
                            parameter => parameter.Key + "=" + parameter.Value.Text)));
            }

            if (hasOptionalParameters)
            {
                buffer.Append(hasMandatoryParameters ? "{&" : "{?");
                buffer.Append(string.Join(",", this.optionalQueryStringParameters.Select(parameter => parameter.Key)));
                buffer.Append("}");
            }

            return buffer.ToString();
        }

        /// <summary>
        /// Returns a string that represents the current URI template, attempting to resolve any bound variables.
        /// </summary>
        /// <returns>A string that represents the current URI template.</returns>
        public override string ToString()
        {
            var buffer = new StringBuilder();

            foreach (var segment in this.pathSegments)
            {
                var varspec = segment as IUriTemplateVariableComponent;

                buffer.Append(
                    varspec == null ?
                        segment.Text : varspec.ToString(string.Join(",", this.boundVariables[varspec.Name])));
            }

            var hasParameters = false;

            foreach (var parameter in this.mandatoryQueryStringParameters)
            {
                var parameterName = parameter.Key;
                var variable = parameter.Value;
                var values = this.boundVariables[variable.Name];

                if (variable.IsExploded)
                {
                    var hasValue = false;
                    foreach (var value in values)
                    {
                        if (hasParameters)
                        {
                            buffer.Append("&");
                        }
                        else
                        {
                            buffer.Append("?");
                            hasParameters = true;
                        }

                        buffer.AppendFormat("{0}={1}", parameterName, value);
                        hasValue = true;
                    }

                    if (!hasValue)
                    {
                        buffer.AppendFormat("{0}=", parameterName);
                    }
                }
                else
                {
                    if (hasParameters)
                    {
                        buffer.Append("&");
                    }
                    else
                    {
                        buffer.Append("?");
                        hasParameters = true;
                    }

                    buffer.AppendFormat("{0}={1}", parameterName, string.Join(",", values));
                }
            }

            foreach (var parameter in this.optionalQueryStringParameters)
            {
                var parameterName = parameter.Key;
                var variable = parameter.Value;
                var values = this.boundVariables[variable.Name];

                if (variable.IsExploded)
                {
                    foreach (var value in values)
                    {
                        if (hasParameters)
                        {
                            buffer.Append("&");
                        }
                        else
                        {
                            buffer.Append("?");
                            hasParameters = true;
                        }

                        buffer.AppendFormat("{0}={1}", parameterName, value);
                    }
                }
                else
                {
                    var value = string.Join(",", values);
                    if (!string.IsNullOrWhiteSpace(value))
                    {
                        if (hasParameters)
                        {
                            buffer.Append("&");
                        }
                        else
                        {
                            buffer.Append("?");
                            hasParameters = true;
                        }

                        buffer.AppendFormat("{0}={1}", parameterName, value);
                    }
                }
            }

            return buffer.ToString();
        }

        /// <summary>
        /// Returns a string that represents the current URI template.
        /// </summary>
        /// <param name="resolveTemplate">
        /// <see langword="true"/> if the string representation should be resolved, with no unspecified parameters;
        /// otherwise, <see langword="false"/>.
        /// </param>
        /// <returns>A string that represents the current URI template.</returns>
        public string ToString(bool resolveTemplate)
        {
            return resolveTemplate ? this.ToString() : this.GetTemplateString();
        }

        /// <summary>
        /// Converts a collection of values to an immutable array.
        /// </summary>
        /// <typeparam name="T">The type of element stored by the array.</typeparam>
        /// <param name="values">The collection of values.</param>
        /// <returns>An immutable array containing the specified values.</returns>
        private static ImmutableArray<T> ToImmutableArray<T>(IEnumerable<T> values)
        {
            return values == null ? ImmutableArray<T>.Empty : values.ToImmutableArray();
        }

        /// <summary>
        /// Gets the exception that is thrown when an attempt is made
        /// to construct an unsupported or invalid URI template.
        /// </summary>
        /// <param name="name">The name of the URI template parameter.</param>
        /// <returns>
        /// The exception that is thrown when an attempt is made
        /// to construct an unsupported or invalid URI template.
        /// </returns>
        private static ArgumentException UnsupportedUriTemplateError(string name = "value")
        {
            return new ArgumentException("URI template is unsupported or malformed.", name);
        }

        /// <summary>
        /// Attempts to match a <see cref="Uri"/> to a <see cref="IUriTemplate"/>.
        /// </summary>
        /// <param name="candidateUri">The <see cref="Uri"/> to match against the template.</param>
        /// <returns>A collection of key/value pairs representing bound variables and their values.</returns>
        private IEnumerable<KeyValuePair<string, string>> MatchComponents(Uri candidateUri)
        {
            var absoluteUri = candidateUri.IsAbsoluteUri ?
                candidateUri :
                new Uri(new Uri("http://api.rakuten.com"), candidateUri);

            var i = absoluteUri.Segments.Length;
            if (this.pathSegments.Length != i)
            {
                throw new UriTemplateNotMatchedException();
            }

            while (i != 0)
            {
                var candidateSegment = absoluteUri.Segments[--i];
                var pathSegment = this.pathSegments[i];

                var varspec = pathSegment as VarSpec;
                if (varspec == null)
                {
                    var pathSegmentText = pathSegment.Text;

                    var pathSegmentLength = pathSegmentText.Length;
                    if (pathSegmentLength != candidateSegment.Length)
                    {
                        throw new UriTemplateNotMatchedException();
                    }

                    for (var j = 0; j < pathSegmentLength; j++)
                    {
                        var segmentCharacter = pathSegmentText[j];
                        var candidateCharacter = candidateSegment[j];

                        if ((candidateCharacter == segmentCharacter) ||
                           (((candidateCharacter >= 'a' && candidateCharacter <= 'z') ||
                            (candidateCharacter >= 'A' && candidateCharacter <= 'Z'))
                            && (candidateCharacter - segmentCharacter == 32)))
                        {
                            continue;
                        }

                        throw new UriTemplateNotMatchedException();
                    }
                }
                else
                {
                    foreach (var value in candidateSegment.TrimEnd('/').Split(','))
                    {
                        yield return new KeyValuePair<string, string>(varspec.Name, value);
                    }
                }
            }

            var candidateParameters = HttpUtility.ParseQueryString(absoluteUri.Query);

            foreach (var parameter in this.mandatoryQueryStringParameters)
            {
                var candidateParameter = candidateParameters[parameter.Key];

                if (candidateParameter == null)
                {
                    throw new UriTemplateNotMatchedException();
                }

                foreach (var value in candidateParameter.Split(','))
                {
                    yield return new KeyValuePair<string, string>(parameter.Key, value);
                }
            }

            foreach (var parameter in this.optionalQueryStringParameters)
            {
                var candidateParameter = candidateParameters[parameter.Key];

                if (candidateParameter != null)
                {
                    foreach (var value in candidateParameter.Split(','))
                    {
                        yield return new KeyValuePair<string, string>(parameter.Key, value);
                    }
                }
            }
        }

        /// <summary>
        /// The exception that is thrown when a URI does not match the specified template.
        /// </summary>
        private class UriTemplateNotMatchedException : Exception
        {
        }
    }
}
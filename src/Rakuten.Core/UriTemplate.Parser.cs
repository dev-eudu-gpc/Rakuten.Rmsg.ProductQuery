//----------------------------------------------------------------------------------------------------------------------
// <copyright file="UriTemplate.Parser.cs" company="Rakuten">
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
        /// Provides functionality for parsing a stream of URI template tokens.
        /// </summary>
        private static class Parser
        {
            /// <summary>
            /// Parses a stream of URI template tokens, returning a collection of URI template components.
            /// </summary>
            /// <param name="tokens">The URI template tokens to parse.</param>
            /// <returns>A collection of URI template components built from the supplied tokens.</returns>
            public static IEnumerable<IComponent> Parse(IEnumerator<Token> tokens)
            {
                Contract.Requires(tokens != null);

                if (!tokens.MoveNext())
                {
                    yield break;
                }

                var variableNames = new HashSet<string>();
                for (var context = new ParsingContext();;)
                {
                    var token = tokens.Current;
                    switch (token.TokenType)
                    {
                        case TokenType.EndOfTemplate:
                            yield break;
                        case TokenType.RightBrace:
                            throw new FormatException(
                                string.Format(UriTemplateErrorMessages.UnexpectedCharacters, token));
                    }

                    if (context.Operator != null || token.TokenType == TokenType.LeftBrace)
                    {
                        var expressionContext = ParseExpression(context, tokens);
                        context = expressionContext;
                        var expression = expressionContext.Value;
                        if (expression.Varspecs.Length != 0)
                        {
                            var repeatedVariable = expression.Varspecs.FirstOrDefault(
                                varspec => !variableNames.Add(varspec.Name));

                            if (repeatedVariable != null)
                            {
                                throw new FormatException(
                                    string.Format(UriTemplateErrorMessages.RepeatedVariableName, repeatedVariable.Name));
                            }

                            yield return expressionContext.Value;
                        }
                    }
                    else
                    {
                        var literalContext = ParseLiteral(context, tokens);
                        context = literalContext;
                        yield return literalContext.Value;
                    }
                }
            }

            /// <summary>
            /// Parses an expression from a stream of URI template tokens.
            /// </summary>
            /// <param name="context">Information about the current state of the parsing operation.</param>
            /// <param name="tokens">The URI template tokens to parse.</param>
            /// <returns>
            /// A <see cref="ParsingContext{Expression}"/> instance providing the parsed expression
            /// together with information about the current state of the parsing operation.
            /// </returns>
            private static ParsingContext<Expression> ParseExpression(
                ParsingContext context,
                IEnumerator<Token> tokens)
            {
                Contract.Requires(context != null);
                Contract.Requires(tokens != null);

                var token = tokens.Read();

                var isQueryComponent = context.IsQueryComponent;
                var isPartialSegment = context.IsPartialSegment;
                Operator op = context.Operator;

                if (token.TokenType == TokenType.QuestionMark)
                {
                    if (op != null)
                    {
                        throw new FormatException(UriTemplateErrorMessages.MultipleOperators);
                    }

                    if (isQueryComponent)
                    {
                        // The template contains two question marks, e.g. "/test?test={?value}"; this is not supported.
                        throw new FormatException(UriTemplateErrorMessages.MultipleQuestionMarks);
                    }

                    op = new Operator('?');
                    isQueryComponent = true;
                    tokens.Read();
                }
                else if (token.TokenType == TokenType.Ampersand)
                {
                    if (op != null)
                    {
                        throw new FormatException(UriTemplateErrorMessages.MultipleOperators);
                    }

                    op = new Operator('&');
                    tokens.Read();
                }

                if (!isQueryComponent && op != null)
                {
                    throw new FormatException(UriTemplateErrorMessages.OperatorInNonQueryExpression);
                }

                if (!isQueryComponent && context.IsPartialSegment && op != '?')
                {
                    throw new FormatException(UriTemplateErrorMessages.MixedLiteralAndVariableComponents);
                }

                context = new ParsingContext(isPartialSegment, isQueryComponent, op);
                var varspecContext = ParseVarspecs(context, tokens);

                return new ParsingContext<Expression>(
                    varspecContext.IsPartialSegment,
                    varspecContext.IsQueryComponent,
                    varspecContext.Operator,
                    new Expression(op, varspecContext.Value));
            }

            /// <summary>
            /// Parses a literal from a stream of URI template tokens.
            /// </summary>
            /// <param name="context">Information about the current state of the parsing operation.</param>
            /// <param name="tokens">The URI template tokens to parse.</param>
            /// <returns>
            /// A <see cref="ParsingContext{Literal}"/> instance providing the parsed literal
            /// together with information about the current state of the parsing operation.
            /// </returns>
            private static ParsingContext<Literal> ParseLiteral(ParsingContext context, IEnumerator<Token> tokens)
            {
                Contract.Requires(context != null);
                Contract.Requires(tokens != null);

                var text = new StringBuilder();
                var isQueryComponent = context.IsQueryComponent;
                var isPartialSegment = true;
                do
                {
                    var token = tokens.Current;
                    switch (token.TokenType)
                    {
                        case TokenType.Ampersand:
                        case TokenType.Asterisk:
                        case TokenType.Comma:
                        case TokenType.Literal:
                            isPartialSegment = true;
                            break;
                        case TokenType.EqualsSign:
                            isPartialSegment = !isQueryComponent;
                            break;
                        case TokenType.QuestionMark:
                            isPartialSegment = false;
                            isQueryComponent = true;
                            break;
                        case TokenType.Slash:
                            isPartialSegment = false;
                            break;
                        case TokenType.RightBrace:
                            throw new FormatException(
                                string.Format(UriTemplateErrorMessages.UnexpectedCharacters, token));
                        case TokenType.LeftBrace:
                            return new ParsingContext<Literal>(
                                isPartialSegment,
                                isQueryComponent,
                                value: new Literal(text.ToString()));
                    }

                    text.Append(token);
                }
                while (tokens.MoveNext());

                return new ParsingContext<Literal>(
                    isPartialSegment,
                    isQueryComponent,
                    value: new Literal(text.ToString()));
            }

            /// <summary>
            /// Parses a collection of variable specifiers from a stream of URI template tokens.
            /// </summary>
            /// <param name="context">Information about the current state of the parsing operation.</param>
            /// <param name="tokens">The URI template tokens to parse.</param>
            /// <returns>
            /// A <see cref="ParsingContext{T}"/> of <see cref="List{Varspec}"/> providing the parsed
            /// variable specifiers together with information about the current state of the parsing operation.
            /// </returns>
            private static ParsingContext<List<Varspec>> ParseVarspecs(
                ParsingContext context,
                IEnumerator<Token> tokens)
            {
                Contract.Requires(context != null);
                Contract.Requires(tokens != null);

                var varspecs = new List<Varspec>();

                var isQueryComponent = context.IsQueryComponent;
                var op = context.Operator;

                for (var token = tokens.Current;; token = tokens.Read())
                {
                    var name = token.ToString();
                    if (token.TokenType != TokenType.Literal)
                    {
                        throw new FormatException(string.Format(UriTemplateErrorMessages.InvalidVariableName, name));
                    }

                    token = tokens.Read();

                    var isExploded = token.TokenType == TokenType.Asterisk;
                    if (isExploded)
                    {
                        token = tokens.Read();
                    }

                    if (token.TokenType == TokenType.RightBrace)
                    {
                        tokens.MoveNext();
                        varspecs.Add(new Varspec(name, isExploded));

                        // Are there any adjacent expressions that should be merged with this one?
                        token = tokens.Current;
                        if (token.TokenType == TokenType.LeftBrace)
                        {
                            if (!isQueryComponent)
                            {
                                throw new FormatException(UriTemplateErrorMessages.AdjacentVarspecs);
                            }

                            if (op == null)
                            {
                                // Adjacent expressions where the first expression lacks an operator whilst
                                // the second has one are valid, but cannot be merged (e.g. ?a={a}{&b}).
                                token = tokens.Read();
                                if (TryParse(token, out op))
                                {
                                    break;
                                }

                                throw new FormatException(UriTemplateErrorMessages.AdjacentVarspecs);
                            }

                            if (op == '?' || tokens.Read().TokenType != TokenType.Ampersand)
                            {
                                throw new FormatException(UriTemplateErrorMessages.AdjacentVarspecs);
                            }

                            tokens.Read();
                            var varspecContext = ParseVarspecs(context, tokens);

                            varspecs.AddRange(varspecContext.Value);
                            context = varspecContext;
                        }

                        break;
                    }

                    if (token.TokenType != TokenType.Comma)
                    {
                        throw new FormatException(string.Format(UriTemplateErrorMessages.UnexpectedCharacters, name));
                    }

                    if (op == null)
                    {
                        throw new FormatException(UriTemplateErrorMessages.MultipleVariablesWithoutOperator);
                    }

                    varspecs.Add(new Varspec(name, isExploded));
                }

                return new ParsingContext<List<Varspec>>(context.IsPartialSegment, isQueryComponent, op, varspecs);
            }

            /// <summary>
            /// Converts a URI template token to an operator. A return value indicates whether the conversion succeeded.
            /// </summary>
            /// <param name="token">A token representing an operator.</param>
            /// <param name="result">
            /// When this method returns, contains the operator equivalent to the token <paramref name="token"/>,
            /// if the conversion succeeded, or <see langword="null"/> if the conversion failed.
            /// The conversion fails if the token does not represent an operator. The parameter is passed uninitialized.
            /// </param>
            /// <returns>
            /// <see langword="true"/> if <paramref name="result"/> was converted successfully;
            /// otherwise, <see langword="false"/>.
            /// </returns>
            private static bool TryParse(Token token, out Operator result)
            {
                Contract.Requires(token != null);

                switch (token.TokenType)
                {
                    case TokenType.Ampersand:
                        result = new Operator('&');
                        return true;
                    case TokenType.QuestionMark:
                        result = new Operator('?');
                        return true;
                    default:
                        result = null;
                        return false;
                }
            }

            /// <summary>
            /// Provides information about the current state of the parsing operation.
            /// </summary>
            private class ParsingContext
            {
                /// <summary>
                /// Initializes a new instance of the <see cref="ParsingContext"/> class.
                /// </summary>
                /// <param name="isPartialSegment">
                /// A value indicating whether the parser has yet to complete parsing of a URI segment.
                /// </param>
                /// <param name="isQueryComponent">
                /// A value indicating whether the parser is currently parsing a query component.
                /// </param>
                /// <param name="op">The last operator encountered during the parsing operation.</param>
                public ParsingContext(
                    bool isPartialSegment = false,
                    bool isQueryComponent = false,
                    Operator op = null)
                {
                    this.IsPartialSegment = isPartialSegment;
                    this.IsQueryComponent = isQueryComponent;
                    this.Operator = op;
                }

                /// <summary>
                /// Gets a value indicating whether the parser has yet to complete parsing of a URI segment.
                /// </summary>
                public bool IsPartialSegment { get; private set; }

                /// <summary>
                /// Gets a value indicating whether the parser is currently parsing a query component.
                /// </summary>
                public bool IsQueryComponent { get; private set; }

                /// <summary>
                /// Gets the last operator encountered during the parsing operation.
                /// </summary>
                public Operator Operator { get; private set; }
            }

            /// <summary>
            /// Provides information about the current state of the parsing operation,
            /// together with the parsed URI component.
            /// </summary>
            /// <typeparam name="T">The type of URI component parsed.</typeparam>
            private class ParsingContext<T> : ParsingContext
            {
                /// <summary>
                /// Initializes a new instance of the <see cref="ParsingContext{T}"/> class.
                /// </summary>
                /// <param name="isPartialSegment">
                /// A value indicating whether the parser has yet to complete parsing of a URI segment.
                /// </param>
                /// <param name="isQueryComponent">
                /// A value indicating whether the parser is currently parsing a query component.
                /// </param>
                /// <param name="op">The last operator encountered during the parsing operation.</param>
                /// <param name="value">The parsed URI component.</param>
                public ParsingContext(
                    bool isPartialSegment = false,
                    bool isQueryComponent = false,
                    Operator op = null,
                    T value = default(T))
                    : base(isPartialSegment, isQueryComponent, op)
                {
                    this.Value = value;
                }

                /// <summary>
                /// Gets the parsed URI component.
                /// </summary>
                public T Value { get; private set; }
            }
        }
    }
}
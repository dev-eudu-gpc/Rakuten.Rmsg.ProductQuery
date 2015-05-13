//----------------------------------------------------------------------------------------------------------------------
// <copyright file="UriTemplate.Lexer.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------------------------------------------------
namespace Rakuten
{
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;

    /// <summary>
    /// Represents a Uniform Resource Indicator (URI) template, as defined by RFC 6570.
    /// </summary>
    public partial class UriTemplate
    {
        /// <summary>
        /// Provides functionality for lexing a URI template string.
        /// </summary>
        private static class Lexer
        {
            /// <summary>
            /// Lexes a URI template string, returning a collection of tokens.
            /// </summary>
            /// <param name="template">The URI template to lex.</param>
            /// <returns>A collection of tokens representing the specified URI template.</returns>
            public static IEnumerable<Token> Lex(string template)
            {
                Contract.Requires(template != null);

                Reader[] readers =
                {
                    new SymbolReader('&', TokenType.Ampersand),
                    new SymbolReader('*', TokenType.Asterisk),
                    new SymbolReader('{', TokenType.LeftBrace),
                    new SymbolReader('}', TokenType.RightBrace),
                    new SymbolReader(',', TokenType.Comma),
                    new SymbolReader('=', TokenType.EqualsSign),
                    new SymbolReader('?', TokenType.QuestionMark),
                    new SymbolReader('/', TokenType.Slash)
                };

                Token token;
                var startIndex = 0;
                for (var i = 0; i < template.Length;)
                {
                    var reader = readers.FirstOrDefault(r => r.CanRead(template[i]));
                    if (reader == null)
                    {
                        ++i;
                    }
                    else
                    {
                        token = ReadLiteral(template, startIndex, i - startIndex);
                        if (token != null)
                        {
                            yield return token;
                        }

                        startIndex = i;
                        token = reader.ReadToken(template, startIndex);
                        if (token != null)
                        {
                            yield return token;
                            startIndex = i += token.Length;
                        }
                    }
                }

                token = ReadLiteral(template, startIndex, template.Length - startIndex);
                if (token != null)
                {
                    yield return token;
                }

                yield return new Token(TokenType.EndOfTemplate);
            }

            /// <summary>
            /// Reads a literal token from the specified template string.
            /// </summary>
            /// <param name="template">The template string from which to read.</param>
            /// <param name="index">The zero-based starting character position of a literal in the template string.</param>
            /// <param name="length">The number of characters in the literal.</param>
            /// <returns>A token representing a literal component in the URI template.</returns>
            private static Token ReadLiteral(string template, int index, int length)
            {
                return length <= 0 ?
                    null :
                    new Token(
                        TokenType.Literal,
                        length,
                        template.Substring(index, length));
            }

            /// <summary>
            /// Provides functionality for reading tokens from a template string.
            /// </summary>
            private abstract class Reader
            {
                /// <summary>
                /// Returns a value indicating whether this reader can read a specified character.
                /// </summary>
                /// <param name="c">The character to read.</param>
                /// <returns>
                /// <see langword="true"/> if this reader can read the <paramref name="c"/> parameter;
                /// otherwise, <see langword="false"/>.
                /// </returns>
                public abstract bool CanRead(char c);

                /// <summary>
                /// Reads a token from the specified template string.
                /// </summary>
                /// <param name="template">The template string from which to read.</param>
                /// <param name="index">The zero-based starting character position of a component in the template string.</param>
                /// <returns>A token representing a component in the URI template.</returns>
                public Token ReadToken(string template, int index)
                {
                    return this.ReadSpecializedToken(template, index);
                }

                /// <summary>
                /// Reads a token from the specified template string.
                /// </summary>
                /// <param name="template">The template string from which to read.</param>
                /// <param name="index">The zero-based starting character position of a component in the template string.</param>
                /// <returns>A token representing a component in the URI template.</returns>
                protected abstract Token ReadSpecializedToken(string template, int index);
            }

            /// <summary>
            /// Provides functionality for reading symbols from a template string.
            /// </summary>
            private class SymbolReader : Reader
            {
                /// <summary>
                /// The symbol to read.
                /// </summary>
                private readonly char symbol;

                /// <summary>
                /// The type of token to return.
                /// </summary>
                private readonly TokenType tokenType;

                /// <summary>
                /// Initializes a new instance of the <see cref="SymbolReader"/> class.
                /// </summary>
                /// <param name="symbol">The symbol to read.</param>
                /// <param name="tokenType">The type of token to return.</param>
                public SymbolReader(char symbol, TokenType tokenType)
                {
                    this.symbol = symbol;
                    this.tokenType = tokenType;
                }

                /// <summary>
                /// Returns a value indicating whether this reader can read a specified character.
                /// </summary>
                /// <param name="c">The character to read.</param>
                /// <returns>
                /// <see langword="true"/> if this reader can read the <paramref name="c"/> parameter;
                /// otherwise, <see langword="false"/>.
                /// </returns>
                public override bool CanRead(char c)
                {
                    return c == this.symbol;
                }

                /// <summary>
                /// Reads a token from the specified template string.
                /// </summary>
                /// <param name="template">The template string from which to read.</param>
                /// <param name="index">The zero-based starting character position of a component in the template string.</param>
                /// <returns>A token representing a component in the URI template.</returns>
                protected override Token ReadSpecializedToken(string template, int index)
                {
                    return new Token(this.tokenType, this.symbol);
                }
            }
        }
    }
}
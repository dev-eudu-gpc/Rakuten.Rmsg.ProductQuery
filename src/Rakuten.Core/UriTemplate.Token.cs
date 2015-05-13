//----------------------------------------------------------------------------------------------------------------------
// <copyright file="UriTemplate.Token.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------------------------------------------------
namespace Rakuten
{
    using System.Diagnostics.Contracts;
    using System.Globalization;

    /// <summary>
    /// Represents a Uniform Resource Indicator (URI) template, as defined by RFC 6570.
    /// </summary>
    public partial class UriTemplate
    {
        /// <summary>
        /// Represents a token within a URI template string.
        /// </summary>
        private sealed class Token
        {
            /// <summary>
            /// The value of this token.
            /// </summary>
            private readonly string value;

            /// <summary>
            /// Initializes a new instance of the <see cref="Token"/> class.
            /// </summary>
            /// <param name="tokenType">The type of the token.</param>
            public Token(TokenType tokenType)
            {
                Contract.Requires(tokenType == TokenType.EndOfTemplate);

                this.TokenType = tokenType;
                this.value = string.Empty;
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="Token"/> class.
            /// </summary>
            /// <param name="tokenType">The type of the token.</param>
            /// <param name="length">The number of characters this token occupies in a URI template string.</param>
            /// <param name="value">The value of the token.</param>
            public Token(TokenType tokenType, int length, string value)
            {
                Contract.Requires(tokenType != TokenType.EndOfTemplate);
                Contract.Requires(length > 0);
                Contract.Requires(!string.IsNullOrEmpty(value));

                this.Length = length;
                this.TokenType = tokenType;
                this.value = value;
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="Token"/> class.
            /// </summary>
            /// <param name="tokenType">The type of the token.</param>
            /// <param name="value">The value of the token.</param>
            public Token(TokenType tokenType, char value)
                : this(tokenType, 1, value.ToString(CultureInfo.InvariantCulture))
            {
            }

            /// <summary>
            /// Gets the number of characters this token occupies in a URI template string.
            /// </summary>
            public int Length { get; private set; }

            /// <summary>
            /// Gets the type of this token.
            /// </summary>
            public TokenType TokenType { get; private set; }

            /// <summary>
            /// Gets a string that represents the token.
            /// </summary>
            /// <returns>A string that represents the token.</returns>
            public override string ToString()
            {
                return this.value;
            }
        }
    }
}
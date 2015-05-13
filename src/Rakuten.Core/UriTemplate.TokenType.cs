//----------------------------------------------------------------------------------------------------------------------
// <copyright file="UriTemplate.TokenType.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------------------------------------------------
namespace Rakuten
{
    /// <summary>
    /// Represents a Uniform Resource Indicator (URI) template, as defined by RFC 6570.
    /// </summary>
    public partial class UriTemplate
    {
        /// <summary>
        /// Specifies the type of a token within a URI template.
        /// </summary>
        private enum TokenType
        {
            /// <summary>
            /// Indicates an ampersand ('&amp;').
            /// </summary>
            Ampersand,

            /// <summary>
            /// Indicates an asterisk ('*').
            /// </summary>
            Asterisk,

            /// <summary>
            /// Indicates a comma (',').
            /// </summary>
            Comma,

            /// <summary>
            /// Indicates the end of a URI template.
            /// </summary>
            EndOfTemplate,

            /// <summary>
            /// Indicates an equals sign ('=').
            /// </summary>
            EqualsSign,

            /// <summary>
            /// Indicates a left brace ('{').
            /// </summary>
            LeftBrace,

            /// <summary>
            /// Indicates a literal string.
            /// </summary>
            Literal,

            /// <summary>
            /// Indicates a question mark ('?').
            /// </summary>
            QuestionMark,

            /// <summary>
            /// Indicates a right brace ('}').
            /// </summary>
            RightBrace,

            /// <summary>
            /// Indicates a slash ('/').
            /// </summary>
            Slash
        }
    }
}
//----------------------------------------------------------------------------------------------------------------------
// <copyright file="UriTemplate.Literal.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------------------------------------------------
namespace Rakuten
{
    using System.Diagnostics.Contracts;
    using System.Linq;

    /// <summary>
    /// Represents a Uniform Resource Indicator (URI) template, as defined by RFC 6570.
    /// </summary>
    public partial class UriTemplate
    {
        /// <summary>
        /// Represents a literal (constant) component of a URI template.
        /// </summary>
        private sealed class Literal : IComponent
        {
            /// <summary>
            /// The text representation of this literal.
            /// </summary>
            private readonly string text;

            /// <summary>
            /// Initializes a new instance of the <see cref="Literal"/> class.
            /// </summary>
            /// <param name="text">The literal text.</param>
            public Literal(string text)
            {
                Contract.Requires(!string.IsNullOrEmpty(text));

                this.text = text;
            }

            /// <summary>
            /// Expands the literal.
            /// </summary>
            /// <param name="values">The values of variables to use when expanding the literal.</param>
            /// <returns>An expansion of the literal.</returns>
            public string Expand(ILookup<string, string> values)
            {
                return this.text;
            }

            /// <summary>
            /// Gets a string that represents the literal.
            /// </summary>
            /// <returns>A string that represents the literal.</returns>
            public override string ToString()
            {
                return this.text;
            }
        }
    }
}
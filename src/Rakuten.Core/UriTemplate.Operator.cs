//----------------------------------------------------------------------------------------------------------------------
// <copyright file="UriTemplate.Operator.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------------------------------------------------
namespace Rakuten
{
    using System;
    using System.Globalization;

    /// <summary>
    /// Represents a Uniform Resource Indicator (URI) template, as defined by RFC 6570.
    /// </summary>
    public partial class UriTemplate
    {
        /// <summary>
        /// Represents an operator within a URI template expression, as defined by RFC 6570.
        /// </summary>
        private sealed class Operator
        {
            /// <summary>
            /// The character denoting this operator.
            /// </summary>
            private readonly char character;

            /// <summary>
            /// Initializes a new instance of the <see cref="Operator"/> class.
            /// </summary>
            /// <param name="character">The character denoting the operator.</param>
            public Operator(char character)
            {
                if ("?&".IndexOf(character) == -1)
                {
                    throw new FormatException(string.Format(UriTemplateErrorMessages.UnsupportedOperator, character));
                }

                this.character = character;
            }

            /// <summary>
            /// Converts a character to an <see cref="Operator" /> object.
            /// </summary>
            /// <param name="c">A character denoting an operator.</param>
            /// <returns>
            /// An <see cref="Operator" /> object representing the operator denoted by <paramref name="c"/>.
            /// </returns>
            public static implicit operator Operator(char c)
            {
                return c == '\0' ? null : new Operator(c);
            }

            /// <summary>
            /// Converts an <see cref="Operator"/> object to a character.
            /// </summary>
            /// <param name="o">An <see cref="Operator" /> object.</param>
            /// <returns>A character denoting the specified operator.</returns>
            public static implicit operator char(Operator o)
            {
                return o == null ? '\0' : o.character;
            }

            /// <summary>
            /// Determines whether the specified <see cref="Operator" /> is equal to this <see cref="Operator" />.
            /// </summary>
            /// <param name="obj">The <see cref="Operator" /> to compare to the current <see cref="Operator" />.</param>
            /// <returns>
            /// <see langword="true"/> if the specified <see cref="Operator" /> is equal to
            /// the current <see cref="Operator" />; otherwise <see langword="false"/>.
            /// </returns>
            public override bool Equals(object obj)
            {
                var other = obj as Operator;
                return other != null && this.character == other.character;
            }

            /// <summary>
            /// Gets a hash code for this <see cref="Operator" />.
            /// </summary>
            /// <returns>An <see cref="Int32" /> that contains the hash code for the <see cref="Operator" />.</returns>
            public override int GetHashCode()
            {
                return this.character;
            }

            /// <summary>
            /// Gets a string that represents the operator.
            /// </summary>
            /// <returns>A string that represents the operator.</returns>
            public override string ToString()
            {
                return this.character.ToString(CultureInfo.InvariantCulture);
            }
        }
    }
}
//----------------------------------------------------------------------------------------------------------------------
// <copyright file="Literal.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------------------------------------------------
namespace Rakuten
{
    /// <summary>
    /// Represents a literal (constant) component of a URI template, as defined by RFC 6570.
    /// </summary>
    public class Literal : IUriTemplateComponent
    {
        /// <summary>
        /// /// <summary>
        /// Gets the text representation of the URI template component.
        /// </summary>
        /// </summary>
        private readonly string text;

        /// <summary>
        /// Initializes a new instance of the <see cref="Literal"/> class.
        /// </summary>
        /// <param name="text">The text of this literal.</param>
        public Literal(string text)
        {
            this.text = text;
        }

        /// <summary>
        /// Gets the text representation of the URI template component.
        /// </summary>
        public string Text
        {
            get { return this.text; }
        }

        /// <summary>
        /// Returns a text representation of the current <see cref="Literal"/>.
        /// </summary>
        /// <returns>A text representation of the current <see cref="VarSpec"/>.</returns>
        public override string ToString()
        {
            return this.Text ?? string.Empty;
        }
    }
}
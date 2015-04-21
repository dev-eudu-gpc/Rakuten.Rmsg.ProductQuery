//----------------------------------------------------------------------------------------------------------------------
// <copyright file="VariablePathSegment.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------------------------------------------------
namespace Rakuten
{
    /// <summary>
    /// Represents a variable contained within the path segment of a Uniform Resource Indicator (URI) template,
    /// as defined by RFC 6570.
    /// </summary>
    public class VariablePathSegment : VarSpec, IUriTemplateVariableComponent
    {
        /// <summary>
        /// Indicates whether this path segment includes a trailing slash.
        /// </summary>
        private readonly bool hasTrailingSlash;

        /// <summary>Initializes a new instance of the <see cref="VariablePathSegment"/> class.</summary>
        /// <param name="name">The name of the variable.</param>
        /// <param name="isExploded">
        /// A value indicating whether the variable should be considered to hold a composite value.
        /// </param>
        /// <param name="hasTrailingSlash">
        /// A value indicating whether this path segment includes a trailing slash.
        /// </param>
        public VariablePathSegment(string name, bool isExploded, bool hasTrailingSlash) : base(name, isExploded)
        {
            this.hasTrailingSlash = hasTrailingSlash;
        }

        /// <summary>Gets the text representation of the path segment.</summary>
        public override string Text
        {
            get { return this.hasTrailingSlash ? base.Text + "/" : base.Text; }
        }

        /// <summary>
        /// Returns a string that represents the current <see cref="VariablePathSegment"/>.
        /// </summary>
        /// <param name="value">The value of the variable.</param>
        /// <returns>A text representation of the current <see cref="VariablePathSegment"/>.</returns>
        public string ToString(string value)
        {
            if (value == null)
            {
                return this.Text;
            }

            return this.hasTrailingSlash ? value + "/" : value;
        }
    }
}
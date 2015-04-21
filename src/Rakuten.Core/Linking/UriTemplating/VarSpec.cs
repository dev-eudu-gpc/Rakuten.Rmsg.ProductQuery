//----------------------------------------------------------------------------------------------------------------------
// <copyright file="VarSpec.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------------------------------------------------
namespace Rakuten
{
    using System.Diagnostics.Contracts;

    /// <summary>
    /// Represents a variable contained within a Uniform Resource Indicator (URI) template, as defined by RFC 6570.
    /// </summary>
    public class VarSpec : IVarSpec
    {
        /// <summary>
        /// Indicates whether the variable should be considered to hold a composite value.
        /// </summary>
        private readonly bool isExploded;

        /// <summary>The name of the variable.</summary>
        private readonly string name;

        /// <summary>Initializes a new instance of the <see cref="VarSpec"/> class.</summary>
        /// <param name="name">The name of the variable.</param>
        public VarSpec(string name) : this(name, false)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(name));
        }

        /// <summary>Initializes a new instance of the <see cref="VarSpec"/> class.</summary>
        /// <param name="name">The name of the variable.</param>
        /// <param name="isExploded">
        /// A value indicating whether the variable should be considered to hold a composite value.
        /// </param>
        public VarSpec(string name, bool isExploded)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(name));

            this.isExploded = isExploded;
            this.name = name;
        }

        /// <summary>
        /// Gets a value indicating whether the variable should be considered to hold a composite value.
        /// </summary>
        public bool IsExploded
        {
            get { return this.isExploded; }
        }

        /// <summary>Gets the name of the variable.</summary>
        public string Name
        {
            get { return this.name; }
        }

        /// <summary>
        /// Gets the text representation of the URI template variable.
        /// </summary>
        public virtual string Text
        {
            get
            {
                return string.Format("{{{0}{1}}}", this.Name, this.isExploded ? "*" : string.Empty);
            }
        }

        /// <summary>
        /// Returns a string that represents the current <see cref="VarSpec"/>.
        /// </summary>
        /// <returns>A text representation of the current <see cref="VarSpec"/>.</returns>
        public override string ToString()
        {
            return this.Text;
        }
    }
}
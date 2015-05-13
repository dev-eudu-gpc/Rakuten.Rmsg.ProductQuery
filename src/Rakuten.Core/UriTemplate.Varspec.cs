//----------------------------------------------------------------------------------------------------------------------
// <copyright file="UriTemplate.Varspec.cs" company="Rakuten">
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
        /// Represents a variable contained within a Uniform Resource Indicator (URI) template, as defined by RFC 6570.
        /// </summary>
        private sealed class Varspec
        {
            /// <summary>Initializes a new instance of the <see cref="Varspec"/> class.</summary>
            /// <param name="name">The name of the variable.</param>
            /// <param name="isExploded">
            /// A value indicating whether the variable should be considered to hold a composite value.
            /// </param>
            public Varspec(string name, bool isExploded)
            {
                Contract.Requires(!string.IsNullOrWhiteSpace(name));

                this.IsExploded = isExploded;
                this.Name = name;
            }

            /// <summary>
            /// Gets a value indicating whether the variable should be considered to hold a composite value.
            /// </summary>
            public bool IsExploded { get; private set; }

            /// <summary>
            /// Gets the name of the variable.
            /// </summary>
            public string Name { get; private set; }

            /// <summary>
            /// Expands the variable using simple variable expansion.
            /// </summary>
            /// <param name="values">The values of variables to use when expanding the variable.</param>
            /// <returns>An expansion of the variable.</returns>
            public string Expand(ILookup<string, string> values)
            {
                return string.Join(",", values[this.Name]);
            }

            /// <summary>
            /// Expands the variable using the specified operator.
            /// </summary>
            /// <param name="op">The operator to use when performing the expansion.</param>
            /// <param name="values">The values of variables to use when expanding the variable.</param>
            /// <returns>An expansion of the variable.</returns>
            public string Expand(Operator op, ILookup<string, string> values)
            {
                if (!values.Contains(this.Name))
                {
                    return string.Empty;
                }

                if (op == null)
                {
                    return this.Expand(values);
                }

                if (this.IsExploded)
                {
                    return string.Join("&", values[this.Name].Select(value => this.Name + "=" + value));
                }

                var text = string.Join(",", values[this.Name]);
                return text.Length == 0 ? string.Empty : this.Name + "=" + text;
            }

            /// <summary>
            /// Returns a string that represents the current <see cref="Varspec"/>.
            /// </summary>
            /// <returns>A text representation of the current <see cref="Varspec"/>.</returns>
            public override string ToString()
            {
                return this.IsExploded ? this.Name + "*" : this.Name;
            }
        }
    }
}
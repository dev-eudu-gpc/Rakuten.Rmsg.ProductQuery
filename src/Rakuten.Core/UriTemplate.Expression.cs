//----------------------------------------------------------------------------------------------------------------------
// <copyright file="UriTemplate.Expression.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------------------------------------------------
namespace Rakuten
{
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.Diagnostics.Contracts;
    using System.Linq;

    /// <summary>
    /// Represents a Uniform Resource Indicator (URI) template, as defined by RFC 6570.
    /// </summary>
    public partial class UriTemplate
    {
        /// <summary>
        /// Represents a template expression, as defined by RFC 6570.
        /// </summary>
        private sealed class Expression : IComponent
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="Expression"/> class.
            /// </summary>
            /// <param name="op">The operator to use for the expression.</param>
            /// <param name="varspecs">The variables to include in the expression.</param>
            public Expression(Operator op, IEnumerable<Varspec> varspecs)
            {
                Contract.Requires(varspecs != null);

                this.Operator = op;
                this.Varspecs = varspecs.ToImmutableArray();
            }

            /// <summary>
            /// Gets the operator for this expression.
            /// </summary>
            public Operator Operator { get; private set; }

            /// <summary>
            /// Gets the variable specifiers contained in this expression.
            /// </summary>
            public ImmutableArray<Varspec> Varspecs { get; private set; }

            /// <summary>
            /// Expands the expression.
            /// </summary>
            /// <param name="values">The values of variables to use when expanding the expression.</param>
            /// <returns>An expansion of the expression.</returns>
            public string Expand(ILookup<string, string> values)
            {
                return this.Operator == null ?
                    this.ExpandUsingNoOperator(values) :
                    this.ExpandUsingOperator(this.Operator, values);
            }

            /// <summary>
            /// Gets a string that represents the expression.
            /// </summary>
            /// <returns>A string that represents the expression.</returns>
            public override string ToString()
            {
                return "{"
                    + this.Operator
                    + string.Join(",", this.Varspecs.Select(varspec => varspec.ToString()))
                    + "}";
            }

            /// <summary>
            /// Expands the current expression using simple variable expansion.
            /// </summary>
            /// <param name="values">The values of variables to use when expanding the expression.</param>
            /// <returns>An expansion of the current expression.</returns>
            private string ExpandUsingNoOperator(ILookup<string, string> values)
            {
                return string.Join(",", this.Varspecs.Select(v => v.Expand(values)));
            }

            /// <summary>
            /// Expands the current expression using the specified operator.
            /// </summary>
            /// <param name="op">The operator to use when expanding the expression.</param>
            /// <param name="values">The values of variables to use when expanding the expression.</param>
            /// <returns>An expansion of the current expression.</returns>
            private string ExpandUsingOperator(Operator op, ILookup<string, string> values)
            {
                var components = this.Varspecs.Select(v => v.Expand(op, values)).Where(s => !string.IsNullOrEmpty(s));
                var expansion = string.Join("&", components);

                return string.IsNullOrEmpty(expansion) ? string.Empty : op + expansion;
            }
        }
    }
}
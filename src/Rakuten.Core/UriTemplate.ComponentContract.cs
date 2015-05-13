//----------------------------------------------------------------------------------------------------------------------
// <copyright file="UriTemplate.ComponentContract.cs" company="Rakuten">
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
        /// Represents a component of a URI template, as defined by RFC 6570.
        /// </summary>
        [ContractClassFor(typeof(IComponent))]
        private abstract class ComponentContract : IComponent
        {
            /// <summary>
            /// Expands the URI template component.
            /// </summary>
            /// <param name="values">The values of variables to use when expanding the component.</param>
            /// <returns>An expansion of the current URI template component.</returns>
            public string Expand(ILookup<string, string> values)
            {
                Contract.Ensures(Contract.Result<string>() != null);
                return default(string);
            }
        }
    }
}
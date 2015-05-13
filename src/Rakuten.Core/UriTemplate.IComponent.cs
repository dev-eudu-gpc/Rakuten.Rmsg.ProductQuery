//----------------------------------------------------------------------------------------------------------------------
// <copyright file="UriTemplate.IComponent.cs" company="Rakuten">
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
        [ContractClass(typeof(ComponentContract))]
        private interface IComponent
        {
            /// <summary>
            /// Expands the URI template component.
            /// </summary>
            /// <param name="values">The values of variables to use when expanding the component.</param>
            /// <returns>An expansion of the current URI template component.</returns>
            string Expand(ILookup<string, string> values);
        }
    }
}
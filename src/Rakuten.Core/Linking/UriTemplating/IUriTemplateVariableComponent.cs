//----------------------------------------------------------------------------------------------------------------------
// <copyright file="IUriTemplateVariableComponent.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------------------------------------------------
namespace Rakuten
{
    /// <summary>
    /// Defines properties of a component of a URI template, as defined by RFC 6570.
    /// </summary>
    public interface IUriTemplateVariableComponent : IUriTemplateComponent
    {
        /// <summary>Gets the name of the variable.</summary>
        string Name { get; }

        /// <summary>
        /// Gets the text representation of the URI template component with the specified value.
        /// </summary>
        /// <param name="value">The value of the URI template variable.</param>
        /// <returns>A text representation of the component with the specified value.</returns>
        string ToString(string value);
    }
}
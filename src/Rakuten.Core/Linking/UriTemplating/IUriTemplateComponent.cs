//----------------------------------------------------------------------------------------------------------------------
// <copyright file="IUriTemplateComponent.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------------------------------------------------
namespace Rakuten
{
    /// <summary>
    /// Defines properties of a component of a URI template, as defined by RFC 6570.
    /// </summary>
    public interface IUriTemplateComponent
    {
        /// <summary>
        /// Gets the text representation of the URI template component.
        /// </summary>
        string Text { get; }
    }
}
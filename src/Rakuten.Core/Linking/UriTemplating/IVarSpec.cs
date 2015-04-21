//----------------------------------------------------------------------------------------------------------------------
// <copyright file="IVarSpec.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------------------------------------------------
namespace Rakuten
{
    /// <summary>
    /// Defines properties of a variable within a URI template, as defined by RFC 6570.
    /// </summary>
    public interface IVarSpec : IUriTemplateComponent
    {
        /// <summary>
        /// Gets a value indicating whether the variable should be considered to hold a composite value.
        /// </summary>
        bool IsExploded { get; }

        /// <summary>Gets the name of the variable.</summary>
        string Name { get; }
    }
}
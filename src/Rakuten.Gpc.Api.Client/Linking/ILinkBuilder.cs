// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="ILinkBuilder.cs" company="Rakuten">
//   Copyright (c) Rakuten. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------
namespace Rakuten
{
    /// <summary>Provides functionality for building a link.</summary>
    /// <typeparam name="T">The type of link to build.</typeparam>
    public interface ILinkBuilder<T> where T : LinkTemplate
    {
        /// <summary>Gets a template that represents the link under construction.</summary>
        T Template { get; }

        /// <summary>Provides functionality for building a link with the specified relation type.</summary>
        /// <param name="relationType">The link relation type.</param>
        /// <returns>An instance that can be used to build a link of the specified relation type.</returns>
        ITypedLinkBuilder<T> WithType(string relationType);
    }
}
// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="ITypedLinkBuilder.cs" company="Rakuten">
//   Copyright (c) Rakuten. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------
namespace Rakuten
{
    /// <summary>Provides functionality for constructing a link that includes a target type relation.</summary>
    /// <typeparam name="T">The type of link to build.</typeparam>
    public interface ITypedLinkBuilder<T> : ILinkBuilder<T> where T : LinkTemplate
    {
        /// <summary>
        /// Provides functionality for constructing a link that does not include a target type relation.
        /// </summary>
        /// <returns>
        /// An instance that can be used to construct a link that does not include a target type relation.
        /// </returns>
        ILinkBuilder<T> WithoutTargetType();
    }
}
// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="ILinkActivator.cs" company="Rakuten">
//   Copyright (c) Rakuten. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------
namespace Rakuten
{
    /// <summary>Provides functionality for creating links.</summary>
    /// <typeparam name="T">The type of link to create.</typeparam>
    public interface ILinkActivator<out T> where T : LinkTemplate
    {
        /// <summary>Creates an instance of the link.</summary>
        /// <param name="relationType">The type of link relation to create.</param>
        /// <param name="target">The target of the link.</param>
        /// <param name="attributes">The attributes of the link target.</param>
        /// <returns>A new link instance.</returns>
        T Create(string relationType, IUriTemplate target, ITargetAttributes attributes);
    }
}
//----------------------------------------------------------------------------------------------------------------------
// <copyright file="ILink.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------------------------------------------------
namespace Rakuten
{
    /// <summary>Represents a link to a resource.</summary>
    public interface ILink : ITargetAttributes
    {
        /// <summary>Gets the link relation type.</summary>
        string RelationType { get; }

        /// <summary>Gets the location of the resource this link points to.</summary>
        string Target { get; }
    }
}
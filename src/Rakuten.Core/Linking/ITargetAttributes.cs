//----------------------------------------------------------------------------------------------------------------------
// <copyright file="ITargetAttributes.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------------------------------------------------
namespace Rakuten
{
    /// <summary>Represents target attributes for a link.</summary>
    public interface ITargetAttributes
    {
        /// <summary>Gets the language of the resource this link points to.</summary>
        string LanguageTag { get; }

        /// <summary>Gets the media type of the representation this link points to.</summary>
        string MediaType { get; }

        /// <summary>Gets the title of the resource this link points to.</summary>
        string Title { get; }
    }
}
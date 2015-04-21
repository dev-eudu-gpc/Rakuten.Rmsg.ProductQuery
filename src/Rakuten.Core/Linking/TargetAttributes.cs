// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="TargetAttributes.cs" company="Rakuten">
//   Copyright (c) Rakuten. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------
namespace Rakuten
{
    /// <summary>Represents target attributes for a link.</summary>
    public class TargetAttributes : ITargetAttributes
    {
        /// <summary>Initializes a new instance of the <see cref="TargetAttributes"/> class.</summary>
        public TargetAttributes()
        {
        }

        /// <summary>Initializes a new instance of the <see cref="TargetAttributes"/> class.</summary>
        /// <param name="languageTag">The language of the resource this link points to.</param>
        /// <param name="mediaType">The media type of the representation this link points to.</param>
        /// <param name="title">The title of the resource this link points to.</param>
        public TargetAttributes(string languageTag, string mediaType, string title)
        {
            this.LanguageTag = languageTag;
            this.MediaType = mediaType;
            this.Title = title;
        }

        /// <summary>Gets the language of the resource this link points to.</summary>
        public string LanguageTag { get; private set; }

        /// <summary>Gets the media type of the representation this link points to.</summary>
        public string MediaType { get; private set; }

        /// <summary>Gets the title of the resource this link points to.</summary>
        public string Title { get; private set; }
    }
}
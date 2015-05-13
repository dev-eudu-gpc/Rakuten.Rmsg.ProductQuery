// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="TargetAttributesExtensions.cs" company="Rakuten">
//   Copyright (c) Rakuten. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------
namespace Rakuten
{
    using System;
    using System.Globalization;

    /// <summary>Provides useful extensions to the <see cref="ITargetAttributes"/> interface.</summary>
    public static class TargetAttributesExtensions
    {
        /// <summary>Returns target attributes with a specified language.</summary>
        /// <param name="attributes">The target attributes.</param>
        /// <param name="value">The language of the target.</param>
        /// <returns>Target attributes with the specified language.</returns>
        public static ITargetAttributes WithLanguageTag(this ITargetAttributes attributes, CultureInfo value)
        {
            if (value == null)
            {
                if (attributes == null || string.IsNullOrWhiteSpace(attributes.LanguageTag))
                {
                    return attributes;
                }

                return new TargetAttributes(
                    languageTag: null,
                    mediaType: attributes.MediaType,
                    title: attributes.Title);
            }

            if (attributes == null)
            {
                return new TargetAttributes(value.Name, mediaType: null, title: null);
            }

            return value.Name.Equals(attributes.LanguageTag) ?
                attributes :
                new TargetAttributes(value.Name, attributes.MediaType, attributes.Title);
        }

        /// <summary>Returns target attributes with a specified language.</summary>
        /// <param name="attributes">The target attributes.</param>
        /// <param name="value">The media type of the target.</param>
        /// <returns>Target attributes with the specified media type.</returns>
        public static ITargetAttributes WithMediaType(this ITargetAttributes attributes, string value)
        {
            if (value == null)
            {
                return attributes == null || string.IsNullOrWhiteSpace(attributes.MediaType) ?
                    attributes :
                    new TargetAttributes(attributes.LanguageTag, mediaType: null, title: attributes.Title);
            }

            if (attributes == null)
            {
                return new TargetAttributes(languageTag: null, mediaType: value, title: null);
            }

            return value.Equals(attributes.MediaType, StringComparison.OrdinalIgnoreCase) ?
                attributes :
                new TargetAttributes(attributes.LanguageTag, mediaType: value, title: attributes.Title);
        }

        /// <summary>Returns target attributes with a specified title.</summary>
        /// <param name="attributes">The target attributes.</param>
        /// <param name="value">The title of the target.</param>
        /// <returns>Target attributes with the specified title.</returns>
        public static ITargetAttributes WithTitle(this ITargetAttributes attributes, string value)
        {
            if (value == null)
            {
                return attributes == null || string.IsNullOrWhiteSpace(attributes.Title) ?
                    attributes :
                    new TargetAttributes(attributes.LanguageTag, attributes.MediaType, title: null);
            }

            if (attributes == null)
            {
                return new TargetAttributes(languageTag: null, mediaType: null, title: value);
            }

            return value.Equals(attributes.Title, StringComparison.OrdinalIgnoreCase) ?
                attributes :
                new TargetAttributes(attributes.LanguageTag, attributes.MediaType, title: value);
        }
    }
}
//----------------------------------------------------------------------------------------------------------------------
// <copyright file="UriTemplateExtensions.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------------------------------------------------
namespace Rakuten
{
    using System;

    /// <summary>Provides useful extensions to the <see cref="IUriTemplate"/> interface.</summary>
    public static class UriTemplateExtensions
    {
        /// <summary>Returns a URI derived from the current template.</summary>
        /// <param name="template">The current URI template.</param>
        /// <returns>A URI derived from the current template.</returns>
        public static Uri ToUri(this IUriTemplate template)
        {
            return template == null ?
                null :
                new Uri(template.ToString(resolveTemplate: true), UriKind.RelativeOrAbsolute);
        }
    }
}
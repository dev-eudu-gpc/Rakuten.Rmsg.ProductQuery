// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ApiExceptionMetadata.cs" company="Rakuten">
//   Copyright (c) Rakuten. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Rakuten.Web.Http.ExceptionHandling
{
    using System;

    /// <summary>
    /// A structure representing data elements extracted from an attributed <see cref="Rakuten.Gpc.ApiException"/>.
    /// </summary>
    public struct ApiExceptionMetadata
    {
        /// <summary>
        /// The title of the error.
        /// </summary>
        public string Title;

        /// <summary>
        /// The type of the error.
        /// </summary>
        public Uri Type;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiExceptionMetadata"/> struct.
        /// </summary>
        /// <param name="type">
        /// The type of the error extracted from the attributed data.
        /// </param>
        /// <param name="title">
        /// The title of the error extracted from the attributed data.
        /// </param>
        public ApiExceptionMetadata(Uri type, string title)
        {
            this.Type = type;
            this.Title = title;
        }
    }
}
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IApiExceptionMetadataProvider.cs" company="Rakuten">
//   Copyright (c) Rakuten. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Rakuten.Web.Http.ExceptionHandling
{
    using System;

    /// <summary>
    /// Defines a class that will extract metadata from an <see cref="Exception"/>.
    /// </summary>
    public interface IApiExceptionMetadataProvider
    {
        /// <summary>
        /// Gets metadata for the specified exception.
        /// </summary>
        /// <param name="exception">
        /// The exception to get the metadata for.
        /// </param>
        /// <returns>
        /// A <see cref="ApiExceptionMetadata"/> object for the exception.
        /// </returns>
        ApiExceptionMetadata GetMetadataForException(Exception exception);
    }
}

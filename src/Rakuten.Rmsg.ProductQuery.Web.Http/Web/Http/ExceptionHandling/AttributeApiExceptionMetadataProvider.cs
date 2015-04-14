// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AttributeApiExceptionMetadataProvider.cs" company="Rakuten">
//   Copyright (c) Rakuten. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Rakuten.Web.Http.ExceptionHandling
{
    using System;
    using System.Linq;

    using Rakuten.Gpc;

    /// <summary>
    /// A default implementation of a <see cref="Rakuten.Gpc.ApiException"/> metadata provider.
    /// </summary>
    public class AttributeApiExceptionMetadataProvider : IApiExceptionMetadataProvider
    {
        /// <summary>
        /// Gets metadata for the specified exception from attributed data.
        /// </summary>
        /// <param name="exception">
        /// The exception to get the metadata for.
        /// </param>
        /// <returns>
        /// A <see cref="ApiExceptionMetadata"/> object for the exception.
        /// </returns>
        public ApiExceptionMetadata GetMetadataForException(Exception exception)
        {
            if (exception == null)
            {
                throw new ArgumentNullException("exception");
            }

            ApiExceptionAttribute attribute = this.GetAttribute(exception);

            return attribute == null ?
                default(ApiExceptionMetadata) :
                new ApiExceptionMetadata(attribute.Type, attribute.Title);
        }

        /// <summary>
        /// Attempts to extract a <see cref="ApiExceptionAttribute"/> from the specified exception.
        /// </summary>
        /// <param name="exception">
        /// The exception from which to retrieve the attribute.
        /// </param>
        /// <returns>
        /// Returns the located <see cref="ApiExceptionAttribute"/>; otherwise, a null reference will be returned.
        /// </returns>
        private ApiExceptionAttribute GetAttribute(Exception exception)
        {
            return this.GetAttribute(exception, inherit: false) ?? this.GetAttribute(exception, inherit: true);
        }

        /// <summary>
        /// Attempts to extract a <see cref="ApiExceptionAttribute"/> from the specified exception.
        /// </summary>
        /// <param name="exception">
        /// The exception from which to retrieve the attribute.
        /// </param>
        /// <param name="inherit">
        /// True to search the exceptions inheritance chain to find the attribute; otherwise, false. 
        /// </param>
        /// <returns>
        /// Returns the located <see cref="ApiExceptionAttribute"/>; otherwise, a null reference will be returned.
        /// </returns>
        private ApiExceptionAttribute GetAttribute(Exception exception, bool inherit)
        {
            Type exceptionType = exception.GetType();
            object attribute = exceptionType
                .GetCustomAttributes(inherit)
                .FirstOrDefault(a => a is ApiExceptionAttribute);

            return attribute as ApiExceptionAttribute;
        }
    }
}
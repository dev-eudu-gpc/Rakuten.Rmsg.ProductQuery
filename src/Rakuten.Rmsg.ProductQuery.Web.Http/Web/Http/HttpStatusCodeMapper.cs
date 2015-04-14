//---------------------------------------------------------------------------------------------------------------------
// <copyright file="HttpStatusCodeMapper.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//---------------------------------------------------------------------------------------------------------------------
namespace Rakuten.Web.Http
{
    using System;
    using System.Net;

    using Rakuten.Gpc.Api;

    /// <summary>
    /// Maps a HTTP status code to an exception.
    /// </summary>
    public class HttpStatusCodeMapper
    {
        /// <summary>
        /// Returns the respective HTTP status code for the specified exception.
        /// </summary>
        /// <param name="exception">
        /// The exception for which the HTTP status code should be returned.
        /// </param>
        /// <returns>
        /// A HTTP status code.
        /// </returns>
        public HttpStatusCode Map(Exception exception)
        {
            if (exception is ObjectNotFoundException)
            {
                return HttpStatusCode.NotFound;
            }

            if (exception is ValidationFailedException)
            {
                return HttpStatusCode.Forbidden;
            }

            if (exception is BadRequestException)
            {
                return HttpStatusCode.BadRequest;
            }

            if (exception is ConstraintViolationException)
            {
                return HttpStatusCode.Forbidden;
            }
            
            return HttpStatusCode.InternalServerError;
        }
    }
}
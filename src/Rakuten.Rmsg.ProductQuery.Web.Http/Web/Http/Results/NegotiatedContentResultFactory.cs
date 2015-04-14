// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="NegotiatedContentResultFactory.cs" company="Rakuten">
//   Copyright (c) Rakuten. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------
namespace Rakuten.Web.Http.Results
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Formatting;
    using System.Web.Http;
    using System.Web.Http.Results;

    /// <summary>
    /// An instance that will created instances of <see cref="NegotiatedContentResult{T}"/> for the specified type.
    /// </summary>
    public class NegotiatedContentResultFactory
    {
        /// <summary>
        /// A cache of <see cref="NegotiatedContentResult{T}"/> by problem type.
        /// </summary>
        private readonly Dictionary<Type, Type> cache;

        /// <summary>
        /// Initializes a new instance of the <see cref="NegotiatedContentResultFactory"/> class.
        /// </summary>
        public NegotiatedContentResultFactory()
        {
            this.cache = new Dictionary<Type, Type>();
        }

        /// <summary>
        /// Creates a new <see cref="NegotiatedContentResult{T}"/> instance for the specified type and instance.
        /// </summary>
        /// <param name="type">The type from which to create the instance.</param>
        /// <param name="instance">The instance to be represented by the <see cref="IHttpActionResult"/>.</param>
        /// <param name="negotiator">The <see cref="IContentNegotiator"/> instance.</param>
        /// <param name="request">The original <see cref="HttpRequestMessage"/>.</param>
        /// <param name="formatters">The collection of <see cref="MediaTypeFormatter"/>s.</param>
        /// <param name="statusCode">The status code of the response.</param>
        /// <returns>
        /// The instantiated <see cref="NegotiatedContentResult{T}"/> instance for the specified type and instance.
        /// </returns>
        public IHttpActionResult Create(
            Type type, 
            object instance, 
            IContentNegotiator negotiator, 
            HttpRequestMessage request, 
            IEnumerable<MediaTypeFormatter> formatters, 
            HttpStatusCode statusCode = HttpStatusCode.InternalServerError)
        {
            Contract.Requires(type != null);
            Contract.Requires(instance != null);
            Contract.Requires(negotiator != null);
            Contract.Requires(request != null);
            Contract.Requires(formatters != null);

            Type resultType = GetResult(type, this.cache);

            return (IHttpActionResult)Activator.CreateInstance(
                resultType,
                new[] { statusCode, instance, negotiator, request, formatters });
        }

        /// <summary>
        /// Creates a <see cref="NegotiatedContentResult{T}"/> for the specified type.
        /// </summary>
        /// <param name="type">
        /// The type for which a <see cref="NegotiatedContentResult{T}"/> should be created.
        /// </param>
        /// <param name="cache">
        /// The cached collection of <see cref="NegotiatedContentResult{T}"/> by problem type.
        /// </param>
        /// <returns>
        /// A <see cref="NegotiatedContentResult{T}"/>.
        /// </returns>
        private static Type GetResult(Type type, Dictionary<Type, Type> cache)
        {
            Contract.Requires(type != null);
            Contract.Requires(cache != null);

            Type result;

            if (!cache.TryGetValue(type, out result))
            {
                result = typeof(NegotiatedContentResult<>).MakeGenericType(new[] { type });
                cache.Add(type, result);
            }

            return result;
        }
    }
}
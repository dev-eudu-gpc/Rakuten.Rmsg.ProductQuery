//------------------------------------------------------------------------------
// <copyright file="AcceptedNegotiatedContentResult.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
namespace System.Web.Http.Results
{
    using System.Net;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Web.Http;
    using System.Web.Http.Results;

    /// <summary>
    /// An HTTP result with a status code of 202 Accepted and content in the response body.
    /// </summary>
    /// <typeparam name="T">The type of the object supplying the content.</typeparam>
    public class AcceptedNegotiatedContentResult<T> : NegotiatedContentResult<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AcceptedNegotiatedContentResult{T}"/> class
        /// </summary>
        /// <param name="content">The content for the response body.</param>
        /// <param name="controller">The controller from which to obtain the dependencies needed for execution.</param>
        public AcceptedNegotiatedContentResult(T content, ApiController controller)
            : base(HttpStatusCode.Accepted, content, controller)
        {
        }

        /// <summary>
        /// Constructs an HTTP response with a status code of 202 and the supplied
        /// content in the response body.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <returns>An HTTP response with a status code of 202 and the supplied content in the response body.</returns>
        public override Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            return base
                .ExecuteAsync(cancellationToken)
                .ContinueWith(task => task.Result, cancellationToken);
        }
    }
}
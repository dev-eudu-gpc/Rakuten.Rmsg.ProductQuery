// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="ImageResult.cs" company="Rakuten">
//   Copyright (c) Rakuten. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------
namespace System.Web.Http.Results
{
    using System.Diagnostics.Contracts;
    using System.IO;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Web.Http;

    /// <summary>
    /// Represents an HTTP result that contains an image in the response body
    /// </summary>
    public class ImageResult : IHttpActionResult
    {
        /// <summary>
        /// The controller from which to obtain the dependencies needed for execution.
        /// </summary>
        private readonly ApiController controller;

        /// <summary>
        /// The stream that provides the image data.
        /// </summary>
        private readonly Stream stream;

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageResult"/> class
        /// </summary>
        /// <param name="stream">The stream that provides the image data.</param>
        /// <param name="controller">The controller from which to obtain the dependencies needed for execution.</param>
        public ImageResult(Stream stream, ApiController controller)
        {
            Contract.Requires(controller != null);
            Contract.Requires(stream != null);

            this.controller = controller;
            this.stream = stream;
            this.stream.Position = 0;
        }

        /// <summary>
        /// Constructs an HTTP response containing the image in the response body.
        /// </summary>
        /// <returns>An HTTP response containing the image in the response body.</returns>
        public HttpResponseMessage Execute()
        {
            var response = this.controller.Request.CreateResponse(HttpStatusCode.OK);

            response.Content = new StreamContent(this.stream);
            response.Headers.CacheControl = new CacheControlHeaderValue
            {
                MaxAge = TimeSpan.FromDays(7),
                Public = true
            };

            response.Content.Headers.ContentType = new MediaTypeHeaderValue("image/png");

            return response;
        }

        /// <summary>
        /// Constructs an HTTP response containing the image in the response body.
        /// </summary>
        /// <param name="cancellationToken">A token for cancellation</param>
        /// <returns>An HTTP response containing the image in the response body.</returns>
        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            return Task.FromResult(this.Execute());
        }
    }
}
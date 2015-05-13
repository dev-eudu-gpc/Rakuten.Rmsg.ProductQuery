// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="StubApiClientFactory.cs" company="Rakuten">
//   Copyright (c) Rakuten. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.WebJob.Tests.Unit
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;

    using Rakuten.Gpc.Api.Client.Fakes;
    using Rakuten.Net.Http.Fakes;

    /// <summary>
    /// Represents the factory for creating new <see cref="StubApiClient"/> instances.
    /// </summary>
    internal class StubApiClientFactory
    {
        /// <summary>
        /// Creates a configured <see cref="StubApiClient"/> instance that will use the specified delegate.
        /// </summary>
        /// <typeparam name="T">The type expected back from the response.</typeparam>
        /// <param name="stub">The delegate to be used when handling the response.</param>
        /// <returns>A new <see cref="StubApiClient"/> instance.</returns>
        public static StubApiClient Create<T>(Func<HttpResponseMessage, Task<T>> stub)
        {
            var stubIHttpResponseHandler = new StubIHttpResponseHandler();
            stubIHttpResponseHandler.ReadAsyncOf1HttpResponseMessage(message => stub(message));

            return new StubApiClient(
                new StubIApiClientContext
                {
                    AuthorizationTokenGet = () => "1234567890",
                    BaseAddressGet = () => new Uri("http://localhost")
                },
                new StubIHttpRequestHandler
                {
                    GetAsyncUri = uri => Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK))
                },
                stubIHttpResponseHandler);
        }
    }
}
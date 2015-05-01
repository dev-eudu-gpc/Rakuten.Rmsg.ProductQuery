//------------------------------------------------------------------------------
// <copyright file="ProductQueryGroupControllerTest.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.Web.Http.Tests
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Net.Http;
    using System.Net.Http.Fakes;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Web.Http;
    using System.Web.Http.Controllers;
    using System.Web.Http.Results;
    using System.Web.Http.Routing;
    using Microsoft.QualityTools.Testing.Fakes;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Microsoft.WindowsAzure.Storage;
    using Rakuten.Fakes;
    using Rakuten.Gpc.Api;
    using Rakuten.Rmsg.ProductQuery.Configuration.Fakes;
    using Rakuten.Rmsg.ProductQuery.Web.Http.Commands;

    /// <summary>
    /// Defines tests for the <see cref="ProductQueryGroupController"/> class.
    /// </summary>
    [TestClass]
    public class ProductQueryGroupControllerTest
    {
        /// <summary>
        /// Verifies that an attempt to get the status of a product query group
        /// with everything valid returns the correct result.
        /// </summary>
        /// <returns>A task that performs the test.</returns>
        [TestMethod]
        public async Task GetProgressReturnsCorrectResultWhenEverythingIsValid()
        {
            // Arrange
            var id = Guid.NewGuid();
            var date = DateTime.UtcNow.AddMinutes(-1);

            ProductQueryGroupController controller = this.CreateController();

            // Act
            IHttpActionResult result = await controller.GetProgressAsync(
                id.ToString(),
                date.Year.ToString("00"),
                date.Month.ToString("00"),
                date.Day.ToString("00"),
                date.Hour.ToString("00"),
                date.Minute.ToString("00"));

            // Assert
            Assert.IsInstanceOfType(result, typeof(ImageResult));
        }

        /// <summary>
        /// Verifies that an attempt to the status of a product query group
        /// with a date that is too recent returns the correct result.
        /// </summary>
        /// <returns>A task that performs the test.</returns>
        [TestMethod]
        public async Task GetProgressReturnsCorrectResultWhenTheDateIsTooRecent()
        {
            // Arrange
            var intervalInSeconds = 60;
            var id = Guid.NewGuid();
            var date = DateTime.UtcNow.AddSeconds(intervalInSeconds - 1);
            var expectedLocationHeader = string.Format(
                "/product-query-group/{0}/status/{1}/{2}/{3}/{4}/{5}",
                id.ToString(),
                date.Year.ToString("00"),
                date.Month.ToString("00"),
                date.Day.ToString("00"),
                date.Hour.ToString("00"),
                date.Minute.ToString("00"));

            ProductQueryGroupController controller = this.CreateController(
                apiContext: new StubIApiContext() { ProgressMapIntervalInSecondsGet = () => intervalInSeconds },
                monitorUriTemplate: StubIUriTemplateFactory.Create(expectedLocationHeader) as StubIUriTemplate);

            // Act
            IHttpActionResult result = await controller.GetProgressAsync(
                id.ToString(),
                date.Year.ToString("00"),
                date.Month.ToString("00"),
                date.Day.ToString("00"),
                date.Hour.ToString("00"),
                date.Minute.ToString("00"));

            HttpResponseMessage response = await result.ExecuteAsync(new CancellationToken());

            // Assert
            Assert.IsInstanceOfType(result, typeof(SeeOtherResult));
            Assert.IsNotNull(response.Headers.Location);
            Assert.AreEqual(expectedLocationHeader, response.Headers.Location.ToString(), false);
        }

        /// <summary>
        /// Returns a new instance of <see cref="ProductQueryGroupController"/>.
        /// </summary>
        /// <param name="apiContext">The context in which this instance is running.</param>
        /// <param name="getProgressCommand">
        /// A command that gets an image representing the progress of a given group at a given point in time.
        /// </param>
        /// <param name="monitorUriTemplate">
        /// A link template representing the canonical location for
        /// that status of a product query at a particular point in time
        /// </param>
        /// <returns>A new instance of <see cref="ProductQueryGroupController"/>.</returns>
        private ProductQueryGroupController CreateController(
            StubIApiContext apiContext = null,
            StubICommand<GetProgressCommandParameters, Task<Stream>> getProgressCommand = null,
            StubIUriTemplate monitorUriTemplate = null)
        {
            var context = apiContext ?? new StubIApiContext();

            var get = getProgressCommand ??
                new StubICommand<GetProgressCommandParameters, Task<Stream>>
                {
                    ExecuteT0 = parameters => Task.Run(() => new MemoryStream() as Stream)
                };

            var uriTemplate = monitorUriTemplate ?? StubIUriTemplateFactory.Create();

            // Arrange controller
            return new ProductQueryGroupController(context, get, uriTemplate)
            {
                Configuration = new HttpConfiguration(),
                Request = new StubHttpRequestMessage()
            };
        }
    }
}
//------------------------------------------------------------------------------
// <copyright file="GetProgressCommandTest.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.Web.Http.Tests
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Rakuten.Fakes;
    using Rakuten.Rmsg.ProductQuery.Configuration.Fakes;
    using Rakuten.Rmsg.ProductQuery.Web.Http.Commands;
    using Rakuten.Rmsg.ProductQuery.Web.Http.Commands.Fakes;

    /// <summary>
    /// Defines tests for the <see cref="GetProgressCommand"/> class.
    /// </summary>
    [TestClass]
    public class GetProgressCommandTest
    {
        /// <summary>
        /// Verifies that the command returns the correct stream
        /// </summary>
        /// <returns>A task that performs the test.</returns>
        [TestMethod]
        public async Task GetProgressCommandReturnsStream()
        {
            // Arrange
            var context = new StubIApiContext();
            var command = this.CreateCommand();

            // Act
            var id = Guid.NewGuid().ToString();
            var dateTime = new DateTime(2011, 10, 11, 12, 13, 0);
            var result = await command.ExecuteAsync(
                new StubGetProgressCommandParameters(
                    id,
                    dateTime.Year.ToString(),
                    dateTime.Month.ToString(),
                    dateTime.Day.ToString(),
                    dateTime.Hour.ToString(),
                    dateTime.Minute.ToString()));

            // Assert
            Assert.IsInstanceOfType(result, typeof(Stream));
        }

        /// <summary>
        /// Returns a new instance of <see cref="GetProgressCommand"/>.
        /// </summary>
        /// <param name="apiContext">The context in which this instance is running.</param>
        /// <param name="monitorUriTemplate">A link template representing the canonical location of the monitor for the resource.</param>
        /// <param name="createImageCommand">A command that creates an image from a progress map.</param>
        /// <param name="getProgressDatabaseCommand">A command that gets product query data from the database.</param>
        /// <returns>A new instance of <see cref="GetProgressCommand"/>.</returns>
        private GetProgressCommand CreateCommand(
            StubIApiContext apiContext = null,
            StubIUriTemplate monitorUriTemplate = null,
            StubICommand<CreateProgressImageCommandParameters, Task<Stream>> createImageCommand = null,
            StubICommand<GetProgressDatabaseCommandParameters, Task<IQueryable<ProductQueryProgress>>> getProgressDatabaseCommand = null)
        {
            ////// Arrange stub dependencies
            ////var context = context ?? new StubIApiContext();
            ////var get = getProgressDatabaseCommand ??
            ////    new StubICommand<CreateProgressImageCommandParameters, Task<Stream>>
            ////    {
            ////        ExecuteT0 = parameters => Task.Run(() => new ProductQuery())
            ////    };

            ////var create = createImageCommand ??
            ////    new StubICommand<GetProgressDatabaseCommandParameters, Task<IQueryable<ProductQueryProgress>>>
            ////    {
            ////        ExecuteT0 = parameters => Task.Run(() => new ProductQuery())
            ////    };

            ////var monitorTemplate = monitorUriTemplate ?? new StubIUriTemplate();

            ////// Arrange controller
            ////return new GetProgressCommand(context, monitorUriTemplate, createImageCommand, GetProgressDatabaseCommand);

            // Arrange stub dependencies
            var context = apiContext ?? new StubIApiContext();
            var create = createImageCommand ??
                new StubICommand<CreateProgressImageCommandParameters, Task<Stream>>
                {
                    ExecuteT0 = parameters => Task.Run(() => new MemoryStream() as Stream)
                };
            var getProgress = getProgressDatabaseCommand ??
                new StubICommand<GetProgressDatabaseCommandParameters, Task<IQueryable<ProductQueryProgress>>>
                {
                    ExecuteT0 = parameters => Task.Run(() => Enumerable.Empty<ProductQueryProgress>().AsQueryable())
                };

            return new GetProgressCommand(context, create, getProgress);
        }
    }
}
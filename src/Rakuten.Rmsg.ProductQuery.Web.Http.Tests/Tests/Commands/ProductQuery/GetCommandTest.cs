//------------------------------------------------------------------------------
// <copyright file="GetCommandTest.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.Web.Http.Tests
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.QualityTools.Testing.Fakes;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Rakuten.Fakes;
    using Rakuten.Rmsg.ProductQuery.Configuration.Fakes;
    using Rakuten.Rmsg.ProductQuery.Web.Http.Commands;
    using Rakuten.Rmsg.ProductQuery.Web.Http.Commands.Fakes;
    using Rakuten.Rmsg.ProductQuery.Web.Http.Links;
    using Rakuten.Rmsg.ProductQuery.Web.Http.Links.Fakes;

    /// <summary>
    /// Defines tests for the <see cref="GetCommand"/> class.
    /// </summary>
    [TestClass]
    public class GetCommandTest
    {
        /// <summary>
        /// Verifies that the get command returns the product query found by the database command.
        /// </summary>
        /// <returns>A task that performs the test.</returns>
        [TestMethod]
        public async Task GetCommandReturnsProductQueryWhenFoundInDatabase()
        {
            // Arrange
            using (var shimsContext = ShimsContext.Create())
            {
                var stubIUriTemplate = StubIUriTemplateFactory.Create();
                ShimProductQueryLink.AllInstances.ForCultureString = (a, b) => new ProductQueryLink(stubIUriTemplate);
                ShimProductQueryLink.AllInstances.ForIdString = (a, b) => new ProductQueryLink(stubIUriTemplate);
                ////ShimLinkTemplate.AllInstances.ToLinkBoolean = (a, b) => new Link();
                ////ShimLinkTemplate.AllInstances.Expand = a => new Link();

                var parameters = new StubGetCommandParameters(Guid.NewGuid().ToString(), "en-US");

                var command = this.CreateCommand();

                // Act
                var result = await command.ExecuteAsync(parameters);

                // Assert
                Assert.IsTrue(result != null);
            }
        }

        /// <summary>
        /// Verifies that the get command returns null when a product query is not found in the database.
        /// </summary>
        /// <returns>A task that performs the test.</returns>
        [TestMethod]
        public async Task GetCommandReturnsNullWhenNotFoundInDatabase()
        {
            // Arrange
            using (var shimsContext = ShimsContext.Create())
            {
                var stubIUriTemplate = StubIUriTemplateFactory.Create();
                ShimProductQueryLink.AllInstances.ForCultureString = (a, b) => new ProductQueryLink(stubIUriTemplate);
                ShimProductQueryLink.AllInstances.ForIdString = (a, b) => new ProductQueryLink(stubIUriTemplate);
                ////ShimLinkTemplate.AllInstances.ToLinkBoolean = (a, b) => new Link();
                ShimLinkTemplate.AllInstances.Expand = a => new Link();

                var parameters = new StubGetCommandParameters(Guid.NewGuid().ToString(), "en-US");

                var command = this.CreateCommand(
                    getDatabaseCommand: new StubICommand<GetDatabaseCommandParameters, Task<ProductQuery>>
                    {
                        ExecuteT0 = p => Task.Run(() => null as ProductQuery)
                    });

                // Act
                var result = await command.ExecuteAsync(parameters);

                // Assert
                Assert.IsTrue(result == null);
            }
        }

        /// <summary>
        /// Verifies that the get command populates the link collection of the
        /// product query with a self link.
        /// </summary>
        /// <returns>A task that performs the test.</returns>
        [TestMethod]
        public async Task GetCommandPopulatesProductQueryLinkCollectionWithSelfLink()
        {
            // Arrange
            var parameters = new StubGetCommandParameters(Guid.NewGuid().ToString(), "en-US");

            var command = this.CreateCommand();

            // Act
            var result = await command.ExecuteAsync(parameters);

            // Assert
            Assert.IsTrue(result.Links != null);
            Assert.IsTrue(result.Links.Any<Link>(link => link.RelationType.Equals("self", StringComparison.InvariantCultureIgnoreCase)));
        }

        /// <summary>
        /// Verifies that the get command populates the link collection of the
        /// product query with a monitor link.
        /// </summary>
        /// <returns>A task that performs the test.</returns>
        [TestMethod]
        public async Task GetCommandPopulatesProductQueryLinkCollectionWithMonitorLink()
        {
            // Arrange
            var parameters = new StubGetCommandParameters(Guid.NewGuid().ToString(), "en-US");

            var command = this.CreateCommand();

            // Act
            var result = await command.ExecuteAsync(parameters);

            // Assert
            Assert.IsTrue(result.Links != null);
            Assert.IsTrue(result.Links.Any(link => link.RelationType.Equals("monitor", StringComparison.InvariantCultureIgnoreCase)));
        }

        /// <summary>
        /// Verifies that the get command populates the link collection of the
        /// product query with an enclosure link when the product query has a blob URI
        /// </summary>
        /// <returns>A task that performs the test.</returns>
        [TestMethod]
        public async Task GetCommandPopulatesProductQueryLinkCollectionWithEnclosureLinkWhenProductQueryHasBlobUri()
        {
            // Arrange
            var parameters = new StubGetCommandParameters(Guid.NewGuid().ToString(), "en-US");

            var command = this.CreateCommand(
                getDatabaseCommand: new StubICommand<GetDatabaseCommandParameters, Task<ProductQuery>>
                    {
                        ExecuteT0 = p => Task.Run(() => ProductQueryFactory.Create(
                            id: Guid.NewGuid(),
                            cultureName: "en-US",
                            blobUri: "bloburi"))
                    });

            // Act
            var result = await command.ExecuteAsync(parameters);

            // Assert
            Assert.IsTrue(result.Links != null);
            Assert.IsTrue(result.Links.Any(link => link.RelationType.Equals("enclosure", StringComparison.InvariantCultureIgnoreCase)));
        }

        /// <summary>
        /// Verifies that the get command does not populate the link collection of the
        /// product query with an enclosure link when the product query does not have a blob URI
        /// </summary>
        /// <returns>A task that performs the test.</returns>
        [TestMethod]
        public async Task GetCommandDoesNotPopulateProductQueryLinkCollectionWithEnclosureLinkWhenProductQueryDoesNotHaveBlobUri()
        {
            // Arrange
            var parameters = new StubGetCommandParameters(Guid.NewGuid().ToString(), "en-US");

            var command = this.CreateCommand();

            // Act
            var result = await command.ExecuteAsync(parameters);

            // Assert
            Assert.IsTrue(result.Links != null);
            Assert.IsFalse(result.Links.Any(link => link.RelationType.Equals("enclosure", StringComparison.InvariantCultureIgnoreCase)));
        }

        /// <summary>
        /// Returns a new instance of <see cref="GetCommand"/>.
        /// </summary>
        /// <param name="apiContext">The context in which this instance is running.</param>
        /// <param name="productQueryUriTemplate">A link template representing the canonical location of the resource.</param>
        /// <param name="azureBlobUriTemplate">A link template representing the canonical location of the blob in Azure storage.</param>
        /// <param name="monitorUriTemplate">A link template representing the canonical location of the monitor for the resource.</param>
        /// <param name="getDatabaseCommand">A command that gets product query data from the database.</param>
        /// <returns>A new instance of <see cref="GetCommand"/>.</returns>
        private GetCommand CreateCommand(
            StubIApiContext apiContext = null,
            StubIUriTemplate productQueryUriTemplate = null,
            StubIUriTemplate azureBlobUriTemplate = null,
            StubIUriTemplate monitorUriTemplate = null,
            StubICommand<GetDatabaseCommandParameters, Task<ProductQuery>> getDatabaseCommand = null)
        {
            var apiContextParameter = apiContext ?? new StubIApiContext();
            var productQueryUriTemplateParameter = productQueryUriTemplate ?? StubIUriTemplateFactory.Create();
            var azureBlobUriTemplateParameter = azureBlobUriTemplate ?? StubIUriTemplateFactory.Create();
            var monitorUriTemplateParameter = monitorUriTemplate ?? StubIUriTemplateFactory.Create();
            var getDatabaseCommandParameter = getDatabaseCommand ?? new StubICommand<GetDatabaseCommandParameters, Task<ProductQuery>>
            {
                ExecuteT0 = p => Task.Run(() => ProductQueryFactory.Create(Guid.NewGuid(), "en-US"))
            };

            return new GetCommand(
                apiContextParameter,
                productQueryUriTemplateParameter,
                azureBlobUriTemplateParameter,
                monitorUriTemplateParameter,
                getDatabaseCommandParameter);
        }
    }
}
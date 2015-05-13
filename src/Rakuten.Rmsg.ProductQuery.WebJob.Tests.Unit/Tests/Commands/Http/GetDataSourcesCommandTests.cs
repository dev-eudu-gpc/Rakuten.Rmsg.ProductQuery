// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="GetDataSourcesCommandTests.cs" company="Rakuten">
//   Copyright (c) Rakuten. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.WebJob.Tests.Unit
{
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Rakuten.Fakes;
    using Rakuten.Rmsg.ProductQuery.WebJob.Api;
    using Rakuten.Rmsg.ProductQuery.WebJob.Linking.Fakes;

    /// <summary>
    /// A suite of tests for the <see cref="GetDataSourcesCommand"/> class.
    /// </summary>
    [TestClass]
    public class GetDataSourcesCommandTests
    {
        /// <summary>
        /// Ensures that the return value is a non null collection.
        /// </summary>
        /// <returns>A <see cref="Task"/> the represents the asynchronous operation.</returns>
        public async Task GetDataSourcesCommandReturnsANonNullCollection()
        {
            // Arrange
            var link = new StubDataSourcesLink(new StubUriTemplate(
                "/v1/datasources"));

            // Act
            var dataSources = await GetDataSourcesCommand.Execute(
                link,
                () => StubApiClientFactory.Create(message => Task.FromResult(Enumerable.Empty<DataSource>())));

            // Assert
            Assert.IsNotNull(dataSources);
        }
    }
}
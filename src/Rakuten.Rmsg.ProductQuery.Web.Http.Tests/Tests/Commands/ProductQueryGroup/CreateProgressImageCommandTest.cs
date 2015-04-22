//------------------------------------------------------------------------------
// <copyright file="CreateProductQueryGroupProgressImageCommandTest.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.Web.Http.Tests
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows.Media.Imaging;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Rakuten.Rmsg.ProductQuery.Configuration.Fakes;
    using Rakuten.Rmsg.ProductQuery.Web.Http.Commands;

    /// <summary>
    /// Defines tests for the <see cref="CreateProductQueryGroupProgressImageCommand"/> class.
    /// </summary>
    [TestClass]
    public class CreateProgressImageCommandTest
    {
        /// <summary>
        /// Ensures that the progress map image has the correct colour for pixels at
        /// an index that has no product query.
        /// </summary>
        /// <returns>A task</returns>
        [TestMethod]
        public async Task ProgressMapImageHasCorrectColourForEmptyIndexes()
        {
            // Arrange a progress map
            int maxQueriesPerGroup = 10000;
            var progressMap = new List<ProductQueryProgress>
            {
                new ProductQueryProgress(1, ProductQueryStatus.New, 0, 0, 0)
            };

            // Arrange stubs
            var context = new StubIApiContext() { MaximumQueriesPerGroupGet = () => maxQueriesPerGroup };

            // Arrange dependencies
            var parameters = new CreateProgressImageCommandParameters(progressMap.AsQueryable());

            // Arrange the object to be tested
            var command = new CreateProgressImageCommand(context);

            // Act
            Stream result = await command.ExecuteAsync(parameters);

            byte[] pixels = ProgressMapImageFactory.ImageStreamToBytes(result);

            // Assert
            for (int i = 0; i < pixels.Length; i++)
            {
                if (!progressMap.Any(q => q.Index == i))
                {
                    Assert.AreEqual(0, pixels[i], string.Format("Incorrect colour at position {0}", i));
                }
            }
        }

        /// <summary>
        /// Ensures that the progress map has the correct colour for a 100% complete product query.
        /// </summary>
        /// <returns>A task.</returns>
        [TestMethod]
        public async Task ProgressMapImageHasCorrectColourForOneHundredPercentCompleted()
        {
            // Arrange a progress map
            int maxQueriesPerGroup = 10000;
            var progressMap = new List<ProductQueryProgress>
            {
                new ProductQueryProgress(1, ProductQueryStatus.Completed, 10, 10, 0)
            };

            // Arrange stubs
            var context = new StubIApiContext() { MaximumQueriesPerGroupGet = () => maxQueriesPerGroup, };

            // Arrange dependencies
            var parameters = new CreateProgressImageCommandParameters(progressMap.AsQueryable());

            // Arrange the object to be tested
            var command = new CreateProgressImageCommand(context);

            // Act
            Stream result = await command.ExecuteAsync(parameters);

            byte[] pixels = ProgressMapImageFactory.ImageStreamToBytes(result);

            // Assert
            ProgressMapAssert.ArePercentagesEqual(progressMap.AsQueryable(), pixels);
        }

        /// <summary>
        /// Ensures that the progress map has the correct colour for a 0% complete product query.
        /// </summary>
        /// <returns>A task.</returns>
        [TestMethod]
        public async Task ProgressMapImageHasCorrectColourForZeroPercentCompleted()
        {
            // Arrange a progress map
            int maxQueriesPerGroup = 10000;
            var progressMap = new List<ProductQueryProgress>
            {
                new ProductQueryProgress(1, ProductQueryStatus.New, 0, 0, 0)
            };

            // Arrange stubs
            var context = new StubIApiContext() { MaximumQueriesPerGroupGet = () => maxQueriesPerGroup, };

            // Arrange dependencies
            var parameters = new CreateProgressImageCommandParameters(progressMap.AsQueryable());

            // Arrange the object to be tested
            var command = new CreateProgressImageCommand(context);

            // Act
            Stream result = await command.ExecuteAsync(parameters);

            byte[] pixels = ProgressMapImageFactory.ImageStreamToBytes(result);

            // Assert
            ProgressMapAssert.ArePercentagesEqual(progressMap.AsQueryable(), pixels);
        }

        /// <summary>
        /// Ensures that the progress map image has the correct colours when the
        /// supplied progress map is empty.
        /// </summary>
        /// <returns></returns>
        public async Task ProgressMapImageHasCorrectColoursForAnEmptyProgressMap()
        {
            // Arrange a progress map
            int maxQueriesPerGroup = 10000;
            var progressMap = Enumerable.Empty<ProductQueryProgress>();

            // Arrange stubs
            var context = new StubIApiContext() { MaximumQueriesPerGroupGet = () => maxQueriesPerGroup };

            // Arrange dependencies
            var parameters = new CreateProgressImageCommandParameters(progressMap.AsQueryable());

            // Arrange the object to be tested
            var command = new CreateProgressImageCommand(context);

            // Act
            Stream result = await command.ExecuteAsync(parameters);

            byte[] pixels = ProgressMapImageFactory.ImageStreamToBytes(result);

            // Assert
            for (int i = 0; i < pixels.Length; i++)
            {
                Assert.AreEqual(0, pixels[i], string.Format("Incorrect colour at position {0}", i));
            }
        }

        /// <summary>
        /// Ensures that the progress map has the correct colours for a progress map
        /// that completely fills the image.
        /// </summary>
        /// <returns>A task.</returns>
        [TestMethod]
        public async Task ProgressMapImageHasCorrectColoursForFullProgressMap()
        {
            // Arrange a progress map containing 10,000 queries covering items completed from 0 to 10,000
            int maxQueriesPerGroup = 10000;
            int totalItemCount = maxQueriesPerGroup;
            var progressMap = Enumerable.Range(0, totalItemCount - 1)
                .Select(completedCount => new ProductQueryProgress(completedCount + 1, ProductQueryStatus.Submitted, totalItemCount, completedCount, 0))
                .AsQueryable();

            // Arrange stubs
            var context = new StubIApiContext() { MaximumQueriesPerGroupGet = () => maxQueriesPerGroup };

            // Arrange dependencies
            var parameters = new CreateProgressImageCommandParameters(progressMap);

            // Arrange the object to be tested
            var command = new CreateProgressImageCommand(context);

            // Act
            Stream result = await command.ExecuteAsync(parameters);

            byte[] pixels = ProgressMapImageFactory.ImageStreamToBytes(result);

            // Assert
            ProgressMapAssert.ArePercentagesEqual(progressMap, pixels);
        }
        /// <summary>
        /// Ensures that the progress map image has the correct dimensions when the
        /// maximum number of queries per group is set to a non square number.
        /// </summary>
        /// <returns>A task</returns>
        [TestMethod]
        public async Task ProgressMapImageHasCorrectDimensionsForNonSquareMaxQueriesPerGroup()
        {
            // Arrange a progress map
            int maxQueriesPerGroup = 50;
            var progressMap = Enumerable.Empty<ProductQueryProgress>();

            // Arrange stubs
            var context = new StubIApiContext() { MaximumQueriesPerGroupGet = () => maxQueriesPerGroup };

            // Arrange dependencies
            var parameters = new CreateProgressImageCommandParameters(progressMap.AsQueryable());

            // Arrange the object to be tested
            var command = new CreateProgressImageCommand(context);

            // Act
            Stream result = await command.ExecuteAsync(parameters);

            BitmapImage image = ProgressMapImageFactory.ImageStreamToImage(result);

            // Assert
            int expected = (int)Math.Ceiling(Math.Sqrt(maxQueriesPerGroup));
            Assert.AreEqual(expected, image.PixelWidth);
            Assert.AreEqual(expected, image.PixelHeight);
        }

        /// <summary>
        /// Ensures that the progress map image has the correct dimensions when the
        /// maximum number of queries per group is set to a square number.
        /// </summary>
        /// <returns>A task</returns>
        [TestMethod]
        public async Task ProgressMapImageHasCorrectDimensionsForSquareMaxQueriesPerGroup()
        {
            // Arrange a progress map
            int maxQueriesPerGroup = 100;
            var progressMap = Enumerable.Empty<ProductQueryProgress>();

            // Arrange stubs
            var context = new StubIApiContext() { MaximumQueriesPerGroupGet = () => maxQueriesPerGroup };

            // Arrange dependencies
            var parameters = new CreateProgressImageCommandParameters(progressMap.AsQueryable());

            // Arrange the object to be tested
            var command = new CreateProgressImageCommand(context);

            // Act
            Stream result = await command.ExecuteAsync(parameters);

            BitmapImage image = ProgressMapImageFactory.ImageStreamToImage(result);

            // Assert
            int expected = (int)Math.Ceiling(Math.Sqrt(maxQueriesPerGroup));
            Assert.AreEqual(expected, image.PixelWidth);
            Assert.AreEqual(expected, image.PixelHeight);
        }
    }
}
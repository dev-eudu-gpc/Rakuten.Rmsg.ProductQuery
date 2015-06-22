﻿//------------------------------------------------------------------------------
// <copyright file="ProductQueryFileSteps.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.Web.Http.Tests.Integration.Steps
{
    using System.IO;
    using System.Linq;
    using LumenWorks.Framework.IO.Csv;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Rakuten.IO.Delimited.Serialization;
    using TechTalk.SpecFlow;

    /// <summary>
    /// Steps pertaining to the product query file.
    /// </summary>
    [Binding]
    public class ProductQueryFileSteps
    {
        /// <summary>
        /// Creates a new file for a product query using the products in
        /// scenario storage as the source for EAN.
        /// </summary>
        [Given(@"a product query file for the new product has been created")]
        public void GivenAProductQueryFileForTheNewProductHasBeenCreated()
        {
            var items = ScenarioStorage.Products
                .Select(p => ItemFactory.Create("EAN", p.GetEAN()));

            // Create a product query file for the EANs
            var filename = ProductQueryFileFactory.Create(items);

            // Store details in scenario storage
            ScenarioStorage.ProductQueryFileName = filename;
            ScenarioStorage.Items = items.ToList();
        }

        /// <summary>
        /// Creates a new file for a product query with the image url columns populated
        /// using the products in scenario storage as the source for EAN.
        /// </summary>
        [Given(@"a product query file containing image urls for the new product has been created")]
        public void GivenAProductQueryFileContainingImageUrlsForTheNewProductHasBeenCreated()
        {
            var items = ScenarioStorage.Products
                .Select(p => ItemFactory.Create(
                    "EAN",
                    p.GetEAN(),
                    "image01.jpg",
                    "image02.jpg",
                    "image03.jpg",
                    "image04.jpg",
                    "image05.jpg",
                    "image06.jpg",
                    "image07.jpg",
                    "image08.jpg",
                    "image09.jpg",
                    "image10.jpg"));

            // Create a product query file for the EANs
            var filename = ProductQueryFileFactory.Create(items);

            // Store details in scenario storage
            ScenarioStorage.ProductQueryFileName = filename;
            ScenarioStorage.Items = items.ToList();
        }

        /// <summary>
        /// Parses the items from the results file and dumps them
        /// in scenario storage.
        /// </summary>
        [When(@"the items have been parsed from the results file")]
        public void WhenTheItemsHaveBeenParsedFromTheResultsFile()
        {
            using (var streamReader = new StreamReader(ScenarioStorage.ResultFileName, true))
            using (var delimitedReader = new CsvReader(streamReader, true, ','))
            {
                var serializer = new LumenWorksSerializer<Item>();
                ScenarioStorage.ResultItems = serializer.ReadFileByHeaders(delimitedReader).ToList();
            }
        }

        /// <summary>
        /// Verifies that the items in the results file have the correct manufacturer.
        /// </summary>
        [Then(@"the items in the results file have the correct manufacturer")]
        public void ThenTheItemsInTheResultsFileHaveTheCorrectManufacturer()
        {
            // Arrange
            var sourceProducts = ScenarioStorage.Products;
            var resultItems = ScenarioStorage.ResultItems;

            foreach (var result in resultItems)
            {
                var product = sourceProducts.FirstOrDefault(p => p.GetEAN() == result.GtinValue);

                Assert.IsNotNull(product);
                Assert.AreEqual(product.Manufacturer, result.Manufacturer);
            }
        }

        /// <summary>
        /// Verifies that the items in the results file have the correct manufacturer part number.
        /// </summary>
        [Then(@"the items in the results file have the correct manufacturer part number")]
        public void ThenTheItemsInTheResultsFileHaveTheCorrectManufacturerPartNumber()
        {
            // Arrange
            var sourceProducts = ScenarioStorage.Products;
            var resultItems = ScenarioStorage.ResultItems;

            foreach (var result in resultItems)
            {
                var product = sourceProducts.FirstOrDefault(p => p.GetEAN() == result.GtinValue);

                Assert.IsNotNull(product);
                Assert.AreEqual(product.PartNumber, result.ManufacturerPartNumber);
            }
        }

        /// <summary>
        /// Verifies that the items in the results file have the correct brand.
        /// </summary>
        [Then(@"the items in the results file have the correct brand")]
        public void ThenTheItemsInTheResultsFileHaveTheCorrectBrand()
        {
            // Arrange
            var sourceProducts = ScenarioStorage.Products;
            var resultItems = ScenarioStorage.ResultItems;

            foreach (var result in resultItems)
            {
                var product = sourceProducts.FirstOrDefault(p => p.GetEAN() == result.GtinValue);

                Assert.IsNotNull(product);
                Assert.AreEqual(product.GetBrand(), result.Brand);
            }
        }

        /// <summary>
        /// Verifies that the items in the results file have the correct video url.
        /// </summary>
        [Then(@"the items in the results file have the correct video URL")]
        public void ThenTheItemsInTheResultsFileHaveTheCorrectVideoURL()
        {
            // Arrange
            var sourceProducts = ScenarioStorage.Products;
            var resultItems = ScenarioStorage.ResultItems;

            foreach (var result in resultItems)
            {
                var product = sourceProducts.FirstOrDefault(p => p.GetEAN() == result.GtinValue);

                Assert.IsNotNull(product);
                Assert.AreEqual(product.GetVideoUrl(), result.VideoUrl);
            }
        }

        /// <summary>
        /// Verifies that the items in the results file have the correct images.
        /// </summary>
        [Then(@"the items in the results file have the correct images")]
        public void ThenTheItemsInTheResultsFileHaveTheCorrectImages()
        {
            // Arrange
            var sourceProducts = ScenarioStorage.Products;
            var resultItems = ScenarioStorage.ResultItems;

            foreach (var result in resultItems)
            {
                var product = sourceProducts.FirstOrDefault(p => p.GetEAN() == result.GtinValue);

                Assert.IsNotNull(product);
                Assert.AreEqual(product.GetImageUrl(1), result.ImageUrl1);
                Assert.AreEqual(product.GetImageUrl(2), result.ImageUrl2);
                Assert.AreEqual(product.GetImageUrl(3), result.ImageUrl3);
                Assert.AreEqual(product.GetImageUrl(4), result.ImageUrl4);
                Assert.AreEqual(product.GetImageUrl(5), result.ImageUrl5);
            }
        }

        /// <summary>
        /// Verifies that the images in the result file are the same as those in the source file.
        /// </summary>
        [Then(@"the images in the file have been preserved")]
        public void ThenTheImagesInTheFileHaveBeenPreserved()
        {
            // Arrange
            var sourceItems = ScenarioStorage.Items;
            var resultItems = ScenarioStorage.ResultItems;

            // Assert
            for (int i = 0; i < sourceItems.Count; i++)
            {
                var resultItem = resultItems[i];

                Assert.AreEqual(sourceItems[i].ImageUrl1, resultItems[i].ImageUrl1);
                Assert.AreEqual(sourceItems[i].ImageUrl2, resultItems[i].ImageUrl2);
                Assert.AreEqual(sourceItems[i].ImageUrl3, resultItems[i].ImageUrl3);
                Assert.AreEqual(sourceItems[i].ImageUrl4, resultItems[i].ImageUrl4);
                Assert.AreEqual(sourceItems[i].ImageUrl5, resultItems[i].ImageUrl5);
                Assert.AreEqual(sourceItems[i].ImageUrl6, resultItems[i].ImageUrl6);
                Assert.AreEqual(sourceItems[i].ImageUrl7, resultItems[i].ImageUrl7);
                Assert.AreEqual(sourceItems[i].ImageUrl8, resultItems[i].ImageUrl8);
                Assert.AreEqual(sourceItems[i].ImageUrl9, resultItems[i].ImageUrl9);
                Assert.AreEqual(sourceItems[i].ImageUrl10, resultItems[i].ImageUrl10);
            }
        }
    }
}
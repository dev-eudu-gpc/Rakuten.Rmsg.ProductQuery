//------------------------------------------------------------------------------
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
        /// Creates a new product query file that has rows for the products created
        /// in earlier steps and an additional row that has insufficient columns.
        /// </summary>
        [Given(@"a product query file for the new product and an additional row with insufficient columns has been created")]
        public void GivenAProductQueryFileForTheNewProductAndAnAdditionalRowWithInsufficientColumnsHasBeenCreated()
        {
            // Create the items that have GTINs
            var items = ScenarioStorage.Products
                .Select(p => ItemFactory.Create("EAN", p.GetEAN()))
                .ToList();

            // Create a product query file for the items
            var filename = ProductQueryFileFactory.Create(items);
            ProductQueryFileFactory.AddRowWithInsufficientColumns(filename);

            // Storage
            ScenarioStorage.ProductQueryFileName = filename;
            ScenarioStorage.Items = items;
        }

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
        /// Creates a new file for a product query with a single row
        /// but with the GTIN type column left blank.
        /// </summary>
        [Given(@"a product query file with no GTIN type for the new product has been created")]
        public void GivenAProductQueryFileWithNoGTINTypeForTheNewProductHasBeenCreated()
        {
            var items = ScenarioStorage.Products
                .Select(p => ItemFactory.Create(string.Empty, p.GetEAN()));

            // Create a product query file for the products
            var filename = ProductQueryFileFactory.Create(items);

            // Store details in scenario storage
            ScenarioStorage.ProductQueryFileName = filename;
            ScenarioStorage.Items = items.ToList();
        }

        /// <summary>
        /// Creates a new file for a product query with a single row
        /// but with the GTIN value column left blank.
        /// </summary>
        [Given(@"a product query file with no GTIN value for the new product has been created")]
        public void GivenAProductQueryFileWithNoGTINValueForTheNewProductHasBeenCreated()
        {
            var items = ScenarioStorage.Products
                .Select(p => ItemFactory.Create("EAN", string.Empty));

            // Create a product query file for the products
            var filename = ProductQueryFileFactory.Create(items);

            // Store details in scenario storage
            ScenarioStorage.ProductQueryFileName = filename;
            ScenarioStorage.Items = items.ToList();
        }

        /// <summary>
        /// Creates a new file for a product query with a single row
        /// but with no header row
        /// </summary>
        [Given(@"a product query file with no header row has been created")]
        public void GivenAProductQueryFileWithNoHeaderRowHasBeenCreated()
        {
            var items = ScenarioStorage.Products
                .Select(p => ItemFactory.Create("EAN", p.GetEAN()));

            // Create a product query file for the products
            var filename = ProductQueryFileFactory.Create(items, false);

            // Store details in scenario storage
            ScenarioStorage.ProductQueryFileName = filename;
            ScenarioStorage.Items = items.ToList();
        }

        /// <summary>
        /// Creates a new file for a product with some rows having GTINs
        /// but others not.  The rows that have GTINs are taken from the 
        /// products in scenario storage and those that don't are randomly 
        /// generated.
        /// </summary>
        [Given(@"a product query file with only some rows having GTINs has been created")]
        public void GivenAProductQueryFileWithOnlySomeRowsHavingGTINsHasBeenCreated()
        {
            // Create the items that have GTINs
            var items = ScenarioStorage.Products
                .Select(p => ItemFactory.Create("EAN", p.GetEAN()))
                .ToList();

            // Create items without GTINs
            items.Add(ItemFactory.Create(null, null));
            items.Add(ItemFactory.Create(null, null));
            items.Add(ItemFactory.Create(null, null));

            // Create a product query file for the items
            var filename = ProductQueryFileFactory.Create(items);

            // Storage
            ScenarioStorage.ProductQueryFileName = filename;
            ScenarioStorage.Items = items;
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
            Assert.AreEqual(sourceItems.Count, resultItems.Count);

            for (int i = 0; i < sourceItems.Count; i++)
            {
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

        /// <summary>
        /// Verifies that the items in the results file are the same as 
        /// those in the source file and they all have the same data
        /// in both target and source.
        /// </summary>
        [Then(@"the item in the results file is the same as the item in the source file")]
        public void ThenTheItemInTheResultsFileIsTheSameAsTheItemInTheSourceFile()
        {
            // Arrange
            var sourceItems = ScenarioStorage.Items;
            var resultItems = ScenarioStorage.ResultItems;

            // Assert
            Assert.AreEqual(sourceItems.Count, resultItems.Count);

            for (int i = 0; i  < sourceItems.Count; i++)
            {
                Assert.AreEqual(sourceItems[i], resultItems[i]);
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
        /// Verifies that the items in the results file have the correct manufacturer.
        /// </summary>
        [Then(@"the items in the results file have the correct manufacturer")]
        public void ThenTheItemsInTheResultsFileHaveTheCorrectManufacturer()
        {
            // Arrange
            var sourceProducts = ScenarioStorage.Products;
            var resultItems = ScenarioStorage.ResultItems;

            // Assert
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
        ///  Verifies that the items in the source file that do not have a GTIN value
        ///  have the same values in the results file.
        /// </summary>
        [Then(@"the items that do not have a GTIN value are the same in the results file as in the source file")]
        public void ThenTheItemsThatDoNotHaveAGTINValueAreTheSameInTheResultsFileAsInTheSourceFile()
        {
            // Arrange
            var sourceItems = ScenarioStorage.Items
                .Where(item => string.IsNullOrWhiteSpace(item.GtinValue))
                .ToList();
            var resultItems = ScenarioStorage.ResultItems
                .Where(item => string.IsNullOrWhiteSpace(item.GtinValue))
                .ToList();

            // Assert
            Assert.AreEqual(sourceItems.Count, resultItems.Count);

            for (int i = 0; i < sourceItems.Count; i++)
            {
                Assert.AreEqual(sourceItems[i], resultItems[i]);
            }
        }

        /// <summary>
        /// Verifies that the items in the source file that have insufficient columns
        /// are the same in the results file.
        /// </summary>
        [Then(@"the items that have insufficient columns are not present in the results file")]
        public void ThenTheItemsThatHaveInsufficientColumnsAreNotPresentInTheResultsFile()
        {
            // Arrange
            var resultItems = ScenarioStorage.ResultItems
                .Where(item => string.IsNullOrWhiteSpace(item.GtinValue));

            // Assert
            Assert.AreEqual(0, resultItems.Count());
        }

        /// <summary>
        /// Verifies that the items in the results file that have a GTIN value 
        /// have the correct brand.
        /// </summary>
        [Then(@"the valid items in the results file have the correct brand")]
        public void ThenTheValidItemsInTheResultsFileHaveTheCorrectBrand()
        {
            // Arrange
            var sourceProducts = ScenarioStorage.Products;
            var resultItems = ScenarioStorage.ResultItems
                .Where(item => !string.IsNullOrEmpty(item.GtinValue));

            foreach (var result in resultItems)
            {
                var product = sourceProducts.FirstOrDefault(p => p.GetEAN() == result.GtinValue);

                Assert.IsNotNull(product);
                Assert.AreEqual(product.GetBrand(), result.Brand);
            }
        }

        /// <summary>
        /// Verifies that the items in the results file that have a GTIN value 
        /// have the correct images.
        /// </summary>
        [Then(@"the valid items in the results file have the correct images")]
        public void ThenTheValidItemsInTheResultsFileHaveTheCorrectImages()
        {
            // Arrange
            var sourceProducts = ScenarioStorage.Products;
            var resultItems = ScenarioStorage.ResultItems
                .Where(item => !string.IsNullOrEmpty(item.GtinValue));

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
        /// Verifies that the items in the results file that have a GTIN value 
        /// have the correct manufacturer.
        /// </summary>
        [Then(@"the valid items in the results file have the correct manufacturer")]
        public void ThenTheValidItemsInTheResultsFileHaveTheCorrectManufacturer()
        {
            // Arrange
            var sourceProducts = ScenarioStorage.Products;
            var resultItems = ScenarioStorage.ResultItems
                .Where(item => !string.IsNullOrEmpty(item.GtinValue));

            // Assert
            foreach (var result in resultItems)
            {
                var product = sourceProducts.FirstOrDefault(p => p.GetEAN() == result.GtinValue);

                Assert.IsNotNull(product);
                Assert.AreEqual(product.Manufacturer, result.Manufacturer);
            }
        }

        /// <summary>
        /// Verifies that the items in the results file that have a GTIN value 
        /// have the correct manufacturer part number.
        /// </summary>
        [Then(@"the valid items in the results file have the correct manufacturer part number")]
        public void ThenTheValidItemsInTheResultsFileHaveTheCorrectManufacturerPartNumber()
        {
            // Arrange
            var sourceProducts = ScenarioStorage.Products;
            var resultItems = ScenarioStorage.ResultItems
                .Where(item => !string.IsNullOrEmpty(item.GtinValue));

            foreach (var result in resultItems)
            {
                var product = sourceProducts.FirstOrDefault(p => p.GetEAN() == result.GtinValue);

                Assert.IsNotNull(product);
                Assert.AreEqual(product.PartNumber, result.ManufacturerPartNumber);
            }
        }

        /// <summary>
        /// Verifies that the items in the results file that have a GTIN value 
        /// have the correct video url.
        /// </summary>
        [Then(@"the valid items in the results file have the correct video URL")]
        public void ThenTheValidItemsInTheResultsFileHaveTheCorrectVideoURL()
        {
            // Arrange
            var sourceProducts = ScenarioStorage.Products;
            var resultItems = ScenarioStorage.ResultItems
                .Where(item => !string.IsNullOrEmpty(item.GtinValue));

            foreach (var result in resultItems)
            {
                var product = sourceProducts.FirstOrDefault(p => p.GetEAN() == result.GtinValue);

                Assert.IsNotNull(product);
                Assert.AreEqual(product.GetVideoUrl(), result.VideoUrl);
            }
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
    }
}
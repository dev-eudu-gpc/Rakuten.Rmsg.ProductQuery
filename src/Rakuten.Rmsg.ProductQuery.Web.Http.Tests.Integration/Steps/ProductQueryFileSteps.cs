//------------------------------------------------------------------------------
// <copyright file="ProductQueryFileSteps.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.Web.Http.Tests.Integration.Steps
{
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Rakuten.Rmsg.ProductQuery.Web.Http.Tests.Integration.Resources;
    using TechTalk.SpecFlow;

    /// <summary>
    /// Steps pertaining to the product query file.
    /// </summary>
    [Binding]
    public class ProductQueryFileSteps
    {
        /// <summary>
        /// An object for storing and retrieving information from the scenario context.
        /// </summary>
        private readonly ScenarioStorage scenarioStorage;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductQueryFileSteps"/> class
        /// </summary>
        /// <param name="scenarioStorage">An object for sharing information between steps.</param>
        public ProductQueryFileSteps(ScenarioStorage scenarioStorage)
        {
            Contract.Requires(scenarioStorage != null);

            this.scenarioStorage = scenarioStorage;
        }

        /// <summary>
        /// Creates a new file for a product query with the image url columns populated
        /// using the products in scenario storage as the source for EAN.
        /// </summary>
        [Given(@"a product query file containing image urls for the new product has been created")]
        public void GivenAProductQueryFileContainingImageUrlsForTheNewProductHasBeenCreated()
        {
            var items = new List<Item>
            {
                ItemFactory.Create(
                    "EAN",
                    this.scenarioStorage.Gpc.Product.GetEAN(),
                    "image01.jpg",
                    "image02.jpg",
                    "image03.jpg",
                    "image04.jpg",
                    "image05.jpg",
                    "image06.jpg",
                    "image07.jpg",
                    "image08.jpg",
                    "image09.jpg",
                    "image10.jpg")
            };

            // Create a product query file for the EANs
            var filename = ProductQueryFileFactory.Create(items);

            // Store pertinent details for subsequent steps
            this.scenarioStorage.Files.SourceFileName = filename;
            this.scenarioStorage.Files.SourceItems = items;
        }

        /// <summary>
        /// Creates a new product query file that has rows for the products created
        /// in earlier steps and an additional row that has insufficient columns.
        /// </summary>
        [Given(@"a product query file for the new product and an additional row with insufficient columns has been created")]
        public void GivenAProductQueryFileForTheNewProductAndAnAdditionalRowWithInsufficientColumnsHasBeenCreated()
        {
            // Create the items that have GTINs
            var items = new List<Item>
            {
                ItemFactory.Create("EAN", this.scenarioStorage.Gpc.Product.GetEAN())
            };

            // Create a product query file for the items
            var filename = ProductQueryFileFactory.Create(items);

            // Add a row with insufficient columns the to file
            ProductQueryFileFactory.AddRowWithInsufficientColumns(filename);

            // Store pertinent details for subsequent steps
            this.scenarioStorage.Files.SourceFileName = filename;
            this.scenarioStorage.Files.SourceItems = items;
        }

        /// <summary>
        /// Creates a new file for a product query using the products in
        /// scenario storage as the source for EAN.
        /// </summary>
        [Given(@"a product query file for the new product has been created")]
        public void GivenAProductQueryFileForTheNewProductHasBeenCreated()
        {
            // Create the items
            var items = new List<Item>
            {
                ItemFactory.Create("EAN", this.scenarioStorage.Gpc.Product.GetEAN())
            };

            // Create a product query file for the EANs
            var filename = ProductQueryFileFactory.Create(items);

            // Store pertinent details for subsequent steps
            this.scenarioStorage.Files.SourceFileName = filename;
            this.scenarioStorage.Files.SourceItems = items;
        }

        /// <summary>
        /// Creates a new file for a product query with a single row
        /// but with the GTIN type column left blank.
        /// </summary>
        [Given(@"a product query file with no GTIN type for the new product has been created")]
        public void GivenAProductQueryFileWithNoGTINTypeForTheNewProductHasBeenCreated()
        {
            // Create a product query file for the products
            var items = new List<Item>
            {
                ItemFactory.Create(string.Empty, this.scenarioStorage.Gpc.Product.GetEAN())
            };
            var filename = ProductQueryFileFactory.Create(items);

            // Store pertinent details for subsequent steps
            this.scenarioStorage.Files.SourceFileName = filename;
            this.scenarioStorage.Files.SourceItems = items;
        }

        /// <summary>
        /// Creates a new file for a product query with a single row
        /// but with the GTIN value column left blank.
        /// </summary>
        [Given(@"a product query file with no GTIN value for the new product has been created")]
        public void GivenAProductQueryFileWithNoGTINValueForTheNewProductHasBeenCreated()
        {
            // Create a product query file for the products
            var items = new List<Item>
            {
                ItemFactory.Create("EAN", string.Empty)
            };

            var filename = ProductQueryFileFactory.Create(items);

            // Store pertinent details for subsequent steps
            this.scenarioStorage.Files.SourceFileName = filename;
            this.scenarioStorage.Files.SourceItems = items;
        }

        /// <summary>
        /// Creates a new file for a product query with a single row
        /// but with no header row
        /// </summary>
        [Given(@"a product query file with no header row has been created")]
        public void GivenAProductQueryFileWithNoHeaderRowHasBeenCreated()
        {
            // Create a product query file for the products
            var items = new List<Item>
            {
                ItemFactory.Create("EAN", this.scenarioStorage.Gpc.Product.GetEAN())
            };
            var filename = ProductQueryFileFactory.Create(items, false);

            // Store pertinent details for subsequent steps
            this.scenarioStorage.Files.SourceFileName = filename;
            this.scenarioStorage.Files.SourceItems = items;
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
            var items = new List<Item>
            {
                ItemFactory.Create("EAN", this.scenarioStorage.Gpc.Product.GetEAN())
            };

            // Create items without GTINs
            items.Add(ItemFactory.Create(null, null));
            items.Add(ItemFactory.Create(null, null));
            items.Add(ItemFactory.Create(null, null));

            // Create a product query file for the items
            var filename = ProductQueryFileFactory.Create(items);

            // Store pertinent details for subsequent steps
            this.scenarioStorage.Files.SourceFileName = filename;
            this.scenarioStorage.Files.SourceItems = items;
        }

        /// <summary>
        /// Verifies that the images in the result file are the same as those in the source file.
        /// </summary>
        [Then(@"the images in the file have been preserved")]
        public void ThenTheImagesInTheFileHaveBeenPreserved()
        {
            // Arrange
            var sourceItems = this.scenarioStorage.Files.SourceItems;
            var resultItems = ItemFactory.Get(this.scenarioStorage.Files.ResultFileName);

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
        /// Verifies that the items in the results file are the same as the items in the source file.
        /// </summary>
        [Then(@"the items in the results file are the same as the items in the source file")]
        public void ThenTheItemsInTheResultsFileAreTheSameAsTheItemsInTheSourceFile()
        {
            // Arrange
            var sourceItems = this.scenarioStorage.Files.SourceItems;
            var resultItems = ItemFactory.Get(this.scenarioStorage.Files.ResultFileName);

            // Assert
            Assert.AreEqual(sourceItems.Count, resultItems.Count);

            for (int i = 0; i < sourceItems.Count; i++)
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
            var sourceProduct = this.scenarioStorage.Gpc.Product;
            var resultItems = ItemFactory.Get(this.scenarioStorage.Files.ResultFileName);

            // Assert
            foreach (var result in resultItems)
            {
                var product = sourceProduct;

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
            var sourceProduct = this.scenarioStorage.Gpc.Product;
            var resultItems = ItemFactory.Get(this.scenarioStorage.Files.ResultFileName);

            // Assert
            foreach (var result in resultItems)
            {
                var product = sourceProduct;

                Assert.IsNotNull(product);
                Assert.IsNotNull(product);
                Assert.AreEqual(product.GetImageUrl(1), result.ImageUrl1, true);
                Assert.AreEqual(product.GetImageUrl(2), result.ImageUrl2, true);
                Assert.AreEqual(product.GetImageUrl(3), result.ImageUrl3, true);
                Assert.AreEqual(product.GetImageUrl(4), result.ImageUrl4, true);
                Assert.AreEqual(product.GetImageUrl(5), result.ImageUrl5, true);
            }
        }

        /// <summary>
        /// Verifies that the items in the results file have the correct manufacturer.
        /// </summary>
        [Then(@"the items in the results file have the correct manufacturer")]
        public void ThenTheItemsInTheResultsFileHaveTheCorrectManufacturer()
        {
            // Arrange
            var sourceProduct = this.scenarioStorage.Gpc.Product;
            var resultItems = ItemFactory.Get(this.scenarioStorage.Files.ResultFileName);

            // Assert
            foreach (var result in resultItems)
            {
                var product = sourceProduct;

                Assert.IsNotNull(product);
                Assert.AreEqual(product.Manufacturer, result.Manufacturer, true);
            }
        }

        /// <summary>
        /// Verifies that the items in the results file have the correct manufacturer part number.
        /// </summary>
        [Then(@"the items in the results file have the correct manufacturer part number")]
        public void ThenTheItemsInTheResultsFileHaveTheCorrectManufacturerPartNumber()
        {
            // Arrange
            var sourceProduct = this.scenarioStorage.Gpc.Product;
            var resultItems = ItemFactory.Get(this.scenarioStorage.Files.ResultFileName);

            // Assert
            foreach (var result in resultItems)
            {
                var product = sourceProduct;

                Assert.IsNotNull(product);
                Assert.AreEqual(product.PartNumber, result.ManufacturerPartNumber, true);
            }
        }

        /// <summary>
        /// Verifies that the items in the results file have the correct video url.
        /// </summary>
        [Then(@"the items in the results file have the correct video URL")]
        public void ThenTheItemsInTheResultsFileHaveTheCorrectVideoURL()
        {
            // Arrange
            var sourceProduct = this.scenarioStorage.Gpc.Product;
            var resultItems = ItemFactory.Get(this.scenarioStorage.Files.ResultFileName);

            // Assert
            foreach (var result in resultItems)
            {
                var product = sourceProduct;

                Assert.IsNotNull(product);
                Assert.AreEqual(product.GetVideoUrl(), result.VideoUrl, true);
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
            var sourceItems = this.scenarioStorage.Files.SourceItems
                .Where(i => string.IsNullOrWhiteSpace(i.GtinValue))
                .ToList();
            var resultItems = ItemFactory.Get(this.scenarioStorage.Files.ResultFileName)
                .Where(i => string.IsNullOrWhiteSpace(i.GtinValue))
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
            var resultItems = ItemFactory.Get(this.scenarioStorage.Files.ResultFileName)
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
            var sourceProduct = this.scenarioStorage.Gpc.Product;
            var resultItems = ItemFactory.Get(this.scenarioStorage.Files.ResultFileName)
                .Where(item => !string.IsNullOrEmpty(item.GtinValue));

            // Assert
            foreach (var result in resultItems)
            {
                var product = sourceProduct;

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
            var sourceProduct = this.scenarioStorage.Gpc.Product;
            var resultItems = ItemFactory.Get(this.scenarioStorage.Files.ResultFileName)
                .Where(item => !string.IsNullOrEmpty(item.GtinValue));

            // Assert
            foreach (var result in resultItems)
            {
                var product = sourceProduct;

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
            var sourceProduct = this.scenarioStorage.Gpc.Product;
            var resultItems = ItemFactory.Get(this.scenarioStorage.Files.ResultFileName)
                .Where(item => !string.IsNullOrEmpty(item.GtinValue));

            // Assert
            foreach (var result in resultItems)
            {
                var product = sourceProduct;

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
            var sourceProduct = this.scenarioStorage.Gpc.Product;
            var resultItems = ItemFactory.Get(this.scenarioStorage.Files.ResultFileName)
                .Where(item => !string.IsNullOrEmpty(item.GtinValue));

            // Assert
            foreach (var result in resultItems)
            {
                var product = sourceProduct;

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
            var sourceProduct = this.scenarioStorage.Gpc.Product;
            var resultItems = ItemFactory.Get(this.scenarioStorage.Files.ResultFileName)
                .Where(item => !string.IsNullOrEmpty(item.GtinValue));

            // Assert
            foreach (var result in resultItems)
            {
                var product = sourceProduct;

                Assert.IsNotNull(product);
                Assert.AreEqual(product.GetVideoUrl(), result.VideoUrl);
            }
        }
    }
}
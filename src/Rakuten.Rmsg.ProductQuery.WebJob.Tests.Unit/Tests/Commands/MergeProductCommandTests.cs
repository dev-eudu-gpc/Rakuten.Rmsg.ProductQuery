// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="MergeProductCommandTests.cs" company="Rakuten">
//   Copyright (c) Rakuten. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.WebJob.Tests.Unit
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Rakuten.Rmsg.ProductQuery.WebJob.Api;

    /// <summary>
    /// A suite of tests for the <see cref="MergeProductCommand"/> class.
    /// </summary>
    [TestClass]
    public class MergeProductCommandTests
    {
        /// <summary>
        /// Ensures that the a whitespace brand is not overwritten on an item with the brand defined on the product.
        /// </summary>
        /// <returns>A <see cref="Task"/> the represents the asynchronous operation.</returns>
        [TestMethod]
        public async Task MergeProductCommandDoesNotOverwriteAWhitespaceBrandWithAnotherValue()
        {
            // Arrange
            var item = new Item { Brand = " " };

            var product = new Product
            {
                Attributes = new List<ProductAttributeSet>
                {
                    new ProductAttributeSet
                    {
                        Name = "Brand", 
                        Attributes = new SortedDictionary<string, object>
                        {
                            { "Brand", "XBOX" }
                        }
                    }
                }
            };

            // Act
            item = await MergeProductCommand.Execute(item, product);

            // Assert
            Assert.AreEqual(" ", item.Brand);
        }

        /// <summary>
        /// Ensures that the a whitespace manufacturer is not overwritten on an item with the manufacturer defined on the product.
        /// </summary>
        /// <returns>A <see cref="Task"/> the represents the asynchronous operation.</returns>
        [TestMethod]
        public async Task MergeProductCommandDoesNotOverwriteAWhitespaceManufacturerWithAnotherValue()
        {
            // Arrange
            var item = new Item { Manufacturer = " " };

            var product = new Product { Manufacturer = "Microsoft" };

            // Act
            item = await MergeProductCommand.Execute(item, product);

            // Assert
            Assert.AreEqual(" ", item.Manufacturer);
        }

        /// <summary>
        /// Ensures that the a whitespace part number is not overwritten on an item with the part number defined on 
        /// the product.
        /// </summary>
        /// <returns>A <see cref="Task"/> the represents the asynchronous operation.</returns>
        [TestMethod]
        public async Task MergeProductCommandDoesNotOverwriteAWhitespacePartNumberWithAnotherValue()
        {
            // Arrange
            var item = new Item { ManufacturerPartNumber = " " };

            var product = new Product { PartNumber = "XBOXONE" };

            // Act
            item = await MergeProductCommand.Execute(item, product);

            // Assert
            Assert.AreEqual(" ", item.ManufacturerPartNumber);
        }

        /// <summary>
        /// Ensures that the a whitespace video URL is not overwritten on an item with the video URL defined on the 
        /// product.
        /// </summary>
        /// <returns>A <see cref="Task"/> the represents the asynchronous operation.</returns>
        [TestMethod]
        public async Task MergeProductCommandDoesNotOverwriteAWhitespaceVideoUrlWithAnotherValue()
        {
            // Arrange
            var item = new Item { VideoUrl = " " };

            var product = new Product
            {
                Attributes = new List<ProductAttributeSet>
                {
                    new ProductAttributeSet
                    {
                        Name = "Rakuten Common Attributes", 
                        Attributes = new SortedDictionary<string, object>
                        {
                            { "Video URL", "https://www.youtube.com/watch?v=iVcEkD3zFj8" }
                        }
                    }
                }
            };

            // Act
            item = await MergeProductCommand.Execute(item, product);

            // Assert
            Assert.AreEqual(" ", item.VideoUrl);
        }

        /// <summary>
        /// Ensures that the brand is not overwritten on an item with the brand defined on the product.
        /// </summary>
        /// <returns>A <see cref="Task"/> the represents the asynchronous operation.</returns>
        [TestMethod]
        public async Task MergeProductCommandDoesNotOverwriteTheBrandWithAnotherValue()
        {
            // Arrange
            var item = new Item { Brand = "PlayStation" };

            var product = new Product
            {
                Attributes = new List<ProductAttributeSet>
                {
                    new ProductAttributeSet
                    {
                        Name = "Brand", 
                        Attributes = new SortedDictionary<string, object>
                        {
                            { "Brand", "XBOX" }
                        }
                    }
                }
            };

            // Act
            item = await MergeProductCommand.Execute(item, product);

            // Assert
            Assert.AreEqual("PlayStation", item.Brand);
        }

        /// <summary>
        /// Ensures that the brand is not overwritten on an item with the brand defined on the product.
        /// </summary>
        /// <returns>A <see cref="Task"/> the represents the asynchronous operation.</returns>
        [TestMethod]
        public async Task MergeProductCommandDoesNotOverwriteTheBrandWithNull()
        {
            // Arrange
            var item = new Item { Brand = "PlayStation" };

            var product = new Product
            {
                Attributes = new List<ProductAttributeSet>
                {
                    new ProductAttributeSet
                    {
                        Name = "Brand", 
                        Attributes = new SortedDictionary<string, object>
                        {
                            { "Brand", null }
                        }
                    }
                }
            };

            // Act
            item = await MergeProductCommand.Execute(item, product);

            // Assert
            Assert.AreEqual("PlayStation", item.Brand);
        }

        /// <summary>
        /// Ensures that the images are not overwritten on an item with the images defined on the product when at least 
        /// one image has been supplied with the item.
        /// </summary>
        /// <returns>A <see cref="Task"/> the represents the asynchronous operation.</returns>
        [TestMethod]
        public async Task MergeProductCommandDoesNotOverwriteTheImagesWhenAtLeastOneIsAlreadyDefined()
        {
            // Arrange
            var item = new Item
            {
                ImageUrl1 = "https://psmedia.playstation.com/is/image/psmedia/ps4-system-imageblock-us-13jun14?$TwoColumn_Image$"
            };

            var product = new Product
            {
                Attributes = new List<ProductAttributeSet>
                {
                    new ProductAttributeSet
                    {
                        Name = "images", 
                        Attributes = new SortedDictionary<string, object>
                        {
                            { "image URL main", "http://dri1.img.digitalrivercontent.net/Storefront/Company/msintl/images/English/en-INTL_Xbox_One_Console_250GB_RKH-00107/en-INTL_L_XboxOne_Console_mnco_1.png" },
                            { "image location 2", "http://dri1.img.digitalrivercontent.net/Storefront/Company/msintl/images/English/en-INTL-Xbox-One-Console-5C5-00001/PDP/en-INTL-PDP-Xbox-One-Console-5C5-00001-Large.jpg" },
                            { "image location 3", "https://www.microsoft.com/global/en-us/news/publishingimages/ImageGallery/Images/Events/2013/XboxReveal/05-21Mattrick_Page.jpg" },
                            { "image location 4", "https://www.microsoft.com/hardware/_base_v1//products/xbox-one-controller-for-windows/gm_xbox1c_otherviews01.jpg" },
                            { "image location 5", "http://dri2.img.digitalrivercontent.net/Storefront/Company/msintl/images/English/en-INTL-Microsoft-XboxOne-Console-Solar-6RZ-00018/en-INTL-M-Microsoft-XboxOne-Console-Solar-6RZ-00018-mnco.jpg" }
                        }
                    }
                }
            };

            // Act
            item = await MergeProductCommand.Execute(item, product);

            // Assert
            Assert.AreEqual("https://psmedia.playstation.com/is/image/psmedia/ps4-system-imageblock-us-13jun14?$TwoColumn_Image$", item.ImageUrl1);
            Assert.AreEqual(null, item.ImageUrl2);
            Assert.AreEqual(null, item.ImageUrl3);
            Assert.AreEqual(null, item.ImageUrl4);
            Assert.AreEqual(null, item.ImageUrl5);
        }

        /// <summary>
        /// Ensures that the manufacturer part number is not overwritten on an item with the manufacturer defined on 
        /// the product.
        /// </summary>
        /// <returns>A <see cref="Task"/> the represents the asynchronous operation.</returns>
        [TestMethod]
        public async Task MergeProductCommandDoesNotOverwriteTheManufacturerPartNumberWithAnotherValue()
        {
            // Arrange
            var item = new Item { ManufacturerPartNumber = "XBOX360" };

            var product = new Product { PartNumber = "XBOXONE" };

            // Act
            item = await MergeProductCommand.Execute(item, product);

            // Assert
            Assert.AreEqual("XBOX360", item.ManufacturerPartNumber);
        }

        /// <summary>
        /// Ensures that the manufacturer is not overwritten on an item with the manufacturer part number defined on 
        /// the product.
        /// </summary>
        /// <returns>A <see cref="Task"/> the represents the asynchronous operation.</returns>
        [TestMethod]
        public async Task MergeProductCommandDoesNotOverwriteTheManufacturerPartNumberWithNull()
        {
            // Arrange
            var item = new Item { ManufacturerPartNumber = "XBOX360" };

            var product = new Product();

            // Act
            item = await MergeProductCommand.Execute(item, product);

            // Assert
            Assert.AreEqual("XBOX360", item.ManufacturerPartNumber);
        }

        /// <summary>
        /// Ensures that the manufacturer is not overwritten on an item with the manufacturer defined on the product.
        /// </summary>
        /// <returns>A <see cref="Task"/> the represents the asynchronous operation.</returns>
        [TestMethod]
        public async Task MergeProductCommandDoesNotOverwriteTheManufacturerWithAnotherValue()
        {
            // Arrange
            var item = new Item { Manufacturer = "Nokia" };

            var product = new Product { Manufacturer = "Microsoft" };

            // Act
            item = await MergeProductCommand.Execute(item, product);

            // Assert
            Assert.AreEqual("Nokia", item.Manufacturer);
        }

        /// <summary>
        /// Ensures that the manufacturer is not overwritten on an item with the manufacturer defined on the product.
        /// </summary>
        /// <returns>A <see cref="Task"/> the represents the asynchronous operation.</returns>
        [TestMethod]
        public async Task MergeProductCommandDoesNotOverwriteTheManufacturerWithNull()
        {
            // Arrange
            var item = new Item { Manufacturer = "Nokia" };

            var product = new Product();

            // Act
            item = await MergeProductCommand.Execute(item, product);

            // Assert
            Assert.AreEqual("Nokia", item.Manufacturer);
        }

        /// <summary>
        /// Ensures that the video url is not overwritten on an item with the video url defined on the product.
        /// </summary>
        /// <returns>A <see cref="Task"/> the represents the asynchronous operation.</returns>
        [TestMethod]
        public async Task MergeProductCommandDoesNotOverwriteTheVideoUrlWithAnotherValue()
        {
            // Arrange
            var item = new Item { VideoUrl = "https://www.youtube.com/watch?v=bIgrSv_BEy8" };

            var product = new Product
            {
                Attributes = new List<ProductAttributeSet>
                {
                    new ProductAttributeSet
                    {
                        Name = "Rakuten Common Attributes", 
                        Attributes = new SortedDictionary<string, object>
                        {
                            { "Video URL", "https://www.youtube.com/watch?v=iVcEkD3zFj8" }
                        }
                    }
                }
            };

            // Act
            item = await MergeProductCommand.Execute(item, product);

            // Assert
            Assert.AreEqual("https://www.youtube.com/watch?v=bIgrSv_BEy8", item.VideoUrl);
        }

        /// <summary>
        /// Ensures that the video url is not overwritten on an item with the video url defined on the product.
        /// </summary>
        /// <returns>A <see cref="Task"/> the represents the asynchronous operation.</returns>
        [TestMethod]
        public async Task MergeProductCommandDoesNotOverwriteTheVideoUrlWithNull()
        {
            // Arrange
            var item = new Item { VideoUrl = "https://www.youtube.com/watch?v=bIgrSv_BEy8" };

            var product = new Product
            {
                Attributes = new List<ProductAttributeSet>
                {
                    new ProductAttributeSet
                    {
                        Name = "Rakuten Common Attributes", 
                        Attributes = new SortedDictionary<string, object>
                        {
                            { "Video URL", null }
                        }
                    }
                }
            };

            // Act
            item = await MergeProductCommand.Execute(item, product);

            // Assert
            Assert.AreEqual("https://www.youtube.com/watch?v=bIgrSv_BEy8", item.VideoUrl);
        }

        /// <summary>
        /// Ensures that the brand is updated on an item when one was previously not supplied.
        /// </summary>
        /// <returns>A <see cref="Task"/> the represents the asynchronous operation.</returns>
        [TestMethod]
        public async Task MergeProductCommandPopulatesTheBrand()
        {
            // Arrange
            var item = new Item();

            var product = new Product
            {
                Attributes = new List<ProductAttributeSet>
                {
                    new ProductAttributeSet
                    {
                        Name = "Brand", 
                        Attributes = new SortedDictionary<string, object>
                        {
                            { "Brand", "XBOX" }
                        }
                    }
                }
            };

            // Act
            item = await MergeProductCommand.Execute(item, product);

            // Assert
            Assert.AreEqual("XBOX", item.Brand);
        }

        /// <summary>
        /// Ensures that the images are updated on an item when none were previously not supplied.
        /// </summary>
        /// <returns>A <see cref="Task"/> the represents the asynchronous operation.</returns>
        [TestMethod]
        public async Task MergeProductCommandPopulatesTheImages()
        {
            // Arrange
            var item = new Item();

            var product = new Product
            {
                Attributes = new List<ProductAttributeSet>
                {
                    new ProductAttributeSet
                    {
                        Name = "images", 
                        Attributes = new SortedDictionary<string, object>
                        {
                            { "image URL main", "http://dri1.img.digitalrivercontent.net/Storefront/Company/msintl/images/English/en-INTL_Xbox_One_Console_250GB_RKH-00107/en-INTL_L_XboxOne_Console_mnco_1.png" },
                            { "image location 2", "http://dri1.img.digitalrivercontent.net/Storefront/Company/msintl/images/English/en-INTL-Xbox-One-Console-5C5-00001/PDP/en-INTL-PDP-Xbox-One-Console-5C5-00001-Large.jpg" },
                            { "image location 3", "https://www.microsoft.com/global/en-us/news/publishingimages/ImageGallery/Images/Events/2013/XboxReveal/05-21Mattrick_Page.jpg" },
                            { "image location 4", "https://www.microsoft.com/hardware/_base_v1//products/xbox-one-controller-for-windows/gm_xbox1c_otherviews01.jpg" },
                            { "image location 5", "http://dri2.img.digitalrivercontent.net/Storefront/Company/msintl/images/English/en-INTL-Microsoft-XboxOne-Console-Solar-6RZ-00018/en-INTL-M-Microsoft-XboxOne-Console-Solar-6RZ-00018-mnco.jpg" }
                        }
                    }
                }
            };

            // Act
            item = await MergeProductCommand.Execute(item, product);

            // Assert
            Assert.AreEqual("http://dri1.img.digitalrivercontent.net/Storefront/Company/msintl/images/English/en-INTL_Xbox_One_Console_250GB_RKH-00107/en-INTL_L_XboxOne_Console_mnco_1.png", item.ImageUrl1);
            Assert.AreEqual("http://dri1.img.digitalrivercontent.net/Storefront/Company/msintl/images/English/en-INTL-Xbox-One-Console-5C5-00001/PDP/en-INTL-PDP-Xbox-One-Console-5C5-00001-Large.jpg", item.ImageUrl2);
            Assert.AreEqual("https://www.microsoft.com/global/en-us/news/publishingimages/ImageGallery/Images/Events/2013/XboxReveal/05-21Mattrick_Page.jpg", item.ImageUrl3);
            Assert.AreEqual("https://www.microsoft.com/hardware/_base_v1//products/xbox-one-controller-for-windows/gm_xbox1c_otherviews01.jpg", item.ImageUrl4);
            Assert.AreEqual("http://dri2.img.digitalrivercontent.net/Storefront/Company/msintl/images/English/en-INTL-Microsoft-XboxOne-Console-Solar-6RZ-00018/en-INTL-M-Microsoft-XboxOne-Console-Solar-6RZ-00018-mnco.jpg", item.ImageUrl5);
        }

        /// <summary>
        /// Ensures that the manufacturer is updated on an item when one was previously not supplied.
        /// </summary>
        /// <returns>A <see cref="Task"/> the represents the asynchronous operation.</returns>
        [TestMethod]
        public async Task MergeProductCommandPopulatesTheManufacturer()
        {
            // Arrange
            var item = new Item();

            var product = new Product { Manufacturer = "Microsoft" };

            // Act
            item = await MergeProductCommand.Execute(item, product);

            // Assert
            Assert.AreEqual("Microsoft", item.Manufacturer);
        }

        /// <summary>
        /// Ensures that the manufacturer part number is updated on an item when one was previously not supplied.
        /// </summary>
        /// <returns>A <see cref="Task"/> the represents the asynchronous operation.</returns>
        [TestMethod]
        public async Task MergeProductCommandPopulatesTheManufacturerPartNumber()
        {
            // Arrange
            var item = new Item();

            var product = new Product { PartNumber = "XBOXONE" };

            // Act
            item = await MergeProductCommand.Execute(item, product);

            // Assert
            Assert.AreEqual("XBOXONE", item.ManufacturerPartNumber);
        }

        /// <summary>
        /// Ensures that the video url is updated on an item when one was previously not supplied.
        /// </summary>
        /// <returns>A <see cref="Task"/> the represents the asynchronous operation.</returns>
        [TestMethod]
        public async Task MergeProductCommandPopulatesTheVideoUrl()
        {
            // Arrange
            var item = new Item();

            var product = new Product 
            { 
                Attributes = new List<ProductAttributeSet>
                {
                    new ProductAttributeSet
                    {
                        Name = "Rakuten Common Attributes", 
                        Attributes = new SortedDictionary<string, object>
                        {
                            { "Video URL", "https://www.youtube.com/watch?v=iVcEkD3zFj8" }
                        }
                    }
                } 
            };

            // Act
            item = await MergeProductCommand.Execute(item, product);

            // Assert
            Assert.AreEqual("https://www.youtube.com/watch?v=iVcEkD3zFj8", item.VideoUrl);
        }
    }
}
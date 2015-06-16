// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="ParseItemsCommandTests.cs" company="Rakuten">
//   Copyright (c) Rakuten. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace Rakuten.Rmsg.ProductQuery.WebJob.Tests.Unit
{
    using System.Collections.ObjectModel;
    using System.Diagnostics.Contracts;
    using System.IO;
    using System.Threading.Tasks;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Rakuten.IO.Delimited.Serialization.Fakes;

    /// <summary>
    /// A suite of tests for the <see cref="ParseItemsCommand"/> class.
    /// </summary>
    [TestClass]
    public class ParseItemsCommandTests
    {
        /// <summary>
        /// Ensures that headers are written to a file.
        /// </summary>
        /// <returns>A <see cref="Task"/> the represents the asynchronous operation.</returns>
        [TestMethod]
        public async Task ParseItemsCommandWritesHeadersToAStream()
        {
            // Arrange
            var items = new Collection<Item>();

            var stream = new MemoryStream();

            var writer = new StreamWriter(stream);

            var stubLumenWorksSerializer = new StubLumenWorksSerializer<Item>();

            // Act
            await ParseItemsCommand.Execute(items, writer, stubLumenWorksSerializer);

            // Assert
            writer.Flush();
            stream.Position = 0;
            var content = new StreamReader(stream).ReadLine();

            Assert.IsFalse(string.IsNullOrWhiteSpace(content));
        }

        /// <summary>
        /// Ensures that all headers are written to a file.
        /// </summary>
        /// <returns>A <see cref="Task"/> the represents the asynchronous operation.</returns>
        [TestMethod]
        public async Task ParseItemsCommandWritesAllHeadersToAStream()
        {
            // Arrange
            var items = new Collection<Item>();

            var stream = new MemoryStream();

            var writer = new StreamWriter(stream);

            var stubLumenWorksSerializer = new StubLumenWorksSerializer<Item>();

            // Act
            await ParseItemsCommand.Execute(items, writer, stubLumenWorksSerializer);

            // Assert
            writer.Flush();
            stream.Seek(0, SeekOrigin.Begin);
            var content = new StreamReader(stream).ReadLine();

            Assert.AreEqual("sku,base_sku,manufacturer,manufacturer_part_number,url,name,tagline,description_1,legal_information,shipping_instructions,labels,price,image_url_1,image_url_2,image_url_3,image_url_4,image_url_5,image_url_6,image_url_7,image_url_8,image_url_9,image_url_10,video_url,rakuten_product_category_id,gtin_type,gtin_value,attribute_1,attribute_2,attribute_3,attribute_4,attribute_5,brand,display_start_date,display_end_date,available_start_date,available_end_date,shipping_preparation_time,free_shipping,shipping_width,shipping_height,shipping_length,weight,display_quantity,operator_for_quantity,quantity,return_quantity_in_cancel,purchase_quantity_limit,shop_product_unique_identifier_1,shop_product_unique_identifier_2,shop_product_unique_identifier_3,shop_product_unique_identifier_4,shop_product_unique_identifier_5,shop_product_unique_identifier_6,shop_product_unique_identifier_7,shop_product_unique_identifier_8,shop_product_unique_identifier_9,shop_product_unique_identifier_10,shop_product_unique_identifier_11,shop_product_unique_identifier_12,shop_product_unique_identifier_13,shop_product_unique_identifier_14,shop_product_unique_identifier_15,shipping_option_1,shipping_option_2,shipping_option_3,shipping_option_4,shipping_option_5,shipping_option_6,shipping_option_7,shipping_option_8,shipping_option_9,shipping_option_10,shipping_option_11,shipping_option_12,shipping_option_13,shipping_option_14,shipping_option_15,shipping_option_16,shipping_option_17,shipping_option_18,shipping_option_19,shipping_option_20", content);
        }

        /// <summary>
        /// Ensures that when a property has not been specified it is still included in the output.
        /// </summary>
        /// <returns>A <see cref="Task"/> the represents the asynchronous operation.</returns>
        [TestMethod]
        public async Task ParseItemsCommandWritesEmptyProperties()
        {
            // Arrange
            var items = new Collection<Item> { new Item { BaseSku = "SKU123456789!" } };

            var stream = new MemoryStream();

            var writer = new StreamWriter(stream);

            var stubLumenWorksSerializer = new StubLumenWorksSerializer<Item>();

            // Act
            await ParseItemsCommand.Execute(items, writer, stubLumenWorksSerializer);

            // Assert
            writer.Flush();
            stream.Seek(0, SeekOrigin.Begin);

            var reader = new StreamReader(stream);
            reader.ReadLine();
            var content = reader.ReadLine();
            Contract.Assume(content != null);

            var array = content.Split(',');

            Assert.IsFalse(string.IsNullOrWhiteSpace(array[1]));
        }

        /// <summary>
        /// Ensures that an item is written to a file.
        /// </summary>
        /// <returns>A <see cref="Task"/> the represents the asynchronous operation.</returns>
        [TestMethod]
        public async Task ParseItemsCommandWritesItemsToAStream()
        {
            // Arrange
            var items = new Collection<Item> { new Item { BaseSku = "SKU123456789!" } };

            var stream = new MemoryStream();

            var writer = new StreamWriter(stream);

            var stubLumenWorksSerializer = new StubLumenWorksSerializer<Item>();

            // Act
            await ParseItemsCommand.Execute(items, writer, stubLumenWorksSerializer);

            // Assert
            writer.Flush();
            stream.Seek(0, SeekOrigin.Begin);

            var reader = new StreamReader(stream);
            reader.ReadLine();
            var content = reader.ReadLine();

            Assert.IsFalse(string.IsNullOrWhiteSpace(content));
        }

        /// <summary>
        /// Ensures that an item is written to a file.
        /// </summary>
        /// <returns>A <see cref="Task"/> the represents the asynchronous operation.</returns>
        [TestMethod]
        public async Task ParseItemsCommandWritesAllItemDataToAStream()
        {
            // Arrange
            var items = new Collection<Item> { new Item { BaseSku = "SKU123456789!", Sku = "123456789!" } };

            var stream = new MemoryStream();

            var writer = new StreamWriter(stream);

            var stubLumenWorksSerializer = new StubLumenWorksSerializer<Item>();

            // Act
            await ParseItemsCommand.Execute(items, writer, stubLumenWorksSerializer);

            // Assert
            writer.Flush();
            stream.Seek(0, SeekOrigin.Begin);

            var reader = new StreamReader(stream);
            reader.ReadLine();
            var content = reader.ReadLine();

            Assert.AreEqual("123456789!,SKU123456789!,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,", content);
        }
    }
}
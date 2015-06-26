//------------------------------------------------------------------------------
// <copyright file="ItemFactory.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.Web.Http.Tests.Integration
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using LumenWorks.Framework.IO.Csv;
    using Rakuten.IO.Delimited.Serialization;
    using Rakuten.Rmsg.ProductQuery.Web.Http.Tests.Integration.Resources;

    /// <summary>
    /// Factory methods for the <see cref="Item"/> class.
    /// </summary>
    public static class ItemFactory
    {
        /// <summary>
        /// Creates a minimum viable item for a product query.
        /// </summary>
        /// <param name="gtinType">The GTIN type.</param>
        /// <param name="gtinValue">The GTIN value</param>
        /// <param name="imageUrl1">Image URL 1.</param>
        /// <param name="imageUrl2">Image URL 2.</param>
        /// <param name="imageUrl3">Image URL 3.</param>
        /// <param name="imageUrl4">Image URL 4.</param>
        /// <param name="imageUrl5">Image URL 5.</param>
        /// <param name="imageUrl6">Image URL 6.</param>
        /// <param name="imageUrl7">Image URL 7.</param>
        /// <param name="imageUrl8">Image URL 8.</param>
        /// <param name="imageUrl9">Image URL 9.</param>
        /// <param name="imageUrl10">Image URL 10</param>
        /// <returns>A new instance of <see cref="Item"/>.</returns>
        public static Item Create(
            string gtinType,
            string gtinValue,
            string imageUrl1 = null,
            string imageUrl2 = null,
            string imageUrl3 = null,
            string imageUrl4 = null,
            string imageUrl5 = null,
            string imageUrl6 = null,
            string imageUrl7 = null,
            string imageUrl8 = null,
            string imageUrl9 = null,
            string imageUrl10 = null)
        {
            var item = new Item
            {
                GtinType = gtinType,
                GtinValue = gtinValue,
                ImageUrl1 = imageUrl1,
                ImageUrl2 = imageUrl2,
                ImageUrl3 = imageUrl3,
                ImageUrl4 = imageUrl4,
                ImageUrl5 = imageUrl5,
                ImageUrl6 = imageUrl6,
                ImageUrl7 = imageUrl7,
                ImageUrl8 = imageUrl8,
                ImageUrl9 = imageUrl9,
                ImageUrl10 = imageUrl10
            };

            return item;
        }

        /// <summary>
        /// Gets all items from the specified file.
        /// </summary>
        /// <param name="filename">The name of the file.</param>
        /// <returns>All items from the specified file.</returns>
        public static List<Item> Get(string filename)
        {
            using (var streamReader = new StreamReader(filename, true))
            using (var delimitedReader = new CsvReader(streamReader, true, ','))
            {
                var serializer = new LumenWorksSerializer<Item>();
                return serializer.ReadFileByHeaders(delimitedReader).ToList();
            }
        }
    }
}

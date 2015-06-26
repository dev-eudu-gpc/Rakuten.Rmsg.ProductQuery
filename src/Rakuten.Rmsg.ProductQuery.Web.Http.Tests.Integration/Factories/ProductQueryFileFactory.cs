//------------------------------------------------------------------------------
// <copyright file="ProductQueryFileFactory.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.Web.Http.Tests.Integration
{
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using Rakuten.IO.Delimited.Serialization;
    using Rakuten.Rmsg.ProductQuery.Web.Http.Tests.Integration.Resources;

    /// <summary>
    /// Factory methods for product query files.
    /// </summary>
    public static class ProductQueryFileFactory
    {
        /// <summary>
        /// Creates a file containing the specified items.
        /// </summary>
        /// <param name="items">The items to write to the file.</param>
        /// <param name="includeHeaderRow">A value indicating whether to include the header row in the file.</param>
        /// <returns>The name of the file.</returns>
        public static string Create(
            IEnumerable<Item> items,
            bool includeHeaderRow = true)
        {
            var serializer = new LumenWorksSerializer<Item>();
            var fileName = Path.Combine(System.IO.Path.GetTempPath(), "rmsg-product-query-source.tmp");

            using (var writer = new StreamWriter(fileName, false, Encoding.UTF8))
            {
                if (includeHeaderRow)
                {
                    serializer.WriteHeaders(writer);
                }

                foreach (var item in items)
                {
                    serializer.WriteLine(item, writer);
                }

                writer.Flush();
            }

            return fileName;
        }

        /// <summary>
        /// Adds a row that has insufficient columns to the specified file.
        /// </summary>
        /// <param name="filename">The name of the file to add the row to.</param>
        public static void AddRowWithInsufficientColumns(string filename)
        {
            using (var writer = new StreamWriter(filename, true, Encoding.UTF8))
            {
                writer.WriteLine(new string(',', 1));
            }
        }
    }
}

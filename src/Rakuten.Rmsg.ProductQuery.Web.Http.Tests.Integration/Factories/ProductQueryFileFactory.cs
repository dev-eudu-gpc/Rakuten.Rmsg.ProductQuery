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

    /// <summary>
    /// Factory methods for product query files.
    /// </summary>
    public static class ProductQueryFileFactory
    {
        /// <summary>
        /// Creates a file containing the specified items.
        /// </summary>
        /// <param name="items">The items to write to the file.</param>
        /// <returns>The name of the file.</returns>
        public static string Create(IEnumerable<Item> items)
        {
            var serializer = new LumenWorksSerializer<Item>();
            var fileName = System.IO.Path.GetTempFileName();

            using (var writer = new StreamWriter(fileName, false, Encoding.UTF8))
            {
                serializer.WriteHeaders(writer);

                foreach (var item in items)
                {
                    serializer.WriteLine(item, writer);
                }

                writer.Flush();
            }

            return fileName;
        }
    }
}

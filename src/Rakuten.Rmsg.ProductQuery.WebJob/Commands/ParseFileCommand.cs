// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="ParseFileCommand.cs" company="Rakuten">
//   Copyright (c) Rakuten. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.WebJob
{
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.IO;
    using System.Threading.Tasks;

    using LumenWorks.Framework.IO.Csv;

    using Rakuten.IO.Delimited.Serialization;

    /// <summary>
    /// Parses the data from a given stream into a collection of instances.
    /// </summary>
    internal class ParseFileCommand
    {
        /// <summary>
        /// Parses the specified stream into a collection of <see cref="Item"/> instances using the given serializer.
        /// </summary>
        /// <param name="serializer">The serializer to be used.</param>
        /// <param name="stream">The stream containing the data.</param>
        /// <returns>A collection of <see cref="Item"/> instances.</returns>
        public static Task<IEnumerable<Item>> Execute(LumenWorksSerializer<Item> serializer, Stream stream)
        {
            Contract.Requires(serializer != null);
            Contract.Requires(stream != null);

            return Task.Run(() =>
            {
                using (var streamReader = new StreamReader(stream))
                using (var reader = new CsvReader(streamReader, true, ','))
                {
                    return serializer.ReadFileByIndex(reader);
                }
            });
        }
    }
}
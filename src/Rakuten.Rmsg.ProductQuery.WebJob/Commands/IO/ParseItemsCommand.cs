// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="ParseItemsCommand.cs" company="Rakuten">
//   Copyright (c) Rakuten. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.WebJob
{
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;

    using Rakuten.IO.Delimited.Serialization;

    /// <summary>
    /// Parses a collection of <see cref="Item"/> instances into a stream.
    /// </summary>
    internal class ParseItemsCommand
    {
        /// <summary>
        /// Writes the collection of <see cref="Item"/>s onto the given <see cref="Stream"/>.
        /// </summary>
        /// <param name="items">The collection of items.</param>
        /// <param name="stream">The stream onto which the collection of items should be written.</param>
        /// <param name="serializer">The serializer to be used.</param>
        /// <returns>A <see cref="Task"/> the represents the asynchronous operation.</returns>
        public static async Task<Stream> Execute(
            IEnumerable<Item> items, 
            Stream stream, 
            LumenWorksSerializer<Item> serializer)
        {
            using (var writer = new StreamWriter(stream))
            {
                await serializer.WriteHeadersAsync(writer);

                foreach (var item in items)
                {
                    await serializer.WriteLineAsync(item, writer);
                }
            }

            return stream;
        }
    }
}
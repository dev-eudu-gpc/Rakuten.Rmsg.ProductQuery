// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="ParseFileTransformFactory.cs" company="Rakuten">
//   Copyright (c) Rakuten. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.WebJob
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    using Rakuten.Threading.Tasks.Dataflow;

    /// <summary>
    /// Represents a factory that create a delegate to parse a given stream into a collection of <see cref="Item"/> 
    /// instances.
    /// </summary>
    internal class ParseFileTransformFactory
    {
        /// <summary>
        /// Returns a delegate that when executed will take the stream and attempt to convert it into a collection of 
        /// <see cref="MessageState"/> instances.
        /// </summary>
        /// <param name="command">The delegate that should be executed.</param>
        /// <param name="id">The unique identifier for the current query.</param>
        /// <param name="culture">The culture in which the product data should be expressed.</param>
        /// <returns>A <see cref="Func{T1,T2,TResult}"/>.</returns>
        public static Func<Stream, TextWriter, Task<IEnumerable<ItemMessageState>>> Create(
            Func<Stream, Task<IEnumerable<Item>>> command,
            Guid id,
            CultureInfo culture)
        {
            Contract.Requires(id != Guid.Empty);

            return async (stream, writer) =>
            {
                try
                {
                    IEnumerable<Item> items = await command(stream);

                    return from item in items select new ItemMessageState(id, culture, item);
                }
                catch (Exception ex)
                {
                    writer.WriteLine("An issue was encountered parsing the uploaded file: " + ex.ToString());

                    return null;
                }
            };
        }
    }
}
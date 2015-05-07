// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="GetQueryItemTransformFactory.cs" company="Rakuten">
//   Copyright (c) Rakuten. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace Rakuten.Rmsg.ProductQuery.WebJob
{
    using System;
    using System.IO;
    using System.Threading.Tasks;

    using Rakuten.Rmsg.ProductQuery.WebJob.Entities;

    /// <summary>
    /// Represents the factory that will create a delegate to retrieve the record of a product query from persistent 
    /// storage.
    /// </summary>
    internal class GetQueryItemTransformFactory
    {
        /// <summary>
        /// Returns a delegate that when executed will attempt to retrieve the corresponding record from persistent 
        /// storage of a <see cref="Item"/>.
        /// </summary>
        /// <param name="command">The delegate that should be executed.</param>
        /// <returns>A <see cref="Func{T1,T2,TResult}"/>.</returns>
        public static Func<ItemMessageState, TextWriter, Task<ItemMessageState>> Create(
            Func<Guid, string, Task<ProductQueryItem>> command)
        {
            return async (state, writer) =>
            {
                try
                {
                    var queryItem = await command(state.Id, state.Item.GtinValue);

                    return new ItemMessageState(state.Id, state.Culture, state.Item, queryItem);
                }
                catch (Exception ex)
                {
                    writer.WriteLine("An issue was encountered parsing the uploaded file: " + ex.ToString());

                    return state.AddException(ex);
                }
            };
        }
    }
}
// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="MergeProductTransformFactory.cs" company="Rakuten">
//   Copyright (c) Rakuten. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.WebJob
{
    using System;
    using System.IO;
    using System.Threading.Tasks;

    using Rakuten.Rmsg.ProductQuery.WebJob.Api;

    /// <summary>
    /// Represents the factory for merging a products details into a <see cref="Item"/>.
    /// </summary>
    internal class MergeProductTransformFactory
    {
        /// <summary>
        /// Returns a delegate that when executed merges the product data with the original source item data.
        /// </summary>
        /// <param name="command">The delegate that should be executed.</param>
        /// <returns>A <see cref="Func{T1,T2,TResult}"/>.</returns>
        public static Func<ItemMessageState, TextWriter, Task<ItemMessageState>> Create(
            Func<Item, Product, Task<Item>> command)
        {
            return async (state, writer) =>
            {
                try
                {
                    var item = await command(state.Item, state.Product);

                    return new ItemMessageState(state.Id, state.Culture, item);
                }
                catch (Exception ex)
                {
                    writer.WriteLine("An issue was encountered merging the product details: " + ex.ToString());

                    return state.AddException(ex);
                }
            };
        }
    }
}
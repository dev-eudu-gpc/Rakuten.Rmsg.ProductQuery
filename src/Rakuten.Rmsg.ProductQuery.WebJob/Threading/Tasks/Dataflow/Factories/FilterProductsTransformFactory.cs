// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="FilterProductsTransformFactory.cs" company="Rakuten">
//   Copyright (c) Rakuten. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.WebJob
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;

    using Rakuten.Rmsg.ProductQuery.WebJob.Api;

    /// <summary>
    /// The filter products transform factory.
    /// </summary>
    internal class FilterProductsTransformFactory
    {
        /// <summary>
        /// Returns a delegate that when executed will filter a collection of products down to a single product using
        /// the specified collection of data sources.
        /// </summary>
        /// <param name="command">The delegate that should be executed.</param>
        /// <returns>A <see cref="Func{T1,T2,TResult}"/>.</returns>
        public static Func<ItemMessageState, TextWriter, Task<ItemMessageState>> Create(
            Func<IEnumerable<Product>, Task<Product>> command)
        {
            return async (state, writer) =>
            {
                try
                {
                    var product = await command(state.Products);
                    if (product != null)
                    {
                        state.Query.Gran = product.Id;
                    }

                    return new ItemMessageState(state.Id, state.Culture, state.Item, state.Query);
                }
                catch (Exception ex)
                {
                    writer.WriteLine("An issue was encountered filtering the collection of products: " + ex.ToString());

                    return state.AddException(ex);
                }
            };
        }
    }
}
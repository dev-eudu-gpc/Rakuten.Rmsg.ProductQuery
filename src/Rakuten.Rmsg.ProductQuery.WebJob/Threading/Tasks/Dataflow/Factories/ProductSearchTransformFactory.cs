// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="ProductSearchTransformFactory.cs" company="Rakuten">
//   Copyright (c) Rakuten. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.WebJob
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Threading.Tasks;

    using Rakuten.Rmsg.ProductQuery.WebJob.Api;

    /// <summary>
    /// Represents the factory that will create a delegate to search for a product.
    /// </summary>
    internal class ProductSearchTransformFactory
    {
        /// <summary>
        /// Returns a delegate that when executed will search for products with a specific GTIN and are expressed in a
        /// specific culture.
        /// </summary>
        /// <param name="command">The delegate that should be executed.</param>
        /// <returns>A <see cref="Func{T1,T2,TResult}"/>.</returns>
        public static Func<ItemMessageState, TextWriter, Task<ItemMessageState>> Create(
            Func<string, string, CultureInfo, Task<IEnumerable<Product>>> command)
        {
            return async (state, writer) =>
            {
                try
                {
                    var products = await command(state.Item.GtinType, state.Item.GtinValue, state.Culture);

                    return new ItemMessageState(state.Id, state.Culture, state.Item, state.Query, products);
                }
                catch (Exception ex)
                {
                    writer.WriteLine("An issue was encountered retrieving a collection of products: " + ex.ToString());

                    return state.AddException(ex);
                }
            };
        }
    }
}
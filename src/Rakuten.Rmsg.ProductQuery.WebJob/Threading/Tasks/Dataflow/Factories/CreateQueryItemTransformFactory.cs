// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="CreateQueryItemTransformFactory.cs" company="Rakuten">
//   Copyright (c) Rakuten. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.WebJob
{
    using System;
    using System.IO;
    using System.Threading.Tasks;

    /// <summary>
    /// Represents a factory that will create a delegate to create a <see cref="ProductQueryItem"/>.
    /// </summary>
    internal class CreateQueryItemTransformFactory
    {
        /// <summary>
        /// Returns a delegate that when executed will attempt to write a record to persistent storage for this query.
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
                    var query = await command(state.Id, state.Item.GtinValue);

                    return new ItemMessageState(state.Id, state.Culture, state.Item, query);
                }
                catch (Exception ex)
                {
                    writer.WriteLine("An issue was encountered creating a query record: " + ex.ToString());

                    return state.AddException(ex);
                }
            };
        }
    }
}
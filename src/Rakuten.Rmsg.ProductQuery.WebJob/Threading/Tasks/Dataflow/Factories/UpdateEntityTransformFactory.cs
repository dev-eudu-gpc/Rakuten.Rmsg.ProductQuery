// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="UpdateEntityTransformFactory.cs" company="Rakuten">
//   Copyright (c) Rakuten. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.WebJob
{
    using System;
    using System.IO;
    using System.Threading.Tasks;

    /// <summary>
    /// Represents the factory that creates a delegate to update the record of a product query within the persistent 
    /// data store.
    /// </summary>
    internal class UpdateEntityTransformFactory
    {
        /// <summary>
        /// Returns a delegate that when executed will update the record of a product query in the persistent storage
        /// with the identified GRAN.
        /// </summary>
        /// <param name="command">The delegate that should be executed.</param>
        /// <returns>A <see cref="Func{T1,T2,TResult}"/>.</returns>
        public static Func<ItemMessageState, TextWriter, Task<ItemMessageState>> Create(
            Func<ProductQueryItem, Task> command)
        {
            return async (state, writer) =>
            {
                try
                {
                    await command(state.Query);

                    return state;
                }
                catch (Exception ex)
                {
                    writer.WriteLine("An issue was encountered updating the query item record: " + ex.ToString());

                    return state.AddException(ex);
                }
            };
        }
    }
}
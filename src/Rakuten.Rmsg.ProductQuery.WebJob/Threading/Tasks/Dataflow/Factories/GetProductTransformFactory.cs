// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="GetProductTransformFactory.cs" company="Rakuten">
//   Copyright (c) Rakuten. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.WebJob
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Threading.Tasks;

    using Rakuten.Rmsg.ProductQuery.WebJob.Api;

    /// <summary>
    /// Represents the factory for creating a delegate that will retrieve a products details.
    /// </summary>
    internal class GetProductTransformFactory
    {
        /// <summary>
        /// Returns a delegate that when executed retrieves the details of the specific product in a given culture.
        /// </summary>
        /// <param name="command">The delegate that should be executed.</param>
        /// <returns>A <see cref="Func{T1,T2,TResult}"/>.</returns>
        public static Func<ItemMessageState, TextWriter, Task<ItemMessageState>> Create(
            Func<string, CultureInfo, Task<Product>> command)
        {
            return async (state, writer) =>
            {
                try
                {
                    var product = await command(state.Query.Gran, state.Culture);

                    return new ItemMessageState(state.Id, state.Culture, state.Item, product);
                }
                catch (Exception ex)
                {
                    writer.WriteLine("An issue was encountered fetching the identified product: " + ex.ToString());

                    return state.AddException(ex);
                }
            };
        }
    }
}
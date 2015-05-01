//------------------------------------------------------------------------------
// <copyright file="IIdentifierLink.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.Linking
{
    /// <summary>
    /// Defines the members for a link which contains an identifier
    /// </summary>
    public interface IIdentifierLink<T>
    {
        /// <summary>
        /// Gets a link with the specified unique identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>A link that includes the specified unique identifier.</returns>
        T ForId(string id);
    }
}

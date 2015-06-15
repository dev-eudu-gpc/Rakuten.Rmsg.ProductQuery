//------------------------------------------------------------------------------
// <copyright file="IdentifierType.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.Web.Http.Tests.Integration
{
    /// <summary>
    /// Represents the list of identifier types
    /// </summary>
    public enum IdentifierType
    {
        /// <summary>
        /// Represents an EAN.
        /// </summary>
        Ean,

        /// <summary>
        /// Represents an ISBN.
        /// </summary>
        Isbn,

        /// <summary>
        /// Represents a manufacturer part number
        /// </summary>
        Mpn,

        /// <summary>
        /// Represents a JAN.
        /// </summary>
        Jan,

        /// <summary>
        /// Represents a UPC.
        /// </summary>
        Upc
    }
}
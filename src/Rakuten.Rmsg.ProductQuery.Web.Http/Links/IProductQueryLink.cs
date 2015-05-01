//------------------------------------------------------------------------------
// <copyright file="IProductQueryLink.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.Web.Http.Links
{
    using Rakuten.Rmsg.ProductQuery.Linking;

    /// <summary>
    /// Represents the members for a product query link
    /// </summary>
    internal interface IProductQueryLink : IIdentifierLink<IProductQueryLink>, ICultureLink<IProductQueryLink>
    {
    }
}
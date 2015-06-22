//---------------------------------------------------------------------------------------------------------------------
// <copyright file="AttributeSetName.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//---------------------------------------------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.Web.Http.Tests.Integration
{
    /// <summary>
    /// A set of known attribute set names.
    /// </summary>
    public enum AttributeSetName
    {
        /// <summary>
        /// The alternative identifier attribute set.
        /// </summary>
        [TextualRepresentation("AlternativeID")]
        AlternativeId,

        /// <summary>
        /// The alternative identifiers attribute set.
        /// </summary>
        [TextualRepresentation("Alternative Identifiers")]
        AlternativeIdentifiers,

        /// <summary>
        /// The brand attribute set.
        /// </summary>
        [TextualRepresentation("Brand")]
        Brand,

        /// <summary>
        /// The common attributes attribute set.
        /// </summary>
        [TextualRepresentation("Rakuten Common Attributes")]
        Common,

        /// <summary>
        /// The GTIN attribute set.
        /// </summary>
        [TextualRepresentation("GTIN")]
        Gtin,

        /// <summary>
        /// The images attribute set.
        /// </summary>
        [TextualRepresentation("Images")]
        Images,

        /// <summary>
        /// The offers attribute set.
        /// </summary>
        [TextualRepresentation("Offer Information")]
        OfferInformation,

        /// <summary>
        /// The product discovery attribute set.
        /// </summary>
        [TextualRepresentation("Product Discovery Information")]
        ProductDiscoveryInformation
    }
}
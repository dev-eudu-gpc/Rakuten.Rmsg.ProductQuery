//----------------------------------------------------------------------------------------------------------------------
// <copyright file="LinkRelationTypes.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------------------------------------------------
namespace Rakuten
{
    /// <summary>
    /// Contains link relation types.
    /// </summary>
    public static class LinkRelationTypes
    {
        /// <summary>
        /// Refers to a substitute for this context.
        /// </summary>
        public const string Alternate = "alternate";

        /// <summary>
        /// Designates the preferred version of a resource (the IRI and its contents).
        /// </summary>
        public const string Canonical = "canonical";

        /// <summary>
        /// The target IRI points to a resource which represents the collection resource for the context IRI.
        /// </summary>
        public const string Collection = "collection";

        /// <summary>
        /// Identifies a related resource that is potentially large and might require special handling.
        /// </summary>
        public const string Enclosure = "enclosure";

        /// <summary>
        /// An IRI that refers to the furthest preceding resource in a series of resources.
        /// </summary>
        public const string First = "first";

        /// <summary>
        /// Refers to an icon representing the link's context.
        /// </summary>
        public const string Icon = "icon";

        /// <summary>
        /// The target IRI points to a resource that is a member of the collection represented by the context IRI.
        /// </summary>
        public const string Item = "item";

        /// <summary>
        /// An IRI that refers to the furthest following resource in a series of resources.
        /// </summary>
        public const string Last = "last";

        /// <summary>
        /// Indicates that the link's context is a part of a series, and that the next in the series is the link target.
        /// </summary>
        public const string Next = "next";

        /// <summary>
        /// Identifies a resource at which payment is accepted.
        /// </summary>
        public const string Payment = "payment";

        /// <summary>
        /// Indicates that the link's context is a part of a series,
        /// and that the previous in the series is the link target.
        /// </summary>
        public const string Previous = "prev";

        /// <summary>
        /// Identifying that a resource representation conforms to a certain profile,
        /// without affecting the non-profile semantics of the resource representation.
        /// </summary>
        public const string Profile = "profile";

        /// <summary>
        /// Identifies a related resource.
        /// </summary>
        public const string Related = "related";

        /// <summary>
        /// Refers to a resource that can be used to search through the link's context and related resources.
        /// </summary>
        public const string Search = "search";

        /// <summary>
        /// Conveys an identifier for the link's context.
        /// </summary>
        public const string Self = "self";

        /// <summary>
        /// Refers to a parent document in a hierarchy of documents.
        /// </summary>
        public const string Up = "up";
    }
}
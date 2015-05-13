// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="LinkBuilder.cs" company="Rakuten">
//   Copyright (c) Rakuten. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------
namespace Rakuten
{
    using System.Diagnostics.Contracts;

    /// <summary>Provides functionality for constructing a link.</summary>
    public class LinkBuilder : LinkBuilder<LinkTemplate>
    {
        /// <summary>Initializes a new instance of the <see cref="LinkBuilder"/> class.</summary>
        /// <param name="target">The target of the link.</param>
        public LinkBuilder(string target) : base((r, t, a) => new LinkTemplate(r, t, a), new UriTemplate(target))
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(target));
        }

        /// <summary>Provides functionality for building a link with the specified relation type.</summary>
        /// <param name="relationType">The link relation type.</param>
        /// <returns>An instance that can be used to build a link of the specified relation type.</returns>
        public override ITypedLinkBuilder<LinkTemplate> WithType(string relationType)
        {
            return new LinkBuilder<LinkTemplate>(this.TargetOfLink).WithType(relationType);
        }
    }
}
// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="LinkReader.cs" company="Rakuten">
//   Copyright (c) Rakuten. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------
namespace Rakuten
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>Reads a collection of links.</summary>
    public class LinkReader : LinkReader<LinkTemplate>
    {
        /// <summary>Returns a collection of typed links.</summary>
        /// <param name="links">The links to read.</param>
        /// <returns>A collection of typed links.</returns>
        public override LinkCollection<LinkTemplate> ReadAllLinks(IEnumerable<ILink> links)
        {
            return links == null ?
                new LinkCollection<LinkTemplate>(Enumerable.Empty<LinkTemplate>()) :
                new LinkCollection<LinkTemplate>(links.Select(l => new LinkTemplate(l)));
        }
    }
}
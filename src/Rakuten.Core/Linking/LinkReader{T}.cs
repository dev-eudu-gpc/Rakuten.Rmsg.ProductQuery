// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="LinkReader{T}.cs" company="Rakuten">
//   Copyright (c) Rakuten. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------
namespace Rakuten
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>Reads a collection of links.</summary>
    /// <typeparam name="T">The type of link to read.</typeparam>
    public class LinkReader<T> where T : LinkTemplate
    {
        /// <summary>Provides the means to instantiate links of type <typeparamref name="T"/>.</summary>
        private readonly ILinkActivator<T> createLink;

        /// <summary>Initializes a new instance of the <see cref="LinkReader{T}"/> class.</summary>
        /// <param name="createLink">Provides the means to instantiate links of type <typeparamref name="T"/>.</param>
        public LinkReader(ILinkActivator<T> createLink)
        {
            this.createLink = createLink;
        }

        /// <summary>Initializes a new instance of the <see cref="LinkReader{T}"/> class.</summary>
        /// <param name="createLink">Provides the means to instantiate links of type <typeparamref name="T"/>.</param>
        public LinkReader(Func<string, IUriTemplate, ITargetAttributes, T> createLink)
            : this(new DefaultLinkActivator<T>(createLink))
        {
        }

        /// <summary>Initializes a new instance of the <see cref="LinkReader{T}"/> class.</summary>
        public LinkReader() : this(new DefaultLinkActivator<T>())
        {
        }

        /// <summary>Returns a collection of typed links.</summary>
        /// <param name="links">The links to read.</param>
        /// <returns>A collection of typed links.</returns>
        public virtual LinkCollection<T> ReadAllLinks(IEnumerable<ILink> links)
        {
            IEnumerable<T> typedLinks = links == null ?
                Enumerable.Empty<T>() :
                links.Select(l => new LinkTemplate(l))
                    .Where(l => l.RelationTypes.Contains(new LinkTypeInfo<T>().Name, StringComparer.OrdinalIgnoreCase))
                    .Select(l => this.createLink.Create(l.RelationType, l.Target, l.TargetAttributes));

            return new LinkCollection<T>(typedLinks);
        }
    }
}
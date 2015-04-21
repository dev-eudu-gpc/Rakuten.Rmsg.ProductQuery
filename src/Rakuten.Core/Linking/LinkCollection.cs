// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="LinkCollection.cs" company="Rakuten">
//   Copyright (c) Rakuten. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------
namespace Rakuten
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.Globalization;
    using System.Linq;

    /// <summary>
    /// Represents a collection of links.
    /// </summary>
    /// <typeparam name="T">The type of the link.</typeparam>
    public class LinkCollection<T> : IEnumerable<T> where T : LinkTemplate
    {
        /// <summary>
        /// The collection of links.
        /// </summary>
        private readonly ImmutableArray<T> links;

        /// <summary>
        /// Initializes a new instance of the <see cref="LinkCollection{T}"/> class.
        /// </summary>
        /// <param name="links">The collection of links.</param>
        public LinkCollection(IEnumerable<T> links)
        {
            this.links = links == null ? ImmutableArray<T>.Empty : links.ToImmutableArray();
        }

        /// <summary>Gets the collection of links.</summary>
        public ImmutableArray<T> Links
        {
            get { return this.links; }
        }

        /// <summary>
        /// Gets links with a specified language.
        /// </summary>
        /// <param name="culture">The language of link to return.</param>
        /// <returns>A collection of links with the specified language.</returns>
        public LinkCollection<T> OfLanguage(CultureInfo culture)
        {
            return new LinkCollection<T>(
                this.links.Where(l => culture.Name.Equals(l.TargetAttributes.LanguageTag)));
        }

        /// <summary>
        /// Gets links of a specified relation type.
        /// </summary>
        /// <param name="relationType">The relation type of link to return.</param>
        /// <returns>A collection of links of the specified relation type.</returns>
        public LinkCollection<T> OfType(string relationType)
        {
            return new LinkCollection<T>(
                this.links.Where(l => l.RelationTypes.Contains(relationType, StringComparer.OrdinalIgnoreCase)));
        }

        /// <summary>
        /// Gets links of a specified type.
        /// </summary>
        /// <typeparam name="TLink">The type of link to return.</typeparam>
        /// <returns>A collection of links of the specified type.</returns>
        public LinkCollection<T> OfType<TLink>() where TLink : LinkTemplate
        {
            return this.OfType(new LinkTypeInfo<TLink>().Name);
        }

        /// <summary
        /// >Returns an enumerator that iterates through the link collection.
        /// </summary>
        /// <returns>An enumerator that iterates through the link collection.</returns>
        public IEnumerator<T> GetEnumerator()
        {
            return ((IEnumerable<T>)this.links).GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through the link collection.
        /// </summary>
        /// <returns>An enumerator that iterates through the link collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
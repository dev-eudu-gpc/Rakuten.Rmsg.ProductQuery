// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="LinkTemplate.cs" company="Rakuten">
//   Copyright (c) Rakuten. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------
namespace Rakuten
{
    using System;
    using System.Collections.Immutable;
    using System.Diagnostics.Contracts;

    /// <summary>Represents a link template.</summary>
    public class LinkTemplate
    {
        /// <summary>Initializes a new instance of the <see cref="LinkTemplate"/> class.</summary>
        /// <param name="target">The location of the resource this link points to.</param>
        public LinkTemplate(IUriTemplate target) : this(relationType: null, target: target)
        {
            Contract.Requires(target != null);
        }

        /// <summary>Initializes a new instance of the <see cref="LinkTemplate"/> class.</summary>
        /// <param name="relationType">The link relation type.</param>
        /// <param name="target">The location of the resource this link points to.</param>
        public LinkTemplate(string relationType, IUriTemplate target) : this(relationType, target, attributes: null)
        {
            Contract.Requires(target != null);
        }

        /// <summary>Initializes a new instance of the <see cref="LinkTemplate"/> class.</summary>
        /// <param name="relationType">The link relation type.</param>
        /// <param name="target">The location of the resource this link points to.</param>
        /// <param name="attributes">The attributes of the resource this link points to.</param>
        public LinkTemplate(string relationType, IUriTemplate target, ITargetAttributes attributes)
        {
            Contract.Requires(target != null);

            this.RelationType = relationType ?? LinkRelationTypes.Self;
            this.RelationTypes = Split(relationType);
            this.Target = target;
            this.TargetAttributes = attributes ?? new TargetAttributes();
        }

        /// <summary>Initializes a new instance of the <see cref="LinkTemplate"/> class.</summary>
        /// <param name="link">A link from which to create the link template.</param>
        public LinkTemplate(ILink link)
        {
            Contract.Requires(link != null);
            Contract.Requires(!string.IsNullOrWhiteSpace(link.Target));

            this.RelationType = link.RelationType ?? LinkRelationTypes.Self;
            this.RelationTypes = Split(link.RelationType);
            this.Target = new UriTemplate(link.Target);
            this.TargetAttributes = new TargetAttributes(link.LanguageTag, link.MediaType, link.Title);
        }

        /// <summary>Gets the link relation type.</summary>
        public string RelationType { get; private set; }

        /// <summary>Gets the link relation types.</summary>
        public ImmutableArray<string> RelationTypes { get; private set; }

        /// <summary>Gets the location of the resource this link points to.</summary>
        public IUriTemplate Target { get; private set; }

        /// <summary>Gets the attributes of the resource this link points to.</summary>
        public ITargetAttributes TargetAttributes { get; private set; }

        /// <summary>
        /// Converts a link template to a link, without resolving any parameters.
        /// </summary>
        /// <param name="template">The link template to convert.</param>
        /// <returns>A link representation of the <see cref="LinkTemplate"/> object.</returns>
        public static implicit operator Link(LinkTemplate template)
        {
            return template == null ? null : template.ToLink(resolveTemplate: false);
        }

        /// <summary>
        /// Converts a link template to a URI, without resolving any parameters.
        /// </summary>
        /// <param name="template">The link template to convert.</param>
        /// <returns>A URI representation of the <see cref="LinkTemplate"/> object.</returns>
        public static implicit operator Uri(LinkTemplate template)
        {
            return template == null ? null : template.ToUri();
        }

        /// <summary>
        /// Converts the value of the current template to an equivalent link representation.
        /// </summary>
        /// <param name="resolveTemplate">
        /// <see langword="true"/> if the string representation should be resolved, with no unspecified parameters;
        /// otherwise, <see langword="false"/>.
        /// </param>
        /// <returns>A link representation of the current <see cref="LinkTemplate"/> object.</returns>
        public Link ToLink(bool resolveTemplate)
        {
            Contract.Requires(this.TargetAttributes != null);

            var attributes = this.TargetAttributes;

            return new Link
            {
                LanguageTag = attributes.LanguageTag,
                MediaType = attributes.MediaType,
                RelationType = this.RelationType,
                Target = this.Target.ToString(resolveTemplate)
            };
        }

        /// <summary>Returns a URI derived from the current template.</summary>
        /// <returns>A URI derived from the current template.</returns>
        public Uri ToUri()
        {
            return this.Target.ToUri();
        }

        /// <summary>
        /// Returns a string array that contains the substrings in the specified
        /// link relation that represent individual relation types.
        /// </summary>
        /// <param name="value">The link relation type.</param>
        /// <returns>An array of individual relation types.</returns>
        private static ImmutableArray<string> Split(string value)
        {
            return string.IsNullOrWhiteSpace(value) ?
                ImmutableArray<string>.Empty :
                value.Split(' ').ToImmutableArray();
        }
    }
}
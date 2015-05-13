// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="LinkBuilder{T}.cs" company="Rakuten">
//   Copyright (c) Rakuten. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------
namespace Rakuten
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Globalization;
    using System.Linq;

    /// <summary>Provides functionality for constructing a link.</summary>
    /// <typeparam name="T">The type of link to build.</typeparam>
    public class LinkBuilder<T> : ILinkBuilder<T> where T : LinkTemplate
    {
        /// <summary>
        /// The target attributes of the link under construction.
        /// </summary>
        private readonly ITargetAttributes attributesOfLink;

        /// <summary>
        /// An instance that can be used to instantiate the link.
        /// </summary>
        private readonly ILinkActivator<T> activator;

        /// <summary>
        /// The relation type of the link under construction.
        /// </summary>
        private readonly string relationTypeOfLink;

        /// <summary>
        /// A template that represents the link under construction.
        /// </summary>
        private T template;

        /// <summary>Initializes a new instance of the <see cref="LinkBuilder{T}"/> class.</summary>
        /// <param name="target">The target of the link.</param>
        public LinkBuilder(string target) : this(new UriTemplate(target))
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(target));
        }

        /// <summary>Initializes a new instance of the <see cref="LinkBuilder{T}"/> class.</summary>
        /// <param name="target">The target of the link.</param>
        public LinkBuilder(IUriTemplate target) : this(new DefaultLinkActivator<T>(), target)
        {
            Contract.Requires(target != null);
        }

        /// <summary>Initializes a new instance of the <see cref="LinkBuilder{T}"/> class.</summary>
        /// <param name="createLink">A delegate that can be used to instantiate the link.</param>
        /// <param name="target">The target of the link.</param>
        public LinkBuilder(Func<string, IUriTemplate, ITargetAttributes, T> createLink, IUriTemplate target)
            : this(new DefaultLinkActivator<T>(createLink), target, null, null)
        {
            Contract.Requires(target != null);
        }

        /// <summary>Initializes a new instance of the <see cref="LinkBuilder{T}"/> class.</summary>
        /// <param name="activator">An instance that can be used to instantiate the link.</param>
        /// <param name="target">The target of the link.</param>
        public LinkBuilder(ILinkActivator<T> activator, IUriTemplate target)
            : this(activator, target, null, null)
        {
            Contract.Requires(activator != null);
            Contract.Requires(target != null);
        }

        /// <summary>Initializes a new instance of the <see cref="LinkBuilder{T}"/> class.</summary>
        /// <param name="activator">An instance that can be used to instantiate the link.</param>
        /// <param name="target">The target of the link.</param>
        /// <param name="relationType">The relation type of the link.</param>
        /// <param name="attributes">The target attributes of the link.</param>
        protected LinkBuilder(
            ILinkActivator<T> activator,
            IUriTemplate target,
            string relationType,
            ITargetAttributes attributes)
        {
            Contract.Requires(activator != null);
            Contract.Requires(target != null);

            this.activator = activator;
            this.TargetOfLink = target;
            this.relationTypeOfLink = relationType;
            this.attributesOfLink = attributes ?? new TargetAttributes();
        }

        /// <summary>Gets a template that represents the link under construction.</summary>
        public virtual T Template
        {
            get
            {
                if (this.template == null)
                {
                    return this.template = this.activator.Create(
                        Concatenate(this.relationTypeOfLink, new LinkTypeInfo<T>().Name),
                        this.TargetOfLink,
                        this.attributesOfLink);
                }

                return this.template;
            }
        }
        
        /// <summary>Gets the target of the link under construction.</summary>
        protected IUriTemplate TargetOfLink { get; private set; }

        /// <summary>Provides functionality for building a link with the specified language.</summary>
        /// <param name="culture">The language of the link target.</param>
        /// <returns>An instance that can be used to build a link to a resource of the specified language.</returns>
        public LinkBuilder<T> WithLanguage(CultureInfo culture)
        {
            return new LinkBuilder<T>(
                this.activator,
                this.TargetOfLink,
                this.relationTypeOfLink,
                this.attributesOfLink.WithLanguageTag(culture));
        }

        /// <summary>Provides functionality for building a link with the specified relation type.</summary>
        /// <param name="relationType">The link relation type.</param>
        /// <returns>An instance that can be used to build a link of the specified relation type.</returns>
        public virtual ITypedLinkBuilder<T> WithType(string relationType)
        {
            return new TypedBuilder(
                this.activator,
                this.TargetOfLink,
                Concatenate(this.relationTypeOfLink, relationType),
                this.attributesOfLink);
        }

        /// <summary>Returns a link relation type by composing a collection of individual relation types.</summary>
        /// <param name="relationType">The individual link relation types.</param>
        /// <returns>A compound link relation type composed from the supplied link relation types.</returns>
        protected static string Concatenate(params string[] relationType)
        {
            return relationType == null ?
                null :
                string.Join(" ", relationType.Where(r => !string.IsNullOrWhiteSpace(r)));
        }

        /// <summary>Provides functionality for constructing a link that includes a target type relation.</summary>
        private sealed class TypedBuilder : LinkBuilder<T>, ITypedLinkBuilder<T>
        {
            /// <summary>Initializes a new instance of the <see cref="LinkBuilder{T}.TypedBuilder"/> class.</summary>
            /// <param name="activator">An instance that can be used to instantiate the link.</param>
            /// <param name="target">The target of the link.</param>
            /// <param name="relationType">The relation type of the link.</param>
            /// <param name="attributes">The target attributes of the link.</param>
            public TypedBuilder(
                ILinkActivator<T> activator,
                IUriTemplate target,
                string relationType,
                ITargetAttributes attributes) : base(activator, target, relationType, attributes)
            {
            }

            /// <summary>
            /// Provides functionality for constructing a link that does not include a target type relation.
            /// </summary>
            /// <returns>
            /// An instance that can be used to construct a link that does not include a target type relation.
            /// </returns>
            public ILinkBuilder<T> WithoutTargetType()
            {
                return new UntypedBuilder(this.activator, this.TargetOfLink, this.relationTypeOfLink, this.attributesOfLink);
            }

            /// <summary>
            /// Provides functionality for constructing a link that does not include a target type relation.
            /// </summary>
            private sealed class UntypedBuilder : LinkBuilder<T>
            {
                /// <summary>Initializes a new instance of the <see cref="UntypedBuilder"/> class.</summary>
                /// <param name="activator">An instance that can be used to instantiate the link.</param>
                /// <param name="target">The target of the link.</param>
                /// <param name="relationType">The relation type of the link.</param>
                /// <param name="attributes">The target attributes of the link.</param>
                public UntypedBuilder(
                    ILinkActivator<T> activator,
                    IUriTemplate target,
                    string relationType,
                    ITargetAttributes attributes) : base(activator, target, relationType, attributes)
                {
                }

                /// <summary>Gets a template that represents the link under construction.</summary>
                public override T Template
                {
                    get
                    {
                        return this.template ?? (this.template = this.activator.Create(
                            this.relationTypeOfLink,
                            this.TargetOfLink,
                            this.attributesOfLink));
                    }
                }
            }
        }
    }
}
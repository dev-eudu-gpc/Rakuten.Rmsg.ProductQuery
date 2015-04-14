// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="UploaderLink.cs" company="Rakuten">
//   Copyright (c) Rakuten. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------
namespace Rakuten.Gpc.Api.Links
{
    using System.Diagnostics.Contracts;

    /// <summary>
    /// Represents a link to a uploader.
    /// </summary>
    [LinkRelation(Name = "http://rels.rakuten.com/uploader")]
    public class UploaderLink : LinkTemplate
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UploaderLink"/> class.
        /// </summary>
        /// <param name="target">The location of the manufacturer this link points to.</param>
        public UploaderLink(IUriTemplate target) : base(target)
        {
            Contract.Requires(target != null);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UploaderLink"/> class.
        /// </summary>
        /// <param name="relationType">The link relation type.</param>
        /// <param name="target">The location of the manufacturer this link points to.</param>
        public UploaderLink(string relationType, IUriTemplate target) : base(relationType, target)
        {
            Contract.Requires(target != null);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UploaderLink"/> class.
        /// </summary>
        /// <param name="relationType">The link relation type.</param>
        /// <param name="target">The location of the manufacturer this link points to.</param>
        /// <param name="attributes">The attributes of the manufacturer this link points to.</param>
        public UploaderLink(string relationType, IUriTemplate target, ITargetAttributes attributes)
            : base(relationType, target, attributes)
        {
            Contract.Requires(target != null);
        }

        /// <summary>
        /// Gets a link to an uploader with the specified unique identifier.
        /// </summary>
        /// <param name="id">The uploader of the translator.</param>
        /// <returns>A link to an uploader with the specified unique identifier.</returns>
        public UploaderLink ForId(string id)
        {
            var resolvedTemplate = this.Target.Bind("id", id);
            Contract.Assume(resolvedTemplate != null);

            return new UploaderLink(this.RelationType, resolvedTemplate, this.TargetAttributes);
        }
    }
}
//------------------------------------------------------------------------------
// <copyright file="AzureBlobLink.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.Web.Http.Links
{
    using System.Diagnostics.Contracts;

    /// <summary>
    /// Represents a link to a product query.
    /// </summary>
    [LinkRelation(Name = "http://rels.rakuten.com/azure-blob")]
    public class AzureBlobLink : LinkTemplate
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AzureBlobLink"/> class.
        /// </summary>
        /// <param name="target">The location of the Azure blob that this link points to.</param>
        public AzureBlobLink(IUriTemplate target)
            : base(target)
        {
            Contract.Requires(target != null);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AzureBlobLink"/> class.
        /// </summary>
        /// <param name="relationType">The link relation type.</param>
        /// <param name="target">The location of the Azure blob that this link points to.</param>
        public AzureBlobLink(string relationType, IUriTemplate target)
            : base(relationType, target)
        {
            Contract.Requires(target != null);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AzureBlobLink"/> class.
        /// </summary>
        /// <param name="relationType">The link relation type.</param>
        /// <param name="target">The location of the Azure blob that this link points to.</param>
        /// <param name="attributes">The attributes of the Azure blob that this link points to.</param>
        public AzureBlobLink(string relationType, IUriTemplate target, ITargetAttributes attributes)
            : base(relationType, target, attributes)
        {
            Contract.Requires(target != null);
        }

        /// <summary>
        /// Gets a link to a specific Azure Blob.
        /// </summary>
        /// <param name="id">The identifier of the Azure blob.</param>
        /// <returns>A link to a Azure blob with the specified unique identifier.</returns>
        public AzureBlobLink ForId(string id)
        {
            var resolvedTemplate = this.Target.Bind("id", id);

            Contract.Assume(resolvedTemplate != null);

            return new AzureBlobLink(this.RelationType, resolvedTemplate, this.TargetAttributes);
        }
    }
}
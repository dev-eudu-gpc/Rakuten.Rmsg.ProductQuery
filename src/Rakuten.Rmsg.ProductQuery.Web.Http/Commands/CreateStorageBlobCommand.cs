//------------------------------------------------------------------------------
// <copyright file="CreateStorageBlobCommand.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.Web.Http.Commands
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Threading.Tasks;

    using Rakuten.Rmsg.ProductQuery.Configuration;
    using Rakuten.WindowsAzure.Storage;

    /// <summary>
    /// Represents a command for preparing a product query
    /// </summary>
    public class CreateStorageBlobCommand : AsyncCommand<CreateStorageBlobCommandParameters, Uri>
    {
        /// <summary>
        /// The context under which this instance is operating.
        /// </summary>
        private readonly IApiContext context;

        /// <summary>
        /// The object for interacting with storage.
        /// </summary>
        private readonly IStorage storage;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateStorageBlobCommand"/> class
        /// </summary>
        /// <param name="storage">A means to interact with storage for the product query</param>
        /// <param name="context">The context in which this instance is running.</param>
        public CreateStorageBlobCommand(
            IStorage storage,
            IApiContext context)
        {
            Contract.Requires(storage != null);
            Contract.Requires(context != null);

            this.context = context;
            this.storage = storage;
        }

        /// <summary>
        /// Creates a blob in storage for a product query.
        /// </summary>
        /// <param name="parameters">The input parameters enabling the product query to be uniquely identified</param>
        /// <returns>A task that does the work.</returns>
        public override Task<Uri> ExecuteAsync(CreateStorageBlobCommandParameters parameters)
        {
            Contract.Requires(parameters != null);

            return Task.Run(() =>
            {
                // Create blob in storage
                string blobFileName = string.Format(
                    this.context.BlobFileNameMask,
                    parameters.DateCreated.ToString("yyyy-MM-dd"),
                    parameters.Id);

                return this.storage.GetSharedAccessSignature(
                    this.context.StorageConnectionString,
                    this.context.BlobContainerName,
                    blobFileName);
            });
        }
    }
}
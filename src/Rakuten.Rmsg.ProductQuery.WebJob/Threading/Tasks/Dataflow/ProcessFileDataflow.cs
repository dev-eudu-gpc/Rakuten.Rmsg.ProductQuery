// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="ProcessFileDataflow.cs" company="Rakuten">
//   Copyright (c) Rakuten. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.WebJob
{
    using System.Diagnostics.Contracts;
    using System.IO;
    using System.Threading.Tasks.Dataflow;

    using Rakuten.Threading.Tasks.Dataflow;

    /// <summary>
    /// Represents a Dataflow pipeline that will process a product query request.
    /// </summary>
    internal class ProcessFileDataflow : Dataflow<Message, MessageState>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProcessFileDataflow"/> class using the specified 
        /// <see cref="IDataflowBlock"/>s.
        /// </summary>
        /// <param name="downloadFileBlock">The <see cref="IDataflowBlock"/> to download a specified blob.</param>
        /// <param name="parseFileBlock">A <see cref="IDataflowBlock"/> that will parse the uploaded file.</param>
        /// <param name="getEntityBlock">
        /// A <see cref="IDataflowBlock"/> that will look for an entity for a product query.
        /// </param>
        /// <param name="createEntityBlock">
        /// The <see cref="IDataflowBlock"/> that will create a record in the persistent storage for a request.
        /// </param>
        /// <param name="searchBlock">A <see cref="IDataflowBlock"/> that will search for a product by GTIN.</param>
        public ProcessFileDataflow(
            IPropagatorBlock<Message, Stream> downloadFileBlock,
            IPropagatorBlock<Stream, MessageState> parseFileBlock,
            IPropagatorBlock<MessageState, MessageState> getEntityBlock,
            IPropagatorBlock<MessageState, MessageState> createEntityBlock,
            IPropagatorBlock<MessageState, MessageState> searchBlock)
        {
            Contract.Requires(downloadFileBlock != null);
            Contract.Requires(parseFileBlock != null);
            Contract.Requires(getEntityBlock != null);
            Contract.Requires(createEntityBlock != null);
            Contract.Requires(searchBlock != null);

            // Set the start block for the pipeline.
            this.StartBlock = downloadFileBlock;

            // Once the file has been downloaded, parse it.
            downloadFileBlock.LinkTo(parseFileBlock);

            // Once the file has been downloaded, check to see if we have a matching database record for each row.
            parseFileBlock.LinkTo(getEntityBlock);

            // If we do not have a database record, create one.
            getEntityBlock.LinkTo(createEntityBlock, state => state.Query == null);

            // If we have a database record but it has not GRAN search for a product.
            getEntityBlock.LinkTo(
                searchBlock, 
                state => state.Query != null && string.IsNullOrWhiteSpace(state.Query.Gran));
        }

        /// <summary>
        /// Posts an item to the process.
        /// </summary>
        /// <param name="item">The item being offered to the process.</param>
        /// <returns>
        /// <see langword="true"/> if the item was accepted by the target block; otherwise, <see langword="false"/>.
        /// </returns>
        public override bool Post(Message item)
        {
            Contract.Requires(item != null);

            return this.StartBlock.Post(item);
        }
    }
}
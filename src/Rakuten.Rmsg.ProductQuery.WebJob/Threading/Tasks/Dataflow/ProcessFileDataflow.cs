// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="ProcessFileDataflow.cs" company="Rakuten">
//   Copyright (c) Rakuten. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.WebJob
{
    using System;
    using System.Diagnostics.Contracts;
    using System.IO;
    using System.Threading.Tasks.Dataflow;

    using Rakuten.Threading.Tasks.Dataflow;

    /// <summary>
    /// The process file dataflow.
    /// </summary>
    internal class ProcessFileDataflow : Dataflow<Message, MessageState>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProcessFileDataflow"/> class.
        /// </summary>
        /// <param name="downloadFileBlock"></param>
        /// <param name="parseFileBlock"></param>
        /// <param name="getEntityBlock"></param>
        public ProcessFileDataflow(
            TransformBlock<Message, Stream> downloadFileBlock,
            TransformManyBlock<Stream, MessageState> parseFileBlock,
            TransformBlock<MessageState, MessageState> getEntityBlock)
        {
            Contract.Requires(downloadFileBlock != null);
            Contract.Requires(parseFileBlock != null);
            Contract.Requires(getEntityBlock != null);

            // Set the start block for the pipeline.
            this.StartBlock = downloadFileBlock;

            downloadFileBlock.LinkTo(parseFileBlock);

            parseFileBlock.LinkTo(getEntityBlock);
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

            if (this.StartBlock == null)
            {
                throw new InvalidOperationException("The start block must be defined before posting data to it.");
            }

            return this.StartBlock.Post(item);
        }
    }
}
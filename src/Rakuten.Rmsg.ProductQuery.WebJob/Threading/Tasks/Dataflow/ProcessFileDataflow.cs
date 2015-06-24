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
    internal class ProcessFileDataflow : PropagatorDataflow<Message, Item>
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
        /// <param name="filterBlock">
        /// A <see cref="IDataflowBlock"/> that will filter a collection of products to a single selected product.
        /// </param>
        /// <param name="updateEntityBlock">
        /// A <see cref="IDataflowBlock"/> that will update the persistent data store with the specified product.
        /// </param>
        /// <param name="getProductBlock">
        /// A <see cref="IDataflowBlock"/> that will retrieve a specific products details.
        /// </param>
        /// <param name="aggregateBlock">
        /// A <see cref="IDataflowBlock"/> that will aggregate item and product data.
        /// </param>
        /// <param name="outputBlock">
        /// A <see cref="IDataflowBlock"/> that will extract and return an <see cref="Item"/> from the message.
        /// </param>
        public ProcessFileDataflow(
            IPropagatorBlock<Message, Stream> downloadFileBlock,
            IPropagatorBlock<Stream, ItemMessageState> parseFileBlock,
            IPropagatorBlock<ItemMessageState, ItemMessageState> getEntityBlock,
            IPropagatorBlock<ItemMessageState, ItemMessageState> createEntityBlock,
            IPropagatorBlock<ItemMessageState, ItemMessageState> searchBlock,
            IPropagatorBlock<ItemMessageState, ItemMessageState> filterBlock,
            IPropagatorBlock<ItemMessageState, ItemMessageState> updateEntityBlock,
            IPropagatorBlock<ItemMessageState, ItemMessageState> getProductBlock,
            IPropagatorBlock<ItemMessageState, ItemMessageState> aggregateBlock,
            TransformBlock<ItemMessageState, Item> outputBlock)
        {
            Contract.Requires(downloadFileBlock != null);
            Contract.Requires(parseFileBlock != null);
            Contract.Requires(getEntityBlock != null);
            Contract.Requires(createEntityBlock != null);
            Contract.Requires(searchBlock != null);
            Contract.Requires(filterBlock != null);
            Contract.Requires(updateEntityBlock != null);
            Contract.Requires(getProductBlock != null);
            Contract.Requires(aggregateBlock != null);
            Contract.Requires(outputBlock != null);

            // Set the start block for the pipeline.
            this.StartBlock = downloadFileBlock;

            // Once the file has been downloaded, parse it.
            downloadFileBlock.LinkTo(parseFileBlock);
            downloadFileBlock.OnFaultOrCompletion(parseFileBlock);

            // Once the file has been downloaded, check to see if we have a matching database record for each row.
            parseFileBlock.LinkTo(
                getEntityBlock, 
                state => !string.IsNullOrWhiteSpace(state.Item.GtinValue), 
                outputBlock);

            parseFileBlock.LinkTo(outputBlock, state => string.IsNullOrWhiteSpace(state.Item.GtinValue));

            parseFileBlock.OnFaultOrCompletion(getEntityBlock);

            // If we do not have a database record, create one.
            getEntityBlock.LinkTo(createEntityBlock, state => state.Query == null, outputBlock);
            getEntityBlock.OnFaultOrCompletion(createEntityBlock);

            // If we have a database record but it does not have a GRAN then search for a product.
            getEntityBlock.LinkTo(
                searchBlock,
                state => state.Query != null && string.IsNullOrWhiteSpace(state.Query.Gran),
                outputBlock);

            // Once we have recorded the request, search for the product.
            createEntityBlock.LinkTo(searchBlock, outputBlock);
            createEntityBlock.OnFaultOrCompletion(searchBlock);

            // Once we have received the product data, get the most appropriate product.
            searchBlock.LinkTo(filterBlock, state => !state.Products.IsDefault, outputBlock);
            searchBlock.OnFaultOrCompletion(filterBlock);

            searchBlock.LinkTo(outputBlock, state => state.Products.IsDefault);

            // Now we have filtered the results, record the GRAN in the database.
            filterBlock.LinkTo(updateEntityBlock, outputBlock);
            filterBlock.OnFaultOrCompletion(updateEntityBlock);

            // If we have a database record that has a registered GRAN then get that product.
            getEntityBlock.LinkTo(
                getProductBlock,
                state => state.Query != null && !string.IsNullOrWhiteSpace(state.Query.Gran),
                outputBlock);

            // Get all available details for the GRAN.
            updateEntityBlock.LinkTo(
                getProductBlock,
                state => !string.IsNullOrWhiteSpace(state.Query.Gran),
                outputBlock);
            updateEntityBlock.OnFaultOrCompletion(getProductBlock);

            updateEntityBlock.LinkTo(outputBlock, state => string.IsNullOrWhiteSpace(state.Query.Gran));

            // Merge the data with the original.
            getProductBlock.LinkTo(aggregateBlock, outputBlock);
            getProductBlock.OnFaultOrCompletion(aggregateBlock);

            // Output the item regardless of any issues encountered.
            aggregateBlock.LinkTo(outputBlock);
            aggregateBlock.OnFaultOrCompletion(outputBlock);

            // Set the block which can be awaited for completion of the pipeline.
            this.Completion = outputBlock.Completion;

            // Set the block from which processed items can be received.
            this.ReceivableBlock = outputBlock;
        }  
    }
}
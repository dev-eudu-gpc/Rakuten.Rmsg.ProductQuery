// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="ProcessFileDataflow.cs" company="Rakuten">
//   Copyright (c) Rakuten. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.WebJob
{
    using System.Diagnostics.Contracts;
    using System.IO;
    using System.Threading.Tasks;
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
        public ProcessFileDataflow(
            IPropagatorBlock<Message, Stream> downloadFileBlock,
            IPropagatorBlock<Stream, ItemMessageState> parseFileBlock,
            IPropagatorBlock<ItemMessageState, QueryMessageState> getEntityBlock,
            IPropagatorBlock<QueryMessageState, QueryMessageState> createEntityBlock,
            IPropagatorBlock<QueryMessageState, ProductsMessageState> searchBlock,
            IPropagatorBlock<ProductsMessageState, QueryMessageState> filterBlock,
            IPropagatorBlock<QueryMessageState, QueryMessageState> updateEntityBlock,
            IPropagatorBlock<QueryMessageState, ProductMessageState> getProductBlock,
            TransformBlock<ProductMessageState, Item> aggregateBlock)
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

            // Set the start block for the pipeline.
            this.StartBlock = downloadFileBlock;

            // Once the file has been downloaded, parse it.
            downloadFileBlock.LinkTo(parseFileBlock);
            downloadFileBlock.OnFaultOrCompletion(parseFileBlock);

            // Once the file has been downloaded, check to see if we have a matching database record for each row.
            parseFileBlock.LinkTo(getEntityBlock);
            parseFileBlock.OnFaultOrCompletion(getEntityBlock);

            // If we do not have a database record, create one.
            getEntityBlock.LinkTo(createEntityBlock, state => state.Query == null);
            getEntityBlock.OnFaultOrCompletion(createEntityBlock);

            // Once we have recorded the request, search for the product.
            createEntityBlock.LinkTo(searchBlock);

            // If we have a database record but it has not GRAN search for a product.
            getEntityBlock.LinkTo(
                searchBlock,
                state => state.Query != null && string.IsNullOrWhiteSpace(state.Query.Gran));

            // Only complete the search block once the create entity and get entity blocks have completed.
            Task.WhenAll(getEntityBlock.Completion, createEntityBlock.Completion)
                .ContinueWith(_ => searchBlock.Complete());

            // Once we have received the product data, get the most appropriate product.
            searchBlock.LinkTo(filterBlock);
            searchBlock.OnFaultOrCompletion(filterBlock);

            // Now we have filtered the results, record the GRAN in the database.
            filterBlock.LinkTo(updateEntityBlock);
            filterBlock.OnFaultOrCompletion(updateEntityBlock);

            // Get all available details for the GRAN.
            updateEntityBlock.LinkTo(getProductBlock);

            // If we have a database record that has a registered GRAN then get that product.
            getEntityBlock.LinkTo(
                getProductBlock,
                state => state.Query != null && !string.IsNullOrWhiteSpace(state.Query.Gran));

            // Only complete the get product block once the update entity and get entity blocks have completed.
            Task.WhenAll(getEntityBlock.Completion, updateEntityBlock.Completion)
                .ContinueWith(_ => getProductBlock.Complete());

            // Merge the data with the original.
            getProductBlock.LinkTo(aggregateBlock);
            getProductBlock.OnFaultOrCompletion(aggregateBlock);

            this.Completion = aggregateBlock.Completion;
            this.ReceivableBlock = aggregateBlock;
        }
    }
}
// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="PropagatorDataflow{TInput,TOutput}.cs" company="Rakuten">
//   Copyright (c) Rakuten. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------
namespace Rakuten.Threading.Tasks.Dataflow
{
    using System.Collections.Generic;
    using System.Threading.Tasks.Dataflow;

    /// <summary>
    /// A base implementation of a Dataflow pipeline that both receives and exposes data.
    /// </summary>
    /// <typeparam name="TInput">The type accepted by the process.</typeparam>
    /// <typeparam name="TOutput">Specifies the type of data supplied by the Dataflow.</typeparam>
    public class PropagatorDataflow<TInput, TOutput> : Dataflow<TInput>, IReceivableDataflow<TOutput>
    {
        /// <summary>
        /// Gets or sets the <see cref="IDataflowBlock"/> from which messages are received.
        /// </summary>
        public IReceivableSourceBlock<TOutput> ReceivableBlock { get; protected set; }

        /// <summary>
        /// Attempts to synchronously receive all available items from the Dataflow.
        /// </summary>
        /// <param name="items">The items received from the source.</param>
        /// <returns>
        /// <see langword="true"/> if one or more items could be received; otherwise, <see langword="false"/>.
        /// </returns>
        public bool TryReceiveAll(out IList<TOutput> items)
        {
            return this.ReceivableBlock.TryReceiveAll(out items);
        }
    }
}
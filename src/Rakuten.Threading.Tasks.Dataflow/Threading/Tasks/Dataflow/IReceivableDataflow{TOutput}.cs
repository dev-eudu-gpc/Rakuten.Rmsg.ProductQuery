// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="IReceivableDataflow{TOutput}.cs" company="Rakuten">
//   Copyright (c) Rakuten. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------
namespace Rakuten.Threading.Tasks.Dataflow
{
    using System.Collections.Generic;

    /// <summary>
    /// Represents a Dataflow pipeline that supports receiving messages.
    /// </summary>
    /// <typeparam name="TOutput">Specifies the type of data supplied by the Dataflow.</typeparam>
    public interface IReceivableDataflow<TOutput>
    {
        /// <summary>
        /// Attempts to synchronously receive all available items from the Dataflow.
        /// </summary>
        /// <param name="items">The items received from the source.</param>
        /// <returns>
        /// <see langword="true"/> if one or more items could be received; otherwise, <see langword="false"/>.
        /// </returns>
        bool TryReceiveAll(out IList<TOutput> items);
    }
}
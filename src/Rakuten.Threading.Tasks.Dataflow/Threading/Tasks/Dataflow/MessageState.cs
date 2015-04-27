// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="MessageState.cs" company="Rakuten">
//   Copyright (c) Rakuten. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------
namespace Rakuten.Threading.Tasks.Dataflow
{
    using System;
    using System.Collections.Immutable;

    /// <summary>
    /// Represents the current state of a message being processed through the DataFlow pipeline.
    /// </summary>
    public abstract class MessageState : IMessageState
    {
        /// <summary>
        /// Gets or sets the collection of exceptions that detail the issues encountered when processing this message.
        /// </summary>
        public ImmutableArray<Exception> Exceptions { get; protected set; }
    }
}
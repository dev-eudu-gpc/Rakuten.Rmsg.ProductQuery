// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="IMessageState.cs" company="Rakuten">
//   Copyright (c) Rakuten. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------
namespace Rakuten.Threading.Tasks.Dataflow
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Defines a representation of the state of a message that is being process through a DataFlow pipeline or network.
    /// </summary>
    public interface IMessageState
    {
        /// <summary>
        /// Gets the collection of exceptions that detail the issues encountered when processing this message.
        /// </summary>
        IEnumerable<Exception> Exceptions { get; }
    }
}
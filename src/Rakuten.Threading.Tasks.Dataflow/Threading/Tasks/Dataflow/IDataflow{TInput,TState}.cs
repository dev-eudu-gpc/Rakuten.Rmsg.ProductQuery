// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="IDataFlow{TInput,TState}.cs" company="Rakuten">
//   Copyright (c) Rakuten. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------
namespace Rakuten.Threading.Tasks.Dataflow
{
    using System.Threading.Tasks;
    using System.Threading.Tasks.Dataflow;

    /// <summary>
    /// Defines a process that consists of one or more linked <see cref="IDataflowBlock"/>s.
    /// </summary>
    /// <typeparam name="TInput">The type accepted by the process.</typeparam>
    /// <typeparam name="TState">The type that defines a messages state.</typeparam>
    public interface IDataflow<in TInput, in TState> where TState : IMessageState
    {
        /// <summary>
        /// Gets the completion task that represents the completion status of process.
        /// </summary>
        Task Completion { get; }

        /// <summary>
        /// Gets the first <see cref="IDataflowBlock"/>.
        /// </summary>
        ITargetBlock<TInput> StartBlock { get; }

        /// <summary>
        /// Signals to the <see cref="IDataflow{TInput,TState}"/> that it should not accept nor produce any more 
        /// messages nor consume any more postponed messages.
        /// </summary>
        void Complete();

        /// <summary>
        /// Posts an item to the process.
        /// </summary>
        /// <param name="item">The item being offered to the process.</param>
        /// <returns>
        /// <see langword="true"/> if the item was accepted by the target block; otherwise, <see langword="false"/>.
        /// </returns>
        bool Post(TInput item);
    }
}
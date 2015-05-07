// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="DataFlow{TInput}.cs" company="Rakuten">
//   Copyright (c) Rakuten. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------
namespace Rakuten.Threading.Tasks.Dataflow
{
    using System;
    using System.Threading.Tasks;
    using System.Threading.Tasks.Dataflow;

    /// <summary>
    /// A base implementation of a process that consists of one or more linked <see cref="IDataflowBlock"/>s.
    /// </summary>
    /// <typeparam name="TInput">The type accepted by the process.</typeparam>
    public abstract class Dataflow<TInput> : ITargetDataflow<TInput>
    {
        /// <summary>
        /// Gets or sets the completion task that represents the completion status of process.
        /// </summary>
        public Task Completion { get; protected set; }

        /// <summary>
        /// Gets or sets the first <see cref="IDataflowBlock"/>.
        /// </summary>
        public ITargetBlock<TInput> StartBlock { get; protected set; }

        /// <summary>
        /// Signals to the <see cref="ITargetDataflow{TInput}"/> that it should not accept nor produce any more 
        /// messages nor consume any more postponed messages.
        /// </summary>
        public void Complete()
        {
            if (this.StartBlock == null)
            {
                throw new InvalidOperationException("The start block must be defined before calling completion.");
            }

            this.StartBlock.Complete();
        }

        /// <summary>
        /// Posts an item to the process.
        /// </summary>
        /// <param name="item">The item being offered to the process.</param>
        /// <returns>
        /// <see langword="true"/> if the item was accepted by the target block; otherwise, <see langword="false"/>.
        /// </returns>
        public virtual bool Post(TInput item)
        {
            if (this.StartBlock == null)
            {
                throw new InvalidOperationException("The start block must be defined before posting data to it.");
            }

            return this.StartBlock.Post(item);
        }
    }
}
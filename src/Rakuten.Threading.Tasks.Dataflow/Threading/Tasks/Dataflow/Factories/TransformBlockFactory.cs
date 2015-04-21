// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="TransformBlockFactory.cs" company="Rakuten">
//   Copyright (c) Rakuten. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------
namespace Rakuten.Threading.Tasks.Dataflow
{
    using System;
    using System.Threading.Tasks;
    using System.Threading.Tasks.Dataflow;

    /// <summary>
    /// Represents the factory for creating new <see cref="TransformBlock{TInput,TOutput}"/> instance.
    /// </summary>
    public class TransformBlockFactory
    {
        /// <summary>
        /// Creates a new <see cref="TransformBlock{TInput,TOutput}"/> instance.
        /// </summary>
        /// <typeparam name="TInput">
        /// Specifies the type of data to be received and operated on by the new instance.
        /// </typeparam>
        /// <typeparam name="TOutput">Specifies the type of data to be output by the new instance.</typeparam>
        /// <param name="transform">The function that should be invoked with each data element received.</param>
        /// <returns>A new <see cref="TransformBlock{TInput,TOutput}"/> instance.</returns>
        public static TransformBlock<TInput, TOutput> Create<TInput, TOutput>(Func<TInput, Task<TOutput>> transform)
        {
            return new TransformBlock<TInput, TOutput>(transform);
        }
    }
}
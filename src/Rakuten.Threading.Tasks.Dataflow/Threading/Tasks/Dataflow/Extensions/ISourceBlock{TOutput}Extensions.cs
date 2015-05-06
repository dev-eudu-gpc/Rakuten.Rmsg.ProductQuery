// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="ISourceBlock{TOutput}Extensions.cs" company="Rakuten">
//   Copyright (c) Rakuten. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------
namespace Rakuten.Threading.Tasks.Dataflow
{
    using System;
    using System.Linq;
    using System.Threading.Tasks.Dataflow;

    /// <summary>
    /// A useful set of methods to extend instances of <see cref="ITargetBlock{TInput}"/>.
    /// </summary>
    // ReSharper disable once InconsistentNaming
    public static class ISourceBlockExtensions
    {
        /// <summary>
        /// Links the <see cref="ISourceBlock{TOutput}"/> to the specified <see cref="ITargetBlock{TInput}"/> using the 
        /// specified filter.
        /// </summary>
        /// <typeparam name="TOutput">Specifies the type of data contained in the source.</typeparam>
        /// <param name="source">The source from which to link.</param>
        /// <param name="onSuccessTarget">
        /// The <see cref="ITargetBlock{TInput}"/> to which to connect the source when the message was processed 
        /// successfully.
        /// </param>
        /// <param name="onErrorTarget">
        /// The <see cref="ITargetBlock{TInput}"/> to which to connect the source when issues were encountered when 
        /// processing the message.
        /// </param>
        public static void LinkTo<TOutput>(
            this ISourceBlock<TOutput> source,
            ITargetBlock<TOutput> onSuccessTarget,
            ITargetBlock<TOutput> onErrorTarget) where TOutput : IMessageState
        {
            source.OnSuccessLinkTo(onSuccessTarget);
            source.OnErrorLinkTo(onErrorTarget);
        }

        /// <summary>
        /// Links the <see cref="ISourceBlock{TOutput}"/> to the specified <see cref="ITargetBlock{TInput}"/> using the 
        /// specified filter.
        /// </summary>
        /// <typeparam name="TOutput">Specifies the type of data contained in the source.</typeparam>
        /// <param name="source">The source from which to link.</param>
        /// <param name="onSuccessTarget">
        /// The <see cref="ITargetBlock{TInput}"/> to which to connect the source when the message was processed 
        /// successfully.
        /// </param>
        /// <param name="predicate">
        /// The filter a message must pass in order for it to propagate from the source to the target 
        /// <paramref name="onSuccessTarget"/>.
        /// </param>
        /// <param name="onErrorTarget">
        /// The <see cref="ITargetBlock{TInput}"/> to which to connect the source when issues were encountered when 
        /// processing the message.
        /// </param>
        public static void LinkTo<TOutput>(
            this ISourceBlock<TOutput> source,
            ITargetBlock<TOutput> onSuccessTarget,
            Predicate<TOutput> predicate,
            ITargetBlock<TOutput> onErrorTarget) where TOutput : IMessageState
        {
            source.OnSuccessLinkTo(predicate, onSuccessTarget);
            source.OnErrorLinkTo(onErrorTarget);
        }

        /// <summary>
        /// Links the <see cref="ISourceBlock{TOutput}"/> to the specified <see cref="ITargetBlock{TInput}"/> using the 
        /// specified filter.
        /// </summary>
        /// <typeparam name="TOutput">Specifies the type of data contained in the source.</typeparam>
        /// <param name="source">The source from which to link.</param>
        /// <param name="target">The <see cref="ITargetBlock{TInput}"/> to which to connect the source.</param>
        /// <returns>
        /// An <see cref="T:System.IDisposable"/> that, upon calling Dispose, will unlink the source from the target.
        /// </returns>
        public static IDisposable OnErrorLinkTo<TOutput>(
            this ISourceBlock<TOutput> source,
            ITargetBlock<TOutput> target) where TOutput : IMessageState
        {
            return source.LinkTo(target, output => output.Exceptions.Any());
        }

        /// <summary>
        /// Links the <see cref="ISourceBlock{TOutput}"/> to the specified <see cref="ITargetBlock{TInput}"/> using the 
        /// specified filter.
        /// </summary>
        /// <typeparam name="TOutput">Specifies the type of data contained in the source.</typeparam>
        /// <param name="source">
        /// The source from which to link.</param><param name="target">The <see cref="ITargetBlock{TInput}"/> to which 
        /// to connect the source.
        /// </param>
        /// <returns>
        /// An <see cref="T:System.IDisposable"/> that, upon calling Dispose, will unlink the source from the target.
        /// </returns>
        public static IDisposable OnSuccessLinkTo<TOutput>(
            this ISourceBlock<TOutput> source, 
            ITargetBlock<TOutput> target) where TOutput : IMessageState
        {
            return source.LinkTo(target, output => !output.Exceptions.Any());
        }

        /// <summary>
        /// Links the <see cref="ISourceBlock{TOutput}"/> to the specified <see cref="ITargetBlock{TInput}"/> using the 
        /// specified filter.
        /// </summary>
        /// <typeparam name="TOutput">Specifies the type of data contained in the source.</typeparam>
        /// <param name="source">The source from which to link.</param>
        /// <param name="predicate">
        /// The filter a message must pass in order for it to propagate from the source to the target.
        /// </param>
        /// <param name="target">The <see cref="ITargetBlock{TInput}"/> to which to connect the source.</param>
        /// <returns>
        /// An <see cref="T:System.IDisposable"/> that, upon calling Dispose, will unlink the source from the target.
        /// </returns>
        public static IDisposable OnSuccessLinkTo<TOutput>(
            this ISourceBlock<TOutput> source,
            Predicate<TOutput> predicate,
            ITargetBlock<TOutput> target) where TOutput : IMessageState
        {
            return source.LinkTo(target, output => predicate(output) && !output.Exceptions.Any());
        }
    }
}
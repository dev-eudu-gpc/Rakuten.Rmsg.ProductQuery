// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="IDataflowBlockExtensions.cs" company="Rakuten">
//   Copyright (c) Rakuten. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------
namespace Rakuten.Threading.Tasks.Dataflow
{
    using System.Diagnostics.Contracts;
    using System.Threading.Tasks.Dataflow;

    /// <summary>
    /// Provides a useful set of extensions to <see cref="IDataflowBlock"/> instances.
    /// </summary>
    // ReSharper disable once InconsistentNaming
    public static class IDataflowBlockExtensions
    {
        /// <summary>
        /// Associates this <see cref="IDataflowBlock"/> instance to the specified <see cref="IDataflowBlock"/> by 
        /// propagating exceptions where the current instance has faulted or by completing the 
        /// <paramref name="target"/>.
        /// </summary>
        /// <param name="source">The instance to associate to the <paramref name="target"/>.</param>
        /// <param name="target">The instance that is dependant on the current.</param>
        public static void OnFaultOrCompletion(this IDataflowBlock source, IDataflowBlock target)
        {
            Contract.Requires(source != null);
            Contract.Requires(target != null);

            source.Completion.ContinueWith(task =>
            {
                if (task.IsFaulted)
                {
                    // Propigates the exception.
                    target.Fault(task.Exception);
                }
                else
                {
                    target.Complete();
                }
            });
        }
    }
}
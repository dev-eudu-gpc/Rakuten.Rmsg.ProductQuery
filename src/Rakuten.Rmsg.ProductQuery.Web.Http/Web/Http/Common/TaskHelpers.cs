// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TaskHelpers.cs" company="Rakuten">
//   Copyright (c) Rakuten. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Rakuten.Threading.Tasks
{
    using System.Threading.Tasks;

    /// <summary>
    /// Helpers for safely using Task libraries. 
    /// </summary>
    internal static class TaskHelpers
    {
        /// <summary>
        /// A completed task that has no result.
        /// </summary>
        private static readonly Task DefaultCompleted = Task.FromResult(default(AsyncVoid));

        /// <summary>
        /// Returns a completed task that has no result. 
        /// </summary>
        /// <returns>
        /// A completed task.
        /// </returns>
        internal static Task Completed()
        {
            return DefaultCompleted;
        }

        /// <summary>
        /// Used as the T in a "conversion" of a Task into a Task{T}
        /// </summary>
        private struct AsyncVoid
        {
        }
    }
}
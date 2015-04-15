//----------------------------------------------------------------------------------------------------------------------
// <copyright file="AsyncCommandAction.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------------------------------------------------
namespace Rakuten
{
    using System.Threading.Tasks;

    /// <summary>
    /// Specifies the interface for a command action, which takes parameters in but returns nothing.
    /// </summary>
    /// <typeparam name="TIn">The type of the parameters for the command.</typeparam>
    public abstract class AsyncCommandAction<TIn> : ICommand<TIn, Task>
    {
        /// <summary>
        /// Executes a command asynchronously.
        /// </summary>
        /// <param name="source">The parameters for the command.</param>
        /// <returns>The return object.</returns>
        public abstract Task ExecuteAsync(TIn source);

        /// <summary>
        /// Executes the command.
        /// </summary>
        /// <param name="parameters">The parameters for the command.</param>
        /// <returns>A task that returns nothing.</returns>
        Task ICommand<TIn, Task>.Execute(TIn parameters)
        {
            return this.ExecuteAsync(parameters);
        }
    }
}
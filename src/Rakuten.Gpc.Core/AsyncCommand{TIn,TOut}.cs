//----------------------------------------------------------------------------------------------------------------------
// <copyright file="AsyncCommand{TIn,TOut}.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------------------------------------------------
namespace Rakuten
{
    using System.Threading.Tasks;

    /// <summary>
    /// Specifies the interface for a command.
    /// </summary>
    /// <typeparam name="TIn">The type of the parameters for the command.</typeparam>
    /// <typeparam name="TOut">The type of the return object.</typeparam>
    public abstract class AsyncCommand<TIn, TOut> : ICommand<TIn, Task<TOut>>
    {
        /// <summary>
        /// Executes a command asynchronously.
        /// </summary>
        /// <param name="source">The parameters for the command.</param>
        /// <returns>The return object.</returns>
        public abstract Task<TOut> ExecuteAsync(TIn source);

        /// <summary>
        /// Executes a command.
        /// </summary>
        /// <param name="parameters">The parameters for the command.</param>
        /// <returns>The return object.</returns>
        Task<TOut> ICommand<TIn, Task<TOut>>.Execute(TIn parameters)
        {
            return this.ExecuteAsync(parameters);
        }
    }
}
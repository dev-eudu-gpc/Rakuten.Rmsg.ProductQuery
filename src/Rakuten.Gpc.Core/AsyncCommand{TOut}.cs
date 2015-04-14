//----------------------------------------------------------------------------------------------------------------------
// <copyright file="AsyncCommand{TOut}.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------------------------------------------------
namespace Rakuten
{
    using System.Threading.Tasks;

    /// <summary>Specifies the interface for a command.</summary>
    /// <typeparam name="TOut">The type of the return object</typeparam>
    public abstract class AsyncCommand<TOut> : ICommand<Task<TOut>>
    {
        /// <summary>Executes a command asynchronously.</summary>
        /// <returns>The return object.</returns>
        public abstract Task<TOut> ExecuteAsync();

        /// <summary>Executes a command.</summary>
        /// <returns>The return object.</returns>
        Task<TOut> ICommand<Task<TOut>>.Execute()
        {
            return this.ExecuteAsync();
        }
    }
}
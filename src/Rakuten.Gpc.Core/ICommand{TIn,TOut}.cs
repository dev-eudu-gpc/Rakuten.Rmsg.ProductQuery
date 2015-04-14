//----------------------------------------------------------------------------------------------------------------------
// <copyright file="ICommand{TIn,TOut}.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------------------------------------------------
namespace Rakuten
{
    /// <summary>
    /// Specifies the interface for a command
    /// </summary>
    /// <typeparam name="TIn">The type of the parameters for the command</typeparam>
    /// <typeparam name="TOut">The type of the return object</typeparam>
    public interface ICommand<in TIn, out TOut>
    {
        /// <summary>
        /// Executes a command
        /// </summary>
        /// <param name="source">The parameters for the command</param>
        /// <returns>The return object</returns>
        TOut Execute(TIn source);
    }
}
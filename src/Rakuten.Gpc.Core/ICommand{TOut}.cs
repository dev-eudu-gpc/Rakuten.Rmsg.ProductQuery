//----------------------------------------------------------------------------------------------------------------------
// <copyright file="ICommand{TOut}.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------------------------------------------------
namespace Rakuten
{
    /// <summary>
    /// Specifies the interface for a command the takes nothing in and gives something out.
    /// A benevolent command, some might say.
    /// </summary>
    /// <typeparam name="TOut">The type of the return object.</typeparam>
    public interface ICommand<out TOut>
    {
        /// <summary>Executes a command.</summary>
        /// <returns>The return object</returns>
        TOut Execute();
    }
}
//---------------------------------------------------------------------------------------------------------------------
// <copyright file="ModuleBuilderFactory.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//---------------------------------------------------------------------------------------------------------------------
namespace Rakuten.Reflection.Emit
{
    using System;

    using System.Reflection.Emit;

    /// <summary>
    /// Provides a method of generating an <see cref="ModuleBuilder"/> instance.
    /// </summary>
    public class ModuleBuilderFactory
    {
        /// <summary>
        /// Creates a transient dynamic module in the provided assembly.
        /// </summary>
        /// <param name="assembly">
        /// The assembly in which the dynamic module should be created.
        /// </param>
        /// <returns>
        /// The dynamic module.
        /// </returns>
        public ModuleBuilder Create(AssemblyBuilder assembly)
        {
            if (assembly == null)
            {
                throw new ArgumentNullException("assembly");
            }

            return assembly.DefineDynamicModule(assembly.GetName().Name, false);
        }
    }
}
//---------------------------------------------------------------------------------------------------------------------
// <copyright file="AssemblyBuilderFactory.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//---------------------------------------------------------------------------------------------------------------------
namespace Rakuten.Reflection.Emit
{
    using System;
    using System.Reflection;
    using System.Reflection.Emit;
    using System.Threading;

    /// <summary>
    /// Provides a method of generating an <see cref="AssemblyBuilder"/> instance.
    /// </summary>
    public class AssemblyBuilderFactory
    {
        /// <summary>
        /// Create an <see cref="AssemblyBuilder"/> with the given name.
        /// </summary>
        /// <param name="name">
        /// The name of the dynamic assembly.
        /// </param>
        /// <returns>
        /// An <see cref="AssemblyBuilder"/> instance for a dynamic assembly with the given name.
        /// </returns>
        public AssemblyBuilder Create(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException("name");
            }

            var assemblyName = new AssemblyName { Name = name };

            AppDomain thisDomain = Thread.GetDomain();

            AssemblyBuilder builder = thisDomain.DefineDynamicAssembly(
                assemblyName,
                AssemblyBuilderAccess.Run);

            return builder;
        }
    }
}
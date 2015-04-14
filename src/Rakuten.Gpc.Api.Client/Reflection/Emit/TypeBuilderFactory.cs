//---------------------------------------------------------------------------------------------------------------------
// <copyright file="TypeBuilderFactory.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//---------------------------------------------------------------------------------------------------------------------
namespace Rakuten.Reflection.Emit
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Reflection;
    using System.Reflection.Emit;

    /// <summary>Provides a method of generating <see cref="TypeBuilder"/> instances.</summary>
    public class TypeBuilderFactory
    {
        /// <summary>The type attributes to apply when creating a new type.</summary>
        private const TypeAttributes DefaultTypeAttributes =
            TypeAttributes.Public |
            TypeAttributes.Class |
            TypeAttributes.AutoClass |
            TypeAttributes.AnsiClass |
            TypeAttributes.BeforeFieldInit |
            TypeAttributes.AutoLayout;

        /// <summary>
        /// Generates a <see cref="TypeBuilder"/> instance within the given dynamic module and with the given name.
        /// </summary>
        /// <param name="module">
        /// The dynamic module in which the <see cref="TypeBuilder"/> instance will be created.
        /// </param>
        /// <param name="name">
        /// The name to give to the type built by the <see cref="TypeBuilder"/> instance.
        /// </param>
        /// <returns>
        /// A <see cref="TypeBuilder"/> instance within the specified dynamic module.
        /// </returns>
        public TypeBuilder Create(ModuleBuilder module, string name)
        {
            Contract.Requires(module != null);
            Contract.Requires(!string.IsNullOrWhiteSpace(name));

            return module.DefineType(name, DefaultTypeAttributes, typeof(object), Type.EmptyTypes);
        }
    }
}
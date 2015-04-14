//---------------------------------------------------------------------------------------------------------------------
// <copyright file="MethodBuilderFactory.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//---------------------------------------------------------------------------------------------------------------------
namespace Rakuten.Reflection.Emit
{
    using System;
    using System.Reflection;
    using System.Reflection.Emit;

    /// <summary>
    /// Provides a method of adding methods to <see cref="TypeBuilder"/> instances.
    /// </summary>
    public class MethodBuilderFactory
    {
        /// <summary>
        /// Adds a new methods of the given <see cref="Type"/> and name to the specified <see cref="TypeBuilder"/> 
        /// instance.
        /// </summary>
        /// <param name="builder">
        /// The <see cref="TypeBuilder"/> instance to which the methods should be added.
        /// </param>
        /// <param name="name">
        /// The name of the methods to be added.
        /// </param>
        /// <param name="parameterTypes">
        /// The types of the parameters of the method.
        /// </param>
        /// <returns>
        /// The <see cref="MethodBuilder"/> instance.
        /// </returns>
        public MethodBuilder Create(TypeBuilder builder, string name, params Type[] parameterTypes)
        {
            if (builder == null)
            {
                throw new ArgumentNullException("builder");
            }

            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("name");
            }

            return builder.DefineMethod(
                name,
                MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig,
                null,
                parameterTypes);
        }

        /// <summary>
        /// Adds a new methods of the given <see cref="Type"/> and name to the specified <see cref="TypeBuilder"/> 
        /// instance.
        /// </summary>
        /// <param name="builder">
        /// The <see cref="TypeBuilder"/> instance to which the methods should be added.
        /// </param>
        /// <param name="type">
        /// The return <see cref="Type"/> of the method.
        /// </param>
        /// <param name="name">
        /// The name of the methods to be added.
        /// </param>
        /// <param name="parameterTypes">
        /// The types of the parameters of the method.
        /// </param>
        /// <returns>
        /// The <see cref="MethodBuilder"/> instance.
        /// </returns>
        public MethodBuilder Create(TypeBuilder builder, Type type, string name, params Type[] parameterTypes)
        {
            if (builder == null)
            {
                throw new ArgumentNullException("builder");
            }

            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("name");
            }

            return builder.DefineMethod(
                name,
                MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig,
                type,
                parameterTypes);
        }
    }
}
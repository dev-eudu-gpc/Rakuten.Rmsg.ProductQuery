//---------------------------------------------------------------------------------------------------------------------
// <copyright file="ConstructorBuilderFactory.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//---------------------------------------------------------------------------------------------------------------------
namespace Rakuten.Reflection.Emit
{
    using System;
    using System.Reflection;
    using System.Reflection.Emit;

    /// <summary>
    /// Provides methods of adding constructors to <see cref="TypeBuilder"/> instances.
    /// </summary>
    public class ConstructorBuilderFactory
    {
        /// <summary>
        /// Adds a new constructor to the given <see cref="TypeBuilder"/> instance that has no parameters.
        /// </summary>
        /// <param name="builder">
        /// The type to which the constructor should be added.
        /// </param>
        /// <returns>
        /// The <see cref="ConstructorBuilder"/> instance for the added constructor.
        /// </returns>
        public ConstructorBuilder Create(TypeBuilder builder)
        {
            return this.Create(builder, Type.EmptyTypes);
        }

        /// <summary>
        /// Adds a new constructor to the given <see cref="TypeBuilder"/> instance with the given parameters.
        /// </summary>
        /// <param name="builder">
        /// The type to which the constructor should be added.
        /// </param>
        /// <param name="parameters">
        /// The parameters that should define the new constructor.
        /// </param>
        /// <returns>
        /// The <see cref="ConstructorBuilder"/> instance for the added constructor.
        /// </returns>
        public ConstructorBuilder Create(TypeBuilder builder, Type[] parameters)
        {
            if (builder == null)
            {
                throw new ArgumentNullException("builder");
            }

            if (parameters == null)
            {
                throw new ArgumentNullException("parameters");
            }

            return builder.DefineConstructor(
                MethodAttributes.Public |
                MethodAttributes.SpecialName |
                MethodAttributes.RTSpecialName,
                CallingConventions.Standard,
                parameters);
        }
    }
}
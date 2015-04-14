//---------------------------------------------------------------------------------------------------------------------
// <copyright file="PropertyBuilderFactory.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//---------------------------------------------------------------------------------------------------------------------
namespace Rakuten.Reflection.Emit
{
    using System;
    using System.Reflection;
    using System.Reflection.Emit;

    /// <summary>
    /// Provides a method of adding properties to <see cref="TypeBuilder"/> instances.
    /// </summary>
    public class PropertyBuilderFactory
    {
        /// <summary>
        /// Adds a new property of the given <see cref="Type"/> and name to the specified <see cref="TypeBuilder"/> 
        /// </summary>
        /// <param name="builder">
        /// The <see cref="TypeBuilder"/> instance to which the property should be added.
        /// </param>
        /// <param name="type">
        /// The <see cref="Type"/> of the property to be added.
        /// </param>
        /// <param name="name">
        /// The name of the property to be added.
        /// </param>
        /// <returns>
        /// The <see cref="PropertyBuilder"/> instance.
        /// </returns>
        public PropertyBuilder Create(TypeBuilder builder, Type type, string name)
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

            return builder.DefineProperty(
                name,
                PropertyAttributes.None,
                type,
                null);
        }
    }
}
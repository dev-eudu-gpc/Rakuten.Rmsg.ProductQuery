//---------------------------------------------------------------------------------------------------------------------
// <copyright file="FieldBuilderFactory.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//---------------------------------------------------------------------------------------------------------------------
namespace Rakuten.Reflection.Emit
{
    using System;
    using System.Reflection;
    using System.Reflection.Emit;

    /// <summary>
    /// Provides a method of adding fields to <see cref="TypeBuilder"/> instances.
    /// </summary>
    public class FieldBuilderFactory
    {
        /// <summary>
        /// Adds a new field of the given <see cref="Type"/> and name to the specified <see cref="TypeBuilder"/> 
        /// instance.
        /// </summary>
        /// <param name="builder">
        /// The <see cref="TypeBuilder"/> instance to which the field should be added.
        /// </param>
        /// <param name="type">
        /// The <see cref="Type"/> of the field to be added.
        /// </param>
        /// <param name="name">
        /// The name of the field to be added.
        /// </param>
        /// <returns>
        /// The <see cref="FieldBuilder"/> instance.
        /// </returns>
        public FieldBuilder Create(TypeBuilder builder, Type type, string name)
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

            return builder.DefineField(
                name,
                type,
                FieldAttributes.Private);
        }
    }
}
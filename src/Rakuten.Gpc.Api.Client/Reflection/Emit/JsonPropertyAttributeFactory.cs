//---------------------------------------------------------------------------------------------------------------------
// <copyright file="JsonPropertyAttributeFactory.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//---------------------------------------------------------------------------------------------------------------------
namespace Rakuten.Reflection.Emit
{
    using System;
    using System.Reflection;
    using System.Reflection.Emit;

    using Newtonsoft.Json;

    /// <summary>
    /// Provides a method of generating an <see cref="CustomAttributeBuilder"/> instance of a 
    /// <see cref="JsonPropertyAttribute"/>.
    /// </summary>
    public class JsonPropertyAttributeFactory
    {
        /// <summary>
        /// The constructor used to instantiate a <see cref="JsonPropertyAttribute"/>.
        /// </summary>
        private static readonly ConstructorInfo Constructor = typeof(JsonPropertyAttribute)
            .GetConstructor(new[] { typeof(string) });

        /// <summary>
        /// Initializes a new <see cref="CustomAttributeBuilder"/> of a 
        /// <see cref="JsonPropertyAttribute"/>.
        /// </summary>
        /// <param name="propertyName">
        /// The value to pass to the attribute constructor.
        /// </param>
        /// <returns>
        /// A <see cref="CustomAttributeBuilder"/>.
        /// </returns>
        public CustomAttributeBuilder Create(string propertyName)
        {
            if (string.IsNullOrWhiteSpace(propertyName))
            {
                throw new ArgumentException("propertyName");
            }

            return new CustomAttributeBuilder(
                Constructor,
                new object[] { propertyName });
        }
    }
}
//----------------------------------------------------------------------------------------------------------------------
// <copyright file="XmlTypeAttributeFactory.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------------------------------------------------
namespace Rakuten.Reflection.Emit
{
    using System;
    using System.Reflection;
    using System.Reflection.Emit;
    using System.Xml.Serialization;

    /// <summary>
    /// Provides a method of generating an <see cref="CustomAttributeBuilder"/> instance
    /// of a <see cref="XmlTypeAttribute"/>.
    /// </summary>
    public class XmlTypeAttributeFactory
    {
        /// <summary>
        /// The constructor used to instantiate an <see cref="XmlTypeAttribute"/>.
        /// </summary>
        private static readonly ConstructorInfo Constructor = typeof(XmlTypeAttribute)
            .GetConstructor(new[] { typeof(string) });

        /// <summary>
        /// Initializes a new <see cref="CustomAttributeBuilder"/> of a <see cref="XmlTypeAttribute"/>.
        /// </summary>
        /// <param name="typeName">The value to pass to the attribute constructor.</param>
        /// <returns>A <see cref="CustomAttributeBuilder"/>.</returns>
        public CustomAttributeBuilder Create(string typeName)
        {
            if (string.IsNullOrWhiteSpace(typeName))
            {
                throw new ArgumentException("typeName");
            }

            return new CustomAttributeBuilder(Constructor, new object[] { typeName });
        }
    }
}
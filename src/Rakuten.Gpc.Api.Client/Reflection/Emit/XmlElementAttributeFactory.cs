//---------------------------------------------------------------------------------------------------------------------
// <copyright file="XmlElementAttributeFactory.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//---------------------------------------------------------------------------------------------------------------------
namespace Rakuten.Reflection.Emit
{
    using System;
    using System.Reflection;
    using System.Reflection.Emit;
    using System.Xml.Serialization;

    /// <summary>
    /// Provides a method of generating an <see cref="CustomAttributeBuilder"/>
    /// instance of a <see cref="XmlElementAttribute"/>.
    /// </summary>
    public class XmlElementAttributeFactory
    {
        /// <summary>
        /// The constructor used to instantiate an <see cref="XmlElementAttribute"/>.
        /// </summary>
        private static readonly ConstructorInfo Constructor = typeof(XmlElementAttribute)
            .GetConstructor(new[] { typeof(string) });

        /// <summary>
        /// Initializes a new <see cref="CustomAttributeBuilder"/> of a <see cref="XmlElementAttribute"/>.
        /// </summary>
        /// <param name="elementName">The value to pass to the attribute constructor.</param>
        /// <returns>A <see cref="CustomAttributeBuilder"/>.</returns>
        public CustomAttributeBuilder Create(string elementName)
        {
            if (string.IsNullOrWhiteSpace(elementName))
            {
                throw new ArgumentException("elementName");
            }

            return new CustomAttributeBuilder(Constructor, new object[] { elementName });
        }
    }
}
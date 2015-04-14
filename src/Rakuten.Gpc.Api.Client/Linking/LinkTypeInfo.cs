// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="LinkTypeInfo.cs" company="Rakuten">
//   Copyright (c) Rakuten. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------
namespace Rakuten
{
    using System.Reflection;

    /// <summary>Represents type declarations for link templates.</summary>
    /// <typeparam name="T">The link type.</typeparam>
    public class LinkTypeInfo<T> where T : LinkTemplate
    {
        /// <summary>
        /// The type property of the link relation attribute associated with the type.
        /// </summary>
        // ReSharper disable once StaticFieldInGenericType
        // The value of this field should vary with the generic parameter, so a static field is the correct choice.
        private static readonly string AttributeName = GetAttributeName();

        /// <summary>Gets the name of the link relation type.</summary>
        public string Name
        {
            get { return AttributeName; }
        }

        /// <summary>Gets the name of the link relation type from an attribute on the class.</summary>
        /// <returns>The name of the link relation type.</returns>
        private static string GetAttributeName()
        {
            var attribute = typeof(T).GetCustomAttribute<LinkRelationAttribute>();
            return attribute == null ? string.Empty : attribute.Name;
        }
    }
}
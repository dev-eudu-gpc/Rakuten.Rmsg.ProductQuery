//---------------------------------------------------------------------------------------------------------------------
// <copyright file="AttributeSetFactory.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//---------------------------------------------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.Web.Http.Tests.Integration
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    /// <summary>
    /// Helpers for creating <see cref="AttributeSet"/>s.
    /// </summary>
    public static class AttributeSetFactory
    {
        /// <summary>
        /// Creates a new attribute set containing the specified images.
        /// </summary>
        /// <param name="images">The images to include in the attribute set.</param>
        /// <returns>An attribute set containing the specified images.</returns>
        public static AttributeSet CreateImages(params Uri[] images)
        {
            if (images == null || images.Length == 0)
            {
                return null;
            }

            var attributes = new SortedDictionary<string, object> { { "image URL main", images[0] } };

            for (int i = images.Length; i != 1;)
            {
                attributes[string.Join(" ", "image location", i--)] = images[i];
            }

            return CreateNewDefinedAttributeSet(AttributeSetName.Images, attributes);
        }

        /// <summary>
        /// Creates a <see cref="AttributeSet"/> with the given name and values.
        /// </summary>
        /// <param name="name">
        /// The name of the <see cref="AttributeSet"/>
        /// </param>
        /// <param name="attributes">
        /// The key/value pairs of the <see cref="AttributeSet"/>
        /// </param>
        /// <returns>
        /// A <see cref="AttributeSet"/> with the specified name populated with the given key/value pairs.
        /// </returns>
        public static AttributeSet CreateNewDefinedAttributeSet(
            AttributeSetName name,
            SortedDictionary<string, object> attributes)
        {
            return new AttributeSet
            {
                Name = GetNameFromEnum(name),
                Attributes = attributes
            };
        }

        /// <summary>
        /// Retrieves the <see cref="System.String"/> name from the specified enumeration.
        /// </summary>
        /// <param name="value">
        /// The enumeration for which the string representation should be retrieved.
        /// </param>
        /// <returns>
        /// The <see cref="System.String"/> representation of the specified enumeration value.
        /// </returns>
        public static string GetNameFromEnum(AttributeSetName value)
        {
            Type type = value.GetType();

            FieldInfo fieldInfo = type.GetField(value.ToString());

            var attributes = fieldInfo.GetCustomAttributes(
                typeof(TextualRepresentationAttribute), false) as TextualRepresentationAttribute[];

            return attributes != null && attributes.Length > 0 ? attributes[0].Text : value.ToString();
        }
    }
}
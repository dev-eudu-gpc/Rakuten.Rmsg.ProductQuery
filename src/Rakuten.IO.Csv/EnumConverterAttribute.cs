// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FieldEnumConverterAttribute.cs" company="Rakuten">
//   Copyright (c) Rakuten. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------
namespace Rakuten.IO.Delimited
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// Allows the behavior of how <see cref="Enum"/>s should be handled during serialization to be defined.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public sealed class EnumConverterAttribute : Attribute
    {
        /// <summary>
        /// The cached <see cref="Type"/> of the <see cref="DescriptionAttribute"/>.
        /// </summary>
        private static readonly Type TypeOfDescriptionAttribute = typeof(DescriptionAttribute);

        /// <summary>
        /// The <see cref="Type"/> of the <see cref="Enum"/>.
        /// </summary>
        private readonly Type enumType;

        /// <summary>
        /// A cached collection of <see cref="Enum"/> values indexed by its description.
        /// </summary>
        private readonly Dictionary<string, Enum> enumValuesByString;

        /// <summary>
        /// A cached collection of <see cref="string"/> descriptions indexed by a <see cref="Enum"/> value.
        /// </summary>
        private readonly Dictionary<Enum, string> stringValuesByEnum;

        /// <summary>
        /// Initializes a new instance of the <see cref="EnumConverterAttribute"/> class.
        /// </summary>
        /// <param name="enumType">The <see cref="Enum"/> type to use.</param>
        /// <param name="useDescriptions">
        /// A value identifying whether <see cref="DescriptionAttribute"/>s should be used during conversion.
        /// </param>
        public EnumConverterAttribute(Type enumType, bool useDescriptions = false)
        {
            this.enumType = enumType;

            this.enumValuesByString = useDescriptions
                ? GetEnumValuesByDescription(enumType)
                : GetEnumValuesByValue(enumType);

            this.stringValuesByEnum = this.enumValuesByString
                .ToDictionary(kvp => kvp.Value, kvp => kvp.Key);
        }

        /// <summary>
        /// Returns the appropriate <see cref="Enum"/> value identified by the string representation.
        /// </summary>
        /// <param name="value">The string representation.</param>
        /// <returns>The <see cref="Enum"/> value identified.</returns>
        public object GetEnumValueByString(string value)
        {
            try
            {
                return this.enumValuesByString[value];
            }
            catch (KeyNotFoundException ex)
            {
                throw new InvalidEnumValueException(value, this.enumType, ex);
            }
        }

        /// <summary>
        /// Returns the string representation of a <see cref="Enum"/> value.
        /// </summary>
        /// <param name="value">The <see cref="Enum"/> value.</param>
        /// <returns>The string representation.</returns>
        public string GetStringValueByEnum(Enum value)
        {
            return this.stringValuesByEnum[value];
        }

        /// <summary>
        /// Returns a collection of <see cref="Enum"/> values keyed by their string representation of a given 
        /// <see cref="Enum"/> <see cref="Type"/>.
        /// </summary>
        /// <param name="enumType">The <see cref="Type"/> of the <see cref="Enum"/>.</param>
        /// <returns>A collection of <see cref="Enum"/> values keyed by their string representation.</returns>
        private static Dictionary<string, Enum> GetEnumValuesByValue(Type enumType)
        {
            return Enum
                .GetValues(enumType)
                .Cast<Enum>()
                .ToDictionary(enumValue => enumValue.ToString());
        }

        /// <summary>
        /// Returns a collection of <see cref="Enum"/> values keyed by their string representation defined using a 
        /// <see cref="DescriptionAttribute"/> of a given <see cref="Enum"/> <see cref="Type"/>.
        /// </summary>
        /// <param name="enumType">The <see cref="Type"/> of the <see cref="Enum"/>.</param>
        /// <returns>A collection of <see cref="Enum"/> values keyed by their string representation.</returns>
        private static Dictionary<string, Enum> GetEnumValuesByDescription(Type enumType)
        {
            var descriptions = new Dictionary<string, Enum>();

            foreach (FieldInfo field in enumType.GetFields().Where(f => f.IsStatic))
            {
                var descriptionAttribute = GetCustomAttribute(field, TypeOfDescriptionAttribute) as DescriptionAttribute;

                descriptions.Add(
                    descriptionAttribute != null
                        ? descriptionAttribute.Description
                        : field.Name, 
                    (Enum)field.GetValue(null));
            }

            return descriptions;
        }
    }
}
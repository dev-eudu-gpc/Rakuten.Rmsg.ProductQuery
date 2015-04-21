// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="StringExtensions.cs" company="Rakuten">
//   Copyright (c) Rakuten. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------
namespace Rakuten.IO.Delimited
{
    using System;
    using System.ComponentModel;
    using System.Linq;

    /// <summary>
    /// Defines a set of methods that extend the <see cref="string"/> class.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// A cached type of <see cref="DescriptionAttribute"/>
        /// </summary>
        private static readonly Type TypeOfDescriptionAttribute = typeof(DescriptionAttribute);

        /// <summary>
        /// Returns the <see cref="Enum"/> from <paramref name="enumType"/> that has the specified description.
        /// </summary>
        /// <param name="description">The description.</param>
        /// <param name="enumType">The <see cref="Enum"/> type.</param>
        /// <returns>The <see cref="Enum"/> to return.</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown if enumType is not an <see cref="Enum"/>, or if no single <see cref="Enum"/> description can be found.
        /// </exception>
        public static Enum GetValueFromDescription(this string description, Type enumType)
        {
            if (!enumType.IsEnum)
            {
                throw new InvalidOperationException();
            }

            var fields = 
                from field in enumType.GetFields()
                let attribute =
                    Attribute.GetCustomAttribute(field, TypeOfDescriptionAttribute) as DescriptionAttribute
                where attribute != null && attribute.Description == description
                select field;

            return (Enum)fields.Single().GetValue(null);
        }
    }
}
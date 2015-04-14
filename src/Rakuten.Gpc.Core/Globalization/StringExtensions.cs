//----------------------------------------------------------------------------------------------------------------------
// <copyright file="StringExtensions.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------------------------------------------------
// ReSharper disable once CheckNamespace
// Extension method should be available whenever working with localized strings.
namespace System.Globalization
{
    /// <summary>
    /// Provides extension methods for strings
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Determines whether a given culture name is a valid culture.
        /// </summary>
        /// <param name="cultureName">The culture name to test.</param>
        /// <returns>
        /// <see langword="true"/> if the string is a value culture name; otherwise, <see langword="false"/>.
        /// </returns>
        public static bool IsValidCulture(this string cultureName)
        {
            bool result;
            try
            {
                // ReSharper disable once ObjectCreationAsStatement
                new CultureInfo(cultureName);
                result = true;
            }
            catch (CultureNotFoundException)
            {
                result = false;
            }

            return result;
        }
    }
}
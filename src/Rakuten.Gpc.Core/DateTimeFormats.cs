//----------------------------------------------------------------------------------------------------------------------
// <copyright file="DateTimeFormats.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------------------------------------------------
namespace Rakuten
{
    /// <summary>
    /// Specifies the format of a date/time string.
    /// </summary>
    public static class DateTimeFormats
    {
        /// <summary>
        /// Indicates the default date/time format.
        /// </summary>
        public const string Default = "{0:dd-MMM-yyyy HH:mm:ss}";

        /// <summary>
        /// Indicates the short default date/time format.
        /// </summary>
        public const string ShortDateWithoutSeconds = "{0:dd/MM/yyyy HH:mm}";
    }
}
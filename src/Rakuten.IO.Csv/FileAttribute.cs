// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="FileOptionsAttribute.cs" company="Rakuten">
//   Copyright (c) Rakuten. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------
namespace Rakuten.IO.Delimited
{
    using System;

    using LumenWorks.Framework.IO.Csv;

    /// <summary>
    /// Specifies the options to be used when serializing values.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public sealed class FileAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FileAttribute"/> class.
        /// </summary>
        /// <param name="commentCharacter">The character that denotes a comment.</param>
        /// <param name="delimiterCharacter">The character to be placed between values.</param>
        /// <param name="escapeCharacter">The escape character</param>
        /// <param name="quoteCharacter">The character to be used to wrap values that contain the delimiter.</param>
        /// <param name="fieldWriteOrder">The order in which to write fields..</param>
        /// <param name="valueTrimmingOptions">The value Trimming Options.</param>
        public FileAttribute(
            char commentCharacter = '#',
            char delimiterCharacter = ',',
            char escapeCharacter = '\'',
            char quoteCharacter = '"',
            FieldOrder fieldWriteOrder = FieldOrder.OrderByClass,
            ValueTrimmingOptions valueTrimmingOptions = ValueTrimmingOptions.UnquotedOnly)
        {
            this.CommentCharacter = commentCharacter;
            this.DelimiterCharacter = delimiterCharacter;
            this.EscapeCharacter = escapeCharacter;
            this.QuoteCharacter = quoteCharacter;
            this.FieldWriteOrder = fieldWriteOrder;
            this.ValueTrimmingOptions = valueTrimmingOptions;
        }

        /// <summary>
        /// Gets or sets the character that starts a comment.
        /// </summary>
        public char CommentCharacter { get; set; }

        /// <summary>
        /// Gets or sets the character to be used between values.
        /// </summary>
        public char DelimiterCharacter { get; set; }

        /// <summary>
        /// Gets or sets the escape character.
        /// </summary>
        public char EscapeCharacter { get; set; }

        /// <summary>
        /// Gets or sets the order in which fields should be written.
        /// </summary>
        public FieldOrder FieldWriteOrder { get; set; }

        /// <summary>
        /// Gets or sets the character to be used to wrap values that contain the <see cref="DelimiterCharacter"/>.
        /// </summary>
        public char QuoteCharacter { get; set; }

        /// <summary>
        /// Gets or sets the value trimming options.
        /// </summary>
        public ValueTrimmingOptions ValueTrimmingOptions { get; set; }
    }
}
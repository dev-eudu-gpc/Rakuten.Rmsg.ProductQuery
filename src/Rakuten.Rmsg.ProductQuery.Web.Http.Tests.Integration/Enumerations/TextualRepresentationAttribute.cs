//---------------------------------------------------------------------------------------------------------------------
// <copyright file="TextualRepresentationAttribute.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//---------------------------------------------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.Web.Http.Tests.Integration
{
    using System;

    /// <summary>
    /// Defines the textual representation of an enumeration value.
    /// </summary>
    internal class TextualRepresentationAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TextualRepresentationAttribute"/> class with the given 
        /// text.
        /// </summary>
        /// <param name="text">
        /// The textual representation of the enumeration member.
        /// </param>
        internal TextualRepresentationAttribute(string text)
        {
            this.Text = text;
        }

        /// <summary>
        /// Gets the textual representation of the enumeration member.
        /// </summary>
        internal string Text { get; private set; }
    }
}
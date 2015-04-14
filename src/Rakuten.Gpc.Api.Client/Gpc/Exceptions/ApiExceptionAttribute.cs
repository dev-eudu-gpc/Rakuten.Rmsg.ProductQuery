// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ApiExceptionAttribute.cs" company="Rakuten">
//   Copyright (c) Rakuten. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Rakuten.Gpc
{
    using System;
    using System.Diagnostics.Contracts;

    /// <summary>
    /// An attribute to decorate exceptions that are thrown as to provide a formatted response.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class ApiExceptionAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApiExceptionAttribute"/> class.
        /// </summary>
        public ApiExceptionAttribute()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiExceptionAttribute"/> class.
        /// </summary>
        /// <param name="typeString">
        /// Depicts the type of this error in the form of a URL that will provide further information to the client.
        /// </param>
        /// <param name="title">
        /// A short description of the error to be given to the client.
        /// </param>
        public ApiExceptionAttribute(string typeString, string title)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(typeString));
            Contract.Requires(!string.IsNullOrWhiteSpace(title));

            this.Title = title;

            // This line could cause a UriFormatException to be thrown; we are not catching it here as we want it to
            // bubble up.
            this.Type = new Uri(typeString);
        }

        /// <summary>
        /// Gets or sets the title of this error giving a short description to the client.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets a <see cref="Uri"/> that when followed should lead to a page giving more information about the error 
        /// to the client.
        /// </summary>
        public Uri Type { get; private set; }
    }
}

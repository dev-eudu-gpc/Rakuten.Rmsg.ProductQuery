// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InvalidCategoryException.cs" company="Rakuten">
//   Copyright (c) Rakuten. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Rakuten.Gpc
{
    using System.Xml.Serialization;

    using Newtonsoft.Json;

    /// <summary>
    /// Represents the error raised when an invalid category was specified.
    /// </summary>
    [ApiException("http://problems.rakuten.co.uk/invalid-category", "The category specified was invalid.")]
    public class InvalidCategoryException : ApiException
    {
        /// <summary>
        /// The format of the message that describes this error.
        /// </summary>
        private const string Detail = "The category '{0}' was not recognised.";

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidCategoryException"/> class.
        /// </summary>
        public InvalidCategoryException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidCategoryException"/> class.
        /// </summary>
        /// <param name="name">
        /// The name of the invalid category.
        /// </param>
        public InvalidCategoryException(string name)
            : base(string.Format(Detail, name))
        {
            this.Name = name;
        }

        /// <summary>
        /// Gets or sets the name of the invalid category.
        /// </summary>
        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement("name")]
        public string Name { get; set; }
    }
}

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InvalidCultureCodeException.cs" company="Rakuten">
//   Copyright (c) Rakuten. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Rakuten.Gpc
{
    using System.Xml.Serialization;

    using Newtonsoft.Json;

    /// <summary>
    /// Represents the error raised when a supplied culture code is invalid
    /// </summary>
    [ApiException("http://problems.rakuten.co.uk/invalid-culture-code", "The culture code supplied was invalid.")]
    public class InvalidCultureCodeException : ApiException
    {
        /// <summary>
        /// The message that describes this error.
        /// </summary>
        private const string Detail =
            @"The culture code '{0}' is not a valid culture code.";

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidCultureCodeException"/> class.
        /// </summary>
        public InvalidCultureCodeException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidCultureCodeException"/> class.
        /// </summary>
        /// <param name="value">The value for the identifier that was invalid.</param>
        public InvalidCultureCodeException(string value)
            : base(string.Format(Detail, value))
        {
            this.Value = value;
        }

        /// <summary>
        /// Gets or sets the invalid value.
        /// </summary>
        [JsonProperty("value", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement("value")]
        public string Value { get; set; }
    }
}
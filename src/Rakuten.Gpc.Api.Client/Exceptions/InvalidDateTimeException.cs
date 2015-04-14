// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InvalidDateTimeException.cs" company="Rakuten">
//   Copyright (c) Rakuten. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Rakuten.Gpc
{
    using System.Xml.Serialization;

    using Newtonsoft.Json;

    /// <summary>
    /// Represents the error raised when a supplied value failed to convert to a DateTime.
    /// </summary>
    [ApiException("http://problems.rakuten.co.uk/invalid-datetime", "The date/time supplied was invalid.")]
    public class InvalidDateTimeException : ApiException
    {
        /// <summary>
        /// The message that describes this error.
        /// </summary>
        private const string Detail =
            @"The value '{0}' failed to convert to a DateTime.";

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidDateTimeException"/> class.
        /// </summary>
        public InvalidDateTimeException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidDateTimeException"/> class.
        /// </summary>
        /// <param name="value">The value for the identifier that was invalid.</param>
        public InvalidDateTimeException(string value) : base(string.Format(Detail, value))
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
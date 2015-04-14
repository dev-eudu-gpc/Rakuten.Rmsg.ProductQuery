// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InvalidGtinLengthException.cs" company="Rakuten">
//   Copyright (c) Rakuten. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Rakuten.Gpc
{
    using System.Xml.Serialization;

    using Newtonsoft.Json;

    /// <summary>
    /// Represents the error raised when a supplied GTIN was of an invalid length for the criteria.
    /// </summary>
    [ApiException("http://problems.rakuten.co.uk/invalid-gtin-length", "The GTIN supplied was invalid.")]
    public class InvalidGtinLengthException : ApiException
    {
        /// <summary>
        /// The message that describes this error.
        /// </summary>
        private const string Detail =
            @"The value '{0}' was of an invalid length for the '{1}' identifier. {2}";

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidGtinLengthException"/> class.
        /// </summary>
        public InvalidGtinLengthException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidGtinLengthException"/> class.
        /// </summary>
        /// <param name="name">
        /// The name of the identifier whose specified value was of an invalid length.
        /// </param>
        /// <param name="value">
        /// The value for the identifier that was invalid.
        /// </param>
        /// <param name="criteria">
        /// A string depicting the length criteria for this identifier.
        /// </param>
        public InvalidGtinLengthException(string name, string value, string criteria)
            : base(string.Format(Detail, value, name, criteria))
        {
            this.Name = name;
            this.Value = value;
        }

        /// <summary>
        /// Gets or sets the name of the identifier whose value was of an invalid length.
        /// </summary>
        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement("name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the invalid value.
        /// </summary>
        [JsonProperty("value", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement("value")]
        public string Value { get; set; }
    }
}

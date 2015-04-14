// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="InvalidMetadataValueException.cs" company="Rakuten">
//   Copyright (c) Rakuten. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------
namespace Rakuten.Gpc
{
    using System.Xml.Serialization;

    using Newtonsoft.Json;

    /// <summary>
    /// Represents the error raised when an invalid category was specified.
    /// </summary>
    [ApiException("http://problems.rakuten.co.uk/invalid-metadata-value", "The value specified was not valid.")]
    public class InvalidMetadataValueException : ApiException
    {
        /// <summary>
        /// The format of the message that describes this error.
        /// </summary>
        private const string Detail = "'{0}' was not recognised as a valid value for '{1}'.";

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidMetadataValueException"/> class.
        /// </summary>
        public InvalidMetadataValueException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidMetadataValueException"/> class.
        /// </summary>
        /// <param name="field">
        /// The name of the field for which the invalid value was supplied.
        /// </param>
        /// <param name="value">
        /// The invalid value supplied for the field.
        /// </param>
        public InvalidMetadataValueException(string field, string value)
            : base(string.Format(Detail, value, field))
        {
            this.Field = field;
            this.Value = value;
        }

        /// <summary>
        /// Gets or sets the field for which an invalid value was supplied.
        /// </summary>
        [JsonProperty("field", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement("field")]
        public string Field { get; set; }

        /// <summary>
        /// Gets or sets the invalid value supplied.
        /// </summary>
        [JsonProperty("value", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement("value")]
        public string Value { get; set; }
    }
}

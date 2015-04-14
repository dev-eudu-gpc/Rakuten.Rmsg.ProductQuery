// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InvalidAttributeValueException.cs" company="Rakuten">
//   Copyright (c) Rakuten. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Rakuten.Gpc
{
    using System.Xml.Serialization;

    using Newtonsoft.Json;

    /// <summary>
    /// Represents the error raised when an attempt was made to set the value of a products attribute to an invalid 
    /// value for the attributes constraints.
    /// </summary>
    [ApiException(
        "http://problems.rakuten.co.uk/invalid-attribute-value", 
        "An invalid value was specified for the attribute.")]
    public class InvalidAttributeValueException : ApiException
    {
        /// <summary>
        /// The format of the message that describes this error.
        /// </summary>
        private const string Detail = "The value '{0}' was invalid for the constraints specified on the '{1}' attribute.";

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidAttributeValueException"/> class.
        /// </summary>
        public InvalidAttributeValueException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidAttributeValueException"/> class.
        /// </summary>
        /// <param name="attribute">
        /// The attribute that had an invalid value specified.
        /// </param>
        /// <param name="value">
        /// The invalid value that was supplied for the attribute.
        /// </param>
        public InvalidAttributeValueException(string attribute, string value)
            : base(string.Format(Detail, value, attribute))
        {
            this.Attribute = attribute;
            this.Value = value;
        }

        /// <summary>
        /// Gets or sets the attribute that has the constraints.
        /// </summary>
        [JsonProperty("attribute", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement("attribute")]
        public string Attribute { get; set; }

        /// <summary>
        /// Gets or sets the invalid value that was specified for the attribute.
        /// </summary>
        [JsonProperty("value", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement("value")]
        public string Value { get; set; }
    }
}

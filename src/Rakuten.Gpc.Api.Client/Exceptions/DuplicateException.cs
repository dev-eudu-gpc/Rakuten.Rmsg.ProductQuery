// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DuplicateException.cs" company="Rakuten">
//   Copyright (c) Rakuten. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Rakuten.Gpc
{
    using System.Xml.Serialization;

    using Newtonsoft.Json;

    /// <summary>
    /// Represents an error response when a value (e.g. manufacturer name) already exists.
    /// </summary>
    [ApiException("http://problems.rakuten.co.uk/duplicate-entity", "The value supplied already exists.")]
    public class DuplicateException : ApiException
    {
        /// <summary>
        /// The format of the message that describes this error.
        /// </summary>
        private const string Detail = "{0} with {1} '{2}' already exists.";

        /// <summary>
        /// Initializes a new instance of the <see cref="DuplicateException"/> class.
        /// </summary>
        public DuplicateException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DuplicateException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public DuplicateException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DuplicateException"/> class.
        /// </summary>
        /// <param name="objectName">The object name which was supplied in the request.</param>
        /// <param name="field">The field which was supplied in the request.</param>
        /// <param name="value">The value which was supplied in the request that already exists.</param>
        public DuplicateException(string objectName, string field, string value)
            : base(string.Format(Detail, objectName, field, value))
        {
            this.Object = objectName;
            this.Field = field;
            this.Value = value;
        }

        /// <summary>
        /// Gets or sets the object name which was supplied in the request.
        /// </summary>
        [JsonProperty("object", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement("object")]
        public string Object { get; set; }

        /// <summary>
        /// Gets or sets the field which was supplied in the request.
        /// </summary>
        [JsonProperty("field", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement("field")]
        public string Field { get; set; }

        /// <summary>
        /// Gets or sets the value which was supplied in the request that already exists.
        /// </summary>
        [JsonProperty("value", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement("value")]
        public string Value { get; set; }
    }
}

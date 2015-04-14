// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PreviousAttributeMissingException.cs" company="Rakuten">
//   Copyright (c) Rakuten. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Rakuten.Gpc
{
    using System.Xml.Serialization;

    using Newtonsoft.Json;

    /// <summary>
    /// Represents the error raised when an attempt was made to set the value of an attribute was made but values were 
    /// not supplied for related attributes.
    /// </summary>
    [ApiException(
        "http://problems.rakuten.co.uk/previous-attribute-missing",
        "The attribute previous to the identified was missing.")]
    public class PreviousAttributeMissingException : ApiException
    {
        /// <summary>
        /// The format of the message that describes this error.
        /// </summary>
        private const string Detail = "Values for related attributes to the '{0}' attribute were not supplied.";

        /// <summary>
        /// Initializes a new instance of the <see cref="PreviousAttributeMissingException"/> class.
        /// </summary>
        public PreviousAttributeMissingException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PreviousAttributeMissingException"/> class.
        /// </summary>
        /// <param name="attribute">
        /// The attribute with related attributes.
        /// </param>
        public PreviousAttributeMissingException(string attribute)
            : base(string.Format(Detail, attribute))
        {
            this.Attribute = attribute;
        }

        /// <summary>
        /// Gets or sets the attribute that has the constraints.
        /// </summary>
        [JsonProperty("attribute", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement("attribute")]
        public string Attribute { get; set; }
    }
}

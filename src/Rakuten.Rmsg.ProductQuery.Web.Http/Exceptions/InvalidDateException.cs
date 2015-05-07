// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InvalidDateException.cs" company="Rakuten">
//   Copyright (c) Rakuten. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.Web.Http
{
    using System.Xml.Serialization;

    using Newtonsoft.Json;
    using Rakuten.Gpc;

    /// <summary>
    /// Represents the error raised when a parameter contains what is expected to be date
    /// but was unable to be parsed as one
    /// </summary>
    [ApiException("http://problems.rakuten.com/invalid-request-parameter", "An invalid request parameter was supplied.")]
    internal class InvalidDateException : ApiException
    {
        /// <summary>
        /// The message that describes this error.
        /// </summary>
        private const string Detail =
            @"The {0} '{1}' in the request URI is invalid. It must be a valid ate.";

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidDateException"/> class.
        /// </summary>
        public InvalidDateException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidDateException"/> class.
        /// </summary>
        /// <param name="parameterName">The name of the parameter that was invalid.</param>
        /// <param name="value">The value for the identifier that was invalid.</param>
        public InvalidDateException(string parameterName, string value)
            : base(string.Format(Detail, parameterName, value))
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
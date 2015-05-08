// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InvalidGuidException.cs" company="Rakuten">
//   Copyright (c) Rakuten. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.Web.Http
{
    using System.Xml.Serialization;

    using Newtonsoft.Json;
    using Rakuten.Gpc;

    /// <summary>
    /// Represents the error raised when a URI part that was expected to be a GUID
    /// was unable to be parsed as one.
    /// </summary>
    [ApiException("http://problems.rakuten.com/invalid-request-parameter", "An invalid request parameter was supplied.")]
    internal class InvalidGuidException : ApiException
    {
        /// <summary>
        /// The message that describes this error.
        /// </summary>
        private const string Detail =
            @"The {0} '{1}' in the request URI is invalid. It must be a GUID.";

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidGuidException"/> class.
        /// </summary>
        public InvalidGuidException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidGuidException"/> class.
        /// </summary>
        /// <param name="parameterName">The name of the parameter that was invalid.</param>
        /// <param name="id">The value for the identifier that was invalid.</param>
        public InvalidGuidException(string parameterName, string id)
            : base(string.Format(Detail, parameterName, id))
        {
            this.Id = id;
        }

        /// <summary>
        /// Gets or sets the invalid value.
        /// </summary>
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement("id")]
        public string Id { get; set; }
    }
}
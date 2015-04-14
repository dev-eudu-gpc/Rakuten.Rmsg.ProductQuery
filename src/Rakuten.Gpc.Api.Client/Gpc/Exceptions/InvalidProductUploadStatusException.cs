// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InvalidProductUploadStatusException.cs" company="Rakuten">
//   Copyright (c) Rakuten. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Rakuten.Gpc
{
    using System.Xml.Serialization;

    using Newtonsoft.Json;

    /// <summary>
    /// Represents the error raised when a supplied product upload status was not valid.
    /// </summary>
    [ApiException(
        "http://problems.rakuten.com/invalid-product-upload-status", 
        "An invalid product upload status was supplied.")]
    public class InvalidProductUploadStatusException : ApiException
    {
        /// <summary>
        /// The message that describes this error.
        /// </summary>
        private const string Detail =
            @"You attempted to update the status of product upload '{0}' to '{1}', which is an invalid status. The only valid status to which this file can be updated is 'submitted'.";

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidProductUploadStatusException"/> class.
        /// </summary>
        public InvalidProductUploadStatusException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidProductUploadStatusException"/> class.
        /// </summary>
        /// <param name="filename">
        /// The name of the file against which an attempt was made to change the status to an invalid value.
        /// </param>
        /// <param name="status">
        /// The invalid status value.
        /// </param>
        public InvalidProductUploadStatusException(string filename, string status)
            : base(string.Format(Detail, filename, status))
        {
            this.Filename = filename;
        }

        /// <summary>
        /// Gets or sets the name of the file against which an attempt was made to change the status.
        /// </summary>
        [JsonProperty("filename", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement("filename")]
        public string Filename { get; set; }
    }
}

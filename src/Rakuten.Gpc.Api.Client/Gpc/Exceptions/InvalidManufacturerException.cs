// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InvalidManufacturerException.cs" company="Rakuten">
//   Copyright (c) Rakuten. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Rakuten.Gpc
{
    using System.Xml.Serialization;

    using Newtonsoft.Json;

    /// <summary>
    /// Represents the error raised when a manufacturer part number cannot be processed without a valid manufacturer..
    /// </summary>
    [ApiException("http://problems.rakuten.co.uk/invalid-manufacturer", "The manufacturer specified was invalid.")]
    public class InvalidManufacturerException : ApiException
    {
        /// <summary>
        /// The format of the message that describes this error.
        /// </summary>
        private const string Detail = "Manufacturer part number '{0}' cannot be processed without a valid manufacturer.";

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidManufacturerException"/> class.
        /// </summary>
        public InvalidManufacturerException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidManufacturerException"/> class.
        /// </summary>
        /// <param name="manufacturerPartNumber">
        /// The name of the invalid category.
        /// </param>
        public InvalidManufacturerException(string manufacturerPartNumber)
            : base(string.Format(Detail, manufacturerPartNumber))
        {
            this.ManufacturerPartNumber = manufacturerPartNumber;
        }

        /// <summary>
        /// Gets or sets the manufacturer part number that has an invalid corresponding manufacturer.
        /// </summary>
        [JsonProperty("manufacturerPartNumber", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement("manufacturerPartNumber")]
        public string ManufacturerPartNumber { get; set; }
    }
}

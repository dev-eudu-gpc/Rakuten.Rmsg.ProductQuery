// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="ManufacturerNotFoundException.cs" company="Rakuten">
//   Copyright (c) Rakuten. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------
namespace Rakuten.Gpc
{
    using System.Xml.Serialization;

    using Newtonsoft.Json;

    /// <summary>
    /// Represents the error raised when a search for a manufacturer via id and culture returned no manufacturer.
    /// </summary>
    [ApiException("http://problems.rakuten.co.uk/manufacturer-not-found", "The requested manufacturer was not found.")]
    public class ManufacturerNotFoundException : ApiException
    {
        /// <summary>
        /// The format of the message that describes this error.
        /// </summary>
        private const string Detail = "A manufacturer identified by Id '{0}' with a culture of '{1}' was not found.";

        /// <summary>
        /// Initializes a new instance of the <see cref="ManufacturerNotFoundException"/> class.
        /// </summary>
        public ManufacturerNotFoundException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ManufacturerNotFoundException"/> class.
        /// </summary>
        /// <param name="id">The Id of the manufacturer</param>
        /// <param name="culture">The culture requested.</param>
        public ManufacturerNotFoundException(string id, string culture)
            : base(string.Format(Detail, id, culture))
        {
            this.Culture = culture;
            this.Id = id;
        }

        /// <summary>
        /// Gets or sets the culture which was supplied in the request.
        /// </summary>
        [JsonProperty("culture", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement("culture")]
        public string Culture { get; set; }

        /// <summary>
        /// Gets or sets the Id which was supplied in the request that could not be found.
        /// </summary>
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement("id")]
        public string Id { get; set; }
    }
}

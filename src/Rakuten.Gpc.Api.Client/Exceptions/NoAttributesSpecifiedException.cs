// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NoAttributesSpecifiedException.cs" company="Rakuten">
//   Copyright (c) Rakuten. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Rakuten.Gpc
{
    using System.Xml.Serialization;

    using Newtonsoft.Json;

    /// <summary>
    /// Represents the error raised when an attempt was made to save a product with 
    /// an attribute set that requires at least one attribute to have a value but none have
    /// </summary>
    [ApiException(
        "http://problems.rakuten.co.uk/no-attributes-specified",
        "At least one attribute needs a value for a particular attribute set.")]
    public class NoAttributesSpecifiedException : ApiException
    {
        /// <summary>
        /// The format of the message that describes this error.
        /// </summary>
        private const string Detail = "At least one attribute needs a value for the attribute set '{0}'.";

        /// <summary>
        /// Initializes a new instance of the <see cref="NoAttributesSpecifiedException"/> class.
        /// </summary>
        public NoAttributesSpecifiedException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NoAttributesSpecifiedException"/> class.
        /// </summary>
        /// <param name="attributeSetName">
        /// The name of the attribute set to which the exception pertains.
        /// </param>
        public NoAttributesSpecifiedException(string attributeSetName) : base(string.Format(Detail, attributeSetName))
        {
            this.AttributeSetName = attributeSetName;
        }

        /// <summary>
        /// Gets or sets the name of the attribute set to which the exception pertains,
        /// </summary>
        [JsonProperty("attributeSetName", NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement("attributeSetName")]
        public string AttributeSetName { get; set; }
    }
}

//---------------------------------------------------------------------------------------------------------------------
// <copyright file="ProductUpload.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//---------------------------------------------------------------------------------------------------------------------
namespace Rakuten.Gpc.Resources
{
    using System.Xml.Serialization;

    /// <summary>
    /// Represents a bulk upload of products.
    /// </summary>
    [XmlType("upload")]
    public class ProductUpload
    {
        /// <summary>
        /// Gets or sets the endpoint to which the file can be uploaded. The value includes the SAS token which is 
        /// unique for the file.
        /// </summary>
        [XmlElement("enclosure")]
        public string Enclosure { get; set; }

        /// <summary>
        /// Gets or sets the name of the file being uploaded.
        /// </summary>
        [XmlElement("filename")]
        public string Filename { get; set; }

        /// <summary>
        /// Gets or sets the description of the file being uploaded.
        /// </summary>
        [XmlElement("description")]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the uploader of the file being uploaded.
        /// </summary>
        [XmlElement("uploader")]
        public string Uploader { get; set; }

        /// <summary>
        /// Gets or sets the endpoint at which the status of this upload can be monitored for its progress.
        /// </summary>
        [XmlElement("monitor")]
        public string Monitor { get; set; }

        /// <summary>
        /// Gets or sets the status of this upload.
        /// </summary>
        [XmlElement("status")]
        public string Status { get; set; }

        /// <summary>
        /// Gets or sets the id of this upload.
        /// </summary>
        [XmlElement("id")]
        public string Id { get; set; }
    }
}

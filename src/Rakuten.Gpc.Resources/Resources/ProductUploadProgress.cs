//---------------------------------------------------------------------------------------------------------------------
// <copyright file="ProductUploadProgress.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//---------------------------------------------------------------------------------------------------------------------
namespace Rakuten.Gpc.Resources
{
    using System;
    using System.Xml.Serialization;

    /// <summary>
    /// Represents the details of a product upload as it is processed.
    /// </summary>
    [XmlType("productUploadProgress")]
    public class ProductUploadProgress
    {
        /// <summary>
        /// Gets or sets the endpoint at which the data of an upload can be retrieved.
        /// </summary>
        public string ContentEndpoint { get; set; }

        /// <summary>
        /// Gets or sets the date and time at which the upload was received.
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Gets or sets the description of the upload
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the period of time for which this upload has been processing.
        /// </summary>
        public string Duration { get; set; }

        /// <summary>
        /// Gets or sets the name of the file that this upload represents.
        /// </summary>
        public string Filename { get; set; }

        /// <summary>
        /// Gets or sets the endpoint at which an output of an uploads process can be retrieved.
        /// </summary>
        public string LogEndpoint { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the product upload
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets a percentage value representing how much of this upload has been processed.
        /// </summary>
        public decimal? PercentComplete { get; set; }

        /// <summary>
        /// Gets or sets the date and time at which the upload was started.
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// Gets or sets the status of this upload.
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Gets or sets a summary of this product upload.
        /// </summary>
        public ProductUploadSummary Summary { get; set; }

        /// <summary>
        /// Gets or sets the endpoint at which a summary of an uploads process can be retrieved.
        /// </summary>
        public string SummaryEndpoint { get; set; }

        /// <summary>
        /// Gets or sets the entity 
        /// </summary>
        public string Uploader { get; set; }
    }
}

//---------------------------------------------------------------------------------------------------------------------
// <copyright file="ProductUploadSummary.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//---------------------------------------------------------------------------------------------------------------------
namespace Rakuten.Gpc.Resources
{
    /// <summary>
    /// Represents a summary of a product upload as it is processed.
    /// </summary>
    public class ProductUploadSummary
    {
        /// <summary>
        /// Gets or sets the number of products created from the data submitted in this upload.
        /// </summary>
        public int Created { get; set; }

        /// <summary>
        /// Gets or sets the number of failed products from the data submitted in this upload.
        /// </summary>
        public int Failed { get; set; }

        /// <summary>
        /// Gets or sets the number of products processed from the data submitted in this upload.
        /// </summary>
        public int Processed { get; set; }

        /// <summary>
        /// Gets or sets the number of succeeded products from the data submitted in this upload.
        /// </summary>
        public int Succeeded { get; set; }

        /// <summary>
        /// Gets or sets the number of products un-changed from the data submitted in this upload.
        /// </summary>
        public int Unchanged { get; set; }

        /// <summary>
        /// Gets or sets the number of products updated from the data submitted in this upload.
        /// </summary>
        public int Updated { get; set; }
    }
}

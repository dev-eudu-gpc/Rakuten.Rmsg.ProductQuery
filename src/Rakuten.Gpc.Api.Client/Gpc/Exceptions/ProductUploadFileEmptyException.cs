// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="ProductUploadFileEmptyException.cs" company="Rakuten">
//   Copyright (c) Rakuten. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------
namespace Rakuten.Gpc
{
    /// <summary>
    /// Represents the error raised when an attempt was made to upload an empty file 
    /// </summary>
    [ApiException("http://problems.rakuten.co.uk/product-upload-file-is-empty", "The product upload file is empty.")]
    public class ProductUploadFileEmptyException : ApiException
    {
        /// <summary>
        /// The format of the message that describes this error.
        /// </summary>
        private const string Detail = "The product upload file is empty and therefore not valid for processing.";

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductUploadFileEmptyException"/> class.
        /// </summary>
        public ProductUploadFileEmptyException()
            : base(Detail)
        {
        }
    }
}

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProductUploadNotFoundException.cs" company="Rakuten">
//   Copyright (c) Rakuten. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Rakuten.Gpc
{
   /// <summary>
    /// Represents the error raised when an attempt was made to retrieve or modify a product upload that does not 
    /// exist.
    /// </summary>
    [ApiException("http://problems.rakuten.co.uk/product-upload-not-found", "The product upload was not found.")]
    public class ProductUploadNotFoundException : ApiException
    {
        /// <summary>
        /// The format of the message that describes this error.
        /// </summary>
        private const string Detail = "A product upload with the name '{0}' could not be found.";

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductUploadNotFoundException"/> class.
        /// </summary>
        public ProductUploadNotFoundException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductUploadNotFoundException"/> class.
        /// </summary>
        /// <param name="filename">
        /// The filename of the product upload that could not be found.
        /// </param>
        public ProductUploadNotFoundException(string filename)
            : base(string.Format(Detail, filename))
        {
        }
    }
}

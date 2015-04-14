// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProductValidationFailedException.cs" company="Rakuten">
//   Copyright (c) Rakuten. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Rakuten.Gpc
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Represents the error raised when a search for a product via GRAN returned no product.
    /// </summary>
    [ApiException("http://problems.rakuten.co.uk/product-validation-failed", "The product failed validation.")]
    public class ProductValidationFailedException : ApiAggregateException
    {
        /// <summary>
        /// The message that describes this error.
        /// </summary>
        private const string Detail =
            @"The product did not successfully pass validation; see inner exceptions for further details.";

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductValidationFailedException"/> class.
        /// </summary>
        public ProductValidationFailedException() 
            : base(Detail)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductValidationFailedException"/> class.
        /// </summary>
        /// <param name="innerExceptions">
        /// The exceptions that are the cause of the current exception.
        /// </param>
        public ProductValidationFailedException(IEnumerable<Exception> innerExceptions)
            : base(Detail, innerExceptions)
        {
        }
    }
}

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ManufacturerValidationFailedException.cs" company="Rakuten">
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
    [ApiException("http://problems.rakuten.co.uk/manufacturer-validation-failed", "The manufacturer failed validation.")]
    public class ManufacturerValidationFailedException : ApiAggregateException
    {
        /// <summary>
        /// The message that describes this error. Currently the only validation is done on the manufacturer name being present and occurs before calling the core API 
        /// which does no validation. When validation is done in the core API then this should hold a list of exceptions like the ProductValidationFailedException
        /// </summary>
        private const string Detail =
            @"The manufacturer did not successfully pass validation; {0}";

        /// <summary>
        /// The default error.
        /// </summary>
        private const string DefaultError = "Manufacturer Name empty";

        /// <summary>
        /// Initializes a new instance of the <see cref="ManufacturerValidationFailedException"/> class.
        /// </summary>
        public ManufacturerValidationFailedException() 
            : base(string.Format(Detail, DefaultError))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ManufacturerValidationFailedException"/> class.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        public ManufacturerValidationFailedException(string message)
            : base(string.Format(Detail, message))
        {
        }
    }
}

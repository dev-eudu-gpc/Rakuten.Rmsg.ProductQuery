// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DuplicateIdentifierException.cs" company="Rakuten">
//   Copyright (c) Rakuten. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Rakuten.Gpc
{
    /// <summary>
    /// Represents an error where an attempt was made to add a product which specified one or more identifiers that 
    /// were already in use.
    /// </summary>
    [ApiException(
        "http://problems.rakuten.co.uk/duplicate-identifier", 
        "One or more identifiers are duplicate.")]
    public class DuplicateIdentifierException : ApiException
    {
        /// <summary>
        /// The format of the message that describes this error.
        /// </summary>
        private const string Detail = "One or more GTINs specified are already in use.";

        /// <summary>
        /// Initializes a new instance of the <see cref="DuplicateIdentifierException"/> class.
        /// </summary>
        public DuplicateIdentifierException()
            : base(Detail)
        {
        }
    }
}

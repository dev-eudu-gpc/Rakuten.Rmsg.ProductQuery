// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MissingIdentifierException.cs" company="Rakuten">
//   Copyright (c) Rakuten. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Rakuten.Gpc
{
    /// <summary>
    /// Represents the error raised when no valid identifiers were specified.
    /// </summary>
    [ApiException("http://problems.rakuten.co.uk/missing-identifier", "No identifiers were specified.")]
    public class MissingIdentifierException : ApiException
    {
        /// <summary>
        /// The message that describes this error.
        /// </summary>
        private const string Detail =
            @"No identifiers were specified for the product; at least one identifier must be specified.";

        /// <summary>
        /// Initializes a new instance of the <see cref="MissingIdentifierException"/> class.
        /// </summary>
        public MissingIdentifierException() : base(Detail)
        {
        }
    }
}

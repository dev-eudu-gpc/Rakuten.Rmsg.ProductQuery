// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MissingManufacturerException.cs" company="Rakuten">
//   Copyright (c) Rakuten. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Rakuten.Gpc
{
    /// <summary>
    /// Represents an error raised when a manufacturer part number was specified but manufacturer was not.
    /// </summary>
    [ApiException("http://problems.rakuten.co.uk/missing-manufacturer", "No manufacturer was specified.")]
    public class MissingManufacturerException : ApiException
    {
        /// <summary>
        /// The message that describes this error.
        /// </summary>
        private const string Detail =
            @"A manufacturer part number was specified but a manufacturer was not; you must supply a manufacturer if specifying a manufacturer part number.";

        /// <summary>
        /// Initializes a new instance of the <see cref="MissingManufacturerException"/> class.
        /// </summary>
        public MissingManufacturerException() : base(Detail)
        {
        }
    }
}

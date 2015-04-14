// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MissingUploaderException.cs" company="Rakuten">
//   Copyright (c) Rakuten. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Rakuten.Gpc
{
    /// <summary>
    /// Represents the error raised when no uploader was specified.
    /// </summary>
    [ApiException("http://problems.rakuten.co.uk/missing-uploader", "No uploader was specified.")]
    public class MissingUploaderException : ApiException
    {
        /// <summary>
        /// The message that describes this error.
        /// </summary>
        private const string Detail =
            @"The field 'Uploader' is mandatory but one was not specified. Please specify an uploader.";

        /// <summary>
        /// Initializes a new instance of the <see cref="MissingUploaderException"/> class.
        /// </summary>
        public MissingUploaderException()
            : base(Detail)
        {
        }
    }
}
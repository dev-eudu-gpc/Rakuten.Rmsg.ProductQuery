// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="DownloadFileCommand.cs" company="Rakuten">
//   Copyright (c) Rakuten. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.WebJob
{
    using System;
    using System.Diagnostics.Contracts;
    using System.IO;
    using System.Threading.Tasks;

    /// <summary>
    /// Reads the Cloud Blob from the specified container identified by the URI to the given stream.
    /// </summary>
    internal class DownloadFileCommand
    {
        /// <summary>
        /// Reads a Cloud Blob identified by the URI from the given container to a <see cref="Stream"/>.
        /// </summary>
        /// <param name="downloadBlob">A delegate that will download the specified blob to the supplied stream.</param>
        /// <param name="message">An instance representing the queue message to be processed.</param>
        /// <param name="stream">The <see cref="Stream"/> to which the contents of the Blob should be read.</param>
        /// <returns>A <see cref="Task"/> the represents the asynchronous operation.</returns>
        public static async Task<Stream> Execute(
            Func<string, Stream, Task<Stream>> downloadBlob, 
            Message message, 
            Stream stream)
        {
            Contract.Requires(message != null);
            Contract.Requires(stream != null);

            var enclosure = new Uri(message.Link.Target);

            if (enclosure.Segments.Length < 3)
            {
                throw new InvalidOperationException("The given blob reference was not of the correct format.");
            }

            // Parse the blob name from the URI.
            var filename = string.Format(
                "{0}/{1}", 
                enclosure.Segments[enclosure.Segments.Length - 2],
                enclosure.Segments[enclosure.Segments.Length - 1]);

            return await downloadBlob(filename, stream);
        }
    }
}
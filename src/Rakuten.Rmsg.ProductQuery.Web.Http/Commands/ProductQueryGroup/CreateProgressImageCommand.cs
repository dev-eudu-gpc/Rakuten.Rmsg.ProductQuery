//------------------------------------------------------------------------------
// <copyright file="CreateProgressImageCommand.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.Web.Http.Commands
{
    using System;
    using System.Diagnostics.Contracts;
    using System.IO;
    using System.Threading.Tasks;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using Rakuten.Rmsg.ProductQuery.Configuration;

    /// <summary>
    /// A command that creates an image that represents the progress of
    /// product queries within a given product query group.
    /// </summary>
    public class CreateProgressImageCommand : AsyncCommand<CreateProgressImageCommandParameters, Stream>
    {
        /// <summary>
        /// The context under which this instance is operating.
        /// </summary>
        private readonly IApiContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateProgressImageCommand"/> class
        /// </summary>
        /// <param name="context">The context in which this instance is running.</param>
        public CreateProgressImageCommand(IApiContext context)
        {
            Contract.Requires(context != null);

            this.context = context;
        }

        /// <summary>
        /// Gets the status of product queries object.
        /// </summary>
        /// <param name="parameters">The input parameters required to build the image.</param>
        /// <returns>A task that creates an image representing the progress of a given query group.</returns>
        public override async Task<Stream> ExecuteAsync(CreateProgressImageCommandParameters parameters)
        {
            Contract.Requires(parameters != null);

            return await Task.Run(() =>
            {
                // Construct an image for the progress map
                var edgeLength = (int)Math.Ceiling(Math.Sqrt(this.context.MaximumQueriesPerGroup));
                var pixels = new byte[edgeLength * edgeLength];

                // Define the image palette
                var myPalette = BitmapPalettes.Gray256;

                // Set the pixel for each product query in the progress map
                foreach (var productQuery in parameters.ProgressMap)
                {
                    pixels[productQuery.Index - 1] = (byte)(productQuery.PercentageComplete * 255m);
                }

                // Create the image
                var image = BitmapSource.Create(
                    edgeLength,
                    edgeLength,
                    96,
                    96,
                    PixelFormats.Indexed8,
                    myPalette,
                    pixels,
                    edgeLength);

                // Push the image to a stream
                var stream = new MemoryStream();
                var encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(image));
                encoder.Save(stream);

                // Return the stream
                return stream;
            });
        }
    }
}
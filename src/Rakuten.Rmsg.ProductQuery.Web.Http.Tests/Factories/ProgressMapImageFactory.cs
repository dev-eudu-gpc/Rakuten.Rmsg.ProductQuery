//----------------------------------------------------------------------------------------------------------------------
// <copyright file="ProgressMapImageFactory.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.Web.Http.Tests
{
    using System.IO;
    using System.Windows;
    using System.Windows.Media.Imaging;

    /// <summary>
    /// Provides functionality for working with progress map images for test purposes.
    /// </summary>
    public static class ProgressMapImageFactory
    {
        /// <summary>
        /// Converts a progress map image into an array of bytes containing the
        /// percentage complete for each product query.
        /// </summary>
        /// <param name="image">The progress map image to convert.</param>
        /// <returns>An array of bytes containing the percentage complete for each product query.</returns>
        public static byte[] ImageStreamToBytes(Stream image)
        {
            var bi = new BitmapImage();
            bi.BeginInit();
            bi.StreamSource = image;
            bi.EndInit();

            var pixelData = new byte[bi.PixelWidth * bi.PixelHeight];

            bi.CopyPixels(Int32Rect.Empty, pixelData, bi.PixelWidth, 0);

            return pixelData;
        }

        /// <summary>
        /// Converts a progress map image into an array of bytes containing the
        /// percentage complete for each product query.
        /// </summary>
        /// <param name="image">The progress map image to convert.</param>
        /// <returns>An array of bytes containing the percentage complete for each product query.</returns>
        public static BitmapImage ImageStreamToImage(Stream image)
        {
            var bi = new BitmapImage();
            bi.BeginInit();
            bi.StreamSource = image;
            bi.EndInit();

            return bi;
        }
    }
}
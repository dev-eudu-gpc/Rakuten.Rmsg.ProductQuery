//---------------------------------------------------------------------------------------------------------------------
// <copyright file="GtinFactory.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//---------------------------------------------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.Web.Http.Tests.Integration
{
    using System;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Helpers for creating GTINs.
    /// </summary>
    public static class GtinFactory
    {
        /// <summary>
        /// An instance of <see cref="System.Random"/> seeded from the current time.
        /// </summary>
        private static readonly Random Random = new Random((int)DateTime.Now.Ticks);

        /// <summary>
        /// Creates a new valid EAN.
        /// </summary>
        /// <returns>
        /// A valid EAN.
        /// </returns>
        public static string CreateEan()
        {
            string ean = "00" + CreateRandomNumericString(10);

            return ean + CreateChecksum(ean);
        }

        /// <summary>
        /// Creates a new valid ISBN.
        /// </summary>
        /// <returns>
        /// A valid ISBN.
        /// </returns>
        public static string CreateIsbn()
        {
            string isbn = "978" + CreateRandomNumericString(9);

            return isbn + CreateChecksum(isbn);
        }

        /// <summary>
        /// Creates a new valid JAN.
        /// </summary>
        /// <returns>
        /// A valid JAN.
        /// </returns>
        public static string CreateJan()
        {
            string jan = "45" + CreateRandomNumericString(10);

            return jan + CreateChecksum(jan);
        }

        /// <summary>
        /// Creates a new valid UPC.
        /// </summary>
        /// <returns>
        /// A valid UPC.
        /// </returns>
        public static string CreateUpc()
        {
            string upc = CreateRandomNumericString(11);

            return upc + CreateChecksum(upc);
        }

        /// <summary>
        /// Creates a new string of digits to the specified length.
        /// </summary>
        /// <param name="length">
        /// The length to which the string should be created.
        /// </param>
        /// <returns>
        /// A string of digits.
        /// </returns>
        public static string CreateRandomNumericString(int length)
        {
            var builder = new StringBuilder();

            for (int i = 0; i < length; i++)
            {
                builder.Append(Random.Next(9));
            }

            return builder.ToString();
        }

        /// <summary>
        /// Creates a checksum for the given string.
        /// </summary>
        /// <param name="value">
        /// The string for which the checksum should be generated.
        /// </param>
        /// <returns>
        /// The generated checksum.
        /// </returns>
        private static int CreateChecksum(string value)
        {
            // the weights need to be adjusted such that the validating digit is multiplied by 1
            var weights =
                value.Length % 2 == 0
                    ? new[] { 1, 3 }
                    : new[] { 3, 1 };

            int sum = value
                .ToArray()
                .Select(
                    digit => digit - '0')
                .Select(
                    (digit, idx) => digit * weights[idx % 2])
                .Sum();

            int checksum = sum % 10;

            return checksum == 0 ? 0 : 10 - checksum;
        }
    }
}
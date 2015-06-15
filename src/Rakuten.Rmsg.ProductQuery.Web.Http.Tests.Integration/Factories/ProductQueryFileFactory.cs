//------------------------------------------------------------------------------
// <copyright file="ProductQueryFileFactory.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.Web.Http.Tests.Integration
{
    using System.Collections.Generic;
    using System.IO;

    /// <summary>
    /// Factory methods for items in a product query file.
    /// </summary>
    public static class ProductQueryFileFactory
    {
        /// <summary>
        /// Creates a file containing the specified EANs.
        /// </summary>
        /// <param name="eans">The EANs for the rows.</param>
        /// <returns>The name of the file.</returns>
        public static string Create(IEnumerable<string> eans)
        {
            var fileName = @"c:\temp\rmsgProductQuery.csv";
            using (var writer = new StreamWriter(fileName, false))
            {
                writer.WriteLine(GetHeaderRow());
                foreach (var ean in eans)
                {
                    writer.WriteLine(GetDetailRow(ean));
                }

                writer.Flush();
            }

            return fileName;
        }

        /// <summary>
        /// Creates a valid row.
        /// </summary>
        /// <param name="ean">The EAN for the row.</param>
        /// <returns>A valid row.</returns>
        private static string GetDetailRow(string ean)
        {
            int numberOfColumns = 82;
            int gtinTypePosition = 24;

            var x = string.Format(
                "{0}{1}{2}{3}{4}",
                new string(',', gtinTypePosition - 1),
                "EAN",
                ",",
                ean,
                new string(',', numberOfColumns - gtinTypePosition - 2));

            return x;
        }

        /// <summary>
        /// Gets the header row for the file.
        /// </summary>
        /// <returns>The header row for the file.</returns>
        private static string GetHeaderRow()
        {
            var headers = new[]
            {
                "sku",
                "base_sku",
                "manufacturer",
                "manufacturer_part_number",
                "url",
                "name",
                "tagline",
                "description_1",
                "legal_information",
                "shipping_instructions",
                "labels",
                "price",
                "image_url_1",
                "image_url_2",
                "image_url_3",
                "image_url_4",
                "image_url_5",
                "image_url_6",
                "image_url_7",
                "image_url_8",
                "image_url_9",
                "image_url_10",
                "video_url",
                "rakuten_product_category_id",
                "gtin_type",
                "gtin_value",
                "attribute_1",
                "attribute_2",
                "attribute_3",
                "attribute_4",
                "attribute_5",
                "brand",
                "display_start_date",
                "display_end_date",
                "available_start_date",
                "available_end_date",
                "shipping_preparation_time",
                "free_shipping",
                "shipping_width",
                "shipping_height",
                "shipping_length",
                "weight",
                "display_quantity",
                "operator_for_quantity",
                "quantity",
                "return_quantity_in_cancel",
                "purchase_quantity_limit",
                "shop_product_unique_identifier_1",
                "shop_product_unique_identifier_2",
                "shop_product_unique_identifier_3",
                "shop_product_unique_identifier_4",
                "shop_product_unique_identifier_5",
                "shop_product_unique_identifier_6",
                "shop_product_unique_identifier_7",
                "shop_product_unique_identifier_8",
                "shop_product_unique_identifier_9",
                "shop_product_unique_identifier_10",
                "shop_product_unique_identifier_11",
                "shop_product_unique_identifier_12",
                "shop_product_unique_identifier_13",
                "shop_product_unique_identifier_14",
                "shop_product_unique_identifier_15",
                "shipping_option_1",
                "shipping_option_2",
                "shipping_option_3",
                "shipping_option_4",
                "shipping_option_5",
                "shipping_option_6",
                "shipping_option_7",
                "shipping_option_8",
                "shipping_option_9",
                "shipping_option_10",
                "shipping_option_11",
                "shipping_option_12",
                "shipping_option_13",
                "shipping_option_14",
                "shipping_option_15",
                "shipping_option_16",
                "shipping_option_17",
                "shipping_option_18",
                "shipping_option_19",
                "shipping_option_20"
            };

            return string.Join(",", headers);
        }
    }
}

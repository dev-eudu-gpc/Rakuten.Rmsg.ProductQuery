// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="Item.cs" company="Rakuten">
//   Copyright (c) Rakuten. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.Web.Http.Tests.Integration
{
    using Rakuten.IO.Delimited;

    /// <summary>
    /// Defines the structure of the file containing a product query.
    /// </summary>
    [DefaultStringsToNull]
    [File(FieldWriteOrder = FieldOrder.OrderByIndex)]
    public class Item
    {
        /// <summary>
        /// Gets or sets the value of an attribute detailing this item.
        /// </summary>
        [Header("attribute_1")]
        [Index(26)]
        public string Attribute1 { get; set; }

        /// <summary>
        /// Gets or sets the value of an attribute detailing this item.
        /// </summary>
        [Header("attribute_2")]
        [Index(27)]
        public string Attribute2 { get; set; }

        /// <summary>
        /// Gets or sets the value of an attribute detailing this item.
        /// </summary>
        [Header("attribute_3")]
        [Index(28)]
        public string Attribute3 { get; set; }

        /// <summary>
        /// Gets or sets the value of an attribute detailing this item.
        /// </summary>
        [Header("attribute_4")]
        [Index(29)]
        public string Attribute4 { get; set; }

        /// <summary>
        /// Gets or sets the value of an attribute detailing this item.
        /// </summary>
        [Header("attribute_5")]
        [Index(30)]
        public string Attribute5 { get; set; }

        /// <summary>
        /// Gets or sets the point in time at which this item will no longer be available.
        /// </summary>
        [Header("available_end_date")]
        [Index(35)]
        public string AvailableEndDate { get; set; }

        /// <summary>
        /// Gets or sets the point in time at which this item will become available.
        /// </summary>
        [Header("available_start_date")]
        [Index(34)]
        public string AvailableStartDate { get; set; }

        /// <summary>
        /// Gets or sets the stock keeping unit (SKU) from which this item derives.
        /// </summary>
        [Header("base_sku")]
        [Index(1)]
        public string BaseSku { get; set; }

        /// <summary>
        /// Gets or sets the products brand.
        /// </summary>
        [Header("brand")]
        [Index(31)]
        public string Brand { get; set; }

        /// <summary>
        /// Gets or sets the description of the item.
        /// </summary>
        [Header("description_1")]
        [Index(7)]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the point in time at which this item will no longer be displayed.
        /// </summary>
        [Header("display_end_date")]
        [Index(33)]
        public string DisplayEndDate { get; set; }

        /// <summary>
        /// Gets or sets the quantity of this item to display.
        /// </summary>
        [Header("display_quantity")]
        [Index(42)]
        public string DisplayQuantity { get; set; }

        /// <summary>
        /// Gets or sets the point in time at which this item will start to be displayed.
        /// </summary>
        [Header("display_start_date")]
        [Index(32)]
        public string DisplayStartDate { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this item has free shipping.
        /// </summary>
        [Header("free_shipping")]
        [Index(37)]
        public string FreeShipping { get; set; }

        /// <summary>
        /// Gets or sets the type of the Global Trade Identification Number (GTIN).
        /// </summary>
        [Header("gtin_type")]
        [Index(24)]
        public string GtinType { get; set; }

        /// <summary>
        /// Gets or sets the products Global Trade Identification Number (GTIN).
        /// </summary>
        [Header("gtin_value")]
        [Index(25)]
        public string GtinValue { get; set; }

        /// <summary>
        /// Gets or sets the Uniform Resource Identifier (URI) of an item image.
        /// </summary>
        [Header("image_url_1")]
        [Index(12)]
        public string ImageUrl1 { get; set; }

        /// <summary>
        /// Gets or sets the Uniform Resource Identifier (URI) of an item image.
        /// </summary>
        [Header("image_url_2")]
        [Index(13)]
        public string ImageUrl2 { get; set; }

        /// <summary>
        /// Gets or sets the Uniform Resource Identifier (URI) of an item image.
        /// </summary>
        [Header("image_url_3")]
        [Index(14)]
        public string ImageUrl3 { get; set; }

        /// <summary>
        /// Gets or sets the Uniform Resource Identifier (URI) of an item image.
        /// </summary>
        [Header("image_url_4")]
        [Index(15)]
        public string ImageUrl4 { get; set; }

        /// <summary>
        /// Gets or sets the Uniform Resource Identifier (URI) of an item image.
        /// </summary>
        [Header("image_url_5")]
        [Index(16)]
        public string ImageUrl5 { get; set; }

        /// <summary>
        /// Gets or sets the Uniform Resource Identifier (URI) of an item image.
        /// </summary>
        [Header("image_url_6")]
        [Index(17)]
        public string ImageUrl6 { get; set; }

        /// <summary>
        /// Gets or sets the Uniform Resource Identifier (URI) of an item image.
        /// </summary>
        [Header("image_url_7")]
        [Index(18)]
        public string ImageUrl7 { get; set; }

        /// <summary>
        /// Gets or sets the Uniform Resource Identifier (URI) of an item image.
        /// </summary>
        [Header("image_url_8")]
        [Index(19)]
        public string ImageUrl8 { get; set; }

        /// <summary>
        /// Gets or sets the Uniform Resource Identifier (URI) of an item image.
        /// </summary>
        [Header("image_url_9")]
        [Index(20)]
        public string ImageUrl9 { get; set; }

        /// <summary>
        /// Gets or sets the Uniform Resource Identifier (URI) of an item image.
        /// </summary>
        [Header("image_url_10")]
        [Index(21)]
        public string ImageUrl10 { get; set; }

        /// <summary>
        /// Gets or sets any labels given to the item.
        /// </summary>
        [Header("labels")]
        [Index(10)]
        public string Labels { get; set; }

        /// <summary>
        /// Gets or sets any legal information associated with the item.
        /// </summary>
        [Header("legal_information")]
        [Index(8)]
        public string LegalInformation { get; set; }

        /// <summary>
        /// Gets or sets the name of the manufacturer of the product.
        /// </summary>
        [Header("manufacturer")]
        [Index(2)]
        public string Manufacturer { get; set; }

        /// <summary>
        /// Gets or sets the part number assigned to the product by its manufacturer.
        /// </summary>
        [Header("manufacturer_part_number")]
        [Index(3)]
        public string ManufacturerPartNumber { get; set; }

        /// <summary>
        /// Gets or sets the name of the item.
        /// </summary>
        [Header("name")]
        [Index(5)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the operator to use when making calculations on quantity.
        /// </summary>
        [Header("operator_for_quantity")]
        [Index(43)]
        public string OperatorForQuantity { get; set; }

        /// <summary>
        /// Gets or sets the price of the item.
        /// </summary>
        [Header("price")]
        [Index(11)]
        public string Price { get; set; }

        /// <summary>
        /// Gets or sets the maximum number of this item that can be purchased at one time.
        /// </summary>
        [Header("purchase_quantity_limit")]
        [Index(46)]
        public string PurchaseQuantityLimit { get; set; }

        /// <summary>
        /// Gets or sets how many of this item are available.
        /// </summary>
        [Header("quantity")]
        [Index(44)]
        public string Quantity { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the category in which this item exists.
        /// </summary>
        [Header("rakuten_product_category_id")]
        [Index(23)]
        public string RakutenProductCategoryId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to return the quantity purchased if an order is cancelled.
        /// </summary>
        [Header("return_quantity_in_cancel")]
        [Index(45)]
        public string ReturnQuantityInCancel { get; set; }

        /// <summary>
        /// Gets or sets the height of this item when shipping.
        /// </summary>
        [Header("shipping_height")]
        [Index(39)]
        public string ShippingHeight { get; set; }

        /// <summary>
        /// Gets or sets any instructions related to shipping the item.
        /// </summary>
        [Header("shipping_instructions")]
        [Index(9)]
        public string ShippingInstructions { get; set; }

        /// <summary>
        /// Gets or sets the length of this item when shipping.
        /// </summary>
        [Header("shipping_length")]
        [Index(40)]
        public string ShippingLength { get; set; }

        /// <summary>
        /// Gets or sets a shipping option for this item.
        /// </summary>
        [Header("shipping_option_1")]
        [Index(62)]
        public string ShippingOption1 { get; set; }

        /// <summary>
        /// Gets or sets a shipping option for this item.
        /// </summary>
        [Header("shipping_option_2")]
        [Index(63)]
        public string ShippingOption2 { get; set; }

        /// <summary>
        /// Gets or sets a shipping option for this item.
        /// </summary>
        [Header("shipping_option_3")]
        [Index(64)]
        public string ShippingOption3 { get; set; }

        /// <summary>
        /// Gets or sets a shipping option for this item.
        /// </summary>
        [Header("shipping_option_4")]
        [Index(65)]
        public string ShippingOption4 { get; set; }

        /// <summary>
        /// Gets or sets a shipping option for this item.
        /// </summary>
        [Header("shipping_option_5")]
        [Index(66)]
        public string ShippingOption5 { get; set; }

        /// <summary>
        /// Gets or sets a shipping option for this item.
        /// </summary>
        [Header("shipping_option_6")]
        [Index(67)]
        public string ShippingOption6 { get; set; }

        /// <summary>
        /// Gets or sets a shipping option for this item.
        /// </summary>
        [Header("shipping_option_7")]
        [Index(68)]
        public string ShippingOption7 { get; set; }

        /// <summary>
        /// Gets or sets a shipping option for this item.
        /// </summary>
        [Header("shipping_option_8")]
        [Index(69)]
        public string ShippingOption8 { get; set; }

        /// <summary>
        /// Gets or sets a shipping option for this item.
        /// </summary>
        [Header("shipping_option_9")]
        [Index(70)]
        public string ShippingOption9 { get; set; }

        /// <summary>
        /// Gets or sets a shipping option for this item.
        /// </summary>
        [Header("shipping_option_10")]
        [Index(71)]
        public string ShippingOption10 { get; set; }

        /// <summary>
        /// Gets or sets a shipping option for this item.
        /// </summary>
        [Header("shipping_option_11")]
        [Index(72)]
        public string ShippingOption11 { get; set; }

        /// <summary>
        /// Gets or sets a shipping option for this item.
        /// </summary>
        [Header("shipping_option_12")]
        [Index(73)]
        public string ShippingOption12 { get; set; }

        /// <summary>
        /// Gets or sets a shipping option for this item.
        /// </summary>
        [Header("shipping_option_13")]
        [Index(74)]
        public string ShippingOption13 { get; set; }

        /// <summary>
        /// Gets or sets a shipping option for this item.
        /// </summary>
        [Header("shipping_option_14")]
        [Index(75)]
        public string ShippingOption14 { get; set; }

        /// <summary>
        /// Gets or sets a shipping option for this item.
        /// </summary>
        [Header("shipping_option_15")]
        [Index(76)]
        public string ShippingOption15 { get; set; }

        /// <summary>
        /// Gets or sets a shipping option for this item.
        /// </summary>
        [Header("shipping_option_16")]
        [Index(77)]
        public string ShippingOption16 { get; set; }

        /// <summary>
        /// Gets or sets a shipping option for this item.
        /// </summary>
        [Header("shipping_option_17")]
        [Index(78)]
        public string ShippingOption17 { get; set; }

        /// <summary>
        /// Gets or sets a shipping option for this item.
        /// </summary>
        [Header("shipping_option_18")]
        [Index(79)]
        public string ShippingOption18 { get; set; }

        /// <summary>
        /// Gets or sets a shipping option for this item.
        /// </summary>
        [Header("shipping_option_19")]
        [Index(80)]
        public string ShippingOption19 { get; set; }

        /// <summary>
        /// Gets or sets a shipping option for this item.
        /// </summary>
        [Header("shipping_option_20")]
        [Index(81)]
        public string ShippingOption20 { get; set; }

        /// <summary>
        /// Gets or sets the length of time designated to prepare the item for shipping.
        /// </summary>
        [Header("shipping_preparation_time")]
        [Index(36)]
        public string ShippingPreparationTime { get; set; }

        /// <summary>
        /// Gets or sets the width of this item when shipping.
        /// </summary>
        [Header("shipping_width")]
        [Index(38)]
        public string ShippingWidth { get; set; }

        /// <summary>
        /// Gets or sets a unique identifier of this item within the context of a shop.
        /// </summary>
        [Header("shop_product_unique_identifier_1")]
        [Index(47)]
        public string ShopProductUniqueIdentifier1 { get; set; }

        /// <summary>
        /// Gets or sets a unique identifier of this item within the context of a shop.
        /// </summary>
        [Header("shop_product_unique_identifier_2")]
        [Index(48)]
        public string ShopProductUniqueIdentifier2 { get; set; }

        /// <summary>
        /// Gets or sets a unique identifier of this item within the context of a shop.
        /// </summary>
        [Header("shop_product_unique_identifier_3")]
        [Index(49)]
        public string ShopProductUniqueIdentifier3 { get; set; }

        /// <summary>
        /// Gets or sets a unique identifier of this item within the context of a shop.
        /// </summary>
        [Header("shop_product_unique_identifier_4")]
        [Index(50)]
        public string ShopProductUniqueIdentifier4 { get; set; }

        /// <summary>
        /// Gets or sets a unique identifier of this item within the context of a shop.
        /// </summary>
        [Header("shop_product_unique_identifier_5")]
        [Index(51)]
        public string ShopProductUniqueIdentifier5 { get; set; }

        /// <summary>
        /// Gets or sets a unique identifier of this item within the context of a shop.
        /// </summary>
        [Header("shop_product_unique_identifier_6")]
        [Index(52)]
        public string ShopProductUniqueIdentifier6 { get; set; }

        /// <summary>
        /// Gets or sets a unique identifier of this item within the context of a shop.
        /// </summary>
        [Header("shop_product_unique_identifier_7")]
        [Index(53)]
        public string ShopProductUniqueIdentifier7 { get; set; }

        /// <summary>
        /// Gets or sets a unique identifier of this item within the context of a shop.
        /// </summary>
        [Header("shop_product_unique_identifier_8")]
        [Index(54)]
        public string ShopProductUniqueIdentifier8 { get; set; }

        /// <summary>
        /// Gets or sets a unique identifier of this item within the context of a shop.
        /// </summary>
        [Header("shop_product_unique_identifier_9")]
        [Index(55)]
        public string ShopProductUniqueIdentifier9 { get; set; }

        /// <summary>
        /// Gets or sets a unique identifier of this item within the context of a shop.
        /// </summary>
        [Header("shop_product_unique_identifier_10")]
        [Index(56)]
        public string ShopProductUniqueIdentifier10 { get; set; }

        /// <summary>
        /// Gets or sets a unique identifier of this item within the context of a shop.
        /// </summary>
        [Header("shop_product_unique_identifier_11")]
        [Index(57)]
        public string ShopProductUniqueIdentifier11 { get; set; }

        /// <summary>
        /// Gets or sets a unique identifier of this item within the context of a shop.
        /// </summary>
        [Header("shop_product_unique_identifier_12")]
        [Index(58)]
        public string ShopProductUniqueIdentifier12 { get; set; }

        /// <summary>
        /// Gets or sets a unique identifier of this item within the context of a shop.
        /// </summary>
        [Header("shop_product_unique_identifier_13")]
        [Index(59)]
        public string ShopProductUniqueIdentifier13 { get; set; }

        /// <summary>
        /// Gets or sets a unique identifier of this item within the context of a shop.
        /// </summary>
        [Header("shop_product_unique_identifier_14")]
        [Index(60)]
        public string ShopProductUniqueIdentifier14 { get; set; }

        /// <summary>
        /// Gets or sets a unique identifier of this item within the context of a shop.
        /// </summary>
        [Header("shop_product_unique_identifier_15")]
        [Index(61)]
        public string ShopProductUniqueIdentifier15 { get; set; }

        /// <summary>
        /// Gets or sets the stock keeping unit (SKU) of the item.
        /// </summary>
        [Header("sku")]
        [Index(0)]
        public string Sku { get; set; }

        /// <summary>
        /// Gets or sets a tag associated with this item.
        /// </summary>
        [Header("tagline")]
        [Index(6)]
        public string Tagline { get; set; }

        /// <summary>
        /// Gets or sets the Uniform Resource Identifier (URI) of the item within its mall.
        /// </summary>
        [Header("url")]
        [Index(4)]
        public string Url { get; set; }

        /// <summary>
        /// Gets or sets the Uniform Resource Identifier (URI) of a video relating to this item.
        /// </summary>
        [Header("video_url")]
        [Index(22)]
        public string VideoUrl { get; set; }

        /// <summary>
        /// Gets or sets the weight of this item.
        /// </summary>
        [Header("weight")]
        [Index(41)]
        public string Weight { get; set; }
    }
}
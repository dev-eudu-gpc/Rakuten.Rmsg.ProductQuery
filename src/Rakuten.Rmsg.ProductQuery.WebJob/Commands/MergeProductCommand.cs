// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="MergeProductCommand.cs" company="Rakuten">
//   Copyright (c) Rakuten. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.WebJob
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Rakuten.Rmsg.ProductQuery.WebJob.Api;

    /// <summary>
    /// Merges details from the specified product into the given item.
    /// </summary>
    internal class MergeProductCommand
    {
        /// <summary>
        /// Takes selected details from the specified product and adds them into the <see cref="Item"/> only adding 
        /// additional information.
        /// </summary>
        /// <param name="item">The item into which the product data should be added.</param>
        /// <param name="product">The product whose details are to be used.</param>
        /// <returns>A <see cref="Task"/> the represents the asynchronous operation.</returns>
        public static Task<Item> Execute(Item item, Product product)
        {
            return Task.Run(() =>
            {
                item.SetIfNullOrEmpty(i => i.Manufacturer, product.Manufacturer);
                item.SetIfNullOrEmpty(i => i.ManufacturerPartNumber, product.PartNumber);

                ProcessAttributeSet(
                    item, 
                    product, 
                    "images", 
                    new Dictionary<string, Action<object>>
                    {
                        { "image URL main", o => item.SetIfNullOrEmpty(i => i.ImageUrl1, o) },
                        { "image location 2", o => item.SetIfNullOrEmpty(i => i.ImageUrl2, o) },
                        { "image location 3", o => item.SetIfNullOrEmpty(i => i.ImageUrl3, o) },
                        { "image location 4", o => item.SetIfNullOrEmpty(i => i.ImageUrl4, o) },
                        { "image location 5", o => item.SetIfNullOrEmpty(i => i.ImageUrl5, o) },
                    },
                    item1 => string.IsNullOrEmpty(item.ImageUrl1));

                ProcessAttributeSet(
                    item,
                    product,
                    "Rakuten Common Attributes",
                    new Dictionary<string, Action<object>>
                    {
                        { "Video URL", o => item.SetIfNullOrEmpty(i => i.VideoUrl, o) }
                    });

                ProcessAttributeSet(
                    item,
                    product,
                    "Brand",
                    new Dictionary<string, Action<object>>
                    {
                        { "Brand", o => item.SetIfNullOrEmpty(i => i.Brand, o) }
                    });

                return item;
            });
        }

        /// <summary>
        /// Locates and maps the specified attribute set from the <see cref="Product"/> into the <see cref="Item"/> 
        /// using the collection of <see cref="Action{T1}"/>s.
        /// </summary>
        /// <param name="item">The item onto which the additional data should be merged.</param>
        /// <param name="product">The product from which the data should be extracted.</param>
        /// <param name="name">The name of the attribute set to process.</param>
        /// <param name="actions">
        /// A collection of <see cref="KeyValuePair{TKey,TValue}"/> that define how to process each attribute in the 
        /// set.
        /// </param>
        /// <param name="predicate">A filter that determines if data can be aggregated.</param>
        private static void ProcessAttributeSet(
            Item item,
            Product product,
            string name,
            Dictionary<string, Action<object>> actions,
            Predicate<Item> predicate)
        {
            if (!predicate(item))
            {
                return;
            }

            ProcessAttributeSet(item, product, name, actions);
        }

        /// <summary>
        /// Locates and maps the specified attribute set from the <see cref="Product"/> into the <see cref="Item"/> 
        /// using the collection of <see cref="Action{T1}"/>s.
        /// </summary>
        /// <param name="item">The item onto which the additional data should be merged.</param>
        /// <param name="product">The product from which the data should be extracted.</param>
        /// <param name="name">The name of the attribute set to process.</param>
        /// <param name="actions">
        /// A collection of <see cref="KeyValuePair{TKey,TValue}"/> that define how to process each attribute in the 
        /// set.
        /// </param>
        private static void ProcessAttributeSet(
            Item item, 
            Product product,
            string name, 
            Dictionary<string, Action<object>> actions)
        {
            if (product.Attributes == null)
            {
                return;
            }

            var set = product.Attributes.FirstOrDefault(
                a => a.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

            if (set == null)
            {
                return;
            }

            foreach (var key in actions.Keys)
            {
                ProcessAttribute(item, set, key, actions[key]);
            }
        }

        /// <summary>
        /// Identifies the specified attribute from the set of attributes and merges its data onto the specified 
        /// <see cref="Item"/> using the specified <see cref="Action{T1}"/>.
        /// </summary>
        /// <param name="item">The item onto which the additional data should be merged.</param>
        /// <param name="set">The named set of attributes.</param>
        /// <param name="key">The unique identifier of the attribute whose data should be extracted.</param>
        /// <param name="action">The action that defines how the data should be applied.</param>
        private static void ProcessAttribute(Item item, ProductAttributeSet set, string key, Action<object> action)
        {
            var kvp = set.Attributes.FirstOrDefault(a => a.Key.Equals(key, StringComparison.OrdinalIgnoreCase));

            ProcessAttribute(item, kvp, action);
        }

        /// <summary>
        /// Merges the data from the <see cref="KeyValuePair{TKey,TValue}"/> into the <see cref="Item"/> using the 
        /// specified <see cref="Action{T1}"/>.
        /// </summary>
        /// <param name="item">The item onto which the additional data should be merged.</param>
        /// <param name="attribute">The attribute.</param>
        /// <param name="action">The action that defines how the data should be applied.</param>
        private static void ProcessAttribute(Item item, KeyValuePair<string, object> attribute, Action<object> action)
        {
            if (!attribute.Equals(default(KeyValuePair<string, object>)))
            {
                action(attribute.Value);
            }
        }
    }
}
//----------------------------------------------------------------------------------------------------------------------
// <copyright file="Page.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------------------------------------------------
namespace Rakuten.Gpc.Resources
{
    using System.Collections.Generic;
    using System.Xml.Serialization;

    using Newtonsoft.Json;

    /// <summary>
    /// Represents a page of results.
    /// </summary>
    /// <typeparam name="T">The type of elements on the page.</typeparam>
    [XmlType("page")]
    public class Page<T> : Resource
    {
        /// <summary>
        /// The total count.
        /// </summary>
        private int? totalCount;

        /// <summary>
        /// The page count.
        /// </summary>
        private int? pageCount;

        /// <summary>
        /// The page size.
        /// </summary>
        private int? pageSize;

        /// <summary>
        /// The current page number.
        /// </summary>
        private int? currentPage;

        /// <summary>
        /// Gets or sets the current page number.
        /// </summary>
        [JsonProperty("currentPage")]
        [XmlElement("currentPage")]
        public int CurrentPage
        {
            get { return this.currentPage ?? 1; }
            set { this.currentPage = value; }
        }

        /// <summary>
        /// Gets or sets the list of items on the current <see cref="Page{T}"/>.
        /// </summary>
        [JsonProperty("items")]
        [XmlArray("items")]
        public List<T> Items { get; set; }

        /// <summary>
        /// Gets or sets the total number of pages in the result set.
        /// </summary>
        [JsonProperty("pageCount")]
        [XmlElement("pageCount")]
        public int PageCount
        {
            get { return this.pageCount ?? 1; }
            set { this.pageCount = value; }
        }

        /// <summary>
        /// Gets or sets the size of the current page.
        /// </summary>
        [JsonProperty("pageSize")]
        [XmlElement("pageSize")]
        public int PageSize
        {
            get { return this.pageSize ?? this.TotalCount; }
            set { this.pageSize = value; }
        }

        /// <summary>
        /// Gets or sets the total number of results.
        /// </summary>
        [JsonProperty("totalCount")]
        [XmlElement("totalCount")]
        public int TotalCount
        {
            get { return this.totalCount ?? (this.Items == null ? 0 : this.Items.Count); }
            set { this.totalCount = value; }
        }

        /// <summary>
        /// Indicates whether to include the <see cref="CurrentPage"/> field
        /// in the serialization of the current instance.
        /// </summary>
        /// <returns>
        /// <see langword="true"/> if the <see cref="CurrentPage"/> field should be included
        /// in the serialization of the current instance; otherwise,
        /// <see langword="false"/>.
        /// </returns>
        public bool ShouldSerializeCurrentPage()
        {
            return this.currentPage.HasValue;
        }

        /// <summary>
        /// Indicates whether to include the <see cref="PageCount"/> field
        /// in the serialization of the current instance.
        /// </summary>
        /// <returns>
        /// <see langword="true"/> if the <see cref="PageCount"/> field should be included
        /// in the serialization of the current instance; otherwise, <see langword="false"/>.
        /// </returns>
        public bool ShouldSerializePageCount()
        {
            return this.pageCount.HasValue;
        }

        /// <summary>
        /// Indicates whether to include the <see cref="PageSize"/> field
        /// in the serialization of the current instance.
        /// </summary>
        /// <returns>
        /// <see langword="true"/> if the <see cref="PageSize"/> field should be included
        /// in the serialization of the current instance; otherwise, <see langword="false"/>.
        /// </returns>
        public bool ShouldSerializePageSize()
        {
            return this.pageSize.HasValue;
        }

        /// <summary>
        /// Indicates whether to include the <see cref="TotalCount"/> field
        /// in the serialization of the current instance.
        /// </summary>
        /// <returns>
        /// <see langword="true"/> if the <see cref="TotalCount"/> field should be included
        /// in the serialization of the current instance; otherwise, <see langword="false"/>.
        /// </returns>
        public bool ShouldSerializeTotalCount()
        {
            return this.totalCount.HasValue;
        }
    }
}
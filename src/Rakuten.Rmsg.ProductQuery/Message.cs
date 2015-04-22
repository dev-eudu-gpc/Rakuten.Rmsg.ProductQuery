// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="Message.cs" company="Rakuten">
//   Copyright (c) Rakuten. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery
{
    using System;
    using System.Diagnostics.Contracts;

    using Newtonsoft.Json;

    /// <summary>
    /// Defines the content of the message placed on the Microsoft Azure Service Bus queue.
    /// </summary>
    public class Message
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Message"/> class with the specified query identifier and blob
        /// URI.
        /// </summary>
        /// <param name="id">The unique identifier of the query that is ready to be processed.</param>
        /// <param name="culture">The culture in which data should be expressed.</param>
        /// <param name="link">A <see cref="Link"/> representing the URI of the blob.</param>
        public Message(Guid id, string culture, Link link)
        {
            Contract.Requires(link != null);

            this.Id = id;
            this.Culture = culture;
            this.Link = link;
        }

        /// <summary>
        /// Gets or sets the culture in which data should be expressed.
        /// </summary>
        public string Culture { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the query to process.
        /// </summary>
        [JsonProperty("id")]
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the location of the blob in Microsoft Azure that is the uploaded file.
        /// </summary>
        [JsonProperty("link")]
        public Link Link { get; set; }
    }
}
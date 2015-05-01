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
    [Serializable]
    public class Message
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Message"/> class with the specified query identifier and blob
        /// URI.
        /// </summary>
        /// <param name="id">The unique identifier of the query that is ready to be processed.</param>
        /// <param name="link">A <see cref="Link"/> representing the URI of the blob.</param>
        public Message(Guid id, Link link)
        {
            Contract.Requires(link != null);

            this.Id = id;
            this.Link = link;
        }

        /// <summary>
        /// Gets the unique identifier of the query to process.
        /// </summary>
        [JsonProperty("id")]
        public Guid Id { get; private set; }

        /// <summary>
        /// Gets the location of the blob in Microsoft Azure that is the uploaded file.
        /// </summary>
        [JsonProperty("link")]
        public Link Link { get; private set; }
    }
}
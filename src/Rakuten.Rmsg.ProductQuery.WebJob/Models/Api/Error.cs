//----------------------------------------------------------------------------------------------------------------------
// <copyright file="Error.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.WebJob.Api
{
    using Newtonsoft.Json;

    /// <summary>
    /// Represents an error or warning returned from the GPC API.
    /// </summary>
    [JsonObject(Title = "apiErrorMessage")]
    public class Error
    {
        /// <summary>
        /// Gets or sets the error number.
        /// </summary>
        [JsonProperty("errorNumber")]
        public int ErrorNumber { get; set; }

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        [JsonProperty("message")]
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the message type.
        /// </summary>
        [JsonProperty("messageType")]
        public string MessageType { get; set; }
    }
}
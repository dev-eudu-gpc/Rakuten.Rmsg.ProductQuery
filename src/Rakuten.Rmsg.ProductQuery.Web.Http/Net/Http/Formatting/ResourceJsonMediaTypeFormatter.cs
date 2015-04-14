//------------------------------------------------------------------------------
// <copyright file="ResourceJsonMediaTypeFormatter.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
namespace Rakuten.Net.Http.Formatting
{
    using System.Net.Http.Formatting;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;

    /// <summary>A <see cref="MediaTypeFormatter"/> for handling JSON.</summary>
    public class ResourceJsonMediaTypeFormatter : JsonMediaTypeFormatter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceJsonMediaTypeFormatter"/> class.
        /// </summary>
        public ResourceJsonMediaTypeFormatter()
        {
            var settings = this.SerializerSettings;
            settings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            settings.NullValueHandling = NullValueHandling.Ignore;
        }
    }
}
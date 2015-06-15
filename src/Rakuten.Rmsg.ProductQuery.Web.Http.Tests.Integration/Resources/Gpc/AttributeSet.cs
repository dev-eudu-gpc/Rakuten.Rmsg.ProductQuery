//---------------------------------------------------------------------------------------------------------------------
// <copyright file="AttributeSet.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//---------------------------------------------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.Web.Http.Tests.Integration
{
    using System.Collections.Generic;

    using Newtonsoft.Json;

    using Rakuten.Json;

    /// <summary>
    /// A named set of key/value pairs.
    /// </summary>
    public class AttributeSet
    {
        /// <summary>
        /// Gets or sets the collection of key/value pairs.
        /// </summary>
        [JsonConverter(typeof(DictionaryConverter))]
        public SortedDictionary<string, object> Attributes { get; set; }

        /// <summary>
        /// Gets or sets the name of the attribute set.
        /// </summary>
        public string Name { get; set; }
    }
}

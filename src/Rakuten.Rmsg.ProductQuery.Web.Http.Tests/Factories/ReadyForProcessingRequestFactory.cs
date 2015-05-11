//----------------------------------------------------------------------------------------------------------------------
// <copyright file="ReadyForProcessingRequestFactory.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.Web.Http.Tests
{
    using System;
    using System.Collections.ObjectModel;
    using System.Globalization;
    using System.IO;
    using System.Windows;
    using System.Windows.Media.Imaging;
    using Rakuten.Fakes;
    using Rakuten.Rmsg.ProductQuery.Web.Http.Links;

    /// <summary>
    /// Provides functionality for working with <see cref="ReadyForProcessingRequest"/> objects for test purposes.
    /// </summary>
    public static class ReadyForProcessingRequestFactory
    {
        /// <summary>
        /// Creates a new <see cref="ReadyForProcessingRequest"/>.
        /// </summary>
        /// <param name="status">The status.</param>
        /// <returns>A new <see cref="ReadyForProcessingRequest"/> object.</returns>
        public static ReadyForProcessingRequest Create(string status = "submitted")
        {
            // Construct links
            return new ReadyForProcessingRequest { Status = status };
        }
    }
}
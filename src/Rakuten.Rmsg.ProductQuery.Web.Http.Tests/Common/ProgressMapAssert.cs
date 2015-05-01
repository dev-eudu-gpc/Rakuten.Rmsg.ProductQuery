//------------------------------------------------------------------------------
// <copyright file="ProgressMapAssert.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.Web.Http.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Verifies true/false propositions associated with progress maps.
    /// </summary>
    public static class ProgressMapAssert
    {
        /// <summary>
        /// Verifies that the pixel is set to the correct value according 
        /// to the source product query progress object
        /// </summary>
        /// <param name="progressMap">The progress object containing the source data for the pixel.</param>
        /// <param name="pixels">The full progress map image as an array of pixels of which the product query is part.</param>
        public static void ArePercentagesEqual(IQueryable<ProductQueryProgress> progressMap,  byte[] pixels)
        {
            foreach (var query in progressMap)
            {
                var expected = (byte)(query.PercentageComplete * 255m);
                var actual = pixels[query.Index - 1];
                Assert.AreEqual(
                    expected,
                    actual,
                    string.Format("The percentages did not match for product query at position {0}", query.Index));
            }
        }
    }
}

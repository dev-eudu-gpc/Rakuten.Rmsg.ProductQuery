//------------------------------------------------------------------------------
// <copyright file="CreateProgressImageCommandParametersTest.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.Web.Http.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Rakuten.Rmsg.ProductQuery.Web.Http.Commands;

    /// <summary>
    /// Defines tests for the <see cref="CreateProgressImageCommandParameters"/> class.
    /// </summary>
    [TestClass]
    public class CreateProgressImageCommandParametersTest
    {
        /// <summary>
        /// Verifies that the <see cref="CreateProgressImageCommandParameters"/> class constructs
        /// the correct object when all parameters are valid.
        /// </summary>
        [TestMethod]
        public void CreateProgressImageCommandParametersReturnsCorrectObjectWhenAllParametersAreValid()
        {
            // Arrange
            var progressMap = new List<ProductQueryProgress>
            {
                new ProductQueryProgress()
            };

            // Act
            var parameters = new CreateProgressImageCommandParameters(progressMap.AsQueryable());

            // Assert
            CollectionAssert.AreEquivalent(progressMap, parameters.ProgressMap.ToList());
        }
   }
}
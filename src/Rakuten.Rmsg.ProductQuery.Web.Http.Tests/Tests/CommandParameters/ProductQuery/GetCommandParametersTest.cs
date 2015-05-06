//------------------------------------------------------------------------------
// <copyright file="GetCommandParametersTest.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.Web.Http.Tests
{
    using System;
    using System.Globalization;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Rakuten.Rmsg.ProductQuery.Web.Http.Commands;

    /// <summary>
    /// Defines tests for the <see cref="GetCommandParameters"/> class.
    /// </summary>
    [TestClass]
    public class GetCommandParametersTest
    {
        /// <summary>
        /// Verifies that the <see cref="GetCommandParameters"/> class constructs
        /// the correct object when all parameters are valid.
        /// </summary>
        [TestMethod]
        public void GetCommandParametersReturnsCorrectObjectWhenAllParametersAreValidAsStronglyType()
        {
            // Arrange
            var id = Guid.NewGuid();
            var culture = CultureInfo.GetCultureInfo("en-GB");

            // Act
            var parameters = new GetCommandParameters(id, culture);

            // Assert
            Assert.AreEqual(id, parameters.Id);
            Assert.AreEqual(culture, parameters.Culture);
        }

        /// <summary>
        /// Verifies that the <see cref="GetCommandParameters"/> class constructs
        /// the correct object when all parameters are valid.
        /// </summary>
        [TestMethod]
        public void GetCommandParametersReturnsCorrectObjectWhenAllParametersAreValidAsStrings()
        {
            // Arrange
            var id = Guid.NewGuid();
            var culture = CultureInfo.GetCultureInfo("en-GB");

            // Act
            var parameters = new GetCommandParameters(id.ToString(), culture.Name);

            // Assert
            Assert.AreEqual(id, parameters.Id);
            Assert.AreEqual(culture, parameters.Culture);
        }
   }
}
//------------------------------------------------------------------------------
// <copyright file="ProductQueryCommandParametersTest.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.Web.Http.Tests
{
    using System;
    using System.Globalization;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Rakuten.Rmsg.ProductQuery.Web.Http.Commands;
    using Rakuten.Rmsg.ProductQuery.Web.Http.Commands.Fakes;

    /// <summary>
    /// Defines tests for the <see cref="ProductQueryCommandParameters"/> class.
    /// </summary>
    [TestClass]
    public class ProductQueryCommandParametersTest
    {
        /// <summary>
        /// Verifies that the <see cref="GetProgressCommandParameters"/> class constructs
        /// the correct object when all parameters are valid.
        /// </summary>
        [TestMethod]
        public void ProductQueryCommandParameterReturnsCorrectObjectWhenAllParametersAreValidAsStronglyType()
        {
            // Arrange
            var id = Guid.NewGuid();
            var culture = CultureInfo.GetCultureInfo("en-GB");

            // Act
            var parameters = new StubProductQueryCommandParameters(id, culture);

            // Assert
            Assert.AreEqual(id, parameters.Id);
            Assert.AreEqual(culture, parameters.Culture);
        }

        /// <summary>
        /// Verifies that the <see cref="GetProgressCommandParameters"/> class constructs
        /// the correct object when all parameters are valid.
        /// </summary>
        [TestMethod]
        public void ProductQueryCommandParameterReturnsCorrectObjectWhenAllParametersAreValidAsStrings()
        {
            // Arrange
            var id = Guid.NewGuid();
            var culture = CultureInfo.GetCultureInfo("en-GB");

            // Act
            var parameters = new StubProductQueryCommandParameters(id.ToString(), culture.Name);

            // Assert
            Assert.AreEqual(id, parameters.Id);
            Assert.AreEqual(culture, parameters.Culture);
        }

        /// <summary>
        /// Verifies that the <see cref="GetProgressCommandParameters"/> class throws
        /// the correct exception if instantiated with an invalid identifier
        /// </summary>
        [TestMethod]
        public void ProductQueryCommandParameterThrowsInvalidGuidException()
        {
            // Act
            Exception caughtException = null;
            try
            {
                var parameters = new StubProductQueryCommandParameters("not-a-guid", "en-GB");
            }
            catch (Exception ex)
            {
                caughtException = ex;
            }

            // Assert
            Assert.IsNotNull(caughtException);
            Assert.IsInstanceOfType(caughtException, typeof(InvalidGuidException));
        }

        /// <summary>
        /// Verifies that the <see cref="GetProgressCommandParameters"/> class throws
        /// the correct exception if instantiated with an invalid identifier
        /// </summary>
        [TestMethod]
        public void ProductQueryCommandParameterThrowsInvalidCultureException()
        {
            // Act
            Exception caughtException = null;
            try
            {
                var parameters = new StubProductQueryCommandParameters(Guid.NewGuid().ToString(), "not-a-culture");
            }
            catch (Exception ex)
            {
                caughtException = ex;
            }

            // Assert
            Assert.IsNotNull(caughtException);
            Assert.IsInstanceOfType(caughtException, typeof(InvalidCultureException));
        }
    }
}
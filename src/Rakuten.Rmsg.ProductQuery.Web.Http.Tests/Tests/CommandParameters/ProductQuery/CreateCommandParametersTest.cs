//------------------------------------------------------------------------------
// <copyright file="CreateCommandParametersTest.cs" company="Rakuten">
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
    /// Defines tests for the <see cref="CreateCommandParameters"/> class.
    /// </summary>
    [TestClass]
    public class CreateCommandParametersTest
    {
        /// <summary>
        /// Verifies that the <see cref="CreateCommandParameters"/> class constructs
        /// the correct object when all parameters are valid.
        /// </summary>
        [TestMethod]
        public void ProductQueryCommandParameterReturnsCorrectObjectWhenAllParametersAreValid()
        {
            // Arrange
            var id = Guid.NewGuid();
            var culture = CultureInfo.GetCultureInfo("en-GB");

            // Act
            var parameters = new CreateCommandParameters(id, culture);

            // Assert
            Assert.AreEqual(id, parameters.Id);
            Assert.AreEqual(culture, parameters.Culture);
        }
    }
}
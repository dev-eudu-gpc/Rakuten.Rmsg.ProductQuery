//------------------------------------------------------------------------------
// <copyright file="CreateDatabaseCommandParametersTest.cs" company="Rakuten">
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
    /// Defines tests for the <see cref="CreateDatabaseCommandParameters"/> class.
    /// </summary>
    [TestClass]
    public class CreateDatabaseCommandParametersTest
    {
        /// <summary>
        /// Verifies that the <see cref="CreateDatabaseCommandParameters"/> class constructs
        /// the correct object when all parameters are valid.
        /// </summary>
        [TestMethod]
        public void CreateDatabaseCommandParametersReturnsCorrectObjectWhenAllParametersAreValid()
        {
            // Arrange
            var id = Guid.NewGuid();
            var culture = CultureInfo.GetCultureInfo("en-GB");
            var dateCreated = DateTime.Now;

            // Act
            var parameters = new CreateDatabaseCommandParameters(id, culture, dateCreated);

            // Assert
            Assert.AreEqual(id, parameters.Id);
            Assert.AreEqual(culture, parameters.Culture);
            Assert.AreEqual(dateCreated, parameters.DateCreated);
        }
    }
}
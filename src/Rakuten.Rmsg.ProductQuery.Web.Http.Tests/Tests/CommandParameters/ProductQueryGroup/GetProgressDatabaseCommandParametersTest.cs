//------------------------------------------------------------------------------
// <copyright file="GetProgressDatabaseCommandParametersTest.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.Web.Http.Tests
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Rakuten.Rmsg.ProductQuery.Web.Http.Commands;

    /// <summary>
    /// Defines tests for the <see cref="GetProgressDatabaseCommandParameters"/> class.
    /// </summary>
    [TestClass]
    public class GetProgressDatabaseCommandParametersTest
    {
        /// <summary>
        /// Verifies that the <see cref="GetProgressDatabaseCommandParameters"/> class constructs
        /// the correct object when all parameters are valid.
        /// </summary>
        [TestMethod]
        public void GetProgressDatabaseCommandParametersReturnsCorrectObjectWhenAllParametersAreValid()
        {
            // Arrange
            var id = Guid.NewGuid();
            var dateTime = DateTime.Now;

            // Act
            var parameters = new GetProgressDatabaseCommandParameters(id, dateTime);

            // Assert
            Assert.AreEqual(id, parameters.Id);
            Assert.AreEqual(dateTime, parameters.Datetime);
        }
    }
}
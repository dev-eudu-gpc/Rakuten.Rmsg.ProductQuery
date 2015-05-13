//------------------------------------------------------------------------------
// <copyright file="UpdateStatusDatabaseCommandParametersTest.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.Web.Http.Tests
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Rakuten.Rmsg.ProductQuery.Web.Http.Commands;

    /// <summary>
    /// Defines tests for the <see cref="UpdateStatusDatabaseCommandParameters"/> class.
    /// </summary>
    [TestClass]
    public class UpdateStatusDatabaseCommandParametersTest
    {
        /// <summary>
        /// Verifies that the <see cref="UpdateStatusDatabaseCommandParameters"/> class constructs
        /// the correct object when all parameters are valid.
        /// </summary>
        [TestMethod]
        public void UpdateStatusDatabaseCommandParametersReturnsCorrectObjectWhenAllParametersAreValid()
        {
            // Arrange
            var id = Guid.NewGuid();
            var status = ProductQueryStatus.New;

            // Act
            var parameters = new UpdateStatusDatabaseCommandParameters(id, status);

            // Assert
            Assert.AreEqual(id, parameters.Id);
            Assert.AreEqual(status, parameters.NewStatus);
        }
    }
}
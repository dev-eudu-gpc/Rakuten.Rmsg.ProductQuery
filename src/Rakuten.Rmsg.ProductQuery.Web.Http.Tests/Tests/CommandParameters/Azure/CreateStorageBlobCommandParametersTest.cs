//------------------------------------------------------------------------------
// <copyright file="CreateStorageBlobCommandParametersTest.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.Web.Http.Tests
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Rakuten.Rmsg.ProductQuery.Web.Http.Commands;

    /// <summary>
    /// Defines tests for the <see cref="CreateStorageBlobCommandParameters"/> class.
    /// </summary>
    [TestClass]
    public class CreateStorageBlobCommandParametersTest
    {
        /// <summary>
        /// Verifies that the <see cref="CreateStorageBlobCommandParameters"/> class constructs
        /// the correct object when all parameters are valid.
        /// </summary>
        [TestMethod]
        public void CreateStorageBlobCommandParametersReturnsCorrectObjectWhenAllParametersAreValid()
        {
            // Arrange
            var id = Guid.NewGuid();
            var dateCreated = DateTime.Now;

            // Act
            var parameters = new CreateStorageBlobCommandParameters(dateCreated, id);

            // Assert
            Assert.AreEqual(id, parameters.Id);
            Assert.AreEqual(dateCreated, parameters.DateCreated);
        }
    }
}
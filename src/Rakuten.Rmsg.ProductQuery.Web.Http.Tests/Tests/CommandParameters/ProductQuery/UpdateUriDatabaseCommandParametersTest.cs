//------------------------------------------------------------------------------
// <copyright file="UpdateUriDatabaseCommandParametersTest.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.Web.Http.Tests
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Rakuten.Rmsg.ProductQuery.Web.Http.Commands;

    /// <summary>
    /// Defines tests for the <see cref="UpdateUriDatabaseCommandParameters"/> class.
    /// </summary>
    [TestClass]
    public class UpdateUriDatabaseCommandParametersTest
    {
        /// <summary>
        /// Verifies that the <see cref="UpdateUriDatabaseCommandParameters"/> class constructs
        /// the correct object when all parameters are valid.
        /// </summary>
        [TestMethod]
        public void UpdateUriDatabaseCommandParametersReturnsCorrectObjectWhenAllParametersAreValid()
        {
            // Arrange
            var id = Guid.NewGuid();
            var uri = new Uri("http://somewhere.domain/path/file.extension");

            // Act
            var parameters = new UpdateUriDatabaseCommandParameters(id, uri);

            // Assert
            Assert.AreEqual(id, parameters.Id);
            Assert.AreEqual(uri, parameters.Uri);
        }
   }
}
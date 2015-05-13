//------------------------------------------------------------------------------
// <copyright file="DispatchMessageCommandParametersTest.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.Web.Http.Tests
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Rakuten.Rmsg.ProductQuery.Web.Http.Commands;

    /// <summary>
    /// Defines tests for the <see cref="DispatchMessageCommandParameters"/> class.
    /// </summary>
    [TestClass]
    public class DispatchMessageCommandParametersTest
    {
        /// <summary>
        /// Verifies that the <see cref="DispatchMessageCommandParameters"/> class constructs
        /// the correct object when all parameters are valid.
        /// </summary>
        [TestMethod]
        public void DispatchMessageCommandParametersReturnsCorrectObjectWhenAllParametersAreValid()
        {
            // Arrange
            var id = Guid.NewGuid();
            var culture = "en-US";
            var blobLink = new Link() { Target = "http://target.com" };

            // Act
            var parameters = new DispatchMessageCommandParameters(id, culture, blobLink);

            // Assert
            Assert.AreEqual(id, parameters.Id);
            Assert.AreEqual(culture, parameters.CultureName, true);
            Assert.AreEqual(blobLink, parameters.BlobLink);
        }
    }
}
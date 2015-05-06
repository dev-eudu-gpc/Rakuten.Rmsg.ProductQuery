//------------------------------------------------------------------------------
// <copyright file="InvalidStatusExceptionTest.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.Web.Http.Tests
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Defines tests for the <see cref="InvalidStatusException"/> class.
    /// </summary>
    [TestClass]
    public class InvalidStatusExceptionTest
    {
        /// <summary>
        /// Verifies that the default, empty constructor can be used.
        /// </summary>
        [TestMethod]
        public void InvalidStatusExceptionEmptyConstructorCanBeUsed()
        {
            // Act
            Exception caughtException = null;
            try
            {
                var result = new InvalidStatusException();
            }
            catch (Exception ex)
            {
                caughtException = ex;
            }

            // Assert
            Assert.IsNull(caughtException);
        }
    }
}
﻿//------------------------------------------------------------------------------
// <copyright file="InvalidDateExceptionTest.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.Web.Http.Tests
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Defines tests for the <see cref="InvalidDateException"/> class.
    /// </summary>
    [TestClass]
    public class InvalidDateExceptionTest
    {
        /// <summary>
        /// Verifies that the default, empty constructor can be used.
        /// </summary>
        [TestMethod]
        public void InvalidDateExceptionEmptyConstructorCanBeUsed()
        {
            // Act
            Exception caughtException = null;
            try
            {
                var result = new InvalidDateException();
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
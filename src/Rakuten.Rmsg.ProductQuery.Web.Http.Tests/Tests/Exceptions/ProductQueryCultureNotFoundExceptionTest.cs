﻿//------------------------------------------------------------------------------
// <copyright file="ProductQueryCultureNotFoundExceptionTest.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.Web.Http.Tests
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Defines tests for the <see cref="ProductQueryCultureNotFoundException"/> class.
    /// </summary>
    [TestClass]
    public class ProductQueryCultureNotFoundExceptionTest
    {
        /// <summary>
        /// Verifies that the default, empty constructor can be used.
        /// </summary>
        [TestMethod]
        public void ProductQueryCultureNotFoundExceptionEmptyConstructorCanBeUsed()
        {
            // Act
            Exception caughtException = null;
            try
            {
                var result = new ProductQueryCultureNotFoundException();
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
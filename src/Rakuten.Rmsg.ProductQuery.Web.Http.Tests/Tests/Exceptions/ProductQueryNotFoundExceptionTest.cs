//------------------------------------------------------------------------------
// <copyright file="ProductQueryNotFoundExceptionTest.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.Web.Http.Tests
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Defines tests for the <see cref="ProductQueryNotFoundException"/> class.
    /// </summary>
    [TestClass]
    public class ProductQueryNotFoundExceptionTest
    {
        /// <summary>
        /// Verifies that the default, empty constructor can be used.
        /// </summary>
        [TestMethod]
        public void ProductQueryNotFoundExceptionEmptyConstructorCanBeUsed()
        {
            // Act
            Exception caughtException = null;
            try
            {
                var result = new ProductQueryNotFoundException();
            }
            catch (Exception ex)
            {
                caughtException = ex;
            }

            // Assert
            Assert.IsNull(caughtException);
        }

        /// <summary>
        /// Verifies that the <see cref="ProductQueryNotFoundException"/> has the 
        /// correct message when instantiated with an ID.
        /// </summary>
        [TestMethod]
        public void ProductQueryNotFoundExceptionHasCorrectMessageWhenIdSupplied()
        {
            // Act
            var id = Guid.NewGuid();
            var expectedMessage = string.Format("A product query with ID '{0}' cannot be found.", id.ToString());
            var result = new ProductQueryNotFoundException(id);

            // Assert
            Assert.AreEqual(expectedMessage, result.Message, true);
        }
    }
}
//------------------------------------------------------------------------------
// <copyright file="ProductQueryTest.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.Web.Http.Tests
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Defines tests for the <see cref="ProductQuery"/> class.
    /// </summary>
    [TestClass]
    public class ProductQueryTest
    {
        /// <summary>
        /// Verifies that the <see cref="ProductQuery"/> class returns the correct
        /// value for the Year property.
        /// </summary>
        [TestMethod]
        public void ProductQueryResourceReturnsCorrectYear()
        {
            // Arrange
            var dateCreated = DateTime.Now;

            // Act
            var result = new ProductQuery { DateCreated = dateCreated };

            // Assert
            Assert.AreEqual(dateCreated.Year.ToString("0000"), result.Year);
        }

        /// <summary>
        /// Verifies that the <see cref="ProductQuery"/> class returns the correct
        /// value for the Month property.
        /// </summary>
        [TestMethod]
        public void ProductQueryResourceReturnsCorrectMonth()
        {
            // Arrange
            var dateCreated = DateTime.Now;

            // Act
            var result = new ProductQuery { DateCreated = dateCreated };

            // Assert
            Assert.AreEqual(dateCreated.Month.ToString("00"), result.Month);
        }

        /// <summary>
        /// Verifies that the <see cref="ProductQuery"/> class returns the correct
        /// value for the Day property.
        /// </summary>
        [TestMethod]
        public void ProductQueryResourceReturnsCorrectDay()
        {
            // Arrange
            var dateCreated = DateTime.Now;

            // Act
            var result = new ProductQuery { DateCreated = dateCreated };

            // Assert
            Assert.AreEqual(dateCreated.Day.ToString("00"), result.Day);
        }

        /// <summary>
        /// Verifies that the <see cref="ProductQuery"/> class returns the correct
        /// value for the Hour property.
        /// </summary>
        [TestMethod]
        public void ProductQueryResourceReturnsCorrectHour()
        {
            // Arrange
            var dateCreated = DateTime.Now;

            // Act
            var result = new ProductQuery { DateCreated = dateCreated };

            // Assert
            Assert.AreEqual(dateCreated.Hour.ToString("00"), result.Hour);
        }

        /// <summary>
        /// Verifies that the <see cref="ProductQuery"/> class returns the correct
        /// value for the Minute property.
        /// </summary>
        [TestMethod]
        public void ProductQueryResourceReturnsCorrectMinute()
        {
            // Arrange
            var dateCreated = DateTime.Now;

            // Act
            var result = new ProductQuery { DateCreated = dateCreated };

            // Assert
            Assert.AreEqual(dateCreated.Minute.ToString("00"), result.Minute);
        }

        /// <summary>
        /// Verifies that the <see cref="ProductQuery"/> class returns the correct
        /// value for the Year property.
        /// </summary>
        [TestMethod]
        public void ProductQueryResourceReturnsCorrectYearWhenDateCreatedHasNoValue()
        {
            // Arrange
            var dateCreated = DateTime.Now;

            // Act
            var result = new ProductQuery();

            // Assert
            Assert.AreEqual(null, result.Year);
        }

        /// <summary>
        /// Verifies that the <see cref="ProductQuery"/> class returns the correct
        /// value for the Month property.
        /// </summary>
        [TestMethod]
        public void ProductQueryResourceReturnsCorrectMonthWhenDateCreatedHasNoValue()
        {
            // Arrange
            var dateCreated = DateTime.Now;

            // Act
            var result = new ProductQuery();

            // Assert
            Assert.AreEqual(null, result.Month);
        }

        /// <summary>
        /// Verifies that the <see cref="ProductQuery"/> class returns the correct
        /// value for the Day property.
        /// </summary>
        [TestMethod]
        public void ProductQueryResourceReturnsCorrectDayWhenDateCreatedHasNoValue()
        {
            // Arrange
            var dateCreated = DateTime.Now;

            // Act
            var result = new ProductQuery();

            // Assert
            Assert.AreEqual(null, result.Day);
        }

        /// <summary>
        /// Verifies that the <see cref="ProductQuery"/> class returns the correct
        /// value for the Hour property.
        /// </summary>
        [TestMethod]
        public void ProductQueryResourceReturnsCorrectHourWhenDateCreatedHasNoValue()
        {
            // Arrange
            var dateCreated = DateTime.Now;

            // Act
            var result = new ProductQuery();

            // Assert
            Assert.AreEqual(null, result.Hour);
        }

        /// <summary>
        /// Verifies that the <see cref="ProductQuery"/> class returns the correct
        /// value for the Minute property.
        /// </summary>
        [TestMethod]
        public void ProductQueryResourceReturnsCorrectMinuteWhenDateCreatedHasNoValue()
        {
            // Arrange
            var dateCreated = DateTime.Now;

            // Act
            var result = new ProductQuery();

            // Assert
            Assert.AreEqual(null, result.Minute);
        }
    }
}
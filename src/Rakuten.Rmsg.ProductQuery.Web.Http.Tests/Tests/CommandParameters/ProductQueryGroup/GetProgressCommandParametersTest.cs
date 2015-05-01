//------------------------------------------------------------------------------
// <copyright file="GetProgressCommandParametersTest.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.Web.Http.Tests
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Rakuten.Rmsg.ProductQuery.Web.Http.Commands;

    /// <summary>
    /// Defines tests for the <see cref="GetProgressCommandParameters"/> class.
    /// </summary>
    [TestClass]
    public class GetProgressCommandParametersTest
    {
        /// <summary>
        /// Verifies that 24 hour clock times are accepted.
        /// </summary>
        [TestMethod]
        public void GetProgressCommandParameterAccepts24HourClock()
        {
            // Act
            var parameters = new GetProgressCommandParameters(Guid.NewGuid().ToString(), "2011", "10", "11", "17", "20");

            // Assert
            Assert.AreEqual(17, parameters.DateTime.Hour);
        }

        /// <summary>
        /// Verifies that single digit days are accepted.
        /// </summary>
        [TestMethod]
        public void GetProgressCommandParameterAcceptsSingleDigitDay()
        {
            // Act
            var parameters = new GetProgressCommandParameters(Guid.NewGuid().ToString(), "2011", "10", "1", "12", "13");

            // Assert
            Assert.AreEqual(1, parameters.DateTime.Day);
        }

        /// <summary>
        /// Verifies that single digit hours are accepted.
        /// </summary>
        [TestMethod]
        public void GetProgressCommandParameterAcceptsSingleDigitHour()
        {
            // Act
            var parameters = new GetProgressCommandParameters(Guid.NewGuid().ToString(), "2011", "10", "11", "1", "13");

            // Assert
            Assert.AreEqual(1, parameters.DateTime.Hour);
        }

        /// <summary>
        /// Verifies that single digit minutes are accepted.
        /// </summary>
        [TestMethod]
        public void GetProgressCommandParameterAcceptsSingleDigitMinute()
        {
            // Act
            var parameters = new GetProgressCommandParameters(Guid.NewGuid().ToString(), "2011", "10", "11", "12", "7");

            // Assert
            Assert.AreEqual(7, parameters.DateTime.Minute);
        }

        /// <summary>
        /// Verifies that single digit months are accepted.
        /// </summary>
        [TestMethod]
        public void GetProgressCommandParameterAcceptsSingleDigitMonth()
        {
            // Act
            var parameters = new GetProgressCommandParameters(Guid.NewGuid().ToString(), "2011", "7", "11", "12", "13");

            // Assert
            Assert.AreEqual(7, parameters.DateTime.Month);
        }

        /// <summary>
        /// Verifies that single digit years are accepted.
        /// </summary>
        [TestMethod]
        public void GetProgressCommandParameterAcceptsSingleDigitYear()
        {
            // Act
            var parameters = new GetProgressCommandParameters(Guid.NewGuid().ToString(), "1", "10", "11", "12", "13");

            // Assert
            Assert.AreEqual(1, parameters.DateTime.Year);
        }

        /// <summary>
        /// Verifies that the <see cref="GetProgressCommandParameters"/> class correctly
        /// interprets times before 12 noon.  The ensures we take leading zeros into
        /// account as representing the hours of the time.
        /// </summary>
        [TestMethod]
        public void GetProgressCommandParameterCorrectlyInterpretsMorningTimes()
        {
            // Arrange
            var dateTime = new DateTime(2011, 07, 17, 0, 52, 0);

            // Act
            var parameters = new GetProgressCommandParameters(
                Guid.NewGuid().ToString(),
                dateTime.Year.ToString(),
                dateTime.Month.ToString(),
                dateTime.Day.ToString(),
                dateTime.Hour.ToString(),
                dateTime.Minute.ToString());

            // Assert
            Assert.AreEqual(dateTime.TimeOfDay, parameters.DateTime.TimeOfDay);
        }

        /// <summary>
        /// Verifies that the <see cref="GetProgressCommandParameters"/> class correctly
        /// interprets times before 1 am.  The ensures we take leading zeros into
        /// account as representing the hours of the time.
        /// </summary>
        [TestMethod]
        public void GetProgressCommandParameterCorrectlyInterpretsTimesBeforeOneAM()
        {
            // Arrange
            var dateTime = new DateTime(2011, 07, 11, 0, 59, 0);

            // Act
            var parameters = new GetProgressCommandParameters(
                Guid.NewGuid().ToString(),
                dateTime.Year.ToString(),
                dateTime.Month.ToString(),
                dateTime.Day.ToString(),
                dateTime.Hour.ToString(),
                dateTime.Minute.ToString());

            // Assert
            Assert.AreEqual(dateTime.TimeOfDay, parameters.DateTime.TimeOfDay);
        }

        /// <summary>
        /// Verifies that the <see cref="GetProgressCommandParameters"/> class constructs
        /// the correct object when all parameters are valid.
        /// </summary>
        [TestMethod]
        public void GetProgressCommandParameterReturnsCorrectObjectWhenAllParametersAreValid()
        {
            // Arrange
            Guid id = Guid.NewGuid();
            DateTime dateTime = new DateTime(2011, 10, 11, 12, 13, 0);

            // Act
            var parameters = new GetProgressCommandParameters(
                id.ToString(),
                dateTime.Year.ToString(),
                dateTime.Month.ToString(),
                dateTime.Day.ToString(),
                dateTime.Hour.ToString(),
                dateTime.Minute.ToString());

            // Assert
            Assert.AreEqual(id, parameters.Id);
            Assert.AreEqual(dateTime, parameters.DateTime);
        }

        /// <summary>
        /// Verifies that the <see cref="GetProgressCommandParameters"/> class throws
        /// the correct exception if instantiated with a negative day.
        /// </summary>
        [TestMethod]
        public void GetProgressCommandParameterThrowsInvalidDateExceptionWhenDayIsNegative()
        {
            // Act
            var caughtException = this.CreateInvalidParameters(Guid.NewGuid().ToString(), day: "-1");

            // Assert
            Assert.IsNotNull(caughtException);
            Assert.IsInstanceOfType(caughtException, typeof(InvalidDateException));
        }

        /// <summary>
        /// Verifies that the <see cref="GetProgressCommandParameters"/> class throws
        /// the correct exception if instantiated with a non-numeric day.
        /// </summary>
        [TestMethod]
        public void GetProgressCommandParameterThrowsInvalidDateExceptionWhenDayIsNotNumeric()
        {
            // Act
            var caughtException = this.CreateInvalidParameters(Guid.NewGuid().ToString(), day: "nan");

            // Assert
            Assert.IsNotNull(caughtException);
            Assert.IsInstanceOfType(caughtException, typeof(InvalidDateException));
        }

        /// <summary>
        /// Verifies that the <see cref="GetProgressCommandParameters"/> class throws
        /// the correct exception if instantiated with a day greater than 31.
        /// </summary>
        [TestMethod]
        public void GetProgressCommandParameterThrowsInvalidDateExceptionWhenDayIsTooLarge()
        {
            // Act
            var caughtException = this.CreateInvalidParameters(Guid.NewGuid().ToString(), day: "32");

            // Assert
            Assert.IsNotNull(caughtException);
            Assert.IsInstanceOfType(caughtException, typeof(InvalidDateException));
        }

        /// <summary>
        /// Verifies that the <see cref="GetProgressCommandParameters"/> class throws
        /// the correct exception if instantiated with a day that is 0.
        /// </summary>
        [TestMethod]
        public void GetProgressCommandParameterThrowsInvalidDateExceptionWhenDayIsZero()
        {
            // Act
            var caughtException = this.CreateInvalidParameters(Guid.NewGuid().ToString(), day: "00");
            
            // Assert
            Assert.IsNotNull(caughtException);
            Assert.IsInstanceOfType(caughtException, typeof(InvalidDateException));
        }

        /// <summary>
        /// Verifies that the <see cref="GetProgressCommandParameters"/> class throws
        /// the correct exception if instantiated with a negative hour.
        /// </summary>
        [TestMethod]
        public void GetProgressCommandParameterThrowsInvalidDateExceptionWhenHourIsNegative()
        {
            // Act
            var caughtException = this.CreateInvalidParameters(Guid.NewGuid().ToString(), hour: "-1");

            // Assert
            Assert.IsNotNull(caughtException);
            Assert.IsInstanceOfType(caughtException, typeof(InvalidDateException));
        }

        /// <summary>
        /// Verifies that the <see cref="GetProgressCommandParameters"/> class throws
        /// the correct exception if instantiated with a non-numeric hour.
        /// </summary>
        [TestMethod]
        public void GetProgressCommandParameterThrowsInvalidDateExceptionWhenHourIsNotNumeric()
        {
            // Act
            var caughtException = this.CreateInvalidParameters(Guid.NewGuid().ToString(), hour: "nan");

            // Assert
            Assert.IsNotNull(caughtException);
            Assert.IsInstanceOfType(caughtException, typeof(InvalidDateException));
        }

        /// <summary>
        /// Verifies that the <see cref="GetProgressCommandParameters"/> class throws
        /// the correct exception if instantiated with a hour greater than 12.
        /// </summary>
        [TestMethod]
        public void GetProgressCommandParameterThrowsInvalidDateExceptionWhenHourIsTooLarge()
        {
            // Act
            var caughtException = this.CreateInvalidParameters(Guid.NewGuid().ToString(), hour: "24");

            // Assert
            Assert.IsNotNull(caughtException);
            Assert.IsInstanceOfType(caughtException, typeof(InvalidDateException));
        }

        /// <summary>
        /// Verifies that the <see cref="GetProgressCommandParameters"/> class throws
        /// the correct exception if instantiated with a negative minute.
        /// </summary>
        [TestMethod]
        public void GetProgressCommandParameterThrowsInvalidDateExceptionWhenMinuteIsNegative()
        {
            // Act
            var caughtException = this.CreateInvalidParameters(Guid.NewGuid().ToString(), minute: "-1");

            // Assert
            Assert.IsNotNull(caughtException);
            Assert.IsInstanceOfType(caughtException, typeof(InvalidDateException));
        }

        /// <summary>
        /// Verifies that the <see cref="GetProgressCommandParameters"/> class throws
        /// the correct exception if instantiated with a non-numeric minute.
        /// </summary>
        [TestMethod]
        public void GetProgressCommandParameterThrowsInvalidDateExceptionWhenMinuteIsNotNumeric()
        {
            // Act
            var caughtException = this.CreateInvalidParameters(Guid.NewGuid().ToString(), minute: "nan");

            // Assert
            Assert.IsNotNull(caughtException);
            Assert.IsInstanceOfType(caughtException, typeof(InvalidDateException));
        }

        /// <summary>
        /// Verifies that the <see cref="GetProgressCommandParameters"/> class throws
        /// the correct exception if instantiated with a minute greater than 59.
        /// </summary>
        [TestMethod]
        public void GetProgressCommandParameterThrowsInvalidDateExceptionWhenMinuteIsTooLarge()
        {
            // Act
            var caughtException = this.CreateInvalidParameters(Guid.NewGuid().ToString(), minute: "60");

            // Assert
            Assert.IsNotNull(caughtException);
            Assert.IsInstanceOfType(caughtException, typeof(InvalidDateException));
        }

        /// <summary>
        /// Verifies that the <see cref="GetProgressCommandParameters"/> class throws
        /// the correct exception if instantiated with a negative month.
        /// </summary>
        [TestMethod]
        public void GetProgressCommandParameterThrowsInvalidDateExceptionWhenMonthIsNegative()
        {
            // Act
            var caughtException = this.CreateInvalidParameters(Guid.NewGuid().ToString(), month: "-1");

            // Assert
            Assert.IsNotNull(caughtException);
            Assert.IsInstanceOfType(caughtException, typeof(InvalidDateException));
        }

        /// <summary>
        /// Verifies that the <see cref="GetProgressCommandParameters"/> class throws
        /// the correct exception if instantiated with a non-numeric month.
        /// </summary>
        [TestMethod]
        public void GetProgressCommandParameterThrowsInvalidDateExceptionWhenMonthIsNotNumeric()
        {
            // Act
            var caughtException = this.CreateInvalidParameters(Guid.NewGuid().ToString(), month: "nan");

            // Assert
            Assert.IsNotNull(caughtException);
            Assert.IsInstanceOfType(caughtException, typeof(InvalidDateException));
        }

        /// <summary>
        /// Verifies that the <see cref="GetProgressCommandParameters"/> class throws
        /// the correct exception if instantiated with a month greater than 12.
        /// </summary>
        [TestMethod]
        public void GetProgressCommandParameterThrowsInvalidDateExceptionWhenMonthIsTooLarge()
        {
            // Act
            var caughtException = this.CreateInvalidParameters(Guid.NewGuid().ToString(), month: "13");

            // Assert
            Assert.IsNotNull(caughtException);
            Assert.IsInstanceOfType(caughtException, typeof(InvalidDateException));
        }

        /// <summary>
        /// Verifies that the <see cref="GetProgressCommandParameters"/> class throws
        /// the correct exception if instantiated with a month that is zero.
        /// </summary>
        [TestMethod]
        public void GetProgressCommandParameterThrowsInvalidDateExceptionWhenMonthIsZero()
        {
            // Act
            var caughtException = this.CreateInvalidParameters(Guid.NewGuid().ToString(), month: "00");

            // Assert
            Assert.IsNotNull(caughtException);
            Assert.IsInstanceOfType(caughtException, typeof(InvalidDateException));
        }

        /// <summary>
        /// Verifies that the <see cref="GetProgressCommandParameters"/> class throws
        /// the correct exception if instantiated with a negative year.
        /// </summary>
        [TestMethod]
        public void GetProgressCommandParameterThrowsInvalidDateExceptionWhenYearIsNegative()
        {
            // Act
            var caughtException = this.CreateInvalidParameters(Guid.NewGuid().ToString(), year: "-1");

            // Assert
            Assert.IsNotNull(caughtException);
            Assert.IsInstanceOfType(caughtException, typeof(InvalidDateException));
        }

        /// <summary>
        /// Verifies that the <see cref="GetProgressCommandParameters"/> class throws
        /// the correct exception if instantiated with a non-numeric year.
        /// </summary>
        [TestMethod]
        public void GetProgressCommandParameterThrowsInvalidDateExceptionWhenYearIsNotNumeric()
        {
            // Act
            var caughtException = this.CreateInvalidParameters(Guid.NewGuid().ToString(), year: "nan");

            // Assert
            Assert.IsNotNull(caughtException);
            Assert.IsInstanceOfType(caughtException, typeof(InvalidDateException));
        }

        /// <summary>
        /// Verifies that the <see cref="GetProgressCommandParameters"/> class throws
        /// the correct exception if instantiated with a year that is 0.
        /// </summary>
        [TestMethod]
        public void GetProgressCommandParameterThrowsInvalidDateExceptionWhenYearIsZero()
        {
            // Act
            var caughtException = this.CreateInvalidParameters(Guid.NewGuid().ToString(), year: "00");

            // Assert
            Assert.IsNotNull(caughtException);
            Assert.IsInstanceOfType(caughtException, typeof(InvalidDateException));
        }

        /// <summary>
        /// Verifies that the <see cref="GetProgressCommandParameters"/> class throws
        /// the correct exception if instantiated with an invalid identifier
        /// </summary>
        [TestMethod]
        public void GetProgressCommandParameterThrowsInvalidGuidException()
        {
            // Act
            var caughtException = this.CreateInvalidParameters("not-a-guid");

            // Assert
            Assert.IsNotNull(caughtException);
            Assert.IsInstanceOfType(caughtException, typeof(InvalidGuidException));
        }

        /// <summary>
        /// Tries to create an instance of <see cref="GetProgressCommandParameters"/> with
        /// with the given parameters with the expectation that they will be invalid and
        /// returns the thrown exception.
        /// </summary>
        /// <param name="id">The unique identifier for the product query group.</param>
        /// <param name="year">The year of the point in time.</param>
        /// <param name="month">The month of the point in time.</param>
        /// <param name="day">The day of the point in time.</param>
        /// <param name="hour">The hour of the point in time.</param>
        /// <param name="minute">The minute of the point in time.</param>
        /// <returns>The exception thrown when trying to instantiate the object.</returns>
        private Exception CreateInvalidParameters(
            string id = null,
            string year = null,
            string month = null,
            string day = null,
            string hour = null,
            string minute = null)
        {
            Exception caughtException = null;

            try
            {
                var parameters = new GetProgressCommandParameters(
                    id ?? Guid.NewGuid().ToString(),
                    year ?? "2011",
                    month ?? "10",
                    day ?? "11",
                    hour ?? "12",
                    minute ?? "13");
            }
            catch (Exception ex)
            {
                caughtException = ex;
            }

            return caughtException;
        }
    }
}
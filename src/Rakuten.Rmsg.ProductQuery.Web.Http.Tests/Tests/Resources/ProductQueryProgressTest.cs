//------------------------------------------------------------------------------
// <copyright file="ProductQueryProgressTest.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.Web.Http.Tests
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows.Media.Imaging;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Rakuten.Rmsg.ProductQuery.Configuration.Fakes;
    using Rakuten.Rmsg.ProductQuery.Web.Http.Commands;

    /// <summary>
    /// Defines tests for the <see cref="ProductQueryProgress"/> class.
    /// </summary>
    [TestClass]
    public class ProductQueryProgressTest
    {
        /// <summary>
        /// Verifies the percentage for a product query in the "completed" status that
        /// has items of which all are completed and with no percentage proportion allocated for finalization
        /// </summary>
        [TestMethod]
        public void PercentageForCompletedProductQueryWithAllCompletedItemsAndFinalizationProporationIsCorrect()
        {
            // Arange
            var result = new ProductQueryProgress(1, ProductQueryStatus.Completed, 10, 10, 0.05m);

            // Assert
            Assert.AreEqual(1m, result.PercentageComplete);
        }

        /// <summary>
        /// Verifies the percentage for a product query in the "completed" status that
        /// has items of which all are completed and with no percentage proportion allocated for finalization
        /// </summary>
        [TestMethod]
        public void PercentageForCompletedProductQueryWithAllCompletedItemsAndNoFinalizationProporationIsCorrect()
        {
            // Arange
            var result = new ProductQueryProgress(1, ProductQueryStatus.Completed, 10, 10, 0);

            // Assert
            Assert.AreEqual(1m, result.PercentageComplete);
        }

        /// <summary>
        /// Verifies the percentage for a product query in the "completed" status that 
        /// has items of which none are completed and with no percentage proportion allocated for finalization
        /// </summary>
        [TestMethod]
        public void PercentageForCompletedProductQueryWithNoCompletedItemsAndFinalizationProporationIsCorrect()
        {
            // Arange
            var result = new ProductQueryProgress(1, ProductQueryStatus.Completed, 10, 0, 0.05m);

            // Assert
            Assert.AreEqual(1m, result.PercentageComplete);
        }

        /// <summary>
        /// Verifies the percentage for a product query in the "completed" status that 
        /// has items of which none are completed and with no percentage proportion allocated for finalization
        /// </summary>
        [TestMethod]
        public void PercentageForCompletedProductQueryWithNoCompletedItemsAndNoFinalizationProporationIsCorrect()
        {
            // Arange
            var result = new ProductQueryProgress(1, ProductQueryStatus.Completed, 10, 0, 0);

            // Assert
            Assert.AreEqual(1m, result.PercentageComplete);
        }

        /// <summary>
        /// Verifies the percentage for a product query in the "completed" status that has no items
        /// and with no percentage proportion allocated for finalization
        /// </summary>
        [TestMethod]
        public void PercentageForCompletedProductQueryWithNoItemsAndFinalizationProporationIsCorrect()
        {
            // Arange
            var result = new ProductQueryProgress(1, ProductQueryStatus.Completed, 0, 0, 0.05m);

            // Assert
            Assert.AreEqual(1m, result.PercentageComplete);
        }

        /// <summary>
        /// Verifies the percentage for a product query in the "completed" status that has no items
        /// and with no percentage proportion allocated for finalization
        /// </summary>
        [TestMethod]
        public void PercentageForCompletedProductQueryWithNoItemsAndNoFinalizationProporationIsCorrect()
        {
            // Arange
            var result = new ProductQueryProgress(1, ProductQueryStatus.Completed, 0, 0, 0);

            // Assert
            Assert.AreEqual(1m, result.PercentageComplete);
        }

        /// <summary>
        /// Verifies the percentage for a product query in the "completed" status that
        /// has items of which some are completed and with no percentage proportion allocated for finalization
        /// </summary>
        [TestMethod]
        public void PercentageForCompletedProductQueryWithSomeCompletedItemsAndFinalizationProporationIsCorrect()
        {
            // Arange
            var result = new ProductQueryProgress(1, ProductQueryStatus.Completed, 10, 5, 0.05m);

            // Assert
            Assert.AreEqual(1m, result.PercentageComplete);
        }

        /// <summary>
        /// Verifies the percentage for a product query in the "completed" status that
        /// has items of which some are completed and with no percentage proportion allocated for finalization
        /// </summary>
        [TestMethod]
        public void PercentageForCompletedProductQueryWithSomeCompletedItemsAndNoFinalizationProporationIsCorrect()
        {
            // Arange
            var result = new ProductQueryProgress(1, ProductQueryStatus.Completed, 10, 5, 0);

            // Assert
            Assert.AreEqual(1m, result.PercentageComplete);
        }

        /// <summary>
        /// Verifies the percentage for a product query in the "new" status that
        /// has items of which all are completed and with no percentage proportion allocated for finalization
        /// </summary>
        [TestMethod]
        public void PercentageForNewProductQueryWithAllCompletedItemsAndFinalizationProporationIsCorrect()
        {
            // Arange
            var result = new ProductQueryProgress(1, ProductQueryStatus.New, 10, 10, 0.05m);

            // Assert
            Assert.AreEqual(0m, result.PercentageComplete);
        }

        /// <summary>
        /// Verifies the percentage for a product query in the "new" status that
        /// has items of which all are completed and with no percentage proportion allocated for finalization
        /// </summary>
        [TestMethod]
        public void PercentageForNewProductQueryWithAllCompletedItemsAndNoFinalizationProporationIsCorrect()
        {
            // Arange
            var result = new ProductQueryProgress(1, ProductQueryStatus.New, 10, 10, 0);

            // Assert
            Assert.AreEqual(0m, result.PercentageComplete);
        }

        /// <summary>
        /// Verifies the percentage for a product query in the "new" status that has
        /// items of which none have been completed and with no percentage proportion allocated for finalization
        /// </summary>
        [TestMethod]
        public void PercentageForNewProductQueryWithNoCompletedItemsAndFinalizationProporationIsCorrect()
        {
            // Arange
            var result = new ProductQueryProgress(1, ProductQueryStatus.New, 10, 0, 0.05m);

            // Assert
            Assert.AreEqual(0m, result.PercentageComplete);
        }

        /// <summary>
        /// Verifies the percentage for a product query in the "new" status that has
        /// items of which none have been completed and with no percentage proportion allocated for finalization
        /// </summary>
        [TestMethod]
        public void PercentageForNewProductQueryWithNoCompletedItemsAndNoFinalizationProporationIsCorrect()
        {
            // Arange
            var result = new ProductQueryProgress(1, ProductQueryStatus.New, 10, 0, 0);

            // Assert
            Assert.AreEqual(0m, result.PercentageComplete);
        }

        /// <summary>
        /// Verifies the percentage for a product query in the "new" status that has no items
        /// and with no percentage proportion allocated for finalization
        /// </summary>
        [TestMethod]
        public void PercentageForNewProductQueryWithNoItemsAndFinalizationProporationIsCorrect()
        {
            // Arange
            var result = new ProductQueryProgress(1, ProductQueryStatus.New, 0, 0, 0.05m);

            // Assert
            Assert.AreEqual(0m, result.PercentageComplete);
        }

        /// <summary>
        /// Verifies the percentage for a product query in the "new" status that has no items
        /// and with no percentage proportion allocated for finalization
        /// </summary>
        [TestMethod]
        public void PercentageForNewProductQueryWithNoItemsAndNoFinalizationProporationIsCorrect()
        {
            // Arange
            var result = new ProductQueryProgress(1, ProductQueryStatus.New, 0, 0, 0);

            // Assert
            Assert.AreEqual(0m, result.PercentageComplete);
        }

        /// <summary>
        /// Verifies the percentage for a product query in the "new" status that
        /// has items of which some are completed and with no percentage proportion allocated for finalization
        /// </summary>
        [TestMethod]
        public void PercentageForNewProductQueryWithSomeCompletedItemsAndFinalizationProporationIsCorrect()
        {
            // Arange
            var result = new ProductQueryProgress(1, ProductQueryStatus.New, 10, 5, 0.05m);

            // Assert
            Assert.AreEqual(0m, result.PercentageComplete);
        }

        /// <summary>
        /// Verifies the percentage for a product query in the "new" status that
        /// has items of which some are completed and with no percentage proportion allocated for finalization
        /// </summary>
        [TestMethod]
        public void PercentageForNewProductQueryWithSomeCompletedItemsAndNoFinalizationProporationIsCorrect()
        {
            // Arange
            var result = new ProductQueryProgress(1, ProductQueryStatus.New, 10, 5, 0);

            // Assert
            Assert.AreEqual(0m, result.PercentageComplete);
        }

        /// <summary>
        /// Verifies that the default percentage is zero.
        /// </summary>
        [TestMethod]
        public void PercentageForDefaultSwitchCaseReturnsZero()
        {
            // Act
            var result = new ProductQueryProgress(1, (ProductQueryStatus)(-1), 10, 10, 0m);

            // Assert
            Assert.AreEqual(0, result.PercentageComplete);
        }

        /// <summary>
        /// Verifies the percentage for a product query in the "submitted" status that
        /// has items of which all are completed and with no percentage proportion allocated for finalization
        /// </summary>
        [TestMethod]
        public void PercentageForSubmittedProductQueryWithAllCompletedItemsAndFinalizationProporationIsCorrect()
        {
            // Arange
            var result = new ProductQueryProgress(1, ProductQueryStatus.Submitted, 10, 10, 0.05m);

            // Assert
            Assert.AreEqual(0.95m, result.PercentageComplete);
        }

        /// <summary>
        /// Verifies the percentage for a product query in the "submitted" status that
        /// has items of which all are completed and with no percentage proportion allocated for finalization
        /// </summary>
        [TestMethod]
        public void PercentageForSubmittedProductQueryWithAllCompletedItemsAndNoFinalizationProporationIsCorrect()
        {
            // Arange
            var result = new ProductQueryProgress(1, ProductQueryStatus.Submitted, 10, 10, 0);

            // Assert
            Assert.AreEqual(1m, result.PercentageComplete);
        }

        /// <summary>
        /// Verifies the percentage for a product query in the "submitted" status that has
        /// items of which none have been completed and with no percentage proportion allocated for finalization
        /// </summary>
        [TestMethod]
        public void PercentageForSubmittedProductQueryWithNoCompletedItemsAndFinalizationProporationIsCorrect()
        {
            // Arange
            var result = new ProductQueryProgress(1, ProductQueryStatus.Submitted, 10, 0, 0.05m);

            // Assert
            Assert.AreEqual(0m, result.PercentageComplete);
        }

        /// <summary>
        /// Verifies the percentage for a product query in the "submitted" status that has
        /// items of which none have been completed and with no percentage proportion allocated for finalization
        /// </summary>
        [TestMethod]
        public void PercentageForSubmittedProductQueryWithNoCompletedItemsAndNoFinalizationProporationIsCorrect()
        {
            // Arange
            var result = new ProductQueryProgress(1, ProductQueryStatus.Submitted, 10, 0, 0);

            // Assert
            Assert.AreEqual(0m, result.PercentageComplete);
        }

        /// <summary>
        /// Verifies the percentage for a product query in the "submitted" status that has no items
        /// and with no percentage proportion allocated for finalization
        /// </summary>
        [TestMethod]
        public void PercentageForSubmittedProductQueryWithNoItemsAndFinalizationProporationIsCorrect()
        {
            // Arange
            var result = new ProductQueryProgress(1, ProductQueryStatus.Submitted, 0, 0, 0.05m);

            // Assert
            Assert.AreEqual(0m, result.PercentageComplete);
        }

        /// <summary>
        /// Verifies the percentage for a product query in the "submitted" status that has no items
        /// and with no percentage proportion allocated for finalization
        /// </summary>
        [TestMethod]
        public void PercentageForSubmittedProductQueryWithNoItemsAndNoFinalizationProporationIsCorrect()
        {
            // Arange
            var result = new ProductQueryProgress(1, ProductQueryStatus.Submitted, 0, 0, 0);

            // Assert
            Assert.AreEqual(0m, result.PercentageComplete);
        }

        /// <summary>
        /// Verifies the percentage for a product query in the "submitted" status that
        /// has items of which some are completed and with no percentage proportion allocated for finalization
        /// </summary>
        [TestMethod]
        public void PercentageForSubmittedProductQueryWithSomeCompletedItemsAndFinalizationProporationIsCorrect()
        {
            // Arange
            var result = new ProductQueryProgress(1, ProductQueryStatus.Submitted, 10, 5, 0.05m);

            // Assert
            Assert.AreEqual(0.475m, result.PercentageComplete);
        }

        /// <summary>
        /// Verifies the percentage for a product query in the "submitted" status that
        /// has items of which some are completed and with no percentage proportion allocated for finalization
        /// </summary>
        [TestMethod]
        public void PercentageForSubmittedProductQueryWithSomeCompletedItemsAndNoFinalizationProporationIsCorrect()
        {
            // Arange
            var result = new ProductQueryProgress(1, ProductQueryStatus.Submitted, 10, 5, 0);

            // Assert
            Assert.AreEqual(0.5m, result.PercentageComplete);
        }
    }
}
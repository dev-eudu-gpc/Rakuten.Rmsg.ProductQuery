//------------------------------------------------------------------------------
// <copyright file="ProductQueryControllerTest.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.Web.Http.Tests
{
    using System;
    using System.Globalization;
    using System.Net.Http;
    using System.Net.Http.Fakes;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Web.Http;
    using System.Web.Http.Controllers;
    using System.Web.Http.Results;
    using System.Web.Http.Routing;
    using Microsoft.QualityTools.Testing.Fakes;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Microsoft.WindowsAzure.Storage;
    using Rakuten.Fakes;
    using Rakuten.Gpc.Api;
    using Rakuten.Rmsg.ProductQuery.Web.Http.Commands;

    /// <summary>
    /// Defines tests for the <see cref="ProductQueryController"/> class.
    /// </summary>
    [TestClass]
    public class ProductQueryControllerTest
    {
        /// <summary>
        /// Verifies that the controller returns the correct response type when a product
        /// query is successfully created.
        /// </summary>
        /// <returns>A task that performs the test.</returns>
        [TestMethod]
        public async Task CreateProductQueryReturnsCorrectResponseTypeWhenProductQuerySucessfullyCreated()
        {
            // Arrange
            var id = Guid.NewGuid();
            var productQuery = ProductQueryFactory.CreateProduct(id);
            var createCommand = new StubICommand<CreateCommandParameters, Task<ProductQuery>>
            {
                ExecuteT0 = parameters => Task.Run(() => productQuery)
            };
            var getCommand = new StubICommand<GetCommandParameters, Task<ProductQuery>>
            {
                ExecuteT0 = parameters => Task.Run(() => null as ProductQuery)
            };

            ProductQueryController controller = this.CreateController(getCommand, createCommand);

            // Act
            IHttpActionResult result = await controller.PutAsync(id.ToString(), productQuery.CultureName, null);

            // Assert
            Assert.IsInstanceOfType(result, typeof(CreatedNegotiatedContentResult<ProductQuery>));
        }

        /// <summary>
        /// Verifies that an attempt to create a product query that already exists but is a
        /// different culture to that specified returns the correct response.
        /// </summary>
        /// <returns>A task that performs the test.</returns>
        [TestMethod]
        public async Task CreateProductQueryThatExistsInDifferentCultureReturnsSeeOtherResult()
        {
            using (ShimsContext.Create())
            {
                // Arrange shims
                ////ShimSeeOtherResult.ConstructorUriHttpRequestMessage = (@this, uri, message) =>
                ////{
                ////    ConstructorInfo constructor = typeof(SeeOtherResult).GetConstructor(new Type[] { typeof(SeeOtherResult) });
                ////    constructor.Invoke(@this, new object[] { new Uri("http://somewhere.com"), new StubHttpRequestMessage() });
                ////};
                ////ShimSeeOtherResult.ConstructorUriInt32HttpRequestMessage = (@this, uri, retryAfter, message) =>
                ////{
                ////    ConstructorInfo constructor = typeof(SeeOtherResult).GetConstructor(new Type[] { typeof(SeeOtherResult) });
                ////    constructor.Invoke(@this, new object[] { new Uri("http://somewhere.com"), 60, new StubHttpRequestMessage() });
                ////};

                ////ShimSeeOtherResult.AllInstances.Execute = message =>
                ////{
                ////    return new StubHttpResponseMessage();
                ////};

                // Arrange
                var id = Guid.NewGuid();
                var requestedCulture = "en-US";
                var actualCulture = "ja-JP";
                var expectedLocationHeader = string.Format("product-query/{0}/culture/{1}", id, actualCulture);
                var getCommand = new StubICommand<GetCommandParameters, Task<ProductQuery>>
                {
                    ExecuteT0 = parameters => Task.Run(() => ProductQueryFactory.CreateProduct(id, actualCulture))
                };

                ProductQueryController controller = this.CreateController(getCommand: getCommand);

                // Act
                IHttpActionResult result = await controller.PutAsync(id.ToString(), requestedCulture, null);
                HttpResponseMessage response = await result.ExecuteAsync(new CancellationToken());

                // Assert
                Assert.IsInstanceOfType(result, typeof(SeeOtherResult));
                Assert.IsNotNull(response.Headers.Location);
                Assert.AreEqual(expectedLocationHeader, response.Headers.Location.ToString(), false);
            }
        }

        /// <summary>
        /// Verifies that an attempt to create a product query that already exists in the 
        /// culture that has been specified returns the correct response.
        /// </summary>
        /// <returns>A task that performs the test.</returns>
        [TestMethod]
        public async Task CreateProductQueryThatExistsInSpecifiedCultureReturnsOkNegotiatedContentResult()
        {
            // Arrange
            var id = Guid.NewGuid();
            var culture = "en-US";
            var getCommand = new StubICommand<GetCommandParameters, Task<ProductQuery>>
            {
                ExecuteT0 = parameters => Task.Run(() => ProductQueryFactory.CreateProduct(id, culture))
            };

            ProductQueryController controller = this.CreateController(getCommand: getCommand);

            // Act
            IHttpActionResult result = await controller.PutAsync(id.ToString(), culture, null);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<ProductQuery>));
        }

        /// <summary>
        /// Verifies that the controller throws an Internal Server Error if an exception is
        /// raised in the create command.
        /// </summary>
        /// <returns>A task that performs the test.</returns>
        [TestMethod]
        public async Task CreateProductQueryThrowsCorrectExceptionWhenCreateCommandFails()
        {
            // Arrange
            var id = Guid.NewGuid();
            var culture = "en-US";
            var createCommand = new StubICommand<CreateCommandParameters, Task<ProductQuery>>
            {
                ExecuteT0 = parameters => { throw new StorageException(); }
            };
            var getCommand = new StubICommand<GetCommandParameters, Task<ProductQuery>>
            {
                ExecuteT0 = parameters => Task.Run(() => null as ProductQuery)
            };

            ProductQueryController controller = this.CreateController(getCommand, createCommand);

            // Act
            Exception caughtException = null;
            try
            {
                IHttpActionResult result = await controller.PutAsync(id.ToString(), culture, null);
            }
            catch (Exception ex)
            {
                caughtException = ex;
            }

            // Assert
            Assert.IsNotNull(caughtException);
            Assert.IsInstanceOfType(caughtException, typeof(InternalServerException));
        }

        /// <summary>
        /// Verifies that an attempt to create a product query with a culture
        /// that is not a valid culture causes the correct exception.
        /// </summary>
        /// <returns>A task that performs the test.</returns>
        [TestMethod]
        public async Task CreateProductQueryWithInvalidCultureThrowsCorrectException()
        {
            // Arrange
            var id = Guid.NewGuid();
            var culture = "not-a-culture";

            ProductQueryController controller = this.CreateController();

            // Act
            Exception caughtException = null;
            try
            {
                IHttpActionResult result = await controller.PutAsync(id.ToString(), culture, null);
            }
            catch (BadRequestException ex)
            {
                caughtException = ex;
            }

            // Assert
            Assert.IsNotNull(caughtException);
            Assert.IsNotNull(caughtException.InnerException);
            Assert.IsInstanceOfType(caughtException.InnerException, typeof(InvalidCultureException));
        }

        /// <summary>
        /// Verifies that an attempt to create a product query with an identifier
        /// that is not a GUID causes the correct exception
        /// </summary>
        /// <returns>A task that performs the test.</returns>
        [TestMethod]
        public async Task CreateProductQueryWithInvalidGuidThrowsCorrectException()
        {
            // Arrange
            var id = "not-a-guid";
            var culture = "en-US";

            ProductQueryController controller = this.CreateController();

            // Act
            Exception caughtException = null;
            try
            {
                IHttpActionResult result = await controller.PutAsync(id.ToString(), culture, null);
            }
            catch (BadRequestException ex)
            {
                caughtException = ex;
            }

            // Assert
            Assert.IsNotNull(caughtException);
            Assert.IsNotNull(caughtException.InnerException);
            Assert.IsInstanceOfType(caughtException.InnerException, typeof(InvalidGuidException));
        }

        /// <summary>
        /// Verifies that an attempt to set a product query as ready for processing
        /// with everything valid returns the correct result.
        /// </summary>
        /// <returns>A task that performs the test.</returns>
        [TestMethod]
        public async Task ReadyForProcessingReturnsCorrectResultWhenEverythingIsValid()
        {
            // Arrange
            var id = Guid.NewGuid();
            var culture = "en-US";
            var productQuery = ProductQueryFactory.CreateProduct(id, culture, ProductQueryStatus.Submitted);

            var readyForProcessingCommand = new StubICommand<ReadyForProcessingCommandParameters, Task<ProductQuery>>
            {
                ExecuteT0 = parameters => Task.Run(() => productQuery)
            };

            ProductQueryController controller = this.CreateController(readyForProcessingCommand: readyForProcessingCommand);

            // Act
            IHttpActionResult result = await controller.PutAsync(id.ToString(), culture, productQuery);

            // Assert
            Assert.IsInstanceOfType(result, typeof(AcceptedNegotiatedContentResult<ProductQuery>));
        }

        /// <summary>
        /// Verifies that an attempt to set a product query as ready for processing
        /// for a valid GUID but different returns the correct result.
        /// </summary>
        /// <returns>A task that performs the test.</returns>
        [TestMethod]
        public async Task ReadyForProcessingReturnsCorrectResultWhenProductQueryExistsInDifferentCulture()
        {
            // Arrange
            var id = Guid.NewGuid();
            var requestedCulture = "en-US";
            var actualCulture = "ja-JP";
            var submittedProductQuery = ProductQueryFactory.CreateProduct(id, requestedCulture, ProductQueryStatus.Submitted);
            var actualProductQuery = ProductQueryFactory.CreateProduct(id, actualCulture);
            var readyForProcessingCommand = new StubICommand<ReadyForProcessingCommandParameters, Task<ProductQuery>>
            {
                ExecuteT0 = parameters =>
                {
                    throw new ProductQueryCultureNotFoundException(
                        id,
                        CultureInfo.GetCultureInfo(requestedCulture),
                        actualProductQuery);
                }
            };

            ProductQueryController controller = this.CreateController(readyForProcessingCommand: readyForProcessingCommand);

            // Act
            IHttpActionResult result = await controller.PutAsync(id.ToString(), requestedCulture, submittedProductQuery);
            HttpResponseMessage response = await result.ExecuteAsync(new CancellationToken());

            // Assert
            Assert.IsInstanceOfType(result, typeof(SeeOtherResult));
            Assert.IsNotNull(response.Headers.Location);
            Assert.AreEqual(actualProductQuery.GetUri().ToString(), response.Headers.Location.ToString(), false);
        }

        /// <summary>
        /// Verifies that an attempt to set a product query as ready for processing
        /// and the product query is not found
        /// </summary>
        /// <returns>A task that performs the test.</returns>
        [TestMethod]
        public async Task ReadyForProcessingThrowsCorrectExceptionWhenProductQueryIsNotFound()
        {
            // Arrange
            var id = Guid.NewGuid();
            var productQuery = ProductQueryFactory.CreateProduct(id: id, status: ProductQueryStatus.Submitted);
            var readyForProcessingCommand = new StubICommand<ReadyForProcessingCommandParameters, Task<ProductQuery>>
            {
                ExecuteT0 = parameters => { throw new ProductQueryNotFoundException(); }
            };

            ProductQueryController controller = this.CreateController(readyForProcessingCommand: readyForProcessingCommand);

            // Act
            Exception caughtException = null;
            try
            {
                var result = await controller.PutAsync(id.ToString(), "en-US", productQuery);
            }
            catch (Exception ex)
            {
                caughtException = ex;
            }

            // Assert
            Assert.IsNotNull(caughtException);
            Assert.IsInstanceOfType(caughtException, typeof(ObjectNotFoundException));
            Assert.IsNotNull(caughtException.InnerException);
            Assert.IsInstanceOfType(caughtException.InnerException, typeof(ProductQueryNotFoundException));
        }

        /// <summary>
        /// Verifies that an attempt to set a product query as ready for processing
        /// with a status other than submitted throws the correct exception
        /// </summary>
        /// <returns>A task that performs the test.</returns>
        [TestMethod]
        public async Task ReadyForProcessingThrowsCorrectExceptionWhenProvidedStatusIsNotSubmitted()
        {
            // Arrange
            var id = Guid.NewGuid();
            var productQuery = ProductQueryFactory.CreateProduct(id: id, status: ProductQueryStatus.New);

            ProductQueryController controller = this.CreateController();

            // Act
            Exception caughtException = null;
            try
            {
                var result = await controller.PutAsync(id.ToString(), "en-US", productQuery);
            }
            catch (Exception ex)
            {
                caughtException = ex;
            }

            // Assert
            Assert.IsNotNull(caughtException);
            Assert.IsInstanceOfType(caughtException, typeof(ValidationFailedException));
            Assert.IsNotNull(caughtException.InnerException);
            Assert.IsInstanceOfType(caughtException.InnerException, typeof(InvalidStatusException));
        }

        /// <summary>
        /// Verifies that an attempt to set a product query as ready for processing
        /// with a culture that is not a valid culture causes the correct exception.
        /// </summary>
        /// <returns>A task that performs the test.</returns>
        [TestMethod]
        public async Task ReadyForProcessingWithInvalidCultureThrowsCorrectException()
        {
            // Arrange
            var id = Guid.NewGuid();
            var culture = "not-a-culture";

            ProductQueryController controller = this.CreateController();

            // Act
            Exception caughtException = null;
            try
            {
                IHttpActionResult result = await controller.PutAsync(id.ToString(), culture, null);
            }
            catch (BadRequestException ex)
            {
                caughtException = ex;
            }

            // Assert
            Assert.IsNotNull(caughtException);
            Assert.IsNotNull(caughtException.InnerException);
            Assert.IsInstanceOfType(caughtException.InnerException, typeof(InvalidCultureException));
        }

        /// <summary>
        /// Verifies that an attempt to set a product query as ready for processing
        /// with an identifier that is not a GUID causes the correct exception
        /// </summary>
        /// <returns>A task that performs the test.</returns>
        [TestMethod]
        public async Task ReadyForProcessingWithInvalidGuidThrowsCorrectException()
        {
            // Arrange
            var id = "not-a-guid";
            var culture = "en-US";

            ProductQueryController controller = this.CreateController();

            // Act
            Exception caughtException = null;
            try
            {
                IHttpActionResult result = await controller.PutAsync(id.ToString(), culture, null);
            }
            catch (BadRequestException ex)
            {
                caughtException = ex;
            }

            // Assert
            Assert.IsNotNull(caughtException);
            Assert.IsNotNull(caughtException.InnerException);
            Assert.IsInstanceOfType(caughtException.InnerException, typeof(InvalidGuidException));
        }

        /// <summary>
        /// Returns a new instance of <see cref="ProductQueryController"/>.
        /// </summary>
        /// <returns>A new instance of <see cref="ProductQueryController"/>.</returns>
        /// <param name="getCommand">A command that gets a specified product query.</param>
        /// <param name="createCommand">A command that creates a new product query.</param>
        /// <param name="readyForProcessingCommand">A command that can make a product query ready for processing.</param>
        /// <returns>Returns a new instance of <see cref="ProductQueryController"/>.</returns>
        private ProductQueryController CreateController(
            StubICommand<GetCommandParameters, Task<ProductQuery>> getCommand = null,
            StubICommand<CreateCommandParameters, Task<ProductQuery>> createCommand = null,
            StubICommand<ReadyForProcessingCommandParameters, Task<ProductQuery>> readyForProcessingCommand = null)
        {
            // Arrange stub dependencies
            var get = getCommand ??
                new StubICommand<GetCommandParameters, Task<ProductQuery>>
                {
                    ExecuteT0 = parameters => Task.Run(() => new ProductQuery())
                };

            var create = createCommand ??
                new StubICommand<CreateCommandParameters, Task<ProductQuery>>
                {
                    ExecuteT0 = parameters => Task.Run(() => new ProductQuery())
                };

            var ready = readyForProcessingCommand ??
                new StubICommand<ReadyForProcessingCommandParameters, Task<ProductQuery>>
                {
                    ExecuteT0 = parameters => Task.Run(() => new ProductQuery())
                };

            // Arrange controller
            return new ProductQueryController(get, create, ready)
            {
                Configuration = new HttpConfiguration(),
                Request = new StubHttpRequestMessage()
            };
        }
    }
}
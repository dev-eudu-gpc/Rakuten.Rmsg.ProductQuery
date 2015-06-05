// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="ProcessFileDataflowTests.cs" company="Rakuten">
//   Copyright (c) Rakuten. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace Rakuten.Rmsg.ProductQuery.WebJob.Tests.Unit
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics.Contracts;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks.Dataflow;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Rakuten.Rmsg.ProductQuery.WebJob.Api;
    using Rakuten.Threading.Tasks.Dataflow;

    /// <summary>
    /// A suite of tests for the <see cref="ProcessFileDataflow"/> class.
    /// </summary>
    [TestClass]
    public class ProcessFileDataflowTests
    {
        /// <summary>
        /// Ensures that the pipeline can handle receiving a file that contains no items.
        /// </summary>
        [TestMethod]
        public void ProcessFileDataflowHandlesAFileThatContainsNoItems()
        {
            // Arrange
            var dataflow = BuildDataflowPipeline(
                parseFile: stream => Enumerable.Empty<ItemMessageState>());

            dataflow.Post(new Message(Guid.NewGuid(), "en-GB", new Link()));
            dataflow.Complete();

            // Act
            var items = PollDataflowUntilCompletion(dataflow);

            // Assert
            Assert.AreEqual(0, items.Count);
        }

        /// <summary>
        /// Ensures that if an exception is thrown when reading a file it is handled.
        /// </summary>
        [TestMethod]
        public void ProcessFileDataflowHandlesAnExceptionThrownWhenFetchingAFile()
        {
            // Arrange
            var dataflow = BuildDataflowPipeline(
                downloadFile: state => { throw new Exception("Ninja Cat riding a Fire-breathing Unicorn"); });

            dataflow.Post(new Message(Guid.NewGuid(), "en-GB", new Link()));
            dataflow.Complete();

            Exception exception = null;

            // Act
            try
            {
                PollDataflowUntilCompletion(dataflow);
            }
            catch (Exception ex)
            {
                exception = ex;
            }

            // Assert
            Assert.IsNotNull(exception);
        }

        /// <summary>
        /// Ensures that if an exception is thrown when reading a file it is handled.
        /// </summary>
        [TestMethod]
        public void ProcessFileDataflowHandlesAnExceptionThrownWhenParsingAFile()
        {
            // Arrange
            var dataflow = BuildDataflowPipeline(
                parseFile: state => { throw new Exception("Ninja Cat riding a Fire-breathing Unicorn"); });

            // Act
            dataflow.Post(new Message(Guid.NewGuid(), "en-GB", new Link()));
            dataflow.Complete();

            Exception exception = null;

            // Act
            try
            {
                PollDataflowUntilCompletion(dataflow);
            }
            catch (Exception ex)
            {
                exception = ex;
            }

            // Assert
            Assert.IsNotNull(exception);
        }

        /// <summary>
        /// Ensures that a partially processed item, an item that has a matching record but no GRAN associated can be
        /// handled via the pipeline.
        /// </summary>
        [TestMethod]
        public void ProcessFileDataflowHandlesAnItemThatHasAMatchingRecord()
        {
            // Arrange
            var dataflow = BuildDataflowPipeline(
                parseFile: stream =>
                    new List<ItemMessageState>
                    {
                        new ItemMessageState(
                            Guid.NewGuid(), 
                            new CultureInfo("en-GB"), 
                            new Item { GtinValue = "1111111111116" })
                    },
                getEntity: state => new ItemMessageState(
                    state.Id,
                    state.Culture,
                    state.Item,
                    new ProductQueryItem
                    {
                        ProductQueryId = state.Id,
                        Gtin = state.Item.GtinValue
                    }));

            dataflow.Post(new Message(Guid.NewGuid(), "en-GB", new Link()));
            dataflow.Complete();

            // Act
            var items = PollDataflowUntilCompletion(dataflow);

            // Assert
            Assert.IsTrue(items.Count > 0);
        }

        /// <summary>
        /// Ensures that an item that has been previously processed and has been assigned a GRAN is handled.
        /// </summary>
        [TestMethod]
        public void ProcessFileDataflowHandlesAnItemThatHasBeenPreviouslyAssignedAGran()
        {
            // Arrange
            var dataflow = BuildDataflowPipeline(
                parseFile: stream =>
                    new List<ItemMessageState>
                    {
                        new ItemMessageState(
                            Guid.NewGuid(), 
                            new CultureInfo("en-GB"), 
                            new Item { GtinValue = "1111111111116" })
                    },
                getEntity: state => new ItemMessageState(
                    state.Id,
                    state.Culture,
                    state.Item,
                    new ProductQueryItem
                    {
                        ProductQueryId = state.Id,
                        Gtin = state.Item.GtinValue,
                        Gran = "000000000000"
                    }));

            dataflow.Post(new Message(Guid.NewGuid(), "en-GB", new Link()));
            dataflow.Complete();

            // Act
            var items = PollDataflowUntilCompletion(dataflow);

            // Assert
            Assert.IsTrue(items.Count > 0);
        }

        /// <summary>
        /// Ensures that the pipeline can handle receiving no results from submitting a product search.
        /// </summary>
        [TestMethod]
        public void ProcessFileDataflowHandlesNoResultsFromAProductSearch()
        {
            // Arrange
            var dataflow = BuildDataflowPipeline(
                parseFile: stream =>
                    new List<ItemMessageState>
                    {
                        new ItemMessageState(
                            Guid.NewGuid(), 
                            new CultureInfo("en-GB"), 
                            new Item { GtinValue = "1111111111116" })
                    },
                createEntity: state => new ItemMessageState(
                    state.Id,
                    state.Culture,
                    state.Item,
                    new ProductQueryItem
                    {
                        ProductQueryId = state.Id,
                        Gtin = state.Item.GtinValue
                    }),
                searchProducts: state => new ItemMessageState(
                    state.Id,
                    state.Culture,
                    state.Item,
                    state.Query,
                    Enumerable.Empty<Product>()));

            dataflow.Post(new Message(Guid.NewGuid(), "en-GB", new Link()));
            dataflow.Complete();

            // Act
            var items = PollDataflowUntilCompletion(dataflow);

            // Assert
            Assert.IsTrue(items.Count > 0);
        }

        /// <summary>
        /// Ensures that a message is processed successfully through the pipeline.
        /// </summary>
        [TestMethod]
        public void ProcessFileDataflowProcessesAFile()
        {
            // Arrange
            var dataflow = BuildDataflowPipeline(
                parseFile: stream => new List<ItemMessageState>
                {
                    new ItemMessageState(
                        Guid.NewGuid(), 
                        new CultureInfo("en-GB"), 
                        new Item
                        {
                            GtinType = "EAN",
                            GtinValue = "1111111111116"
                        })
                },
                createEntity: state => new ItemMessageState(
                    state.Id, 
                    state.Culture, 
                    state.Item, 
                    new ProductQueryItem
                    {
                        ProductQueryId = state.Id,
                        Gtin = state.Item.GtinValue
                    }),
                searchProducts: state => new ItemMessageState(
                    state.Id,
                    state.Culture,
                    state.Item,
                    state.Query,
                    new Collection<Product>
                    {
                        new Product { Id = "000000000000", ImprovedId = "000000000000" }
                    }),
                filterProducts: state =>
                {
                    state.Query.Gran = "000000000000";

                    return new ItemMessageState(
                        state.Id,
                        state.Culture,
                        state.Item,
                        state.Query);
                },
                getProduct: state => new ItemMessageState(
                    state.Id,
                    state.Culture,
                    state.Item,
                    new Product
                    {
                        Id = "000000000000", ImprovedId = "000000000000"
                    }),
                aggregateData: state =>
                {
                    state.Item.Manufacturer = "Microsoft";

                    return new ItemMessageState(
                        state.Id,
                        state.Culture,
                        state.Item,
                        state.Product);
                });

            dataflow.Post(new Message(Guid.NewGuid(), "en-GB", new Link()));
            dataflow.Complete();

            // Act
            var items = PollDataflowUntilCompletion(dataflow);

            // Assert
            Assert.AreEqual("Microsoft", items[0].Manufacturer);
        }

        /// <summary>
        /// Ensures that a message is processed successfully through the pipeline.
        /// </summary>
        [TestMethod]
        public void ProcessFileDataflowReturnsAnItem()
        {
            // Arrange
            var dataflow = BuildDataflowPipeline(
                parseFile: stream =>
                    new List<ItemMessageState>
                    {
                        new ItemMessageState(Guid.NewGuid(), new CultureInfo("en-GB"), new Item())
                    });

            dataflow.Post(new Message(Guid.NewGuid(), "en-GB", new Link()));
            dataflow.Complete();

            // Act
            var items = PollDataflowUntilCompletion(dataflow);

            // Assert
            Assert.IsTrue(items.Count > 0);
        }

        /// <summary>
        /// Ensures that an record within the original file that had no GTIN specified is still returned by the pipeline
        /// to be written back into the file.
        /// </summary>
        [TestMethod]
        public void ProcessFileDataflowReturnsAnItemThatHasNoGtinSpecified()
        {
            // Arrange
            var dataflow = BuildDataflowPipeline(
                parseFile: stream =>
                    new List<ItemMessageState>
                    {
                        new ItemMessageState(
                            Guid.NewGuid(), 
                            new CultureInfo("en-GB"), 
                            new Item { GtinValue = string.Empty })
                    });

            dataflow.Post(new Message(Guid.NewGuid(), "en-GB", new Link()));
            dataflow.Complete();

            // Act
            var items = PollDataflowUntilCompletion(dataflow);

            // Assert
            Assert.IsTrue(items.Count > 0);
        }

        /// <summary>
        /// Ensures that when an exception is handled when aggregating product and item data an item is returned from 
        /// the pipeline.
        /// </summary>
        [TestMethod]
        public void ProcessFileDataflowReturnsAnItemWhenAnExceptionIsHandledAggregatingData()
        {
            // Arrange
            var dataflow = BuildDataflowPipeline(
                parseFile: stream =>
                    new List<ItemMessageState>
                    {
                        new ItemMessageState(
                            Guid.NewGuid(), 
                            new CultureInfo("en-GB"), 
                            new Item { GtinValue = "1111111111116" })
                    },
                aggregateData: state => new ItemMessageState(
                    Guid.NewGuid(),
                    new CultureInfo("en-GB"),
                    new Item(),
                    new List<Exception>
                    {
                        new Exception("Ninja Cat riding a Fire-breathing Unicorn")
                    }));

            dataflow.Post(new Message(Guid.NewGuid(), "en-GB", new Link()));
            dataflow.Complete();

            // Act
            var items = PollDataflowUntilCompletion(dataflow);

            // Assert
            Assert.AreEqual(1, items.Count);
        }

        /// <summary>
        /// Ensures that when an exception is handled when attempting to create a record of the product query in the 
        /// persistent storage.
        /// </summary>
        [TestMethod]
        public void ProcessFileDataflowReturnsAnItemWhenAnExceptionIsHandledWhenCreatingAnEntity()
        {
            // Arrange
            var dataflow = BuildDataflowPipeline(
                parseFile: stream =>
                    new List<ItemMessageState>
                    {
                        new ItemMessageState(
                            Guid.NewGuid(), 
                            new CultureInfo("en-GB"), 
                            new Item { GtinValue = "1111111111116" })
                    },
                createEntity: state => new ItemMessageState(
                    Guid.NewGuid(),
                    new CultureInfo("en-GB"),
                    new Item(),
                    new List<Exception>
                    {
                        new Exception("Ninja Cat riding a Fire-breathing Unicorn")
                    }));

            dataflow.Post(new Message(Guid.NewGuid(), "en-GB", new Link()));
            dataflow.Complete();

            // Act
            var items = PollDataflowUntilCompletion(dataflow);

            // Assert
            Assert.AreEqual(1, items.Count);
        }

        /// <summary>
        /// Ensures that when an exception is handled when attempting to fetch a products data an item is returned from 
        /// the pipeline.
        /// </summary>
        [TestMethod]
        public void ProcessFileDataflowReturnsAnItemWhenAnExceptionIsHandledWhenFetchingAProduct()
        {
            // Arrange
            var dataflow = BuildDataflowPipeline(
                parseFile: stream =>
                    new List<ItemMessageState>
                    {
                        new ItemMessageState(
                            Guid.NewGuid(), 
                            new CultureInfo("en-GB"), 
                            new Item { GtinValue = "1111111111116" })
                    },
                getProduct: state => new ItemMessageState(
                    Guid.NewGuid(),
                    new CultureInfo("en-GB"),
                    new Item(),
                    new ProductQueryItem(),
                    new List<Exception>
                    {
                        new Exception("Ninja Cat riding a Fire-breathing Unicorn")
                    }));

            dataflow.Post(new Message(Guid.NewGuid(), "en-GB", new Link()));
            dataflow.Complete();

            // Act
            var items = PollDataflowUntilCompletion(dataflow);

            // Assert
            Assert.AreEqual(1, items.Count);
        }

        /// <summary>
        /// Ensures that when an exception is handled when attempting to filter a collection of products the item is 
        /// returned from the pipeline.
        /// </summary>
        [TestMethod]
        public void ProcessFileDataflowReturnsAnItemWhenAnExceptionIsHandledWhenFilteringProducts()
        {
            // Arrange
            var dataflow = BuildDataflowPipeline(
                parseFile: stream =>
                    new List<ItemMessageState>
                    {
                        new ItemMessageState(
                            Guid.NewGuid(), 
                            new CultureInfo("en-GB"), 
                            new Item { GtinValue = "1111111111116" })
                    },
                filterProducts: state => new ItemMessageState(
                    Guid.NewGuid(),
                    new CultureInfo("en-GB"),
                    new Item(),
                    new ProductQueryItem(),
                    Enumerable.Empty<Product>(),
                    new List<Exception>
                    {
                        new Exception("Ninja Cat riding a Fire-breathing Unicorn")
                    }));

            dataflow.Post(new Message(Guid.NewGuid(), "en-GB", new Link()));
            dataflow.Complete();

            // Act
            var items = PollDataflowUntilCompletion(dataflow);

            // Assert
            Assert.AreEqual(1, items.Count);
        }

        /// <summary>
        /// Ensures that when an exception is handled when attempting to retrieve the entity relating to the product 
        /// query the item is returned from the pipeline.
        /// </summary>
        [TestMethod]
        public void ProcessFileDataflowReturnsAnItemWhenAnExceptionIsHandledWhenGettingAnEntity()
        {
            // Arrange
            var dataflow = BuildDataflowPipeline(
                parseFile: stream =>
                    new List<ItemMessageState>
                    {
                        new ItemMessageState(
                            Guid.NewGuid(), 
                            new CultureInfo("en-GB"), 
                            new Item { GtinValue = "1111111111116" })
                    },
                getEntity: state => new ItemMessageState(
                    Guid.NewGuid(),
                    new CultureInfo("en-GB"),
                    new Item(),
                    new List<Exception>
                    {
                        new Exception("Ninja Cat riding a Fire-breathing Unicorn")
                    }));

            dataflow.Post(new Message(Guid.NewGuid(), "en-GB", new Link()));
            dataflow.Complete();

            // Act
            var items = PollDataflowUntilCompletion(dataflow);

            // Assert
            Assert.AreEqual(1, items.Count);
        }

        /// <summary>
        /// Ensures that when an exception is handled when attempting to search for a product the item is returned from 
        /// the pipeline.
        /// </summary>
        [TestMethod]
        public void ProcessFileDataflowReturnsAnItemWhenAnExceptionIsHandledWhenSearchingForAProduct()
        {
            // Arrange
            var dataflow = BuildDataflowPipeline(
                parseFile: stream =>
                    new List<ItemMessageState>
                    {
                        new ItemMessageState(
                            Guid.NewGuid(), 
                            new CultureInfo("en-GB"), 
                            new Item { GtinValue = "1111111111116" })
                    },
                searchProducts: state => new ItemMessageState(
                    Guid.NewGuid(),
                    new CultureInfo("en-GB"),
                    new Item(),
                    new ProductQueryItem(),
                    new List<Exception>
                    {
                        new Exception("Ninja Cat riding a Fire-breathing Unicorn")
                    }));

            dataflow.Post(new Message(Guid.NewGuid(), "en-GB", new Link()));
            dataflow.Complete();

            // Act
            var items = PollDataflowUntilCompletion(dataflow);

            // Assert
            Assert.AreEqual(1, items.Count);
        }

        /// <summary>
        /// Ensures that when an exception is handled when attempting to update a product query entity an item is 
        /// returned from the pipeline.
        /// </summary>
        [TestMethod]
        public void ProcessFileDataflowReturnsAnItemWhenAnExceptionIsHandledWhenUpdatingAnEntity()
        {
            // Arrange
            var dataflow = BuildDataflowPipeline(
                parseFile: stream =>
                    new List<ItemMessageState>
                    {
                        new ItemMessageState(
                            Guid.NewGuid(), 
                            new CultureInfo("en-GB"), 
                            new Item { GtinValue = "1111111111116" })
                    },
                updateEntity: state => new ItemMessageState(
                    Guid.NewGuid(),
                    new CultureInfo("en-GB"),
                    new Item(),
                    new ProductQueryItem(),
                    new List<Exception>
                    {
                        new Exception("Ninja Cat riding a Fire-breathing Unicorn")
                    }));

            dataflow.Post(new Message(Guid.NewGuid(), "en-GB", new Link()));
            dataflow.Complete();

            // Act
            var items = PollDataflowUntilCompletion(dataflow);

            // Assert
            Assert.AreEqual(1, items.Count);
        }

        /// <summary>
        /// Initializes a new <see cref="ProcessFileDataflow"/> instance configured with the supplied transforms.
        /// </summary>
        /// <param name="downloadFile">The transform to apply to the received <see cref="Message"/> instance.</param>
        /// <param name="parseFile">The transform to read the <see cref="Stream"/>.</param>
        /// <param name="getEntity">The transform that attempts to read the record from persistent storage.</param>
        /// <param name="createEntity">The transform that attempts to create a record in persistent storage.</param>
        /// <param name="searchProducts">The transform that searches for matching products.</param>
        /// <param name="filterProducts">The transform that filters a collection of products.</param>
        /// <param name="updateEntity">The transform to update the record in persistent storage.</param>
        /// <param name="getProduct">The transform to retrieve product data.</param>
        /// <param name="aggregateData">The transform to merge product and item data.</param>
        /// <param name="outputItem">The transform to return the processed item.</param>
        /// <returns>A new <see cref="ProcessFileDataflow"/> instance.</returns>
        private static PropagatorDataflow<Message, Item> BuildDataflowPipeline(
            Func<Message, Stream> downloadFile = null,
            Func<Stream, IEnumerable<ItemMessageState>> parseFile = null,
            Func<ItemMessageState, ItemMessageState> getEntity = null,
            Func<ItemMessageState, ItemMessageState> createEntity = null,
            Func<ItemMessageState, ItemMessageState> searchProducts = null,
            Func<ItemMessageState, ItemMessageState> filterProducts = null,
            Func<ItemMessageState, ItemMessageState> updateEntity = null,
            Func<ItemMessageState, ItemMessageState> getProduct = null,
            Func<ItemMessageState, ItemMessageState> aggregateData = null,
            Func<ItemMessageState, Item> outputItem = null)
        {
            return new ProcessFileDataflow(
                new TransformBlock<Message, Stream>(downloadFile ?? (message => null)), 
                new TransformManyBlock<Stream, ItemMessageState>(parseFile ?? (stream => Enumerable.Empty<ItemMessageState>())),
                new TransformBlock<ItemMessageState, ItemMessageState>(getEntity ?? (state => state)), 
                new TransformBlock<ItemMessageState, ItemMessageState>(createEntity ?? (state => state)), 
                new TransformBlock<ItemMessageState, ItemMessageState>(searchProducts ?? (state => state)), 
                new TransformBlock<ItemMessageState, ItemMessageState>(filterProducts ?? (state => state)), 
                new TransformBlock<ItemMessageState, ItemMessageState>(updateEntity ?? (state => state)), 
                new TransformBlock<ItemMessageState, ItemMessageState>(getProduct ?? (state => state)), 
                new TransformBlock<ItemMessageState, ItemMessageState>(aggregateData ?? (state => state)),
                new TransformBlock<ItemMessageState, Item>(outputItem ?? (state => state.Item)));
        }

        /// <summary>
        /// Continuously attempts to read messages from the given Dataflow until the pipeline has finished processing.
        /// </summary>
        /// <param name="dataflow">The <see cref="PropagatorDataflow{TInput,TOutput}"/> to poll.</param>
        /// <returns>The collection of <see cref="Item"/>s received from the Dataflow.</returns>
        private static IList<Item> PollDataflowUntilCompletion(PropagatorDataflow<Message, Item> dataflow)
        {
            var items = new List<Item>();

            // While we are receiving items from the dataflow and the dataflow has not finished.
            while (!(dataflow.Completion.IsCanceled || dataflow.Completion.IsCompleted || dataflow.Completion.IsFaulted))
            {
                IList<Item> newItems;

                // Receive a batch of items from the dataflow.
                if (dataflow.TryReceiveAll(out newItems))
                {
                    items.AddRange(newItems);
                }
            }

            if (!dataflow.Completion.IsFaulted)
            {
                return items;
            }

            Contract.Assume(dataflow.Completion.Exception != null);

            throw dataflow.Completion.Exception;
        }
    }
}
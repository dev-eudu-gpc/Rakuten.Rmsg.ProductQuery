//------------------------------------------------------------------------------
// <copyright file="MessageQueueSteps.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.Web.Http.Tests.Integration
{
    using System;
    using System.Diagnostics;
    using System.Diagnostics.Contracts;
    using Microsoft.ServiceBus.Messaging;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Newtonsoft.Json;
    using Rakuten.Rmsg.ProductQuery.Configuration;
    using TechTalk.SpecFlow;

    /// <summary>
    /// Contains steps pertaining to messages and message queues.
    /// </summary>
    [Binding]
    public class MessageQueueSteps
    {
        /// <summary>
        /// The context under which this instance is operating.
        /// </summary>
        private readonly IApiContext apiContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageQueueSteps"/> class
        /// </summary>
        /// <param name="apiContext">A context for the API.</param>
        public MessageQueueSteps(IApiContext apiContext)
        {
            Contract.Requires(apiContext != null);

            this.apiContext = apiContext;
        }

        /// <summary>
        /// Ensures that the dead letter message queue is empty.
        /// </summary>
        [Given(@"the dead letter message queue is empty")]
        public void GivenTheDeadLetterQueueIsEmpty()
        {
            // Create a dead letter queue client
            var deadLetterQueueClient = QueueClientFactory.CreateForDeadLetter(
                this.apiContext,
                ReceiveMode.ReceiveAndDelete);

            // Empty the queue by receiving the messages one by one until there are none left.
            try
            {
                BrokeredMessage message = null;
                do
                {
                    message = deadLetterQueueClient.Receive(new TimeSpan(0, 0, 5));
                    if (message != null)
                    {
                        message.Complete();
                    }
                }
                while (message != null);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
        }

        /// <summary>
        /// Ensures that the message queue is empty.
        /// </summary>
        [Given(@"the message queue is empty")]
        public void GivenTheMessageQueueIsEmpty()
        {
            var client = QueueClientFactory.Create(this.apiContext, ReceiveMode.ReceiveAndDelete);

            BrokeredMessage message = null;
            do
            {
                message = client.Receive(new TimeSpan(0, 0, 5));
            }
            while (message != null);
        }

        /// <summary>
        /// Verifies that a message has been created on the queue for the product
        /// query that is in scenario storage.
        /// </summary>
        [Given(@"a message has been created on the queue")]
        [Then(@"a message has been created on the queue")]
        public void ThenAMessageHasBeenCreatedOnTheQueue()
        {
            // Get the product query that was created from the response
            var content = ScenarioStorage.HttpResponseMessage.Content.ReadAsStringAsync().Result;
            var productQuery = JsonConvert.DeserializeObject<ProductQuery>(content);

            // Verify that the message can be found
            var client = QueueClientFactory.Create(this.apiContext, ReceiveMode.PeekLock);
            var message = client.Peek();
            Assert.IsNotNull(message);

            // Verify the contents of the message body
            var messageBody = message.GetBody<Message>();
            Assert.AreEqual(ScenarioStorage.NewProductQuery.IdAsGuid, messageBody.Id);
            Assert.AreEqual(ScenarioStorage.NewProductQuery.Culture, messageBody.Culture);
            Assert.AreEqual(productQuery.Links.Enclosure.Href, messageBody.Link.Target);

            // Store the message in scenario storage
            ScenarioStorage.Message = message;
        }

        /// <summary>
        /// Verifies that the dead letter queue is empty.
        /// </summary>
        [Then(@"the dead letter queue is empty")]
        public void ThenTheDeadLetterQueueIsEmpty()
        {
            var client = QueueClientFactory.CreateForDeadLetter(this.apiContext, ReceiveMode.PeekLock);

            BrokeredMessage message = client.Peek();

            Assert.IsNull(message);
        }

        /// <summary>
        /// Verifies that the message can be found in the dead letter queue.
        /// </summary>
        [Then(@"the message can be found in the dead letter queue")]
        public void ThenTheMessageCanBeFoundInTheDeadLetterQueue()
        {
            // Get a queue client for the dead letter queue
            QueueClient deadLetterQueueClient = QueueClientFactory.CreateForDeadLetter(
                this.apiContext,
                ReceiveMode.PeekLock);

            // Try and get a message from the queue for up to 1 minute
            // and stop as soon as we get one
            BrokeredMessage message = null;
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            while (message == null && stopwatch.Elapsed < TimeSpan.FromSeconds(60))
            {
                message = deadLetterQueueClient.Peek();
            }

            // Assertions
            Assert.IsNotNull(message);
            Assert.AreEqual(message.MessageId, ScenarioStorage.Message.MessageId, false);

            // Storage
            ScenarioStorage.DeadLetterMessage = message;
        }

        /// <summary>
        /// Verifies that the message queue is empty.
        /// </summary>
        [Then(@"the message queue is empty")]
        public void ThenTheMessageQueueIsEmpty()
        {
            var client = QueueClientFactory.Create(this.apiContext, ReceiveMode.PeekLock);

            BrokeredMessage message = client.Peek();

            Assert.IsNull(message);
        }
    }
}
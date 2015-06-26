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
        /// A queue client using peek lock mode.
        /// </summary>
        private readonly QueueClient peekLockQueueClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageQueueSteps"/> class
        /// </summary>
        /// <param name="apiContext">A context for the API.</param>
        /// <param name="peekLockQueueClient">A queue client using peek lock mode.</param>
        public MessageQueueSteps(
            IApiContext apiContext,
            QueueClient peekLockQueueClient)
        {
            Contract.Requires(apiContext != null);
            Contract.Requires(peekLockQueueClient != null);

            this.apiContext = apiContext;
            this.peekLockQueueClient = peekLockQueueClient;
        }

        /// <summary>
        /// Ensures that the dead letter message queue is empty.
        /// </summary>
        [Given(@"the dead letter message queue is empty")]
        public void GivenTheDeadLetterQueueIsEmpty()
        {
            // Create a dead letter queue client
            var deadLetterQueueClient = QueueClient.CreateFromConnectionString(
                this.apiContext.ServiceBusConnectionString,
                QueueClient.FormatDeadLetterPath(this.peekLockQueueClient.Path),
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
            QueueClient receiveAndDeleteQueueClient = QueueClient.CreateFromConnectionString(
                this.apiContext.ServiceBusConnectionString,
                this.apiContext.MessageQueueName,
                ReceiveMode.ReceiveAndDelete);

            BrokeredMessage message = null;
            do
            {
                message = receiveAndDeleteQueueClient.Receive(new TimeSpan(0, 0, 5));
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
            var message = this.peekLockQueueClient.Peek();

            Assert.IsNotNull(message);
            
            // Verify the contents of the message body
            var messageBody = message.GetBody<Message>();

            Assert.AreEqual(ScenarioStorage.NewProductQuery.IdAsGuid, messageBody.Id);
            Assert.AreEqual(ScenarioStorage.NewProductQuery.Culture, messageBody.Culture);
            Assert.AreEqual(productQuery.Links.Enclosure.Href, messageBody.Link.Target);

            // Store the message in scenario storage
            ScenarioStorage.MessageId = message.MessageId;

            // Clean up
            AbandonPeekLock(message);
        }

        /// <summary>
        /// Verifies that the dead letter queue is empty.
        /// </summary>
        [Then(@"the dead letter queue is empty")]
        public void ThenTheDeadLetterQueueIsEmpty()
        {
            // Arrange
            BrokeredMessage message = this.peekLockQueueClient.Peek();
            if (message != null)
            {
                AbandonPeekLock(message);
            }

            // Assert
            Assert.IsNull(message);
        }

        /// <summary>
        /// Verifies that the message can be found in the dead letter queue.
        /// </summary>
        [Then(@"the message can be found in the dead letter queue")]
        public void ThenTheMessageCanBeFoundInTheDeadLetterQueue()
        {
            // Get a queue client for the dead letter queue
            QueueClient deadLetterQueueClient = QueueClient.CreateFromConnectionString(
                this.apiContext.ServiceBusConnectionString,
                QueueClient.FormatDeadLetterPath(this.peekLockQueueClient.Path),
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
            Assert.AreEqual(message.MessageId, ScenarioStorage.MessageId, true);

            // Clean up
            AbandonPeekLock(message);
        }

        /// <summary>
        /// Verifies that the message queue is empty.
        /// </summary>
        [Then(@"the message queue is empty")]
        public void ThenTheMessageQueueIsEmpty()
        {
            BrokeredMessage message = this.peekLockQueueClient.Peek();

            if (message != null)
            {
                AbandonPeekLock(message);
            }

            Assert.IsNull(message);
        }

        /// <summary>
        /// Waits for the message queue to be empty for a maximum of one minute
        /// </summary>
        [When(@"the message queue is empty")]
        public void WhenTheMessageQueueIsEmpty()
        {
            var stopWatch = new Stopwatch();
            BrokeredMessage message = null;

            stopWatch.Start();
            do
            {
                message = this.peekLockQueueClient.Peek();

                if (message != null)
                {
                    AbandonPeekLock(message);
                }
            }
            while (message != null && stopWatch.ElapsedMilliseconds < 60000);

            Assert.IsNull(message);
        }

        /// <summary>
        /// Tries to abandon a lock on a peek locked message.
        /// </summary>
        /// <param name="message">The message to try and abandon.</param>
        private static void AbandonPeekLock(BrokeredMessage message)
        {
            try
            {
                // A message is only abandonable if there is a lock token.
                var lockToken = message.LockToken;
                message.Abandon();
            }
            catch (ObjectDisposedException)
            {
            }
            catch (InvalidOperationException)
            {
            }
        }
    }
}
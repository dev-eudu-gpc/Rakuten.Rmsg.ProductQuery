//------------------------------------------------------------------------------
// <copyright file="MessageQueueSteps.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.Web.Http.Tests.Integration.Steps
{
    using System;
    using System.Diagnostics;
    using System.Diagnostics.Contracts;
    using Microsoft.ServiceBus.Messaging;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
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
        /// An object for storing and retrieving information from the scenario context.
        /// </summary>
        private readonly ScenarioStorage scenarioStorage;

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageQueueSteps"/> class
        /// </summary>
        /// <param name="apiContext">A context for the API.</param>
        /// <param name="peekLockQueueClient">A queue client using peek lock mode.</param>
        /// <param name="scenarioStorage">An object for sharing information between steps.</param>
        public MessageQueueSteps(
            IApiContext apiContext,
            QueueClient peekLockQueueClient,
            ScenarioStorage scenarioStorage)
        {
            Contract.Requires(apiContext != null);
            Contract.Requires(peekLockQueueClient != null);
            Contract.Requires(scenarioStorage != null);

            this.apiContext = apiContext;
            this.peekLockQueueClient = peekLockQueueClient;
            this.scenarioStorage = scenarioStorage;
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

            // Empty the queue by receiving the messages one by one until there are none left
            BrokeredMessage message = null;
            do
            {
                message = deadLetterQueueClient.Receive(new TimeSpan(0, 0, 5));
            }
            while (message != null);
        }

        /// <summary>
        /// Ensures that the message queue is empty.
        /// </summary>
        [Given(@"the message queue is empty")]
        public void GivenTheMessageQueueIsEmpty()
        {
            // Create a queue client in the correct mode
            QueueClient receiveAndDeleteQueueClient = QueueClient.CreateFromConnectionString(
                this.apiContext.ServiceBusConnectionString,
                this.apiContext.MessageQueueName,
                ReceiveMode.ReceiveAndDelete);

            // Empty the queue by receibing the messages one by one until there are none left
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
        ////[Given(@"a message has been created on the queue")]
        [Then(@"a message has been created on the queue")]
        public void ThenAMessageHasBeenCreatedOnTheQueue()
        {
            // Arrange
            var productQuery = this.scenarioStorage.Creation.SourceProductQuery;
            var messageBody = GetMessageBody(
                this.peekLockQueueClient,
                this.scenarioStorage.Creation.SourceProductQuery.IdAsGuid);

            // Assert
            Assert.IsNotNull(messageBody);

            // Store the message for subsequent steps
            this.scenarioStorage.ReadyForProcessing.MessageBody = messageBody;
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

            var messageBody = GetMessageBody(
                deadLetterQueueClient,
                this.scenarioStorage.Creation.SourceProductQuery.IdAsGuid);

            // Assert
            Assert.IsNotNull(messageBody);
        }

        /// <summary>
        /// Verifies that the message has the correct culture.
        /// </summary>
        [Then(@"the message for the new product query has the correct culture")]
        public void ThenTheMessageForTheNewProductQueryHasTheCorrectCulture()
        {
            Assert.AreEqual(
                this.scenarioStorage.Creation.SourceProductQuery.Culture,
                this.scenarioStorage.ReadyForProcessing.MessageBody.Culture);
        }

        /// <summary>
        /// Verifies that the message has the correct enclosure link.
        /// </summary>
        [Then(@"the message for the new product query has the correct enclosure link")]
        public void ThenTheMessageForTheNewProductQueryHasTheCorrectEnclosureLink()
        {
            Assert.AreEqual(
                this.scenarioStorage.Creation.ResponseProductQuery.Links.Enclosure.Href,
                this.scenarioStorage.ReadyForProcessing.MessageBody.Link.Target);
        }

        /// <summary>
        /// Verifies that the message has the correct product query identifier.
        /// </summary>
        [Then(@"the message for the new product query has the correct product query identifier")]
        public void ThenTheMessageForTheNewProductQueryHasTheCorrectProductQueryIdentifier()
        {
            Assert.AreEqual(
                this.scenarioStorage.Creation.SourceProductQuery.IdAsGuid,
                this.scenarioStorage.ReadyForProcessing.MessageBody.Id);
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

        /// <summary>
        /// Tries to get the body of a message from the given queue for a given product query id.
        /// </summary>
        /// <param name="queueClient">The queue client.</param>
        /// <param name="productQueryId">The product query id.</param>
        /// <returns>The message body, if found, null otherwise.</returns>
        private static Message GetMessageBody(QueueClient queueClient, Guid productQueryId)
        {
            BrokeredMessage message = null;
            Message messageBody = null;

            var stopwatch = new Stopwatch();
            stopwatch.Start();
            while (messageBody == null && stopwatch.ElapsedMilliseconds < 120000)
            {
                message = queueClient.Peek();
                if (message != null)
                {
                    messageBody = message.GetBody<Message>();

                    if (messageBody.Id != productQueryId)
                    {
                        messageBody = null;
                    }
                }
            }

            AbandonPeekLock(message);
            return messageBody;
        }
    }
}
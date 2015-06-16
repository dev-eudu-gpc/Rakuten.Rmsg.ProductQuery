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
        /// A queue client.
        /// </summary>
        private readonly QueueClient queueClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageQueueSteps"/> class
        /// </summary>
        /// <param name="apiContext">A context for the API.</param>
        /// <param name="queueClient">A queue client.</param>
        public MessageQueueSteps(IApiContext apiContext, QueueClient queueClient)
        {
            Contract.Requires(apiContext != null);
            Contract.Requires(queueClient != null);

            this.apiContext = apiContext;
            this.queueClient = queueClient;
        }

        /// <summary>
        /// Ensures that the message queue is empty.
        /// </summary>
        [Given(@"the message queue is empty")]
        public void GivenTheMessageQueueIsEmpty()
        {
            try
            {
                BrokeredMessage message = null;
                do
                {
                    message = this.queueClient.Receive(new TimeSpan(0, 0, 5));
                }
                while (message != null);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
        }

        /// <summary>
        /// Waits for the message queue to be empty for a maximum of one minute
        /// </summary>
        [When(@"the message queue is empty")]
        [When(@"the message queue is empty once again")]
        public void WhenTheMessageQueueIsEmpty()
        {
            var stopWatch = new Stopwatch();
            BrokeredMessage message = null;

            stopWatch.Start();
            do
            {
                message = this.queueClient.Peek();
            }
            while (message != null && stopWatch.ElapsedMilliseconds < 60000);
        }

        /// <summary>
        /// Verifies that the message queue is empty.
        /// </summary>
        [Then(@"the message queue is empty")]
        public void ThenTheMessageQueueIsEmpty()
        {
            BrokeredMessage message = this.queueClient.Peek();

            Assert.IsNull(message);
        }

        /// <summary>
        /// Ensures that the dead letter message queue is empty.
        /// </summary>
        [Given(@"the dead letter message queue is empty")]
        public void GivenTheDeadLetterQueueIsEmpty()
        {
            // Create a dead letter queue client
            QueueClient deadLetterQueueClient = QueueClientFactory.GetDeadLetterQueueClient(
                this.apiContext.ServiceBusConnectionString,
                this.queueClient);

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
            ////var message = this.queueClient.Receive(new TimeSpan(0, 0, 60));
            var message = this.queueClient.Peek();
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
        /// Verifies that the message can be found in the dead letter queue.
        /// </summary>
        [Then(@"the message can be found in the dead letter queue")]
        public void ThenTheMessageCanBeFoundInTheDeadLetterQueue()
        {
            // Get a queue client for the dead letter queue
            QueueClient deadLetterQueueClient = QueueClientFactory.GetDeadLetterQueueClient(
                this.apiContext.ServiceBusConnectionString,
                this.queueClient);

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
    }
}

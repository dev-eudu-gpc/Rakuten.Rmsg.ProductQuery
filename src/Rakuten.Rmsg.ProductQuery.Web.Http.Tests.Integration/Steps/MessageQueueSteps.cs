//------------------------------------------------------------------------------
// <copyright file="MessageQueueSteps.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.Web.Http.Tests.Integration.Steps
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
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
            // Remove all messages
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
        /// Verifies that a message has been created on the queue for the product
        /// query that is in scenario storage.
        /// </summary>
        [Then(@"a message has been created on the queue")]
        public void ThenAMessageHasBeenCreatedOnTheQueue()
        {
            // Get the product query that was created from the response
            var content = ScenarioStorage.HttpResponseMessage.Content.ReadAsStringAsync().Result;
            var productQuery = JsonConvert.DeserializeObject<ProductQuery>(content);

            // Verify that the message can be found
            var message = this.queueClient.Receive(new TimeSpan(0, 0, 60));
            Assert.IsNotNull(message);

            // Verify the contents of the message body
            var messageBody = message.GetBody<Message>();
            Assert.AreEqual(ScenarioStorage.NewProductQuery.IdAsGuid, messageBody.Id);
            Assert.AreEqual(ScenarioStorage.NewProductQuery.Culture, messageBody.Culture);
            Assert.AreEqual(productQuery.Links.Enclosure.Href, messageBody.Link.Target);
        }
    }
}

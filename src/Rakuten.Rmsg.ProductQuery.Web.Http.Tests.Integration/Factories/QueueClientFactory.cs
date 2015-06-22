//------------------------------------------------------------------------------
// <copyright file="QueueClientFactory.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.Web.Http.Tests.Integration
{
    using Microsoft.ServiceBus.Messaging;
    using Rakuten.Rmsg.ProductQuery.Configuration;

    /// <summary>
    /// Provides factory methods for the <see cref="QueueClient"/> class.
    /// </summary>
    public static class QueueClientFactory
    {
        /// <summary>
        /// Creates a new queue client.
        /// </summary>
        /// <param name="apiContext">The context in which the test is operating.</param>
        /// <param name="mode">The received mode for the client.</param>
        /// <returns>A new queue client.</returns>
        public static QueueClient Create(
            IApiContext apiContext,
            ReceiveMode mode = ReceiveMode.PeekLock)
        {
            return QueueClient.CreateFromConnectionString(
                apiContext.ServiceBusConnectionString,
                apiContext.MessageQueueName,
                mode);
        }

        /// <summary>
        /// Gets a queue client for the dead letter queue for a given queue client.
        /// </summary>
        /// <param name="apiContext">The context in which the test is operating.</param>
        /// <param name="mode">The received mode for the client.</param>
        /// <returns>A queue client for the dead letter queue for a given queue client.</returns>
        public static QueueClient CreateForDeadLetter(
            IApiContext apiContext,
            ReceiveMode mode = ReceiveMode.PeekLock)
        {
            var client = QueueClientFactory.Create(apiContext, mode);

            return QueueClient.CreateFromConnectionString(
                apiContext.ServiceBusConnectionString,
                QueueClient.FormatDeadLetterPath(client.Path),
                mode);
        }
    }
}

//------------------------------------------------------------------------------
// <copyright file="QueueClientFactory.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.Web.Http.Tests.Integration
{
    using Microsoft.ServiceBus.Messaging;

    /// <summary>
    /// Provides factory methods for the <see cref="QueueClient"/> class.
    /// </summary>
    public static class QueueClientFactory
    {
        /// <summary>
        /// Gets a queue client for the dead letter queue for a given queue client.
        /// </summary>
        /// <param name="connectionString">The connection string for the queue.</param>
        /// <param name="queueClient">The message queue.</param>
        /// <returns>A queue client for the dead letter queue for a given queue client.</returns>
        public static QueueClient GetDeadLetterQueueClient(
            string connectionString,
            QueueClient queueClient)
        {
            return QueueClient.CreateFromConnectionString(
                connectionString,
                QueueClient.FormatDeadLetterPath(queueClient.Path));
        }
    }
}

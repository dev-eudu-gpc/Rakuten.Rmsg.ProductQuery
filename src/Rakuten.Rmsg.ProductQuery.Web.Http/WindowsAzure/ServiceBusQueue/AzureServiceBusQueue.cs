//---------------------------------------------------------------------------------------------------------------------
// <copyright file="AzureServiceBusQueue.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//---------------------------------------------------------------------------------------------------------------------
namespace Rakuten.WindowsAzure.ServiceBus
{
    using System.Diagnostics.Contracts;
    using System.Threading.Tasks;
    using Microsoft.ServiceBus.Messaging;
    using Rakuten.Rmsg.ProductQuery.Web.Http;

    /// <summary>
    /// Represents an implementation that interacts with storage on Microsoft Azure.
    /// </summary>
    public class AzureServiceBusQueue : IMessageQueue
    {
        /// <summary>
        /// Dispatches a message to a specified Azure service bus message queue
        /// </summary>
        /// <param name="connectionString">The connection string for the Azure service bus.</param>
        /// <param name="queueName">The name of the queue to dispatch the message to.</param>
        /// <param name="productQuery">The product query that forms the message.</param>
        /// <returns>A task that does the work.</returns>
        public Task DispatchMessage(
            string connectionString,
            string queueName,
            ProductQuery productQuery)
        {
            Contract.Requires(connectionString != null);

            return Task.Run(() =>
            {
                QueueClient client = QueueClient.CreateFromConnectionString(connectionString, queueName);

                client.Send(new BrokeredMessage(productQuery));
            });
        }
    }
}
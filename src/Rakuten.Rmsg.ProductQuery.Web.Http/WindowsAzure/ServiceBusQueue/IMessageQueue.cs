//---------------------------------------------------------------------------------------------------------------------
// <copyright file="IMessageQueue.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//---------------------------------------------------------------------------------------------------------------------
namespace Rakuten.WindowsAzure.ServiceBus
{
    using System;
    using System.Text;
    using System.Threading.Tasks;
    using Rakuten.Rmsg.ProductQuery.Web.Http;

    /// <summary>
    /// Defines an object that interacts with Azure Service Bus Queues
    /// </summary>
    public interface IMessageQueue
    {
        /// <summary>
        /// Dispatches a product query message to the queue
        /// </summary>
        /// <param name="connectionString">A valid connection string.</param>
        /// <param name="queueName">The name of the queue to dispatch the message to.</param>
        /// <param name="productQuery">The product query to dispatch the message for.</param>
        /// <returns>A task that does the work</returns>
        Task DispatchMessage(string connectionString, string queueName, ProductQuery productQuery);
    }
}
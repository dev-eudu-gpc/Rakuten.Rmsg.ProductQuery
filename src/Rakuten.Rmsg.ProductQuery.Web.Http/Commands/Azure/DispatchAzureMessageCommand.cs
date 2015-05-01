//------------------------------------------------------------------------------
// <copyright file="DispatchAzureMessageCommand.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.Web.Http.Commands
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Threading.Tasks;
    using Microsoft.ServiceBus.Messaging;
    using Rakuten.Rmsg.ProductQuery.Configuration;

    /// <summary>
    /// Represents a command that dispatches a product query message to the queue.
    /// </summary>
    internal class DispatchAzureMessageCommand : AsyncCommandAction<DispatchMessageCommandParameters>
    {
        /// <summary>
        /// The context under which this instance is operating.
        /// </summary>
        private readonly IApiContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="DispatchAzureMessageCommand"/> class
        /// </summary>
        /// <param name="context">The context in which this instance is running.</param>
        public DispatchAzureMessageCommand(IApiContext context)
        {
            Contract.Requires(context != null);

            this.context = context;
        }

        /// <summary>
        /// Dispatches a message containing the link to the product query's blob.
        /// </summary>
        /// <param name="parameters">Parameters for the message</param>
        /// <returns>A task that does the work.</returns>
        public override Task ExecuteAsync(DispatchMessageCommandParameters parameters)
        {
            // Create the message
            var message = new Message(Guid.NewGuid(), parameters.BlobLink);

            return Task.Run(() =>
            {
                // Create a queue client
                QueueClient client = QueueClient.CreateFromConnectionString(
                    this.context.ServiceBusConnectionString,
                    "rmsg-product-query"); // TODO: [WB 16-Apr-2015] Replace with config setting

                // Dispatch the message
                client.Send(new BrokeredMessage(message));
            });
        }
    }
}
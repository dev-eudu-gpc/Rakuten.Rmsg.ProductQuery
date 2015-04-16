//------------------------------------------------------------------------------
// <copyright file="DispatchMessageCommand.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.Web.Http.Commands
{
    using System;
    using System.Collections.ObjectModel;
    using System.Diagnostics.Contracts;
    using System.Threading.Tasks;
    using Rakuten.Rmsg.ProductQuery.Configuration;
    using Rakuten.WindowsAzure.ServiceBus;
    using Rakuten.WindowsAzure.Storage;

    /// <summary>
    /// Represents a command that dispatches a product query message to the queue.
    /// </summary>
    public class DispatchMessageCommand : AsyncCommandAction<DispatchMessageCommandParameters>
    {
        /// <summary>
        /// The context under which this instance is operating.
        /// </summary>
        private readonly IApiContext context;

        /// <summary>
        /// The object for interacting with storage.
        /// </summary>
        private readonly IMessageQueue messageQueue;

        /// <summary>
        /// Initializes a new instance of the <see cref="DispatchMessageCommand"/> class
        /// </summary>
        /// <param name="context">The context in which this instance is running.</param>
        /// <param name="messageQueue">A means to interact with the message queue.</param>
        public DispatchMessageCommand(
            IApiContext context,
            IMessageQueue messageQueue)
        {
            Contract.Requires(messageQueue != null);
            Contract.Requires(context != null);

            this.context = context;
            this.messageQueue = messageQueue;
        }

        /// <summary>
        /// Creates a blob in storage for a product query.
        /// </summary>
        /// <param name="parameters">The input parameters enabling the product query to be uniquely identified</param>
        /// <returns>A task that does the work.</returns>
        public override async Task ExecuteAsync(DispatchMessageCommandParameters parameters)
        {
            Contract.Requires(parameters != null);

            // Create the message
            var message = new ProductQuery
            {
                Links = new Collection<Link> { parameters.ProductQuery }
            };

            // Dispatch the message
            await this.messageQueue.DispatchMessage(
                this.context.ServiceBusConnectionString,
                "rmsg-product-query", // TODO: [WB 16-Apr-2015] Replace with config setting
                message);
        }
    }
}
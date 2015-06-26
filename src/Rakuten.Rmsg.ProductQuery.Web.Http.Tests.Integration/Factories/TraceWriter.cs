//------------------------------------------------------------------------------
// <copyright file="TraceWriter.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.Web.Http.Tests.Integration
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.ServiceBus.Messaging;

    /// <summary>
    /// Provides trace writing facilities.
    /// </summary>
    public static class TraceWriter
    {
        /// <summary>
        /// Writes a line to the trace writer file.
        /// </summary>
        /// <param name="message">The message to write.</param>
        public static void Write(string message)
        {
            using (var writer = new StreamWriter(@"c:\temp\rmsgprqint.txt", true))
            {
                writer.WriteLine(string.Format("{0} - {1}", DateTime.UtcNow, message));
                writer.Flush();
            }
        }

        /// <summary>
        /// Writes a line to the trace writer file for a method starting.
        /// </summary>
        /// <param name="message">The message to write.</param>
        public static void WriteMethodStart(string message)
        {
            using (var writer = new StreamWriter(@"c:\temp\rmsgprqint.txt", true))
            {
                writer.WriteLine(string.Format("{0} - {1} starting", DateTime.UtcNow, message));
                writer.Flush();
            }
        }

        /// <summary>
        /// Writes a line to the trace writer file for a method ending.
        /// </summary>
        /// <param name="message">The message to write.</param>
        public static void WriteMethodEnd(string message)
        {
            using (var writer = new StreamWriter(@"c:\temp\rmsgprqint.txt", true))
            {
                writer.WriteLine(string.Format("{0} - {1} ending", DateTime.UtcNow, message));
                writer.Flush();
            }
        }

        /// <summary>
        /// Writes a message.
        /// </summary>
        /// <param name="message">The message to write.</param>
        public static void WriteMessage(BrokeredMessage message)
        {
            Write("-------------------------- message");
            Write("\tId: " + message.MessageId);
            Write("\tIs Consumed: " + message.IsBodyConsumed);
            Write("\tDelivery Count: " + message.DeliveryCount);

            var content = message.GetBody<Message>();

            Write("\tcontent.Id: " + content.Id);
        }

        /// <summary>
        /// Writes a message.
        /// </summary>
        /// <param name="message">The message to write.</param>
        public static void WriteMessage(Message message)
        {
            Write("~~~~~~~~~~~~~~~~~~~~~~~~~~ message body");
            Write("\tcontent.Id: " + message.Id);
        }

        /// <summary>
        /// Writes messages from a queue.
        /// </summary>
        /// <param name="client">The queue client.</param>
        public static void WriteMessages(QueueClient client)
        {
            Write("### MESSAGES start");
            var stopWatch = new Stopwatch();
            BrokeredMessage message = null;

            stopWatch.Start();
            do
            {
                message = client.Peek();
                if (message != null)
                {
                    WriteMessage(message);
                    AbandonPeekLock(message);
                }
            }
            while (message != null && stopWatch.ElapsedMilliseconds < 60000);
            
            Write("### MESSAGES end");
        }

        /// <summary>
        /// Gets the value of a given scenario storage key
        /// </summary>
        /// <param name="name">The name of the storage key.</param>
        /// <returns>The value of the storage key.</returns>
        private static string GetStorageValue(string name)
        {
            try
            {
                switch (name)
                {
                    case "NewProductQuery":
                        return ScenarioStorage.NewProductQuery.Id;
                    default:
                        return string.Empty;
                }
            }
            catch
            {
                return "(key not present)";
            }
        }

        /// <summary>
        /// Tries to abandon a lock on a peek locked message.
        /// </summary>
        /// <param name="message">The message to try and abandon.</param>
        private static void AbandonPeekLock(BrokeredMessage message)
        {
            try
            {
                TraceWriter.Write("Abandoning peek lock on message: " + message.MessageId);
                var lockToken = message.LockToken;
                message.Abandon();
                TraceWriter.Write("Lock abandoned: " + message.MessageId);
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

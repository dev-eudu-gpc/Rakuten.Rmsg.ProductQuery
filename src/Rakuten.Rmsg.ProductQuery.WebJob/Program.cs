// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="Program.cs" company="Rakuten">
//   Copyright (c) Rakuten. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.WebJob
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.Azure.WebJobs;

    using Rakuten.Azure.WebJobs;
    using Rakuten.Rmsg.ProductQuery.Configuration;
    using Rakuten.Rmsg.ProductQuery.WebJob.Entities;
    using Rakuten.Threading.Tasks.Dataflow;

    /// <summary>
    /// An application that will execute as a WebJob in Microsoft's Azure.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// The main entry point of the application.
        /// </summary>
        public static void Main()
        {
            // Generate the connection strings to the diagnostics storage.
            IApiContext apiContext = new ApiContextFactory(new AppSettingsConfigurationSource()).Create();

            // Create a new host specifying the configuration.
            var host = new JobHost(new JobHostConfiguration(apiContext.DiagnosticsStorageConnectionString)
            {
                NameResolver = new CloudConfigNameResolver(),
                ServiceBusConnectionString = apiContext.ServiceBusConnectionString
            });

            // Create a new database connection.
            var databaseContext = new ProductQueryContext();

            var stream = new MemoryStream();

            ProcessProductQueryFile.Process = (message, writer) =>
            {
                var transform1 = CreateParseFileTransform(Guid.NewGuid());
                var transform2 = CreateGetQueryItemTransform(databaseContext);

                var dataflow = new ProcessFileDataflow(
                    TransformBlockFactory.Create<Message, Stream>(m => 
                        DownloadFileCommand.Execute(null, m, stream)),
                    TransformManyBlockFactory.Create<Stream, MessageState>(file => transform1(file, writer)),
                    TransformBlockFactory.Create<MessageState, MessageState>(state => transform2(state, writer)));

                dataflow.Post(message);
                dataflow.Complete();

                dataflow.Completion.Wait();

                writer.WriteLine("process.");
            };

            // The following code ensures that the WebJob will be running continuously
            host.RunAndBlock();
        }

        /// <summary>
        /// Returns a delegate that when executed will attempt to retrieve the corresponding record from persistent 
        /// storage of a <see cref="Item"/>.
        /// </summary>
        /// <param name="context">The instance through which the persistent storage can be queried.</param>
        /// <returns>A <see cref="Func{T1,T2,TResult}"/></returns>
        private static Func<MessageState, TextWriter, Task<MessageState>> CreateGetQueryItemTransform(
            ProductQueryContext context)
        {
            return async (state, writer) =>
            {
                try
                {
                    var queryItem = await GetProductQueryItemCommand.Execute(context, state.Id, state.Item.GtinValue);

                    return new MessageState(state.Id, state.Item, queryItem);
                }
                catch (Exception ex)
                {
                    writer.WriteLine("An issue was encountered parsing the uploaded file: " + ex.ToString());

                    return null;
                }
            };
        }

        /// <summary>
        /// Returns a delegate that when executed will take the stream and attempt to convert it into a collection of 
        /// <see cref="MessageState"/> instances.
        /// </summary>
        /// <param name="id">The unique identifier for the current query.</param>
        /// <returns>A <see cref="Func{T1,T2,TResult}"/></returns>
        private static Func<Stream, TextWriter, Task<IEnumerable<MessageState>>> CreateParseFileTransform(Guid id)
        {
            Contract.Requires(id != Guid.Empty);

            return async (stream, writer) =>
            {
                try
                {
                    IEnumerable<Item> items = await ParseFileCommand.Execute(null, stream);

                    return from item in items select new MessageState(id, item);
                }
                catch (Exception ex)
                {
                    writer.WriteLine("An issue was encountered parsing the uploaded file: " + ex.ToString());

                    return null;
                }
            };
        }
    }
}
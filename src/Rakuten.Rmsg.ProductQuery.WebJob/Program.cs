// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="Program.cs" company="Rakuten">
//   Copyright (c) Rakuten. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.WebJob
{
    using Microsoft.Azure.WebJobs;

    using Rakuten.Azure.WebJobs;
    using Rakuten.Rmsg.ProductQuery.Configuration;

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
            IApiContext context = new ApiContextFactory(new AppSettingsConfigurationSource()).Create();

            var host = new JobHost(new JobHostConfiguration(context.DiagnosticsStorageConnectionString)
            {
                NameResolver = new CloudConfigNameResolver(),
                ServiceBusConnectionString = context.ServiceBusConnectionString
            });

            ProcessProductQueryFile.Process = (s, writer) =>
            {
                writer.WriteLine("process.");
            };

            // The following code ensures that the WebJob will be running continuously
            host.RunAndBlock();
        }
    }
}
// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="Program.cs" company="Rakuten">
//   Copyright (c) Rakuten. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.WebJob
{
    using System;
    using System.IO;

    using Microsoft.Azure.WebJobs;

    using Rakuten.Azure.WebJobs;

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



            var host = new JobHost(new JobHostConfiguration { NameResolver = new CloudConfigNameResolver() });

            Action<string, TextWriter> process = (s, writer) =>
            {
                writer.WriteLine("process.");
            };

            ProcessProductQueryFile.Process = process;

            // The following code ensures that the WebJob will be running continuously
            host.RunAndBlock();
        }
    }
}
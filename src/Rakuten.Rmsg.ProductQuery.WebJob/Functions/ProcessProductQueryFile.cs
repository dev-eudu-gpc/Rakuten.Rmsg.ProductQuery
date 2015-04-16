// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="ProcessProductQueryFile.cs" company="Rakuten">
//   Copyright (c) Rakuten. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.WebJob
{
    using System;
    using System.Diagnostics.Contracts;
    using System.IO;

    using Microsoft.Azure.WebJobs;

    /// <summary>
    /// Processes a product query file aggregating it with product data where possible.
    /// </summary>
    public class ProcessProductQueryFile
    {
        /// <summary>
        /// Gets or sets the <see cref="Action{T}"/> that the triggered function will execute.
        /// </summary>
        public static Action<string, TextWriter> Process { get; set; }

        /// <summary>
        /// When triggered will pickup and process a product file from the specified queue.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="log"></param>
        public static void Execute([ServiceBusTrigger("rmsg-product-query")] string message, TextWriter log)
        {
            Contract.Requires(log != null);

            Process(message, log);
            log.WriteLine(message);
        }
    }
}
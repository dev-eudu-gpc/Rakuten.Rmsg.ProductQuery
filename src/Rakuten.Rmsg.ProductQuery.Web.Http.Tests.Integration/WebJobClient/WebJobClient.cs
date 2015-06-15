//------------------------------------------------------------------------------
// <copyright file="WebJobClient.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.Web.Http.Tests.Integration
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;

    /// <summary>
    /// A client for interacting with the web job.
    /// </summary>
    public class WebJobClient
    {
        /// <summary>
        /// The process for the web job.
        /// </summary>
        private readonly Process process;

        /// <summary>
        /// Initializes a new instance of the <see cref="WebJobClient"/> class
        /// </summary>
        public WebJobClient()
        {
            // Configure the process
            this.process = new Process();

            // TODO: [WB 12-Jun-2015] Don't use hard coded path
            this.process.StartInfo.FileName = @"C:\git\dev-eudu-gpc\Rakuten.Rmsg.ProductQuery\src\Rakuten.Rmsg.ProductQuery.WebJob\bin\Release\Rakuten.Rmsg.ProductQuery.WebJob.exe";
            this.process.StartInfo.RedirectStandardError = true;
            this.process.StartInfo.RedirectStandardOutput = true;
            this.process.StartInfo.UseShellExecute = false;
            this.process.ErrorDataReceived += (sender, line) =>
            {
                this.HasErrors = true;
            };
            this.process.OutputDataReceived += (sender, line) =>
            {
                // The web job is considered started when specific text is written to standard output
                this.IsReady = line.Data.Equals("Job host started", StringComparison.InvariantCultureIgnoreCase);
            };
        }

        /// <summary>
        /// Gets a value indicating whether the web job threw exceptions
        /// when starting up.
        /// </summary>
        public bool HasErrors { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the web job is ready.
        /// </summary>
        public bool IsReady { get; private set; }

        /// <summary>
        /// Starts the web job.
        /// </summary>
        public void Start()
        {
            this.process.Start();

            this.process.BeginOutputReadLine();
            this.process.BeginErrorReadLine();

            // Wait until the process is considered ready or errors have been thrown
            while (!this.IsReady || this.HasErrors)
            {
            }
        }

        /// <summary>
        /// Stops the process.
        /// </summary>
        public void Stop()
        {
            this.process.Close();
        }
    }
}

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
    using System.IO;

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

#if DEBUG
            var environment = "Debug";
#else
            var environment = "Release";
#endif

            this.process.StartInfo.FileName = Directory.GetCurrentDirectory() + string.Format("\\..\\..\\..\\Rakuten.Rmsg.ProductQuery.WebJob\\bin\\{0}\\Rakuten.Rmsg.ProductQuery.WebJob.exe", environment);
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

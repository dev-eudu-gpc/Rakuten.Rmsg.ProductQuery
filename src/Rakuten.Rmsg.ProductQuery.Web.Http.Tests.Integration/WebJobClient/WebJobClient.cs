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
    public class WebJobClient : IDisposable
    {
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
        /// Gets or sets the process for the web job.
        /// </summary>
        private Process Process { get; set; }

        /// <summary>
        /// Starts the web job.
        /// </summary>
        public void Start()
        {
            // Ensure any previous running process has been stopped and disposed.
            this.Stop();

            // Configure the process
            this.Process = new Process();

#if DEBUG
            var environment = "Debug";
#else
            var environment = "Release";
#endif

            this.Process.StartInfo.FileName = Directory.GetCurrentDirectory() + string.Format("\\..\\..\\..\\Rakuten.Rmsg.ProductQuery.WebJob\\bin\\{0}\\Rakuten.Rmsg.ProductQuery.WebJob.exe", environment);
            this.Process.StartInfo.RedirectStandardError = true;
            this.Process.StartInfo.RedirectStandardOutput = true;
            this.Process.StartInfo.UseShellExecute = false;
            this.Process.ErrorDataReceived += this.ErrorDataReceivedHandler;
            this.Process.OutputDataReceived += this.OutputDataReceivedHandler;
            this.Process.Start();

            this.Process.BeginOutputReadLine();
            this.Process.BeginErrorReadLine();

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
            if (this.Process != null)
            {
                this.Process.Close();
                this.Process.Dispose();
            }
        }

        /// <summary>
        /// Disposal procedures.
        /// </summary>
        public void Dispose()
        {
            this.Process.Kill();
        }

        /// <summary>
        /// Handlers for the error data received event.
        /// </summary>
        /// <param name="sender">The sender of the error.</param>
        /// <param name="e">Event arguments</param>
        private void ErrorDataReceivedHandler(object sender, DataReceivedEventArgs e)
        {
            this.HasErrors = true;
        }

        /// <summary>
        /// Handler for the output data received event.
        /// </summary>
        /// <param name="sender">The sender of the error.</param>
        /// <param name="e">Event arguments.</param>
        private void OutputDataReceivedHandler(object sender, DataReceivedEventArgs e)
        {
            this.IsReady = this.IsReady || e.Data.Equals("Job host started", StringComparison.InvariantCultureIgnoreCase);
        }
    }
}

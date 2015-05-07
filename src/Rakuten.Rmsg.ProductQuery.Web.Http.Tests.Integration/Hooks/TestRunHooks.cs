//----------------------------------------------------------------------------------------------------------------------
// <copyright file="TestRunHooks.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.Web.Http.Tests.Integration
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Rakuten.Rmsg.ProductQuery.Configuration;
    using TechTalk.SpecFlow;

    /// <summary>
    /// Hooks to support test runs.
    /// </summary>
    [Binding]
    internal static class TestRunHooks
    {
        /// <summary>
        /// The directory in which IIS Express is installed.
        /// </summary>
        private static readonly string IisExpressDirectory = 
            Environment.ExpandEnvironmentVariables(@"%ProgramW6432%\IIS Express");

        /// <summary>
        /// Initializes any global dependencies before a test run.
        /// </summary>
        [BeforeTestRun]
        internal static void BeforeTestRun()
        {
            IApiContext apiContext = new ApiContextFactory(new AppSettingsConfigurationSource()).Create();

            var root = AppDomain.CurrentDomain.BaseDirectory;

            EnsureWebsiteAvailable(
                apiContext.BaseAddress,
                Path.Combine(root, @"..\..\..\Rakuten.Rmsg.ProductQuery.Web.Http"));
        }

        /// <summary>
        /// Ensures that the specified website is available, if not, starts an IIS Express instance to host the 
        /// website.
        /// </summary>
        /// <param name="address">
        /// The website address to ensure is available.
        /// </param>
        /// <param name="directory">
        /// The directory in which the website can be hosted from.
        /// </param>
        private static void EnsureWebsiteAvailable(Uri address, string directory)
        {
            var isAvailable = IsWebsiteAvailableAsync(address).Result;

            if (!isAvailable && address.Host.Equals("localhost", StringComparison.OrdinalIgnoreCase))
            {
                // Looks like we are running locally and IIS express is not started, so start it.
                StartIisExpress(directory, address.Port);
            }
        }

        /// <summary>
        /// Verifies that the specified website is available by making a HTTP request to the 
        /// <paramref name="address"/>.
        /// </summary>
        /// <param name="address">
        /// The URI to which a request should be made to check availability.
        /// </param>
        /// <returns>
        /// A <see cref="Task"/> that will determine if the website is available.
        /// </returns>
        private static async Task<bool> IsWebsiteAvailableAsync(Uri address)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    await client.GetAsync(address.ToString());
                }
                catch (HttpRequestException)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Creates a new process to run IIS Express on the specified port.
        /// </summary>
        /// <param name="directory">
        /// The directory in which the website can be hosted from.
        /// </param>
        /// <param name="port">
        /// The port on which IIS Express should listen.
        /// </param>
        private static void StartIisExpress(string directory, int port)
        {
            var iisExpressFilename = Path.Combine(IisExpressDirectory, "iisexpress.exe");

            var websiteDirectory = Path.GetFullPath(directory);
            var arguments = string.Format("/path:\"{0}\" /port:{1}", websiteDirectory, port);

            Console.WriteLine("Running: {0} {1}", iisExpressFilename, arguments);

            var processStartInfo = new ProcessStartInfo
            {
                FileName = iisExpressFilename,
                Arguments = arguments,
                WorkingDirectory = websiteDirectory
            };

            Process.Start(processStartInfo);
        }
    }
}
//------------------------------------------------------------------------------
// <copyright file="WebJobSteps.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.Web.Http.Tests.Integration.Steps
{
    using System.Diagnostics.Contracts;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using TechTalk.SpecFlow;

    /// <summary>
    /// Steps for working with the web job.
    /// </summary>
    [Binding]
    public class WebJobSteps
    {
        /// <summary>
        /// A client for interacting with the web job.
        /// </summary>
        private readonly WebJobClient webJobClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="WebJobSteps"/> class
        /// </summary>
        /// <param name="webJobClient">A client for interacting with the web job.</param>
        public WebJobSteps(WebJobClient webJobClient)
        {
            Contract.Requires(webJobClient != null);

            this.webJobClient = webJobClient;
        }

        /// <summary>
        /// Ensures that the web job is stopped.
        /// </summary>
        [Given(@"the web job is stopped")]
        public void GivenTheWebJobIsStopped()
        {
            this.webJobClient.Stop();
        }

        /// <summary>
        /// Verifies that the web job has picked up the message.
        /// </summary>
        [Then(@"the web job picks up the new message")]
        public void ThenTheWebJobPicksUpTheNewMessage()
        {
            ScenarioContext.Current.Pending();
        }

        /// <summary>
        /// Starts the web job.
        /// </summary>
        [When(@"the web job is started")]
        public void WhenTheWebJobIsStarted()
        {
            this.webJobClient.Start();

            Assert.IsFalse(this.webJobClient.HasErrors);
        }
    }
}

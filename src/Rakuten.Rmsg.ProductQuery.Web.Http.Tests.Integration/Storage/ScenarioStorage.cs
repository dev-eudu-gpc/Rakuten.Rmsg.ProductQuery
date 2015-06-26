//------------------------------------------------------------------------------
// <copyright file="ScenarioStorage.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.Web.Http.Tests.Integration
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using Newtonsoft.Json;
    using Rakuten.Rmsg.ProductQuery.Web.Http.Tests.Integration.Resources;

    /// <summary>
    /// Represents data shared across scenario steps.
    /// </summary>
    public class ScenarioStorage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ScenarioStorage"/> class
        /// </summary>
        public ScenarioStorage()
        {
            this.Creation = new CreationStorage();
            this.Files = new FileStorage();
            this.Gpc = new GpcStorage();
            this.Monitor = new MonitorStorage();
            this.ReadyForProcessing = new ReadyForProcessingStorage();
        }

        /// <summary>
        /// Gets or sets information pertaining to the product query creation process.
        /// </summary>
        public CreationStorage Creation { get; set; }

        /// <summary>
        /// Gets or sets information pertaining to monitoring a product query group.
        /// </summary>
        public MonitorStorage Monitor { get; set; }

        /// <summary>
        /// Gets or sets information pertaining to files containing
        /// product query items.
        /// </summary>
        public FileStorage Files { get; set; }

        /// <summary>
        /// Gets or sets information pertaining to the ready for processing feature.
        /// </summary>
        public ReadyForProcessingStorage ReadyForProcessing { get; set; }

        /// <summary>
        /// Gets or sets information pertaining to GPC.
        /// </summary>
        public GpcStorage Gpc { get; set; }

        /// <summary>
        /// Gets or sets the last response that was received from a call to the API.
        /// </summary>
        public HttpResponseMessage LastResponse { get; set; }

        /// <summary>
        /// Gets the HTTP problem from the body of the 
        /// last response that was received from a call to the API.
        /// </summary>
        public HttpProblem LastResponseHttpProblem
        {
            get
            {
                return JsonConvert.DeserializeObject<HttpProblem>(
                    this.LastResponse.Content.ReadAsStringAsync().Result);
            }
        }

        /// <summary>
        /// Gets or sets the time at which the scenario execution began.
        /// </summary>
        public DateTime StartTime { get; set; }
    
        /// <summary>
        /// Represents information pertaining to the product query creation process.
        /// </summary>
        public class CreationStorage
        {
            /////// <summary>
            /////// Gets or sets the product query from the database.
            /////// </summary>
            ////public rmsgProductQuery DatabaseEntity { get; set; }

            /// <summary>
            /// Gets or sets the <see cref="ProductQuery"/> that was used
            /// in the request to create the product query.
            /// </summary>
            public ProductQuery SourceProductQuery { get; set; }

            /// <summary>
            /// Gets or sets the response message from the call to create the
            /// product query.
            /// </summary>
            public HttpResponseMessage ResponseMessage { get; set; }

            /// <summary>
            /// Gets a <see cref="ProductQuery"/> from the response message.
            /// </summary>
            public ProductQuery ResponseProductQuery 
            { 
                get
                {
                    return JsonConvert.DeserializeObject<ProductQuery>(
                        this.ResponseMessage.Content.ReadAsStringAsync().Result);
                }
            }

            /// <summary>
            /// Gets a <see cref="HttpProblem"/> from the response message.
            /// </summary>
            public HttpProblem ResponseHttpProblem
            {
                get
                {
                    return JsonConvert.DeserializeObject<HttpProblem>(
                        this.ResponseMessage.Content.ReadAsStringAsync().Result);
                }
            }

            /////// <summary>
            /////// Gets or sets the product query group database entity that
            /////// the created product query belongs to.
            /////// </summary>
            ////public rmsgProductQueryGroup ActualGroup { get; set; }

            /// <summary>
            /// Gets or sets the product query group database entity that
            /// the scenario is expecting the product query to belong to.
            /// </summary>
            public rmsgProductQueryGroup ExpectedGroup { get; set; }
        }

        /// <summary>
        /// Represents information pertaining to product query group monitoring.
        /// </summary>
        public class MonitorStorage
        {
            /// <summary>
            /// Gets or sets the source object for the request that was made.
            /// </summary>
            public ProductQueryMonitorRequest SourceRequest { get; set; }

            /// <summary>
            /// Gets a <see cref="HttpProblem"/> from the response message.
            /// </summary>
            public HttpProblem ResponseHttpProblem
            {
                get
                {
                    return JsonConvert.DeserializeObject<HttpProblem>(
                        this.ResponseMessage.Content.ReadAsStringAsync().Result);
                }
            }

            /// <summary>
            /// Gets or sets the response message from the call to
            /// get the status of a product query group.
            /// </summary>
            public HttpResponseMessage ResponseMessage { get; set; }
        }

        /// <summary>
        /// Represents information pertaining to the ready for processing feature.
        /// </summary>
        public class ReadyForProcessingStorage
        {
            /// <summary>
            /// Gets a <see cref="HttpProblem"/> from the response message.
            /// </summary>
            public HttpProblem ResponseHttpProblem
            {
                get
                {
                    return JsonConvert.DeserializeObject<HttpProblem>(
                        this.ResponseMessage.Content.ReadAsStringAsync().Result);
                }
            }

            /// <summary>
            /// Gets or sets the response message from the call to
            /// inform that a product query is ready for processing.
            /// </summary>
            public HttpResponseMessage ResponseMessage { get; set; }

            /// <summary>
            /// Gets or sets the message body for the product query.
            /// </summary>
            public Message MessageBody { get; set; }
        }

        /// <summary>
        /// Represents information pertaining to files containing
        /// product query items.
        /// </summary>
        public class FileStorage
        {
            /// <summary>
            /// Gets or sets the name of the results file.
            /// </summary>
            public string ResultFileName { get; set; }

            /// <summary>
            /// Gets or sets a collection of the items in the source file.
            /// </summary>
            public List<Item> SourceItems { get; set; }

            /// <summary>
            /// Gets or sets the file name of the source file.
            /// </summary>
            public string SourceFileName { get; set; }
        }

        /// <summary>
        /// Represents information pertaining to GPC.
        /// </summary>
        public class GpcStorage
        {
            /// <summary>
            /// Gets or sets the product that was created in GPC.
            /// </summary>
            public Product Product { get; set; }
        }
    }
}

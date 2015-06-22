﻿//------------------------------------------------------------------------------
// <copyright file="ScenarioStorage.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.Web.Http.Tests.Integration
{
    using System.Collections.Generic;
    using System.Net.Http;
    using Microsoft.ServiceBus.Messaging;
    using TechTalk.SpecFlow;

    /// <summary>
    /// Provides centralized access to strongly typed properties that are
    /// persisted in the the current scenario context.
    /// </summary>
    public static class ScenarioStorage
    {
        /// <summary>
        /// Gets or sets a message identifier.
        /// </summary>
        public static BrokeredMessage DeadLetterMessage
        {
            get
            {
                return ScenarioContext.Current.Get<BrokeredMessage>("deadLetterMessage");
            }

            set
            {
                ScenarioContext.Current.Set<BrokeredMessage>(value, "deadLetterMessage");
            }
        }

        /// <summary>
        /// Gets or sets the HTTP problem that has been deserialized from the response message.
        /// </summary>
        public static HttpProblem HttpProblem
        {
            get
            {
                return ScenarioContext.Current.Get<HttpProblem>("httpProblem");
            }

            set
            {
                ScenarioContext.Current.Set<HttpProblem>(value, "httpProblem");
            }
        }

        /// <summary>
        /// Gets or sets the http response message
        /// </summary>
        public static HttpResponseMessage HttpResponseMessage
        {
            get
            {
                return ScenarioContext.Current.Get<HttpResponseMessage>("httpResponseMessage");
            }

            set
            {
                ScenarioContext.Current.Set<HttpResponseMessage>(value, "httpResponseMessage");
            }
        }

        /// <summary>
        /// Gets or sets a message.
        /// </summary>
        public static BrokeredMessage Message
        {
            get
            {
                return ScenarioContext.Current.Get<BrokeredMessage>("message");
            }

            set
            {
                ScenarioContext.Current.Set<BrokeredMessage>(value, "message");
            }
        }

        /// <summary>
        /// Gets or sets the new product query.
        /// </summary>
        public static ProductQuery NewProductQuery
        {
            get
            {
                return ScenarioContext.Current.Get<ProductQuery>("newProductQuery");
            }

            set
            {
                ScenarioContext.Current.Set<ProductQuery>(value, "newProductQuery");
            }
        }

        /// <summary>
        /// Gets or sets the new product query group.
        /// </summary>
        public static ProductQueryGroup NewProductQueryGroup
        {
            get
            {
                return ScenarioContext.Current.Get<ProductQueryGroup>("newProductQueryGroup");
            }

            set
            {
                ScenarioContext.Current.Set<ProductQueryGroup>(value, "newProductQueryGroup");
            }
        }

        /// <summary>
        /// Gets or sets a product query monitor request.
        /// </summary>
        public static ProductQueryMonitorRequest ProductQueryMonitorRequest
        {
            get
            {
                return ScenarioContext.Current.Get<ProductQueryMonitorRequest>("productQueryGroupRequest");
            }

            set
            {
                ScenarioContext.Current.Set<ProductQueryMonitorRequest>(value, "productQueryGroupRequest");
            }
        }

        /// <summary>
        /// Gets or sets the product query that was retrieved from the database.
        /// </summary>
        public static rmsgProductQuery ProductQueryFromDatabase
        {
            get
            {
                return ScenarioContext.Current.Get<rmsgProductQuery>("productQueryFromDatabase");
            }

            set
            {
                ScenarioContext.Current.Set<rmsgProductQuery>(value, "productQueryFromDatabase");
            }
        }

        /// <summary>
        /// Gets or sets the expected product query group.
        /// </summary>
        public static rmsgProductQueryGroup ProductQueryGroupExpected
        {
            get
            {
                return ScenarioContext.Current.Get<rmsgProductQueryGroup>("productQueryGroup");
            }

            set
            {
                ScenarioContext.Current.Set<rmsgProductQueryGroup>(value, "productQueryGroup");
            }
        }

        /// <summary>
        /// Gets or sets the actual product query group.
        /// </summary>
        public static rmsgProductQueryGroup ProductQueryGroupActual
        {
            get
            {
                return ScenarioContext.Current.Get<rmsgProductQueryGroup>("productQueryGroupFromDatabase");
            }

            set
            {
                ScenarioContext.Current.Set<rmsgProductQueryGroup>(value, "productQueryGroupFromDatabase");
            }
        }

        /// <summary>
        /// Gets or sets a collection of GPC products
        /// </summary>
        public static List<Product> Products
        {
            get
            {
                return ScenarioContext.Current.Get<List<Product>>("products");
            }

            set
            {
                ScenarioContext.Current.Set<List<Product>>(value, "products");
            }
        }

        /// <summary>
        /// Gets or sets the name of the file for the product query.
        /// </summary>
        public static string ProductQueryFileName
        {
            get
            {
                return ScenarioContext.Current.Get<string>("productQueryFileName");
            }

            set
            {
                ScenarioContext.Current.Set<string>(value, "productQueryFileName");
            }
        }

        /// <summary>
        /// Gets or sets the items in the product query results file.
        /// </summary>
        public static List<Item> ResultItems
        {
            get
            {
                return ScenarioContext.Current.Get<List<Item>>("resultItems");
            }

            set
            {
                ScenarioContext.Current.Set<List<Item>>(value, "resultItems");
            }
        }

        /// <summary>
        /// Gets or sets the items in the product query file.
        /// </summary>
        public static List<Item> Items
        {
            get
            {
                return ScenarioContext.Current.Get<List<Item>>("items");
            }

            set
            {
                ScenarioContext.Current.Set<List<Item>>(value, "items");
            }
        }

        /// <summary>
        /// Gets or sets the fully qualified name of the downloaded result file.
        /// </summary>
        public static string ResultFileName
        {
            get
            {
                return ScenarioContext.Current.Get<string>("resultFileName");
            }

            set
            {
                ScenarioContext.Current.Set<string>(value, "resultFileName");
            }
        }
    }
}
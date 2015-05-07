//------------------------------------------------------------------------------
// <copyright file="ScenarioStorage.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.Web.Http.Tests.Integration
{
    using System.Net.Http;
    using TechTalk.SpecFlow;

    /// <summary>
    /// Provides centralized access to strongly typed properties that are
    /// persisted in the the current scenario context.
    /// </summary>
    public static class ScenarioStorage
    {
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
        /// Gets or sets the sparse product query group.
        /// </summary>
        public static rmsgProductQueryGroup SparseProductQueryGroup
        {
            get
            {
                return ScenarioContext.Current.Get<rmsgProductQueryGroup>("sparseProductQueryGroup");
            }

            set
            {
                ScenarioContext.Current.Set<rmsgProductQueryGroup>(value, "sparseProductQueryGroup");
            }
        }

        /// <summary>
        /// Gets or sets a product query group that has been retrieved from the database.
        /// </summary>
        public static rmsgProductQueryGroup ProductQueryGroupFromDatabase
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
    }
}

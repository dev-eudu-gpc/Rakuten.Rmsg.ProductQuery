//----------------------------------------------------------------------------------------------------------------------
// <copyright file="StubIUriTemplateFactory.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.Web.Http.Tests
{
    using Rakuten.Fakes;

    /// <summary>
    /// Provides factory methods for the <see cref="IUriTemplate"/> interface.
    /// </summary>
    internal static class StubIUriTemplateFactory
    {
        /// <summary>Creates a dummy URI template for test purposes.</summary>
        /// <returns>A dummy URI template for test purposes.</returns>
        public static IUriTemplate Create()
        {
            return new StubIUriTemplate
            {
                BindStringString = (name, value) => Create(),
                ToStringBoolean = resolveTemplate => "/"
            };
        }

        /// <summary>Creates a dummy URI template for test purposes.</summary>
        /// <param name="template">The template for the URI template.</param>
        /// <returns>A dummy URI template for test purposes.</returns>
        public static IUriTemplate Create(string template)
        {
            return new StubIUriTemplate
            {
                BindStringString = (name, value) => Create(template),
                ToStringBoolean = resolveTemplate => template
            };
        }
    }
}
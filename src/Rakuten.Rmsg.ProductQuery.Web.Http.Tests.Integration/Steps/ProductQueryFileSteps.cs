//------------------------------------------------------------------------------
// <copyright file="ProductQueryFileSteps.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.Web.Http.Tests.Integration.Steps
{
    using System;
    using System.Linq;
    using TechTalk.SpecFlow;

    /// <summary>
    /// Steps pertaining to the product query file.
    /// </summary>
    [Binding]
    public class ProductQueryFileSteps
    {
        /// <summary>
        /// Creates a new file for a product query using the products in
        /// scenario storage as the source for EAN.
        /// </summary>
        [Given(@"a product query file for the new product has been created")]
        public void GivenAProductQueryFileForTheNewProductHasBeenCreated()
        {
            // Get a list of EANs from the products in scenario storage
            var eans = ScenarioStorage.Products
                .Select(p => p.AttributeSets
                    .FirstOrDefault(set => set.Name.Equals(AttributeSetName.Gtin.ToString(), StringComparison.InvariantCultureIgnoreCase))
                    .Attributes.FirstOrDefault(attribute => attribute.Key.Equals("EAN", StringComparison.InvariantCultureIgnoreCase))
                    .Value.ToString());

            // Create a product query file for the EANs
            var filename = ProductQueryFileFactory.Create(eans);

            // Store details in scenario storage
            ScenarioStorage.ProductQueryFileName = filename;
            ScenarioStorage.ProductEANs = eans.ToList();
        }
    }
}

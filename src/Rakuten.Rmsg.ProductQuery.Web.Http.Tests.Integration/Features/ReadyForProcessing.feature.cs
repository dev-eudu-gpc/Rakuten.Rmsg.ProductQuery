﻿// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated by SpecFlow (http://www.specflow.org/).
//      SpecFlow Version:1.9.0.77
//      SpecFlow Generator Version:1.9.0.0
//      Runtime Version:4.0.30319.34014
// 
//      Changes to this file may cause incorrect behavior and will be lost if
//      the code is regenerated.
//  </auto-generated>
// ------------------------------------------------------------------------------
#region Designer generated code
#pragma warning disable
namespace Rakuten.Rmsg.ProductQuery.Web.Http.Tests.Integration.Features
{
    using TechTalk.SpecFlow;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "1.9.0.77")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [Microsoft.VisualStudio.TestTools.UnitTesting.TestClassAttribute()]
    public partial class ReadyForProcessingFeature
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.ClassInitializeAttribute()]
        public static void FeatureSetup(Microsoft.VisualStudio.TestTools.UnitTesting.TestContext testContext)
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "ReadyForProcessing", "Ensures that when using the API endpoint for flagging a product query as ready fo" +
                    "r processing\nthat the API operates according to the specification.", ProgrammingLanguage.CSharp, ((string[])(null)));
            testRunner.OnFeatureStart(featureInfo);
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.ClassCleanupAttribute()]
        public static void FeatureTearDown()
        {
            testRunner.OnFeatureEnd();
            testRunner = null;
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestInitializeAttribute()]
        public virtual void TestInitialize()
        {
            if (((TechTalk.SpecFlow.FeatureContext.Current != null) 
                        && (TechTalk.SpecFlow.FeatureContext.Current.FeatureInfo.Title != "ReadyForProcessing")))
            {
                Rakuten.Rmsg.ProductQuery.Web.Http.Tests.Integration.Features.ReadyForProcessingFeature.FeatureSetup(null);
            }
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCleanupAttribute()]
        public virtual void ScenarioTearDown()
        {
            testRunner.OnScenarioEnd();
        }
        
        public virtual void ScenarioSetup(TechTalk.SpecFlow.ScenarioInfo scenarioInfo)
        {
            testRunner.OnScenarioStart(scenarioInfo);
        }
        
        public virtual void ScenarioCleanup()
        {
            testRunner.CollectScenarioErrors();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Flagging a product query as ready for processing persists the correct information" +
            " to the database")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "ReadyForProcessing")]
        public virtual void FlaggingAProductQueryAsReadyForProcessingPersistsTheCorrectInformationToTheDatabase()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Flagging a product query as ready for processing persists the correct information" +
                    " to the database", ((string[])(null)));
            this.ScenarioSetup(scenarioInfo);
            testRunner.Given("a valid new product query has been prepared", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
            testRunner.And("a request has been made to submit the new product query", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            testRunner.When("a request is made to flag the product query as ready for processing with a status" +
                    " of submitted", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
            testRunner.And("the product query is retrieved from the database", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            testRunner.Then("the status of the product query from the database is Submitted", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Flagging a product query as ready for processing returns the correct response")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "ReadyForProcessing")]
        public virtual void FlaggingAProductQueryAsReadyForProcessingReturnsTheCorrectResponse()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Flagging a product query as ready for processing returns the correct response", ((string[])(null)));
            this.ScenarioSetup(scenarioInfo);
            testRunner.Given("a valid new product query has been prepared", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
            testRunner.And("a request has been made to submit the new product query", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            testRunner.When("a request is made to flag the product query as ready for processing with a status" +
                    " of submitted", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
            testRunner.And("the product query is retrieved from the database", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            testRunner.And("the product query group for the new product query is retrieved from the database", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            testRunner.Then("the HTTP status code is 202", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
            testRunner.And("the product query in the response body has the correct self link", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            testRunner.And("the product query in the response body has the correct enclosure link", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            testRunner.And("the product query in the response body has the correct monitor link", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            testRunner.And("the product query in the response body has the same created date as that in the d" +
                    "atabase", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            testRunner.And("the product query in the response body has a status of Submitted", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Flagging a product query as ready for processing creates a message on the queue")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "ReadyForProcessing")]
        public virtual void FlaggingAProductQueryAsReadyForProcessingCreatesAMessageOnTheQueue()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Flagging a product query as ready for processing creates a message on the queue", ((string[])(null)));
            this.ScenarioSetup(scenarioInfo);
            testRunner.Given("the web job is not running", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
            testRunner.And("the message queue is empty", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            testRunner.And("a valid new product query has been prepared", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            testRunner.And("a request has been made to submit the new product query", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            testRunner.When("a request is made to flag the product query as ready for processing with a status" +
                    " of submitted", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
            testRunner.Then("the HTTP status code is 202", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
            testRunner.And("a message has been created on the queue", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Flagging a product query as ready for processing with an identifier that exists b" +
            "ut in a different culture returns the correct response")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "ReadyForProcessing")]
        public virtual void FlaggingAProductQueryAsReadyForProcessingWithAnIdentifierThatExistsButInADifferentCultureReturnsTheCorrectResponse()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Flagging a product query as ready for processing with an identifier that exists b" +
                    "ut in a different culture returns the correct response", ((string[])(null)));
            this.ScenarioSetup(scenarioInfo);
            testRunner.Given("a valid new product query with a culture of en-US has been prepared", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
            testRunner.And("a request has been made to submit the new product query", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            testRunner.And("the culture of the new product query is updated to en-GB", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            testRunner.When("a request is made to flag the product query as ready for processing with a status" +
                    " of submitted", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
            testRunner.Then("the HTTP status code is 303", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
            testRunner.And("the HTTP location header is /product-query/{id}/culture/en-US", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Flagging a product query as ready for processing with an invalid GUID returns the" +
            " correct response")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "ReadyForProcessing")]
        public virtual void FlaggingAProductQueryAsReadyForProcessingWithAnInvalidGUIDReturnsTheCorrectResponse()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Flagging a product query as ready for processing with an invalid GUID returns the" +
                    " correct response", ((string[])(null)));
            this.ScenarioSetup(scenarioInfo);
            testRunner.Given("a new product query with an invalid guid has been prepared", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
            testRunner.When("a request is made to flag the product query as ready for processing with a status" +
                    " of submitted", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
            testRunner.Then("the HTTP status code is 400", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
            testRunner.And("an HTTP problem can be retrieved from the response body", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            testRunner.And("the HTTP problem is of type http://problems.rakuten.com/invalid-request-parameter" +
                    "", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            testRunner.And("the HTTP problem title is An invalid request parameter was supplied.", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            testRunner.And("the HTTP problem detail for the product query request is The product query identi" +
                    "fier \'{id}\' in the request URI is invalid. It must be a GUID.", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Flagging a product query as ready for processing with an invalid culture returns " +
            "the correct response")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "ReadyForProcessing")]
        public virtual void FlaggingAProductQueryAsReadyForProcessingWithAnInvalidCultureReturnsTheCorrectResponse()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Flagging a product query as ready for processing with an invalid culture returns " +
                    "the correct response", ((string[])(null)));
            this.ScenarioSetup(scenarioInfo);
            testRunner.Given("a new product query with an invalid culture has been prepared", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
            testRunner.When("a request is made to flag the product query as ready for processing with a status" +
                    " of submitted", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
            testRunner.Then("the HTTP status code is 400", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
            testRunner.And("an HTTP problem can be retrieved from the response body", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            testRunner.And("the HTTP problem is of type http://problems.rakuten.com/invalid-request-parameter" +
                    "", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            testRunner.And("the HTTP problem title is An invalid request parameter was supplied.", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            testRunner.And("the HTTP problem detail for the product query request is The culture \'{culture}\' " +
                    "in the request URI is invalid. It must be a valid language tag (as per BCP 47).", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Flagging a product query as ready for processing with an identifier that does not" +
            " exist returns the correct response")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "ReadyForProcessing")]
        public virtual void FlaggingAProductQueryAsReadyForProcessingWithAnIdentifierThatDoesNotExistReturnsTheCorrectResponse()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Flagging a product query as ready for processing with an identifier that does not" +
                    " exist returns the correct response", ((string[])(null)));
            this.ScenarioSetup(scenarioInfo);
            testRunner.Given("a valid new product query has been prepared", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
            testRunner.When("a request is made to flag the product query as ready for processing with a status" +
                    " of submitted", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
            testRunner.Then("the HTTP status code is 404", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
            testRunner.And("an HTTP problem can be retrieved from the response body", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            testRunner.And("the HTTP problem is of type http://problems.rakuten.com/product-query-not-found", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            testRunner.And("the HTTP problem title is The product query could not be found.", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            testRunner.And("the HTTP problem detail for the product query request is Failed to find a product" +
                    " query with identifier \'{id}\'.", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Flagging a product query as ready for processing and supplying an invalid status " +
            "returns the correct response")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "ReadyForProcessing")]
        public virtual void FlaggingAProductQueryAsReadyForProcessingAndSupplyingAnInvalidStatusReturnsTheCorrectResponse()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Flagging a product query as ready for processing and supplying an invalid status " +
                    "returns the correct response", ((string[])(null)));
            this.ScenarioSetup(scenarioInfo);
            testRunner.Given("a valid new product query has been prepared", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
            testRunner.And("a request has been made to submit the new product query", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            testRunner.When("a request is made to flag the product query as ready for processing with a status" +
                    " of i-am-invalid", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
            testRunner.Then("the HTTP status code is 403", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
            testRunner.And("an HTTP problem can be retrieved from the response body", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            testRunner.And("the HTTP problem is of type http://problems.rakuten.com/invalid-product-query-sta" +
                    "tus", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            testRunner.And("the HTTP problem title is An invalid product query status was supplied.", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            testRunner.And(@"the HTTP problem detail for the product query request is You attempted to update the status of the query at '/product-query/{id}/culture/{culture}' to '{status}', which is an invalid status.  The only valid status to which this query can be set is 'submitted'.", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            testRunner.And("the HTTP problem contains a link of relation type http://rels.rakuten.com/product" +
                    "-query", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            testRunner.And("the HTTP problem link of relation type http://rels.rakuten.com/product-query has " +
                    "an href that points to the product query", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Flagging a product query as ready for processing when it has already been flagged" +
            " as such returns the correct response")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "ReadyForProcessing")]
        public virtual void FlaggingAProductQueryAsReadyForProcessingWhenItHasAlreadyBeenFlaggedAsSuchReturnsTheCorrectResponse()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Flagging a product query as ready for processing when it has already been flagged" +
                    " as such returns the correct response", ((string[])(null)));
            this.ScenarioSetup(scenarioInfo);
            testRunner.Given("a valid new product query has been prepared", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
            testRunner.And("a request has been made to submit the new product query", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            testRunner.When("a request is made to flag the product query as ready for processing with a status" +
                    " of submitted", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
            testRunner.And("a request is made to flag the product query as ready for processing with a status" +
                    " of submitted", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            testRunner.Then("the HTTP status code is 403", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
            testRunner.And("an HTTP problem can be retrieved from the response body", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            testRunner.And("the HTTP problem is of type http://problems.rakuten.com/product-query-already-rea" +
                    "dy-for-processing", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            testRunner.And("the HTTP problem title is The product query has already been flagged as ready for" +
                    " processing.", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            testRunner.And("the HTTP problem detail for the product query request is You attempted to flag th" +
                    "e query at \'/product-query/{id}/culture/{culture}\' as ready for processing but h" +
                    "as already been flagged as such.", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            testRunner.And("the HTTP problem contains a link of relation type http://rels.rakuten.com/product" +
                    "-query", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            testRunner.And("the HTTP problem link of relation type http://rels.rakuten.com/product-query has " +
                    "an href that points to the product query", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion

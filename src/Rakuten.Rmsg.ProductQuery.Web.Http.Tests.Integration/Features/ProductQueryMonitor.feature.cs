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
    public partial class ProductQueryMonitorFeature
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.ClassInitializeAttribute()]
        public static void FeatureSetup(Microsoft.VisualStudio.TestTools.UnitTesting.TestContext testContext)
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "ProductQueryMonitor", "Ensures that when using the API endpoint for monitoring the status of a product q" +
                    "uery group\r\nthat the API operates according to the specification.", ProgrammingLanguage.CSharp, ((string[])(null)));
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
                        && (TechTalk.SpecFlow.FeatureContext.Current.FeatureInfo.Title != "ProductQueryMonitor")))
            {
                Rakuten.Rmsg.ProductQuery.Web.Http.Tests.Integration.Features.ProductQueryMonitorFeature.FeatureSetup(null);
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
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Getting the status of a product query group returns the correct response")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "ProductQueryMonitor")]
        public virtual void GettingTheStatusOfAProductQueryGroupReturnsTheCorrectResponse()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Getting the status of a product query group returns the correct response", ((string[])(null)));
            this.ScenarioSetup(scenarioInfo);
            testRunner.Given("a valid new product query has been prepared", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
            testRunner.And("a request has been made to submit the new product query", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            testRunner.And("the product query group for the new product query has been retrieved from the dat" +
                    "abase", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            testRunner.And("a product query monitor request for the new product query has been prepared", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            testRunner.When("a request is made to get the status of a product query group using the prepared r" +
                    "equest", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
            testRunner.Then("the HTTP status code is 200", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
            testRunner.And("the HTTP content type is image/png", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            testRunner.And("the response body contains an image", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Getting the status of a product query group that does not exist returns the corre" +
            "ct response")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "ProductQueryMonitor")]
        public virtual void GettingTheStatusOfAProductQueryGroupThatDoesNotExistReturnsTheCorrectResponse()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Getting the status of a product query group that does not exist returns the corre" +
                    "ct response", ((string[])(null)));
            this.ScenarioSetup(scenarioInfo);
            testRunner.Given("a product query monitor request for a non-existent product query group has been p" +
                    "repared", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
            testRunner.When("a request is made to get the status of a product query group using the prepared r" +
                    "equest", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
            testRunner.Then("the HTTP status code is 404", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
            testRunner.And("an HTTP problem can be retrieved from the response body", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            testRunner.And("the HTTP problem is of type http://problems.rakuten.com/product-query-group-not-f" +
                    "ound", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            testRunner.And("the HTTP problem title is The product query group could not be found.", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            testRunner.And("the HTTP problem detail for the product query monitor request is Failed to find a" +
                    " product query group with identifier \'{id}\'.", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Getting the status of a product query group with an identifier that is not a vali" +
            "d GUID returns the correct response")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "ProductQueryMonitor")]
        public virtual void GettingTheStatusOfAProductQueryGroupWithAnIdentifierThatIsNotAValidGUIDReturnsTheCorrectResponse()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Getting the status of a product query group with an identifier that is not a vali" +
                    "d GUID returns the correct response", ((string[])(null)));
            this.ScenarioSetup(scenarioInfo);
            testRunner.Given("a product query monitor request with an identifier that is not a GUID has been pr" +
                    "epared", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
            testRunner.When("a request is made to get the status of a product query group using the prepared r" +
                    "equest", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
            testRunner.Then("the HTTP status code is 400", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
            testRunner.And("an HTTP problem can be retrieved from the response body", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            testRunner.And("the HTTP problem is of type http://problems.rakuten.com/invalid-request-parameter" +
                    "", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            testRunner.And("the HTTP problem title is An invalid request parameter was supplied.", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            testRunner.And("the HTTP problem detail for the product query monitor request is The product quer" +
                    "y group identifier \'{id}\' in the request URI is invalid. It must be a GUID.", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Getting the status of a product query group with a date/time that is not a valid " +
            "date/time returns the correct response")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "ProductQueryMonitor")]
        public virtual void GettingTheStatusOfAProductQueryGroupWithADateTimeThatIsNotAValidDateTimeReturnsTheCorrectResponse()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Getting the status of a product query group with a date/time that is not a valid " +
                    "date/time returns the correct response", ((string[])(null)));
            this.ScenarioSetup(scenarioInfo);
            testRunner.Given("a product query monitor request with an invalid date/time has been prepared", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
            testRunner.When("a request is made to get the status of a product query group using the prepared r" +
                    "equest", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
            testRunner.Then("the HTTP status code is 400", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
            testRunner.And("an HTTP problem can be retrieved from the response body", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            testRunner.And("the HTTP problem is of type http://problems.rakuten.com/invalid-request-parameter" +
                    "", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            testRunner.And("the HTTP problem title is An invalid request parameter was supplied.", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            testRunner.And("the HTTP problem detail for the product query monitor request is The date portion" +
                    "s \'{datetime}\' in the request URI do not form a valid date.  Please ensure they " +
                    "are in /yyyy/MM/dd/HH/mm format.", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Getting the status of a product query group for a date/time that is too recent re" +
            "turns the correct response")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "ProductQueryMonitor")]
        public virtual void GettingTheStatusOfAProductQueryGroupForADateTimeThatIsTooRecentReturnsTheCorrectResponse()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Getting the status of a product query group for a date/time that is too recent re" +
                    "turns the correct response", ((string[])(null)));
            this.ScenarioSetup(scenarioInfo);
            testRunner.Given("a product query monitor request with a date/time that is too recent has been prep" +
                    "ared", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
            testRunner.When("a request is made to get the status of a product query group using the prepared r" +
                    "equest", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
            testRunner.Then("the HTTP status code is 303", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
            testRunner.And("the HTTP location header for the product query monitor request is the current dat" +
                    "e/time", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            testRunner.And("the HTTP retry-after header has the same value as the progress map interval in se" +
                    "conds", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion

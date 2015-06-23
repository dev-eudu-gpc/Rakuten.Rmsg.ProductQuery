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
    public partial class NewMessageProcessingFeature
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.ClassInitializeAttribute()]
        public static void FeatureSetup(Microsoft.VisualStudio.TestTools.UnitTesting.TestContext testContext)
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "NewMessageProcessing", "Ensures that message processing performs as expected", ProgrammingLanguage.CSharp, ((string[])(null)));
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
                        && (TechTalk.SpecFlow.FeatureContext.Current.FeatureInfo.Title != "NewMessageProcessing")))
            {
                Rakuten.Rmsg.ProductQuery.Web.Http.Tests.Integration.Features.NewMessageProcessingFeature.FeatureSetup(null);
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
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("A query file with a single valid product is processed correctly")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "NewMessageProcessing")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("WebJob")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("GpcCoreApi")]
        public virtual void AQueryFileWithASingleValidProductIsProcessedCorrectly()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("A query file with a single valid product is processed correctly", new string[] {
                        "WebJob",
                        "GpcCoreApi"});
            this.ScenarioSetup(scenarioInfo);
            testRunner.Given("the web job is stopped", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
            testRunner.And("the message queue is empty", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            testRunner.And("the dead letter message queue is empty", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            testRunner.And("a new product has been created in GPC for the culture en-US", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            testRunner.And("a valid new product query has been prepared for the culture en-US", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            testRunner.And("a product query file for the new product has been created", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            testRunner.And("a request has been made to submit the new product query", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            testRunner.And("the file is uploaded to blob storage", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            testRunner.And("a request is made to flag the product query as ready for processing with a status" +
                    " of submitted", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            testRunner.And("a message has been created on the queue", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            testRunner.When("the web job is started", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
            testRunner.And("the status of the product query is completed", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            testRunner.And("the product query is retrieved from the database", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            testRunner.And("the results file is retrieved from storage", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            testRunner.And("the items have been parsed from the results file", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            testRunner.Then("the message queue is empty", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
            testRunner.And("the dead letter queue is empty", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            testRunner.And("the items in the database match the items in the file", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            testRunner.And("the items in the database have a completed date", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            testRunner.And("the items in the results file have the correct manufacturer", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            testRunner.And("the items in the results file have the correct manufacturer part number", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            testRunner.And("the items in the results file have the correct brand", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            testRunner.And("the items in the results file have the correct video URL", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            testRunner.And("the items in the results file have the correct images", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("A query item with an image in the source file does not have its images updated")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "NewMessageProcessing")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("WebJob")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("GpcCoreApi")]
        public virtual void AQueryItemWithAnImageInTheSourceFileDoesNotHaveItsImagesUpdated()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("A query item with an image in the source file does not have its images updated", new string[] {
                        "WebJob",
                        "GpcCoreApi"});
            this.ScenarioSetup(scenarioInfo);
            testRunner.Given("the web job is stopped", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
            testRunner.And("the message queue is empty", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            testRunner.And("the dead letter message queue is empty", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            testRunner.And("a new product has been created in GPC for the culture en-US", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            testRunner.And("a valid new product query has been prepared for the culture en-US", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            testRunner.And("a product query file containing image urls for the new product has been created", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            testRunner.And("a request has been made to submit the new product query", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            testRunner.And("the file is uploaded to blob storage", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            testRunner.And("a request is made to flag the product query as ready for processing with a status" +
                    " of submitted", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            testRunner.And("a message has been created on the queue", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            testRunner.When("the web job is started", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
            testRunner.And("the status of the product query is completed", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            testRunner.And("the results file is retrieved from storage", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            testRunner.And("the items have been parsed from the results file", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            testRunner.Then("the message queue is empty", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
            testRunner.And("the dead letter queue is empty", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            testRunner.And("the items in the database match the items in the file", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            testRunner.And("the items in the database have a completed date", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            testRunner.And("the images in the file have been preserved", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Query items with no GTIN type are fuzzily matched")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "NewMessageProcessing")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("WebJob")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("GpcCoreApi")]
        public virtual void QueryItemsWithNoGTINTypeAreFuzzilyMatched()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Query items with no GTIN type are fuzzily matched", new string[] {
                        "WebJob",
                        "GpcCoreApi"});
            this.ScenarioSetup(scenarioInfo);
            testRunner.Given("the web job is stopped", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
            testRunner.And("the message queue is empty", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            testRunner.And("the dead letter message queue is empty", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            testRunner.And("a new product has been created in GPC for the culture en-US", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            testRunner.And("a valid new product query has been prepared for the culture en-US", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            testRunner.And("a product query file with no GTIN type for the new product has been created", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            testRunner.And("a request has been made to submit the new product query", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            testRunner.And("the file is uploaded to blob storage", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            testRunner.And("a request is made to flag the product query as ready for processing with a status" +
                    " of submitted", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            testRunner.And("a message has been created on the queue", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            testRunner.When("the web job is started", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
            testRunner.And("the status of the product query is completed", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            testRunner.And("the results file is retrieved from storage", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            testRunner.And("the items have been parsed from the results file", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            testRunner.Then("the message queue is empty", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
            testRunner.And("the dead letter queue is empty", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            testRunner.And("there are no items for the product query in the database", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            testRunner.And("the item in the results file is the same as the item in the source file", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Query items with no GTIN value are ignored")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "NewMessageProcessing")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("WebJob")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("GpcCoreApi")]
        public virtual void QueryItemsWithNoGTINValueAreIgnored()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Query items with no GTIN value are ignored", new string[] {
                        "WebJob",
                        "GpcCoreApi"});
            this.ScenarioSetup(scenarioInfo);
            testRunner.Given("the web job is stopped", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
            testRunner.And("the message queue is empty", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            testRunner.And("the dead letter message queue is empty", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            testRunner.And("a new product has been created in GPC for the culture en-US", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            testRunner.And("a valid new product query has been prepared for the culture en-US", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            testRunner.And("a product query file with no GTIN value for the new product has been created", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            testRunner.And("a request has been made to submit the new product query", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            testRunner.And("the file is uploaded to blob storage", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            testRunner.And("a request is made to flag the product query as ready for processing with a status" +
                    " of submitted", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            testRunner.And("a message has been created on the queue", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            testRunner.When("the web job is started", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
            testRunner.And("the status of the product query is completed", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            testRunner.And("the results file is retrieved from storage", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            testRunner.And("the items have been parsed from the results file", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            testRunner.Then("the message queue is empty", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
            testRunner.And("the dead letter queue is empty", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            testRunner.And("there are no items for the product query in the database", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            testRunner.And("the item in the results file is the same as the item in the source file", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Files with only some rows having GTINs are correctly processed")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "NewMessageProcessing")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("WebJob")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("GpcCoreApi")]
        public virtual void FilesWithOnlySomeRowsHavingGTINsAreCorrectlyProcessed()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Files with only some rows having GTINs are correctly processed", new string[] {
                        "WebJob",
                        "GpcCoreApi"});
            this.ScenarioSetup(scenarioInfo);
            testRunner.Given("the web job is stopped", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
            testRunner.And("the message queue is empty", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            testRunner.And("the dead letter message queue is empty", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            testRunner.And("a new product has been created in GPC for the culture en-US", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            testRunner.And("a valid new product query has been prepared for the culture en-US", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            testRunner.And("a product query file with only some rows having GTINs has been created", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            testRunner.And("a request has been made to submit the new product query", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            testRunner.And("the file is uploaded to blob storage", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            testRunner.And("a request is made to flag the product query as ready for processing with a status" +
                    " of submitted", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            testRunner.And("a message has been created on the queue", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            testRunner.When("the web job is started", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
            testRunner.And("the status of the product query is completed", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            testRunner.And("the results file is retrieved from storage", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            testRunner.And("the items have been parsed from the results file", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            testRunner.Then("the message queue is empty", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
            testRunner.And("the dead letter queue is empty", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            testRunner.And("the items in the database match the valid items in the file", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            testRunner.And("the items in the database have a completed date", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            testRunner.And("the valid items in the results file have the correct manufacturer", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            testRunner.And("the valid items in the results file have the correct manufacturer part number", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            testRunner.And("the valid items in the results file have the correct brand", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            testRunner.And("the valid items in the results file have the correct video URL", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            testRunner.And("the valid items in the results file have the correct images", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            testRunner.And("the items that do not have a GTIN value are the same in the results file as in th" +
                    "e source file", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("A file with no header row is rejected")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "NewMessageProcessing")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("WebJob")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("GpcCoreApi")]
        public virtual void AFileWithNoHeaderRowIsRejected()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("A file with no header row is rejected", new string[] {
                        "WebJob",
                        "GpcCoreApi"});
            this.ScenarioSetup(scenarioInfo);
            testRunner.Given("the web job is stopped", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
            testRunner.And("the message queue is empty", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            testRunner.And("the dead letter message queue is empty", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            testRunner.And("a new product has been created in GPC for the culture en-US", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            testRunner.And("a valid new product query has been prepared for the culture en-US", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            testRunner.And("a product query file with no header row has been created", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            testRunner.And("a request has been made to submit the new product query", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            testRunner.And("the file is uploaded to blob storage", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            testRunner.And("a request is made to flag the product query as ready for processing with a status" +
                    " of submitted", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            testRunner.And("a message has been created on the queue", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            testRunner.When("the web job is started", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
            testRunner.Then("the message can be found in the dead letter queue", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
            testRunner.And("the message queue is empty", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            testRunner.And("there are no items for the product query in the database", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("A file with a row with insufficient columns is processed correctly")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "NewMessageProcessing")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("WebJob")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("GpcCoreApi")]
        public virtual void AFileWithARowWithInsufficientColumnsIsProcessedCorrectly()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("A file with a row with insufficient columns is processed correctly", new string[] {
                        "WebJob",
                        "GpcCoreApi"});
            this.ScenarioSetup(scenarioInfo);
            testRunner.Given("the web job is stopped", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
            testRunner.And("the message queue is empty", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            testRunner.And("the dead letter message queue is empty", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            testRunner.And("a new product has been created in GPC for the culture en-US", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            testRunner.And("a valid new product query has been prepared for the culture en-US", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            testRunner.And("a product query file for the new product and an additional row with insufficient " +
                    "columns has been created", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            testRunner.And("a request has been made to submit the new product query", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            testRunner.And("the file is uploaded to blob storage", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            testRunner.And("a request is made to flag the product query as ready for processing with a status" +
                    " of submitted", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            testRunner.And("a message has been created on the queue", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            testRunner.When("the web job is started", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
            testRunner.And("the status of the product query is completed", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            testRunner.And("the product query is retrieved from the database", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            testRunner.And("the results file is retrieved from storage", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            testRunner.And("the items have been parsed from the results file", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            testRunner.Then("the message queue is empty", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
            testRunner.And("the dead letter queue is empty", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            testRunner.And("the items in the database match the valid items in the file", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            testRunner.And("the items in the database have a completed date", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            testRunner.And("the valid items in the results file have the correct manufacturer", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            testRunner.And("the valid items in the results file have the correct manufacturer part number", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            testRunner.And("the valid items in the results file have the correct brand", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            testRunner.And("the valid items in the results file have the correct video URL", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            testRunner.And("the valid items in the results file have the correct images", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            testRunner.And("the items that have insufficient columns are not present in the results file", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Messages for product queries where no blob has been uploaded are processed correc" +
            "tly")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "NewMessageProcessing")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("WebJob")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("GpcCoreApi")]
        public virtual void MessagesForProductQueriesWhereNoBlobHasBeenUploadedAreProcessedCorrectly()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Messages for product queries where no blob has been uploaded are processed correc" +
                    "tly", new string[] {
                        "WebJob",
                        "GpcCoreApi"});
            this.ScenarioSetup(scenarioInfo);
            testRunner.Given("the web job is stopped", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
            testRunner.And("the message queue is empty", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            testRunner.And("the dead letter message queue is empty", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            testRunner.And("a valid new product query has been prepared for the culture en-US", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            testRunner.And("a request has been made to submit the new product query", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            testRunner.And("a request is made to flag the product query as ready for processing with a status" +
                    " of submitted", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            testRunner.And("a message has been created on the queue", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            testRunner.When("the web job is started", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
            testRunner.Then("the message can be found in the dead letter queue", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
            testRunner.And("the message queue is empty", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            testRunner.And("there are no items for the product query in the database", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion

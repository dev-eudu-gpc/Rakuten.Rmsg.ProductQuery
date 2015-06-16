Feature: NewMessageProcessing
	Ensures the processing of new messages on the queue

@GpcCoreApi @WebJob
Scenario: A query file with a single valid product is processed correctly
	Given the web job is stopped
	And the message queue is empty
	And the dead letter message queue is empty
	And a new product has been created in GPC
	And a valid new product query has been prepared
	And a product query file for the new product has been created
	And a request has been made to submit the new product query
	And the file is uploaded to blob storage
	And a request is made to flag the product query as ready for processing with a status of submitted
	And a message has been created on the queue
	#When the web job is started
	When the message queue is empty once again
	And the product query is retrieved from the database
	And the file is retrieved from storage
	#Then the dead letter queue has remained empty
	Then the items in the database match the items in the file
	#And some assertions about the product details
	#And the item in the updated file has the correct manufacturer
	#And the item in the updated file has the correct manufacturer part number
	#And the item in the updated file has the correct brand
	#And the item in the updated file has the correct video URL
	#And the item in the updated file has the correct images
	And the status of the product query from the database is completed

@GpcCoreApi @WebJob
Scenario: A query item with an image in the source file does not have its images updated

@GpcCoreApi @WebJob
Scenario: Query items with no GTIN type are ignored

@GpcCoreApi @WebJob
Scenario: Query items with no GTIN value are ignored

@GpcCoreApi @WebJob
Scenario: Files with some rows having GTINs and others not are processed correctly

@GpcCoreApi @WebJob
Scenario: A file with no header row is rejected

@GpcCoreApi @WebJob
Scenario: A file with a row with insufficient columns is not processed correctly

@GpcCoreApi @WebJob
Scenario: Messages for product queries where no blob has been uploaded are processed accordingly
    Given the web job is stopped
	And the message queue is empty
	And the dead letter message queue is empty
	And a valid new product query has been prepared
	And a request has been made to submit the new product query
	And a request is made to flag the product query as ready for processing with a status of submitted
	And a message has been created on the queue
	When the web job is started
	Then the message can be found in the dead letter queue
	And the message queue is empty
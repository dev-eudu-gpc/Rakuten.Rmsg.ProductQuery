Feature: NewMessageProcessing
	Ensures that message processing performs as expected

# TODO: Use cultures in tests

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
	When the web job is started
	And the status of the product query is completed
	And the product query is retrieved from the database
	And the results file is retrieved from storage
	And the items have been parsed from the results file
	Then the message queue is empty
	And the dead letter queue is empty
	And the items in the database match the items in the file
	And the items in the results file have the correct manufacturer
	And the items in the results file have the correct manufacturer part number
	And the items in the results file have the correct brand
	And the items in the results file have the correct video URL
	And the items in the results file have the correct images

@GpcCoreApi @WebJob
Scenario: A query item with an image in the source file does not have its images updated
	Given the web job is stopped
	And the message queue is empty
	And the dead letter message queue is empty
	And a new product has been created in GPC
	And a valid new product query has been prepared
	And a product query file containing image urls for the new product has been created
	And a request has been made to submit the new product query
	And the file is uploaded to blob storage
	And a request is made to flag the product query as ready for processing with a status of submitted
	And a message has been created on the queue
	When the web job is started
	And the status of the product query is completed
	And the results file is retrieved from storage
	And the items have been parsed from the results file
	Then the message queue is empty
	And the dead letter queue is empty
	And the items in the database match the items in the file
	And the images in the file have been preserved

@GpcCoreApi @WebJob
Scenario: Query items with no GTIN type are ignored
	Given the web job is stopped
	And the message queue is empty
	And the dead letter message queue is empty
	And a new product has been created in GPC
	And a valid new product query has been prepared
	And a product query file with no GTIN type for the new product has been created
	And a request has been made to submit the new product query
	And the file is uploaded to blob storage
	And a request is made to flag the product query as ready for processing with a status of submitted
	And a message has been created on the queue
	When the web job is started
	And the status of the product query is completed
	And the results file is retrieved from storage
	And the items have been parsed from the results file
	Then the message queue is empty
	And the dead letter queue is empty
	And there are no items for the product query in the database
	And the item in the results file is the same as the item in the source file

@GpcCoreApi @WebJob
Scenario: Query items with no GTIN value are ignored
	Given the web job is stopped
	And the message queue is empty
	And the dead letter message queue is empty
	And a new product has been created in GPC
	And a valid new product query has been prepared
	And a product query file with no GTIN value for the new product has been created
	And a request has been made to submit the new product query
	And the file is uploaded to blob storage
	And a request is made to flag the product query as ready for processing with a status of submitted
	And a message has been created on the queue
	When the web job is started
	And the status of the product query is completed
	And the results file is retrieved from storage
	And the items have been parsed from the results file
	Then the message queue is empty
	And the dead letter queue is empty
	And there are no items for the product query in the database
	And the item in the results file is the same as the item in the source file

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
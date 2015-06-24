Feature: Parsing
	Ensures that messages and blobs are parsed correctly

@WebJob @GpcCoreApi
Scenario: Messages for product queries where no blob has been uploaded are processed correctly
    Given the web job is stopped
	And the message queue is empty
	And the dead letter message queue is empty
	And a valid new product query has been prepared for the culture en-US
	And a request has been made to submit the new product query
	And a request is made to flag the product query as ready for processing with a status of submitted
	And a message has been created on the queue
	When the web job is started
	Then the message can be found in the dead letter queue
	And the message queue is empty
	And there are no items for the product query in the database
	
@WebJob @GpcCoreApi
Scenario: Query items with no GTIN value are ignored
	Given the web job is stopped
	And the message queue is empty
	And the dead letter message queue is empty
	And a new product has been created in GPC for the culture en-US
	And a valid new product query has been prepared for the culture en-US
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

@WebJob @GpcCoreApi
Scenario: A file with only some rows having GTINs is correctly processed
	Given the web job is stopped
	And the message queue is empty
	And the dead letter message queue is empty
	And a new product has been created in GPC for the culture en-US
	And a valid new product query has been prepared for the culture en-US
	And a product query file with only some rows having GTINs has been created
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
	And the items in the database match the valid items in the file
	And the items in the database have a valid completed date
	And the items in the database have the correct GRAN
	And the valid items in the results file have the correct manufacturer
	And the valid items in the results file have the correct manufacturer part number
	And the valid items in the results file have the correct brand
	And the valid items in the results file have the correct video URL
	And the valid items in the results file have the correct images
	And the items that do not have a GTIN value are the same in the results file as in the source file

@WebJob @GpcCoreApi
Scenario: A file with no header row is rejected
	Given the web job is stopped
	And the message queue is empty
	And the dead letter message queue is empty
	And a new product has been created in GPC for the culture en-US
	And a valid new product query has been prepared for the culture en-US
	And a product query file with no header row has been created
	And a request has been made to submit the new product query
	And the file is uploaded to blob storage
	And a request is made to flag the product query as ready for processing with a status of submitted
	And a message has been created on the queue
	When the web job is started
	Then the message can be found in the dead letter queue
	And the message queue is empty
	And there are no items for the product query in the database

@WebJob @GpcCoreApi
Scenario: A file with a row with insufficient columns is processed correctly
	Given the web job is stopped
	And the message queue is empty
	And the dead letter message queue is empty
	And a new product has been created in GPC for the culture en-US
	And a valid new product query has been prepared for the culture en-US
	And a product query file for the new product and an additional row with insufficient columns has been created
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
	And the items in the database match the valid items in the file
	And the items in the database have a valid completed date
	And the items in the database have the correct GRAN
	And the valid items in the results file have the correct manufacturer
	And the valid items in the results file have the correct manufacturer part number
	And the valid items in the results file have the correct brand
	And the valid items in the results file have the correct video URL
	And the valid items in the results file have the correct images
	And the items that have insufficient columns are not present in the results file
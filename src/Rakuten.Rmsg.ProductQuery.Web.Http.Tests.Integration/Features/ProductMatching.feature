Feature: ProductMatching
	Ensures that product matching acts according to the specification

Scenario: Products are matched for the culture specified
	Given the web job is stopped
	And the message queue is empty
	And the dead letter message queue is empty
	And a new product has been created in GPC for the culture en-GB
	And a valid new product query has been prepared for the culture en-GB
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
	And the items in the database have a completed date
	And the items in the results file have the correct manufacturer
	And the items in the results file have the correct manufacturer part number
	And the items in the results file have the correct brand
	And the items in the results file have the correct video URL
	And the items in the results file have the correct images

Scenario: Products are not matched if they do not exist in the specified culture
	Given the web job is stopped
	And the message queue is empty
	And the dead letter message queue is empty
	And a new product has been created in GPC for the culture en-US
	And a valid new product query has been prepared for the culture en-GB
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
	And the items in the database have a completed date
	And the items in the results file are the same as the items in the source file

Scenario: Products that have been improved are not matched against
	Given the web job is stopped
	And the message queue is empty
	And the dead letter message queue is empty
	And a new product has been created in GPC for the culture en-US
	And the product is improved
	And a valid new product query has been prepared for the culture en-US
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
	And the items in the database have a completed date
	And the items in the results file are the same as the items in the source file

Scenario: Products with the highest data source trust score are selected first

Scenario: Products with the most recent updated date are selected second

Scenario: Products with the highest GRAN are selected third

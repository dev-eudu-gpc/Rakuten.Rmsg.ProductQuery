Feature: Matching
	Ensures that product matching acts according to the specification

@WebJob @GpcCoreApi
Scenario: Items are matched successfully
	Given the web job is stopped
	And the message queue is empty
	And the dead letter message queue is empty
	And a new product with a culture of en-GB has been created in GPC
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
	And the items in the file can be found in the database
	And the items in the database have the correct GRAN
	And the items in the database have a valid completed date
	And the items in the results file have the correct manufacturer
	And the items in the results file have the correct manufacturer part number
	And the items in the results file have the correct brand
	And the items in the results file have the correct video URL
	And the items in the results file have the correct images

@WebJob @GpcCoreApi
Scenario: Items are matched against products with the highest data source trust score as first priority
	Given the web job is stopped
	And the message queue is empty
	And the dead letter message queue is empty
	And a new product with a culture of en-US has been created in GPC
	And another new product with the same EAN but a lower data source trust score has been created
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
	And the items in the file can be found in the database
	And the items in the database have the correct GRAN
	And the items in the database have a valid completed date
	And the items in the results file have the correct manufacturer
	And the items in the results file have the correct manufacturer part number
	And the items in the results file have the correct brand
	And the items in the results file have the correct video URL
	And the items in the results file have the correct images

@WebJob @GpcCoreApi
Scenario: Items are matched against products with the most recent updated date as second priority
	Given the web job is stopped
	And the message queue is empty
	And the dead letter message queue is empty
	And a new product with a culture of en-US has been created in GPC
	And another new product with the same EAN and data source but a more recent updated date has been created
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
	And the items in the file can be found in the database
	And the items in the database have the correct GRAN
	And the items in the database have a valid completed date
	And the items in the results file have the correct manufacturer
	And the items in the results file have the correct manufacturer part number
	And the items in the results file have the correct brand
	And the items in the results file have the correct video URL
	And the items in the results file have the correct images

@WebJob @GpcCoreApi
Scenario: Items are matched against products with the highest GRAN as third priority
	Given the web job is stopped
	And the message queue is empty
	And the dead letter message queue is empty
	And a new product with a culture of en-US has been created in GPC
	And another new product with the same EAN, data source and updated date has been created
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
	And the items in the file can be found in the database
	And the items in the database have the correct GRAN
	And the items in the database have a valid completed date
	And the items in the results file have the correct manufacturer
	And the items in the results file have the correct manufacturer part number
	And the items in the results file have the correct brand
	And the items in the results file have the correct video URL
	And the items in the results file have the correct images

@WebJob @GpcCoreApi
Scenario: Items with images in the source file do not have their images updated in the results file
	Given the web job is stopped
	And the message queue is empty
	And the dead letter message queue is empty
	And a new product with a culture of en-US has been created in GPC
	And a valid new product query has been prepared for the culture en-US
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
	And the items in the file can be found in the database
	And the items in the database have the correct GRAN
	And the items in the database have a valid completed date
	And the items in the results file have the correct manufacturer
	And the items in the results file have the correct manufacturer part number
	And the items in the results file have the correct brand
	And the items in the results file have the correct video URL
	And the images in the file have been preserved

@WebJob @GpcCoreApi
Scenario Outline: Items with no GTIN type are matched against all GTIN types
	Given the web job is stopped
	And the message queue is empty
	And the dead letter message queue is empty
	And a new product with a culture of en-US has been created in GPC using <identifier> as the identifier
	And a valid new product query has been prepared for the culture en-US
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
	And the items in the file can be found in the database
	And the items in the database have the correct GRAN
	And the items in the database have a valid completed date
	And the items in the results file have the correct manufacturer
	And the items in the results file have the correct manufacturer part number
	And the items in the results file have the correct brand
	And the items in the results file have the correct video URL
	And the items in the results file have the correct images
Examples:
	| identifier |
	| EAN        |
	| ISBN       |
	| JAN        |
	| UPC        |

@WebJob @GpcCoreApi
Scenario: Items are not matched if no products are found for the GTIN
	Given the web job is stopped
	And the message queue is empty
	And the dead letter message queue is empty
	And a new product with a culture of en-US and an EAN that does not exist has been prepared but not created
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
	And the items in the file can be found in the database
	And the items in the database do not have a GRAN
	And the items in the database have a valid completed date
	And the items in the results file are the same as the items in the source file

@WebJob @GpcCoreApi
Scenario: Items are not matched if no products exist in the specified culture
	Given the web job is stopped
	And the message queue is empty
	And the dead letter message queue is empty
	And a new product with a culture of en-US has been created in GPC
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
	And the items in the file can be found in the database
	And the items in the database do not have a GRAN
	And the items in the database have a valid completed date
	And the items in the results file are the same as the items in the source file

@WebJob @GpcCoreApi
Scenario: Items are not matched against products that have been improved
	Given the web job is stopped
	And the message queue is empty
	And the dead letter message queue is empty
	And a new product with a culture of en-US has been created in GPC
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
	And the items in the file can be found in the database
	And the items in the database do not have a GRAN
	And the items in the database have a valid completed date
	And the items in the results file are the same as the items in the source file
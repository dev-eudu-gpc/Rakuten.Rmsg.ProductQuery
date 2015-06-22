Feature: ReadyForProcessing
	Ensures that when using the API endpoint for flagging a product query as ready for processing
	that the API operates according to the specification.

Scenario: Flagging a product query as ready for processing persists the correct information to the database
	Given a valid new product query has been prepared
	And a request has been made to submit the new product query
	When a request is made to flag the product query as ready for processing with a status of submitted
	And the product query is retrieved from the database
	Then the status of the product query from the database is Submitted

Scenario: Flagging a product query as ready for processing returns the correct response
	Given a valid new product query has been prepared
	And a request has been made to submit the new product query
	When a request is made to flag the product query as ready for processing with a status of submitted
	And the product query is retrieved from the database
	And the product query group for the new product query is retrieved from the database
	Then the HTTP status code is 202
	And the product query in the response body has the correct self link
	And the product query in the response body has the correct enclosure link
	And the product query in the response body has the correct monitor link
	And the product query in the response body has the same created date as that in the database
	And the product query in the response body has a status of Submitted

@WebJob
Scenario: Flagging a product query as ready for processing creates a message on the queue
	Given the web job is stopped
	And the message queue is empty
	And a valid new product query has been prepared
	And a request has been made to submit the new product query
	When a request is made to flag the product query as ready for processing with a status of submitted
	Then the HTTP status code is 202
	And a message has been created on the queue
	
Scenario: Flagging a product query as ready for processing with an identifier that exists but in a different culture returns the correct response
	Given a valid new product query with a culture of en-US has been prepared
	And a request has been made to submit the new product query
	And the culture of the new product query is updated to en-GB
	When a request is made to flag the product query as ready for processing with a status of submitted
	Then the HTTP status code is 303
	And the HTTP location header is /product-query/{id}/culture/en-US

Scenario: Flagging a product query as ready for processing with an invalid GUID returns the correct response
	Given a new product query with an invalid guid has been prepared
	When a request is made to flag the product query as ready for processing with a status of submitted
	Then the HTTP status code is 400
	And an HTTP problem can be retrieved from the response body
	And the HTTP problem is of type http://problems.rakuten.com/invalid-request-parameter
	And the HTTP problem title is An invalid request parameter was supplied.
	And the HTTP problem detail for the product query request is The product query identifier '{id}' in the request URI is invalid. It must be a GUID.

Scenario: Flagging a product query as ready for processing with an invalid culture returns the correct response
	Given a new product query with an invalid culture has been prepared
	When a request is made to flag the product query as ready for processing with a status of submitted
	Then the HTTP status code is 400
	And an HTTP problem can be retrieved from the response body
	And the HTTP problem is of type http://problems.rakuten.com/invalid-request-parameter
	And the HTTP problem title is An invalid request parameter was supplied.
	And the HTTP problem detail for the product query request is The culture '{culture}' in the request URI is invalid. It must be a valid language tag (as per BCP 47).

Scenario: Flagging a product query as ready for processing with an identifier that does not exist returns the correct response
	Given a valid new product query has been prepared
	When a request is made to flag the product query as ready for processing with a status of submitted
	Then the HTTP status code is 404
	And an HTTP problem can be retrieved from the response body
	And the HTTP problem is of type http://problems.rakuten.com/product-query-not-found
	And the HTTP problem title is The product query could not be found.
	And the HTTP problem detail for the product query request is Failed to find a product query with identifier '{id}'.

Scenario: Flagging a product query as ready for processing and supplying an invalid status returns the correct response
	Given a valid new product query has been prepared
	And a request has been made to submit the new product query
	When a request is made to flag the product query as ready for processing with a status of i-am-invalid
	Then the HTTP status code is 403
	And an HTTP problem can be retrieved from the response body
	And the HTTP problem is of type http://problems.rakuten.com/invalid-product-query-status
	And the HTTP problem title is An invalid product query status was supplied.
	And the HTTP problem detail for the product query request is You attempted to update the status of the query at '/product-query/{id}/culture/{culture}' to '{status}', which is an invalid status.  The only valid status to which this query can be set is 'submitted'.
	And the HTTP problem contains a link of relation type http://rels.rakuten.com/product-query
	And the HTTP problem link of relation type http://rels.rakuten.com/product-query has an href that points to the product query

Scenario: Flagging a product query as ready for processing when it has already been flagged as such returns the correct response
	Given a valid new product query has been prepared
	And a request has been made to submit the new product query
	When a request is made to flag the product query as ready for processing with a status of submitted
	And a request is made to flag the product query as ready for processing with a status of submitted
	Then the HTTP status code is 403
	And an HTTP problem can be retrieved from the response body
	And the HTTP problem is of type http://problems.rakuten.com/product-query-already-ready-for-processing
	And the HTTP problem title is The product query has already been flagged as ready for processing.
	And the HTTP problem detail for the product query request is You attempted to flag the query at '/product-query/{id}/culture/{culture}' as ready for processing but has already been flagged as such.
	And the HTTP problem contains a link of relation type http://rels.rakuten.com/product-query
	And the HTTP problem link of relation type http://rels.rakuten.com/product-query has an href that points to the product query
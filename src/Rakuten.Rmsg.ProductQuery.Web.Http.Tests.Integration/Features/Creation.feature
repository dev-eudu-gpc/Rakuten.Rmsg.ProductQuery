Feature: Submission
	Ensures that when using the API endpoint for submitting new product queries
	that the API operates according to the specification.

Scenario: Submitting a valid new product query persists the correct information to the database
	Given a valid new product query has been prepared for the culture en-US
	When a request is made to submit the new product query
	Then the status of the product query in the database is New
	And the date created of the product query from the database is not null
	And the culture of the product query in the database matches the culture in the new product query
	And the URI of the product query from the database matches the storage blob URI

Scenario: Submitting a valid new product query returns the correct response
	Given a valid new product query has been prepared for the culture en-US
	When a request is made to submit the new product query
	Then the HTTP status code is 201
	And the product query in the response body has a status of New
	And the product query in the response body has the same created date as that in the database
	And the product query in the response body has the correct self link
	And the product query in the response body has the correct enclosure link
	And the product query in the response body has the correct monitor link

Scenario: Submitting a new product query with an identifier and culture that exists returns the correct response
	Given a valid new product query has been prepared for the culture en-US
	And a request has been made to submit the new product query
	When a request is made to submit the new product query again
	Then the HTTP status code is 200
	And the product query in the response body has the same status as that in the database
	And the product query in the response body has the same created date as that in the database
	And the product query in the response body has the correct self link
	And the product query in the response body has the correct enclosure link
	And the product query in the response body has the correct monitor link

Scenario: Submitting a new product query with an identifier that exists but in a different culture returns the correct response
	Given a valid new product query has been prepared for the culture en-US
	And a request has been made to submit the new product query
	And the culture of the new product query is updated to en-GB
	When a request is made to submit the new product query
	Then the HTTP status code is 303
	And the HTTP location header is /product-query/{id}/culture/en-US

Scenario: Submitting a new product query with an invalid GUID returns the correct response
	Given a new product query with an invalid guid has been prepared
	When a request is made to submit the new product query
	Then the HTTP status code is 400
	And the HTTP problem is of type http://problems.rakuten.com/invalid-request-parameter
	And the HTTP problem title is An invalid request parameter was supplied.
	And the HTTP problem detail for the product query request is The product query identifier '{id}' in the request URI is invalid. It must be a GUID.

Scenario: Submitting a new product query with an invalid culture returns the correct response
	Given a new product query with an invalid culture has been prepared
	When a request is made to submit the new product query
	Then the HTTP status code is 400
	And the HTTP problem is of type http://problems.rakuten.com/invalid-request-parameter
	And the HTTP problem title is An invalid request parameter was supplied.
	And the HTTP problem detail for the product query request is The culture '{culture}' in the request URI is invalid. It must be a valid language tag (as per BCP 47).
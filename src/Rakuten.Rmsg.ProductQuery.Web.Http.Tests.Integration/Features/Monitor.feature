Feature: Monitor
	Ensures that when using the API endpoint for monitoring the status of a product query group
	that the API operates according to the specification.

Scenario: Getting the status of a product query group returns the correct response
	Given a valid new product query has been prepared for the culture en-US
	And a request has been made to submit the new product query
	When a request is made to get the status of the product query group of the new product query
	Then the HTTP status code is 200
	And the HTTP content type is image/png
	And the response body contains an image

Scenario: Getting the status of a product query group that does not exist returns the correct response
	When a request is made to get the status of a product query group which does not exist
	Then the HTTP status code is 404
	And the HTTP problem is of type http://problems.rakuten.com/product-query-group-not-found
	And the HTTP problem title is The product query group could not be found.
	And the HTTP problem detail for the product query monitor request is Failed to find a product query group with identifier '{id}'.

Scenario: Getting the status of a product query group with an identifier that is not a valid GUID returns the correct response
	When a request is made to get the status of a product query group with an identifier that is not a GUID
	Then the HTTP status code is 400
	And the HTTP problem is of type http://problems.rakuten.com/invalid-request-parameter
	And the HTTP problem title is An invalid request parameter was supplied.
	And the HTTP problem detail for the product query monitor request is The product query group identifier '{id}' in the request URI is invalid. It must be a GUID.

Scenario: Getting the status of a product query group with a date/time that is not a valid date/time returns the correct response
	Given a valid new product query has been prepared for the culture en-US
	And a request has been made to submit the new product query
	When a request is made to get the status of the product query group using an invalid date/time
	Then the HTTP status code is 400
	And the HTTP problem is of type http://problems.rakuten.com/invalid-request-parameter
	And the HTTP problem title is An invalid request parameter was supplied.
	And the HTTP problem detail for the product query monitor request is The date portions '{datetime}' in the request URI do not form a valid date.  Please ensure they are in /yyyy/MM/dd/HH/mm format.

Scenario: Getting the status of a product query group for a date/time that is too recent returns the correct response
	Given a valid new product query has been prepared for the culture en-US
	And a request has been made to submit the new product query
	When a request is made to get the status of a product query group using a date/time that is too recent
	Then the HTTP status code is 303
	And the HTTP location header for the product query monitor request is the current date/time
	And the HTTP retry-after header has the same value as the progress map interval in seconds
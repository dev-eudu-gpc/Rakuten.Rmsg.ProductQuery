Feature: ProductQueryGroup
	Ensures that when adding a new product query
	that the product query group functionality operates according to the specification

Scenario: Submitting a new product query when there is a sparse product query group updates the database correctly
	Given a valid new product query has been prepared
	And a sparse product query group exists
	And a request is made to submit the new product query
	When the product query is retrieved from the database
	And the sparse product query group is retrieved from the database
	Then the product query group identifier of the product query from the database matches the identifier of the sparse product query group
	And the count of product queries in the sparse product query group has been incremented by 1
	And the index of the product query from the database matches the incremented count of the sparse product query group

Scenario: Submitting a new product query when there is no empty or sparse product query groups updates the database correctly
	Given a valid new product query has been prepared
	And there are no empty or sparse product query groups in the database
	And a request is made to submit the new product query
	When the product query is retrieved from the database
	And the new product query group for the new product query is retrieved from the database
	Then the count of product queries in the new product query group is 1
	And the index of the product query from the database is 1

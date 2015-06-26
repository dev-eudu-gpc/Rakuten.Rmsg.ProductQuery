Feature: Grouping
	Ensures that when adding a new product query
	that the product query group functionality operates according to the specification

Scenario: Submitting a new product query when there is no empty or sparse product query groups updates the database correctly
	Given a valid new product query has been prepared for the culture en-US
	And there are no empty or sparse product query groups in the database
	When a request is made to submit the new product query
	Then the count of product queries in the new product query group is 1
	And the index of the product query from the database is 1

Scenario: Submitting a new product query when there is one empty product query group updates the database correctly
	Given a valid new product query has been prepared for the culture en-US
	And only one empty product query group exists
	When a request is made to submit the new product query
	Then the product query group assigned to the new product query is the correct one
	And the count of product queries in the product query group has been incremented by 1
	And the index of the product query from the database matches the incremented count of the product query group

Scenario: Submitting a new product query when there is one sparse product query group updates the database correctly
	Given a valid new product query has been prepared for the culture en-US
	And only one sparse product query group exists
	When a request is made to submit the new product query
	Then the product query group assigned to the new product query is the correct one
	And the count of product queries in the product query group has been incremented by 1
	And the index of the product query from the database matches the incremented count of the product query group
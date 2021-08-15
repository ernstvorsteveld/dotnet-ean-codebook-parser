Feature: UrlHtmlParserSpec
	EAN Codebook parser using url (http or file)

@mytag
Scenario: Get Gaz EAN Code for postalcode and housenumber 
	Given a configured parser 
	And the postalcode is 6412PP
	And the house number is 10
	And the product type is gas
	When retrieve the ean code
	Then ean1 should be 871688540030933384 and ean2 should be 871688540004448586
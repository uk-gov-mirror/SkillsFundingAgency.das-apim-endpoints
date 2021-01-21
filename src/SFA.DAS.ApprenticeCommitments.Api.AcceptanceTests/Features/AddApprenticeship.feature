﻿@innerApi
@outerApi
Feature: AddApprenticeship
	When an Apprenticeship is approved and forwarded here
	As an outer API
	I want to receive the details and save it to the inner api

Scenario Outline: New apprenticeship is recieved and is valid 
	Given apprenticeship details are valid
	And the inner api is ready
	When the apprenticeship is saved
	Then the result should be Accepted
	And the request to the inner api was mapped correctly
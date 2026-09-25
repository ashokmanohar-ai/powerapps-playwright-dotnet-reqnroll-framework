@api @dataverse @smoke
Feature: Dataverse API
  As an automation engineer
  I want to verify the Dataverse API independently of the UI
  So that service failures are isolated quickly

  Scenario: Resolve the current Dataverse user
    Given Dataverse API is configured
    When I request WhoAmI from Dataverse
    Then Dataverse should return HTTP 200
    And the Dataverse response should contain "UserId"

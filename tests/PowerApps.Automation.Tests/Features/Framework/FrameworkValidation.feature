@framework
Feature: Framework validation
  As an automation engineer
  I want the framework to validate without a live Power Apps environment
  So that package and BDD integration issues are caught in CI

  Scenario: Default configuration is safe
    Given the framework configuration is loaded
    Then the browser should default to "chromium"
    And the default timeout should be greater than zero

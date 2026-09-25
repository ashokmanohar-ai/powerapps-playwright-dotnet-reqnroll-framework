@ui @mda @smoke
Feature: Model-Driven App smoke
  As a Power Apps user
  I want the configured Model-Driven App to load
  So that core navigation is available

  Scenario: Open the configured Model-Driven App
    Given I open the configured Model-Driven App
    Then the Model-Driven App URL should contain "dynamics.com"

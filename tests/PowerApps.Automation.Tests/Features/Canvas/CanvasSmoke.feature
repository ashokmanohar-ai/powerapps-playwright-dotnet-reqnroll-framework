@ui @canvas @smoke
Feature: Canvas App smoke
  As a Power Apps user
  I want critical Canvas App controls to work
  So that the app can be released with confidence

  # Replace these example control names with data-control-name values from your app.
  Scenario: Enter data and submit through Canvas controls
    Given I open the configured Canvas App
    When I fill Canvas control "TextInput1" with "Playwright"
    And I click Canvas control "Button1"
    Then Canvas control "Label1" should contain "Playwright"

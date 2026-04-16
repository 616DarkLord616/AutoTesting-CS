Feature: EHU Website Complete User Journey
  As a prospective student
  I want to navigate the EHU website
  So that I can find information, programs, and contacts

  Scenario: 1. Verify Navigation to About Page
    Given I navigate to the EHU home page
    When I click on the About link
    Then the page URL should contain "about"

  Scenario Outline: 2. Verify Search Functionality
    Given I navigate to the EHU home page
    When I search for "<searchTerm>"
    Then the search results should contain "<searchTerm>"
    
    Examples:
      | searchTerm     |
      | study programs |

  Scenario: 3. Verify Language Change to Lithuanian
    Given I navigate to the EHU home page
    When I switch the language to Lithuanian
    Then the page should contain Lithuanian keywords and no English news

  Scenario: 4. Verify Contact Information
    Given I navigate to the Contact page
    Then the page should display expected contact details and social links
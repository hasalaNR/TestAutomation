Feature: Leave Request Management  
  As a manager,  
  I want to request leave

  Background: Manager logs into HRB  
    Given I log into the leave management system as a manager

@TestSetManager
  Scenario: Submit annual leave request: Single day
    When Expand the "Leave" dropdown and navigate to "My leaves"
    And Get the initial 'annual' leave count
    And Click Request New Leave and open the leave request form  
    When Select leave type: Annual leave
    And Pick start date as Today+3 and end date as Today+3
    And Enter note: Test leave Single Day
    And Click button: Request leave
    And Click pop up confirm
    Then Snack bar should displayed with message 'Request has been auto approved!'
    And Return to the "My leaves" page
    And Verify 'annual' leave balance is increased by '1'

@TestSetManager
  Scenario: Submit annual leave request: Multiple days
    When Expand the "Leave" dropdown and navigate to "My leaves"
    And Get the initial 'annual' leave count
    And Click Request New Leave and open the leave request form  
    When Select leave type: Annual leave
    And Pick start date as Today+5 and end date as Today+6
    And Enter note: Test leave Multiple Days
    And Click button: Request leave
    And Click pop up confirm
    Then Snack bar should displayed with message 'Request has been auto approved!'
    And Return to the "My leaves" page
    And Verify 'annual' leave balance is increased by '2'

@TestSetManager
  Scenario: Submit annual leave request: Overlapping
    When Expand the "Leave" dropdown and navigate to "My leaves"
    And Get the initial 'annual' leave count
    And Click Request New Leave and open the leave request form  
    When Select leave type: Annual leave
    And Pick start date as Today+1 and end date as Today+12
    Then Verify the error message: 'Absence period overlaps with Vacation leave'
     
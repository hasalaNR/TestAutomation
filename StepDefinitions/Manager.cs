using TestAutomationLeaveMgt.Pages;
using TestAutomationLeaveMgt.Utilities;
using NUnit.Framework;
using OpenQA.Selenium;
using TechTalk.SpecFlow;
using OpenQA.Selenium.DevTools;

namespace TestAutomationLeaveMgt.StepDefinitions
{
    [Binding]
    public class LeaveManagementSteps
    {
        private IWebDriver driver;
        private LoginPage loginPage;
        private QuickAccessPage quickAccessPage;
        private MyLeavePage myLeavePage;
        private RequestNewLeavePage requestNewLeavePage;

        private string startDate;
        private string endDate;


        [BeforeScenario]
        public void Setup()
        {
            driver = DriverSetup.InitializeDriver();
            loginPage = new LoginPage(driver);
            quickAccessPage = new QuickAccessPage(driver);
            myLeavePage = new MyLeavePage(driver);
            requestNewLeavePage = new RequestNewLeavePage(driver);
        }

        [AfterScenario]
        public void TearDown()
        {
            driver.Quit();
        }

        [Given(@"I log into the leave management system as a (.*)")]
        public void GivenILogIntoTheLeaveManagementSystemAsA(string role)
        {
            // Fetch values from the configuration file based on the role
            string systemUrl = ConfigReader.GetUrl();
            string usernameKey = $"{role}_username";
            string passwordKey = $"{role}_password";
            string username = ConfigReader.GetConfigValue(usernameKey);
            string password = ConfigReader.GetConfigValue(passwordKey);

            // Navigate to the system URL and log in
            driver.Navigate().GoToUrl(systemUrl);
            loginPage.EnterEmail(username);
            loginPage.EnterPassword(password);
            loginPage.ClickLoginButton();

            // Use UtilityClass to verify page load and title
            string expectedTitleFragment = "ESS -";
            if (CommonMethods.WaitForPageTitle(driver, expectedTitleFragment))
            {
                Console.WriteLine("User logged in successfully.");
            }
            else
            {
                throw new Exception("Login failed or incorrect page loaded after login.");
            }
        }

        [When(@"Get the initial '([^']*)' leave count")]
        public void WhenGetTheInitialLeaveCount(string leaveType)
        {
            quickAccessPage.SaveInitialLeaveCounts(leaveType);
        }

        [When(@"I navigate to the ""(.*)"" page")]
        public void WhenINavigateToThePage(string page)
        {
            //quickAccessPage.OpenQuickAccessMenu();
            //quickAccessPage.NavigateToMyLeave();
        }

        [When(@"Expand the ""(.*)"" dropdown and navigate to ""(.*)""")]
        public void WhenExpandDropdownAndNavigateTo(string dropdown, string pageName)
        {
            quickAccessPage.ExpandDropdown(dropdown);
            quickAccessPage.NavigateToPage(pageName);
            Assert.IsTrue(myLeavePage.IsPageDisplayed(pageName), $"{pageName} page is not displayed.");

        }

        [When(@"Click Request New Leave and open the leave request form")]
        public void WhenClickAndOpenTheLeaveRequestForm()
        {
            myLeavePage.ClickRequestNewLeaveButton();
          
            Assert.IsTrue(requestNewLeavePage.IsFormDisplayed("Request new leave"), "Request New Leave form is not displayed in the pop-up.");
        }

        [When(@"Select leave type: (.*)")]
        public void WhenSelectLeaveType(string leaveType)
        {
            requestNewLeavePage.SelectLeaveType(leaveType);
        }

        

        [When(@"Pick start date as Today([+-]\d+) and end date as Today([+-]\d+)")]
        public void WhenPickTStartDateAsTodayAndEndDateAsToday(string startDateOffset, string endDateOffset)
        {
            // Use the page object to handle date range selection
            requestNewLeavePage.SelectDateRangeWithOffset(startDateOffset, endDateOffset);
        }

        [When(@"Enter note: (.*)")]
        public void WhenEnterNote(string notes)
        {
            requestNewLeavePage.EnterNotes(notes);
        }

        [When(@"Click button: (.*)")]
        public void WhenClickButton(string buttonName)
        {
            requestNewLeavePage.ClickButton(buttonName);
            Thread.Sleep(1000);
        }

        [When(@"Click pop up confirm")]
        public void WhenClickPopUpConfirm()
        {
            requestNewLeavePage.ConfirmLeaveRequest();
        }

        [Then(@"Return to the ""([^""]*)"" page")]
        public void ThenReturnToThePage(string pageName)
        {
            Thread.Sleep(1000);
            Assert.IsTrue(myLeavePage.IsPageDisplayed(pageName), "Not in "+ pageName+" page.");
        }

        [Then(@"Snack bar should displayed with message '([^']*)'")]
        public void ThenSnackBarShouldDisplayedWithMessage(string message)
        {
            Assert.IsTrue(requestNewLeavePage.IsSnackBarMessageDisplayed(message), "Success message is not displayed.");
        }

        [Then(@"Verify '([^']*)' leave balance is increased by '([^']*)'")]
        public void ThenVerifyLeaveBalanceIsIncreasedBy(string leavetype, string leavedifference)
        {
            int expectedIncrease = int.Parse(leavedifference);

            // Call the helper method for "annual" or other leave types
            quickAccessPage.VerifyLeaveBalanceIncrease(leavetype, expectedIncrease);
        }

        [Then(@"Verify the error message: '([^']*)'")]
        public void ThenVerifyTheErrorMessage(string errorMessage)
        {
            Assert.IsTrue(requestNewLeavePage.IsErrorMessageCorrect(errorMessage),"Error Message is not displayed.");
        }

    }
}

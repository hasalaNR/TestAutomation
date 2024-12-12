using Microsoft.VisualBasic;
using OpenQA.Selenium;

namespace TestAutomationLeaveMgt.Pages
{
    public class QuickAccessPage
    {
        private int beforeCountAnnual;
        private int beforeCountSick;
        private int afterCountAnnual;
        private int afterCountSick;

        private readonly IWebDriver driver;

        private By leaveDropdown = By.XPath("//div[@class=\"menu-link__title\"]/p[text()='Leaves']");
        private By myLeavesOption = By.XPath("//a[text()='My leaves']");
        private By calendarOption = By.XPath("//a[text()='Calendar']");

        public QuickAccessPage(IWebDriver driver)
        {
            this.driver = driver;
        }

        public bool IsDisplayed()
        {
            return driver.Url.Contains("quickaccess");
        }

        public void ExpandDropdown(string dropdownName)
        {
            driver.FindElement(leaveDropdown).Click();
        }

        public bool IsOptionVisible(string optionName)
        {
            if (optionName == "My Leaves")
            {
                return driver.FindElement(myLeavesOption).Displayed;
            }
            else if (optionName == "Calendar")
            {
                return driver.FindElement(calendarOption).Displayed;
            }
            return false;
        }

        public void NavigateToPage(string pageName)
        {
            driver.FindElement(By.XPath("//a[text()='" + pageName + "']")).Click();
        }

        public void SaveInitialLeaveCounts(string leaveType)
        {
            switch (leaveType.ToLower())
            {
                case "annual":
                    beforeCountAnnual = GetLeaveCount("annual");
                    break;

                case "sick":
                    beforeCountSick = GetLeaveCount("sick"); 
                    break;

                default:
                    throw new Exception("Unknown leave type.");
            }
        }

        public void SaveLeaveCountsAfterAction(string leaveType)
        {
            switch (leaveType.ToLower())
            {
                case "annual":
                    afterCountAnnual = GetLeaveCount("annual");
                    break;

                case "sick":
                    afterCountSick = GetLeaveCount("sick");
                    break;

                default:
                    throw new Exception("Unknown leave type.");
            }
        }

        public int GetLeaveCount(string leaveType)
        {
            string leaveCountText;

            switch (leaveType.ToLower())
            {
                case "annual":
                    leaveCountText = driver.FindElement(By.XPath("//div[contains(@class, 'balance-row') and contains(., 'taken')]//span[@class='label-custom']")).Text;
                    break;

                case "sick":
                    // Modify this XPath 
                    leaveCountText = driver.FindElement(By.XPath("//div[contains(@class, 'balance-row') and contains(., 'taken')]//span[@class='label-custom']")).Text;
                    break;

                default:
                    throw new Exception("Unknown leave type.");
            }

            // Sanitize the string to extract only the numeric part
            string numericPart = new string(leaveCountText.Where(char.IsDigit).ToArray());
            return int.Parse(numericPart);
        }


        public void VerifyLeaveBalanceIncrease(string leaveType, int expectedIncrease)
        {
            SaveLeaveCountsAfterAction(leaveType);

            int actualIncrease = (afterCountAnnual- beforeCountAnnual);

            if (actualIncrease != expectedIncrease)
            {
                throw new Exception($"Leave balance for {leaveType} did not increase by the expected amount. " +
                                    $"Expected: {expectedIncrease}, Actual: {actualIncrease}");
            }
        }

    }
}

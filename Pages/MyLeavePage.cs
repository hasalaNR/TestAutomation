using OpenQA.Selenium;
using TestAutomationLeaveMgt.Utilities;

namespace TestAutomationLeaveMgt.Pages
{
    public class MyLeavePage
    {
        private readonly IWebDriver driver;

        private By pageTitle = By.XPath("//div[@class='page-title']");
        private By requestNewLeaveButton = By.XPath("//button[@aria-label='Request new leave']");

        public MyLeavePage(IWebDriver driver)
        {
            this.driver = driver;
        }

        public bool IsPageDisplayed(string pageName)
        {
            IWebElement pageTitleElement = CommonMethods.WaitForElement(driver, pageTitle, TimeSpan.FromSeconds(50));
            return pageTitleElement.Text.Contains(pageName);
        }

        public void ClickRequestNewLeaveButton()
        {
            driver.FindElement(requestNewLeaveButton).Click();
          
        }
    }
}

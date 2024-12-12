using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace TestAutomationLeaveMgt.Utilities
{
    public class CommonMethods
    {
        /// <summary>
        /// Verifies if the current page title contains the expected text with an explicit wait.
        /// </summary>
        /// <param name="driver">WebDriver instance.</param>
        /// <param name="expectedTitleFragment">Expected substring in the title.</param>
        /// <param name="timeoutInSeconds">Timeout duration in seconds.</param>
        /// <returns>True if the title contains the expected substring; otherwise, false.</returns>
        public static bool WaitForPageTitle(IWebDriver driver, string expectedTitleFragment, int timeoutInSeconds = 10)
        {
            try
            {
                // Explicit wait for the page title to match the expected fragment
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));
                bool isTitleCorrect = wait.Until(drv =>
                {
                    string actualTitle = drv.Title;
                    return actualTitle.Contains(expectedTitleFragment, StringComparison.OrdinalIgnoreCase);
                });

                return isTitleCorrect;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while waiting for page title: {ex.Message}");
                return false;
            }
        }

        public static IWebElement WaitForElement(IWebDriver driver, By locator, TimeSpan timeout)
        {
            WebDriverWait wait = new WebDriverWait(driver, timeout);
            return wait.Until(drv => drv.FindElement(locator).Displayed ? drv.FindElement(locator) : null);
        }


        public static void SetImplicitWait(IWebDriver driver, TimeSpan timeout)
        {
            driver.Manage().Timeouts().ImplicitWait = timeout;
        }

    }
}

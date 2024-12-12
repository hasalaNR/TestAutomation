using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;

namespace TestAutomationLeaveMgt.Pages
{
    public class LoginPage
    {
        private IWebDriver driver;

        public LoginPage(IWebDriver driver)
        {
            this.driver = driver;
        }

        private By EmailField = By.XPath("//input[@name='email']");
        private By PasswordField = By.XPath("//input[@name='password']");
        private By LoginButton = By.XPath("//button[@data-test=\"perform-login\"]");

        public void EnterEmail(string email)
        {
            driver.FindElement(EmailField).SendKeys(email);
        }

        public void EnterPassword(string password)
        {
            driver.FindElement(PasswordField).SendKeys(password);
        }

        public void ClickLoginButton()
        {
            driver.FindElement(LoginButton).Click();
        }
    }
}

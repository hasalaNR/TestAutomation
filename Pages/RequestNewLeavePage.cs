using System.Configuration;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using static OpenQA.Selenium.BiDi.Modules.BrowsingContext.Locator;

namespace TestAutomationLeaveMgt.Pages
{
    public class RequestNewLeavePage
    {
        private readonly IWebDriver driver;

        private string startDate;
        private string endDate;

        private By leaveTypeDropdown = By.XPath("//div[@class='c-dd__selected']");
        private By notesField = By.XPath("//textarea[@id='notes']");
        private By balanceSection = By.XPath("balanceSection");
        private By requestLeaveButton = By.XPath("//button/span[text()='Request leave']");
        private By popupHeading = By.XPath("//div[@class=\"dialog__header heading-02\"]/text()");
        private By confirmButton = By.XPath("//button[@data-test=\"primary-action-button\"]");
        private By snackBarMessage = By.XPath("//div[@class=\"snackBar-message body-02\"]");
        private By redAlert = By.XPath("//div[@class='red-alert']//span");

        public RequestNewLeavePage(IWebDriver driver)
        {
            this.driver = driver;
        }

        public bool IsFormDisplayed(string formName)
        {
            return driver.FindElement(By.XPath("//div[@id='modal-title']")).Text.Contains(formName);
        }

        public void SelectLeaveType(string leaveType)
        {
            var dropdown = driver.FindElement(leaveTypeDropdown);
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", dropdown);

            var type = driver.FindElement(By.XPath("//div[contains(@class, 'c-dd__list__row') and text()='" + leaveType + "']"));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", type); 
        }

        public string GetSelectedLeaveType()
        {
            return driver.FindElement(leaveTypeDropdown).Text;
        }

        public void SelectDateRange(string startDate, string endDate)
        {
            try
            {
                IWebElement startElement = driver.FindElement(By.XPath("//div[@id='" + startDate + "']"));

                if (startDate == endDate)
                {
                    IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
                    js.ExecuteScript("document.body.style.zoom='50%'");

                    Thread.Sleep(1000);

                    js.ExecuteScript("arguments[0].click();", startElement);

                    Thread.Sleep(1000);

                    Actions actions = new Actions(driver);

                    IWebElement selectedtElement = driver.FindElement(By.XPath("//div[@class=\"day-cell selected-start\"]"));
                    actions.MoveToElement(selectedtElement);
                    js.ExecuteScript("arguments[0].click();", selectedtElement);

                    // Reset the zoom level to default (100%) after the operation
                    js.ExecuteScript("document.body.style.zoom='100%'");
                }
                else
                {
                    startElement.Click();

                    IWebElement endElement = driver.FindElement(By.XPath("//div[@id='" + endDate + "']"));

                    Actions actions = new Actions(driver);

                    // Perform the drag-and-drop action
                    actions.ClickAndHold(startElement)
                           .MoveToElement(endElement)
                           .Release()
                           .Build()
                           .Perform();
                }
            }
            catch (NoSuchElementException e)
            {
                Console.WriteLine("Element not found: " + e.Message);
            }
        }

        public void SelectDateRangeWithOffset(string startDateOffset, string endDateOffset)
        {
            DateTime today = DateTime.Today;

            startDate = CalculateDate(today, startDateOffset);
            endDate = CalculateDate(today, endDateOffset);

            Console.WriteLine($"Start Date: {startDate}");
            Console.WriteLine($"End Date: {endDate}");

            SelectDateRange(startDate, endDate);

            IWebElement activeElement = driver.SwitchTo().ActiveElement();
            activeElement.SendKeys(Keys.Enter);
        }

        private string CalculateDate(DateTime today, string offset)
        {
            char operatorSign = offset[0];
            int days = int.Parse(offset.Substring(1));

            DateTime newDate = operatorSign == '+'
                ? today.AddDays(days)
                : today.AddDays(-days);

            return newDate.ToString("yyyy-MM-dd");
        }

        public string GetStartDate()
        {
            return startDate;
        }

        public string GetEndDate()
        {
            return endDate;
        }

        public void EnterNotes(string notes)
        {
            driver.FindElement(notesField).SendKeys(notes);
        }

        public bool IsSectionDisplayed(string sectionName)
        {
            if (sectionName == "Balance")
            {
                return driver.FindElement(balanceSection).Displayed;
            }
            return false;
        }

        public void ClickButton(string buttonName)
        {
            IWebElement button = driver.FindElement(By.XPath("//button/span[text()='" + buttonName + "']"));

            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            js.ExecuteScript("arguments[0].scrollIntoView(true);", button);

            System.Threading.Thread.Sleep(500);
            js.ExecuteScript("arguments[0].click();", button);
        }

        public string GetPopupHeading()
        {
            return driver.FindElement(By.XPath("//div[@class='dialog__header heading-02']")).Text;
        }

        public void ConfirmLeaveRequest()
        {
            IWebElement confirmButtonElement = driver.FindElement(confirmButton);
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            js.ExecuteScript("arguments[0].click();", confirmButtonElement);
        }

        public bool IsSnackBarMessageDisplayed(string message)
        {
            try
            {
                IWebElement snackBarElement = driver.FindElement(snackBarMessage);

                return snackBarElement.Displayed && snackBarElement.Text.Trim() == message.Trim();
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }

        public bool IsErrorMessageCorrect(string expectedErrorMessage)
        {
            try
            {
                IWebElement alert = driver.FindElement(redAlert);

                return alert.Displayed && alert.Text.Trim() == expectedErrorMessage.Trim();
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }
    }
}

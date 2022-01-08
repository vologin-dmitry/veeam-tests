using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;

namespace BrowserJobCalculator
{
    /// <summary>
    /// Class to instantiate class and get number of vacancies 
    /// </summary>
    public class Page : IDisposable
    {
        public IWebDriver Driver { get; }
        public WebDriverWait Wait { get; }
        public Actions Action { get; }
        
        public Page(IWebDriver driver)
        {
            Driver = driver;
            Action = new Actions(Driver);
            Wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(30));
            Driver.Navigate().GoToUrl("https://careers.veeam.ru/vacancies");
            Driver.Manage().Window.Maximize();
            Wait.Until(_ =>
            {
                driver.FindElement(By.Id("cookiescript_accept")).Click();
                return true;
            });
        }

        public int CountVacancies()
        {
            return Driver.FindElements(By.XPath("//*[@class='card card-no-hover card-sm']")).Count;
        }

        public void Dispose()
        {
            Driver?.Dispose();
        }
    }
}
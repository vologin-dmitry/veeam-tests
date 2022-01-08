using OpenQA.Selenium;

namespace BrowserJobCalculator
{
    /// <summary>
    /// Class for finding element in page and clicking on it
    /// </summary>
    public class Clicker
    {
        private readonly Page _page;
        
        public Clicker(Page page)
        {
            _page = page;
        }

        public IWebElement Click(string name)
        {
            var toClick = _page.Driver.FindElement(By.XPath($".//*[contains(text(), '{name}')]"));
            return Click(toClick);
        }

        public IWebElement Click(IWebElement toClick)
        {
            _page.Action.MoveToElement(toClick);
            _page.Wait.Until(_ =>
            {
                toClick.Click();
                return true;
            });
            return toClick;
        }
    }
}

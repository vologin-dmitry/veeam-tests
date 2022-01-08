using OpenQA.Selenium;

namespace BrowserJobCalculator
{
    /// <summary>
    /// Class for selecting one option from a dropdown list
    /// </summary>
    public class DropdownSelector
    {
        private readonly IWebElement _item;
        private readonly Clicker _clicker;
        
        public DropdownSelector(string name, Page page)
        {
            _clicker = new Clicker(page);
            _item = page.Driver.FindElement(By.XPath($".//*[contains(text(), '{name}')]"));
        }

        public void Select(string toSelect)
        {
            var toDisable = _clicker.Click(_item);
            _clicker.Click(toSelect);
            if (toDisable.GetAttribute("aria-expanded") == "true")
                toDisable.Click();
        }
    }
}

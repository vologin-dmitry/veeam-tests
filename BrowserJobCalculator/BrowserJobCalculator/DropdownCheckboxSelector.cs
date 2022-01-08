using OpenQA.Selenium;

namespace BrowserJobCalculator
{
    /// <summary>
    /// Class for selecting one or multiple options from a checkbox dropdown list
    /// </summary>
    public class DropdownCheckboxSelector
    {

        private readonly IWebElement _item;
        private readonly Clicker _clicker;
        
        public DropdownCheckboxSelector(string name, Page page)
        {
            _item = page.Driver.FindElement(By.XPath($".//*[contains(text(), '{name}')]"));
            _clicker = new Clicker(page);
        }

        public void Select(string[] toSelect)
        {
            var toDisable = _clicker.Click(_item);
            foreach (var selectable in toSelect)
            {
                _clicker.Click(selectable);
            }
            toDisable.Click();
        }
    }
}

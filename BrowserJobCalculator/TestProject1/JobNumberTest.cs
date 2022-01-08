using BrowserJobCalculator;
using NUnit.Framework;
using OpenQA.Selenium.Chrome;

namespace JobNumberTests
{
    [Parallelizable(ParallelScope.Fixtures)]
    public class JobNumberTest
    {
        private DropdownCheckboxSelector _checkboxSelector;
        private DropdownSelector _dropdownSelector;
        private Page _page;
        
        [SetUp]
        public void Setup()
        {
            _page = new Page(new ChromeDriver());
            _checkboxSelector = new DropdownCheckboxSelector("Все языки", _page);
            _dropdownSelector = new DropdownSelector("Все отделы", _page);
        }

        [TearDown]
        public void TearDown()
        {
            _page.Dispose();
        }


        [Test]
        [TestCase(new[] {"Русский"}, "Разработка продуктов",  1)]
        [TestCase(new[] {"Английский"}, "Разработка продуктов",  6)]
        [TestCase(new[] {"Английский", "Русский"}, "Продажи",  15)]
        [TestCase(new[] {"Русский"}, "Продакт менеджмент",  0)]

        
        public void Parse_SimpleValues_Calculated(string[] languages, string department, int expectedResult)
        {
            _checkboxSelector.Select(languages); 
            _dropdownSelector.Select(department);
            Assert.AreEqual(expectedResult, _page.CountVacancies());
        }
    }
}
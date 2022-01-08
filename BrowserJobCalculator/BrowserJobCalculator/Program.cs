using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;


namespace BrowserJobCalculator
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                using var page = new Page(new ChromeDriver());
                new DropdownCheckboxSelector("Все языки", page).Select(new[] {"Английский"});
                new DropdownSelector("Все отделы", page).Select("Разработка продуктов");
                Console.WriteLine(page.CountVacancies());
            }
            catch (NoSuchElementException e)
            {
                Console.WriteLine("Element not found! " + e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}

using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;

namespace Lab5.Core
{
    public static class WebDriverFactory
    {
        public static IWebDriver CreateDriver(string browserName = "chrome")
        {
            return browserName.ToLower() switch
            {
                "chrome" => new ChromeDriver(),
                _ => throw new ArgumentException($"Browser '{browserName}' is not supported.")
            };
        }
    }
}
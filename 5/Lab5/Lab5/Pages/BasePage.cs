using Lab5.Core;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;

namespace Lab5.Pages
{
    public abstract class BasePage
    {
        protected IWebDriver Driver => DriverManager.Instance.Driver;
        protected WebDriverWait Wait => new WebDriverWait(Driver, TimeSpan.FromSeconds(10));

        public string GetUrl() => Driver.Url;
        public string GetPageSource() => Driver.PageSource;
    }
}
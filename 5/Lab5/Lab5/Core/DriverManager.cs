using OpenQA.Selenium;
using System;
using System.Threading;

namespace Lab5.Core
{
    public class DriverManager
    {
        private static readonly Lazy<DriverManager> _instance = new Lazy<DriverManager>(() => new DriverManager());

        private readonly ThreadLocal<IWebDriver> _driver = new ThreadLocal<IWebDriver>();

        private DriverManager() { }

        public static DriverManager Instance => _instance.Value;

        public IWebDriver Driver => _driver.Value;

        public void Start(string browser)
        {
            if (_driver.Value == null)
            {
                _driver.Value = WebDriverFactory.CreateDriver(browser);
                _driver.Value.Manage().Window.Maximize();
                _driver.Value.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
            }
        }

        public void Quit()
        {
            if (_driver.Value != null)
            {
                _driver.Value.Quit();
                _driver.Value.Dispose();
                _driver.Value = null;
            }
        }
    }
}
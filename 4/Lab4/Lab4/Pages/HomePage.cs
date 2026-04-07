using OpenQA.Selenium;

namespace Lab4.Pages
{
    public class HomePage : BasePage
    {
        private readonly string _baseUrl = "https://en.ehu.lt/";
        private By AboutLink => By.XPath("//a[contains(text(), 'About')]");

        public HomePage Open()
        {
            Driver.Navigate().GoToUrl(_baseUrl);
            return this;
        }

        public void ClickAbout()
        {
            Wait.Until(drv => drv.FindElement(AboutLink)).Click();
        }

        public void SearchFor(string searchTerm)
        {
            string encodedSearchTerm = searchTerm.Replace(" ", "+");
            Driver.Navigate().GoToUrl($"{_baseUrl}?s={encodedSearchTerm}");
        }

        public void SwitchToLithuanian()
        {
            Driver.Navigate().GoToUrl("https://lt.ehuniversity.lt/");
        }

        public bool IsLithuanianNewsPresent()
        {
            Wait.Until(drv => drv.FindElement(By.XPath("//*[contains(text(), 'Naujienos')]")));
            return GetPageSource().Contains("Naujienos");
        }

        public int GetEnglishNewsHeadlineCount()
        {
            return Driver.FindElements(By.XPath("//h2[contains(text(), 'News')]")).Count;
        }
    }
}
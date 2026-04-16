using OpenQA.Selenium;

namespace Lab5.Pages
{
    public class ContactPage : BasePage
    {
        private readonly string _url = "https://en.ehu.lt/contact/";

        public ContactPage Open()
        {
            Driver.Navigate().GoToUrl(_url);
            Wait.Until(drv => drv.FindElement(By.TagName("body")));
            return this;
        }

        public string GetPageText()
        {
            return Driver.FindElement(By.TagName("body")).Text;
        }

        public int GetSocialLinksCount()
        {
            var links = Driver.FindElements(By.XPath("//a[contains(@href, 'facebook.com') or contains(@href, 'telegram') or contains(@href, 'vk.com')]"));
            return links.Count;
        }
    }
}
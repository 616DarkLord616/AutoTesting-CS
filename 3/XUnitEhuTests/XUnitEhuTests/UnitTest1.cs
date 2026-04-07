using Xunit;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Linq;

namespace EhuTests.XUnit
{
    public class XUnitWebTests : IDisposable
    {
        private readonly IWebDriver driver;
        private readonly WebDriverWait wait;

        public XUnitWebTests()
        {
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        }

        public void Dispose()
        {
            driver?.Quit();
        }

        [Fact]
        [Trait("Category", "Navigation")]
        public void TestCase1_VerifyNavigationToAboutPage()
        {
            string expectedUrl = "https://en.ehuniversity.lt/about/";
            string expectedTitle = "About - European Humanities University";

            driver.Navigate().GoToUrl("https://en.ehu.lt/");

            var aboutLink = wait.Until(drv => drv.FindElement(By.XPath("//a[contains(text(), 'About')]")));
            aboutLink.Click();

            Assert.Equal(expectedUrl, driver.Url);
            Assert.Equal(expectedTitle, driver.Title);

            var headerElement = wait.Until(drv => drv.FindElement(By.XPath("//h1[contains(text(), 'About')]")));
            Assert.Contains("About", headerElement.Text);
        }

        // Data Provider реализован через Theory и InlineData
        [Theory]
        [Trait("Category", "Search")]
        [InlineData("study programs")]
        public void TestCase2_VerifySearchFunctionality(string searchTerm)
        {
            string encodedSearchTerm = searchTerm.Replace(" ", "+");
            string searchUrl = $"https://en.ehu.lt/?s={encodedSearchTerm}";

            driver.Navigate().GoToUrl(searchUrl);

            Assert.Contains($"/?s={encodedSearchTerm}", driver.Url);

            var resultsText = wait.Until(drv => drv.FindElement(By.XPath("//*[contains(text(), 'results found')]")));
            Assert.True(resultsText.Displayed, "No text found with the number of results");
            Assert.Contains(searchTerm.ToLower(), driver.PageSource.ToLower());

            var searchResults = driver.FindElements(By.CssSelector(".news-item, .post, article"));
            Assert.True(searchResults.Count > 0, "No search results found.");
        }

        [Fact]
        [Trait("Category", "Localization")]
        public void TestCase3_VerifyLanguageChangeToLithuanian()
        {
            string expectedUrl = "https://lt.ehuniversity.lt/";

            driver.Navigate().GoToUrl(expectedUrl);
            Assert.Equal(expectedUrl, driver.Url);

            wait.Until(drv => drv.FindElement(By.XPath("//*[contains(text(), 'Naujienos')]")));
            Assert.Contains("Naujienos", driver.PageSource);

            var englishNewsElement = driver.FindElements(By.XPath("//h2[contains(text(), 'News')]"));
            Assert.Empty(englishNewsElement);

            string[] lithuanianKeywords = { "Vilniuje", "Universiteto", "Studijų", "Programos" };
            bool containsLithuanianText = lithuanianKeywords.Any(keyword => driver.PageSource.Contains(keyword));
            Assert.True(containsLithuanianText, "No LT keywords found.");
        }

        [Fact]
        [Trait("Category", "Information")]
        public void TestCase4_VerifyContactFormAndInfo()
        {
            string[] expectedTexts = {
                "franciskscarynacr@gmail.com",
                "+370 68 771365",
                "+375 29 5781488",
                "Facebook",
                "Telegram",
                "VK"
            };

            driver.Navigate().GoToUrl("https://en.ehu.lt/contact/");
            wait.Until(drv => drv.FindElement(By.TagName("body")));

            string pageText = driver.FindElement(By.TagName("body")).Text;

            foreach (var expectedText in expectedTexts)
            {
                Assert.Contains(expectedText, pageText);
            }

            var socialLinks = driver.FindElements(By.XPath("//a[contains(@href, 'facebook.com') or contains(@href, 'telegram') or contains(@href, 'vk.com')]"));
            Assert.True(socialLinks.Count >= 3, "Less than 3 social links found.");
        }
    }
}
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Linq;

[assembly: Parallelizable(ParallelScope.Children)]
[assembly: LevelOfParallelism(4)]

namespace EhuTests.NUnit
{
    [TestFixture]
    [FixtureLifeCycle(LifeCycle.InstancePerTestCase)]
    public class NUnitWebTests
    {
        private IWebDriver driver;
        private WebDriverWait wait;

        [SetUp]
        public void Setup()
        {
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        }

        [TearDown]
        public void Teardown()
        {
            driver?.Quit();
        }

        [Test]
        [Category("Navigation")]
        public void TestCase1_VerifyNavigationToAboutPage()
        {
            string expectedUrl = "https://en.ehuniversity.lt/about/";
            string expectedTitle = "About - European Humanities University";

            driver.Navigate().GoToUrl("https://en.ehu.lt/");

            var aboutLink = wait.Until(drv => drv.FindElement(By.XPath("//a[contains(text(), 'About')]")));
            aboutLink.Click();

            Assert.That(driver.Url, Is.EqualTo(expectedUrl), "The page URL does not match.");
            Assert.That(driver.Title, Is.EqualTo(expectedTitle), "The page title is not correct.");

            var headerElement = wait.Until(drv => drv.FindElement(By.XPath("//h1[contains(text(), 'About')]")));
            Assert.That(headerElement.Text, Does.Contain("About"), "Title does not contain 'About'.");
        }

        private static string[] SearchTerms = { "study programs" };

        [Test]
        [Category("Search")]
        [TestCaseSource(nameof(SearchTerms))]
        public void TestCase2_VerifySearchFunctionality(string searchTerm)
        {
            string encodedSearchTerm = searchTerm.Replace(" ", "+");
            string searchUrl = $"https://en.ehu.lt/?s={encodedSearchTerm}";

            driver.Navigate().GoToUrl(searchUrl);

            Assert.That(driver.Url, Does.Contain($"/?s={encodedSearchTerm}"));

            var resultsText = wait.Until(drv => drv.FindElement(By.XPath("//*[contains(text(), 'results found')]")));
            Assert.That(resultsText.Displayed, Is.True, "No text found with the number of results");
            Assert.That(driver.PageSource.ToLower(), Does.Contain(searchTerm.ToLower()), $"Page does not contain '{searchTerm}'");

            var searchResults = driver.FindElements(By.CssSelector(".news-item, .post, article"));
            Assert.That(searchResults.Count, Is.GreaterThan(0), "No search results found.");
        }

        [Test]
        [Category("Localization")]
        public void TestCase3_VerifyLanguageChangeToLithuanian()
        {
            string expectedUrl = "https://lt.ehuniversity.lt/";

            driver.Navigate().GoToUrl(expectedUrl);
            Assert.That(driver.Url, Is.EqualTo(expectedUrl), "URL does not match lt version.");

            wait.Until(drv => drv.FindElement(By.XPath("//*[contains(text(), 'Naujienos')]")));
            Assert.That(driver.PageSource, Does.Contain("Naujienos"));

            var englishNewsElement = driver.FindElements(By.XPath("//h2[contains(text(), 'News')]"));
            Assert.That(englishNewsElement.Count, Is.EqualTo(0), "English headline found on LT page.");

            string[] lithuanianKeywords = { "Vilniuje", "Universiteto", "Studijų", "Programos" };
            bool containsLithuanianText = lithuanianKeywords.Any(keyword => driver.PageSource.Contains(keyword));
            Assert.That(containsLithuanianText, Is.True, "No LT keywords found.");
        }

        [Test]
        [Category("Information")]
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
                Assert.That(pageText.Contains(expectedText), Is.True, $"Text '{expectedText}' not found.");
            }

            var socialLinks = driver.FindElements(By.XPath("//a[contains(@href, 'facebook.com') or contains(@href, 'telegram') or contains(@href, 'vk.com')]"));
            Assert.That(socialLinks.Count, Is.GreaterThanOrEqualTo(3), "Less than 3 social links found.");
        }
    }
}
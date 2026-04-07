using EhuTests.Tests;
using Lab4.Pages;
using NUnit.Framework;
using System.Linq;

[assembly: Parallelizable(ParallelScope.Children)]
[assembly: LevelOfParallelism(4)]

namespace Lab4.Tests
{
    [TestFixture]
    [FixtureLifeCycle(LifeCycle.InstancePerTestCase)]
    public class EhuWebTests : BaseTest
    {
        private HomePage _homePage;
        private ContactPage _contactPage;

        [SetUp]
        public void InitPages()
        {
            _homePage = new HomePage();
            _contactPage = new ContactPage();
        }

        [Test]
        [Category("Navigation")]
        public void TestCase1_VerifyNavigationToAboutPage()
        {
            _homePage.Open().ClickAbout();

            Assert.That(_homePage.GetUrl(), Does.Contain("about"), "URL mismatch.");
        }

        private static string[] SearchTerms = { "study programs" };

        [Test]
        [Category("Search")]
        [TestCaseSource(nameof(SearchTerms))]
        public void TestCase2_VerifySearchFunctionality(string term)
        {
            _homePage.SearchFor(term);

            Assert.That(_homePage.GetUrl(), Does.Contain(term.Replace(" ", "+")));
            Assert.That(_homePage.GetPageSource().ToLower(), Does.Contain(term.ToLower()));
        }

        [Test]
        [Category("Localization")]
        public void TestCase3_VerifyLanguageChangeToLithuanian()
        {
            _homePage.SwitchToLithuanian();

            Assert.That(_homePage.GetUrl(), Is.EqualTo("https://lt.ehuniversity.lt/"));
            Assert.That(_homePage.IsLithuanianNewsPresent(), Is.True, "Naujienos not found.");
            Assert.That(_homePage.GetEnglishNewsHeadlineCount(), Is.EqualTo(0), "English 'News' found on LT page.");

            string[] lithuanianKeywords = { "Vilniuje", "Universiteto", "Studijų", "Programos" };
            bool containsLithuanianText = lithuanianKeywords.Any(keyword => _homePage.GetPageSource().Contains(keyword));
            Assert.That(containsLithuanianText, Is.True, "No LT keywords found.");
        }

        [Test]
        [Category("Information")]
        public void TestCase4_VerifyContactFormAndInfo()
        {
            _contactPage.Open();

            string pageText = _contactPage.GetPageText();
            string[] expectedTexts = {
                "franciskscarynacr@gmail.com",
                "+370 68 771365",
                "+375 29 5781488",
                "Facebook",
                "Telegram",
                "VK"
            };

            foreach (var expectedText in expectedTexts)
            {
                Assert.That(pageText, Does.Contain(expectedText), $"Text '{expectedText}' not found.");
            }

            Assert.That(_contactPage.GetSocialLinksCount(), Is.GreaterThanOrEqualTo(3), "Less than 3 social links found.");
        }
    }
}
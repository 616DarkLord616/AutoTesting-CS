using Lab5.Pages;
using NUnit.Framework;
using Reqnroll;
using System.Linq;

namespace Lab5.Steps
{
    [Binding]
    public class EhuJourneySteps
    {
        private HomePage _homePage = new HomePage();
        private ContactPage _contactPage = new ContactPage();

        [Given(@"I navigate to the EHU home page")]
        public void GivenINavigateToTheEHUHomePage()
        {
            _homePage.Open();
        }

        [When(@"I click on the About link")]
        public void WhenIClickOnTheAboutLink()
        {
            _homePage.ClickAbout();
        }

        [Then(@"the page URL should contain ""(.*)""")]
        public void ThenThePageURLShouldContain(string expectedText)
        {
            Assert.That(_homePage.GetUrl(), Does.Contain(expectedText), "URL mismatch.");
        }

        [When(@"I search for ""(.*)""")]
        public void WhenISearchFor(string searchTerm)
        {
            _homePage.SearchFor(searchTerm);
        }

        [Then(@"the search results should contain ""(.*)""")]
        public void ThenTheSearchResultsShouldContain(string searchTerm)
        {
            Assert.That(_homePage.GetUrl(), Does.Contain(searchTerm.Replace(" ", "+")));
            Assert.That(_homePage.GetPageSource().ToLower(), Does.Contain(searchTerm.ToLower()));
        }

        [When(@"I switch the language to Lithuanian")]
        public void WhenISwitchTheLanguageToLithuanian()
        {
            _homePage.SwitchToLithuanian();
        }

        [Then(@"the page should contain Lithuanian keywords and no English news")]
        public void ThenThePageShouldContainLithuanianKeywordsAndNoEnglishNews()
        {
            Assert.That(_homePage.GetUrl(), Is.EqualTo("https://lt.ehuniversity.lt/"));
            Assert.That(_homePage.IsLithuanianNewsPresent(), Is.True, "Naujienos not found.");
            Assert.That(_homePage.GetEnglishNewsHeadlineCount(), Is.EqualTo(0), "English 'News' found on LT page.");

            string[] lithuanianKeywords = { "Vilniuje", "Universiteto", "Studijų", "Programos" };
            bool containsLithuanianText = lithuanianKeywords.Any(keyword => _homePage.GetPageSource().Contains(keyword));
            Assert.That(containsLithuanianText, Is.True, "No LT keywords found.");
        }

        [Given(@"I navigate to the Contact page")]
        public void GivenINavigateToTheContactPage()
        {
            _contactPage.Open();
        }

        [Then(@"the page should display expected contact details and social links")]
        public void ThenThePageShouldDisplayExpectedContactDetailsAndSocialLinks()
        {
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
using Lab5.Core;
using NUnit.Framework;

namespace EhuTests.Tests
{
    public class BaseTest
    {
        [SetUp]
        public void Setup()
        {
            DriverManager.Instance.Start("chrome");
        }

        [TearDown]
        public void Teardown()
        {
            DriverManager.Instance.Quit();
        }
    }
}
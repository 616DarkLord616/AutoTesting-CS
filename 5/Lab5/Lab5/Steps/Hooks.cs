using Lab5.Core;
using Reqnroll;
using System;

namespace Lab5.Steps
{
    [Binding]
    public class Hooks
    {
        private readonly ScenarioContext _scenarioContext;

        public Hooks(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }

        [BeforeScenario]
        public void BeforeScenario()
        {
            DriverManager.Instance.Start("chrome");
        }

        [AfterScenario]
        public void AfterScenario()
        {
            if (_scenarioContext.TestError != null)
            {
                Console.WriteLine("===============================================");
                Console.WriteLine($"[FAILED SCENARIO] -> {_scenarioContext.ScenarioInfo.Title}");
                Console.WriteLine($"[ERROR MESSAGE]   -> {_scenarioContext.TestError.Message}");
                Console.WriteLine($"[STACK TRACE]     -> {_scenarioContext.TestError.StackTrace}");
                Console.WriteLine("===============================================");
            }

            DriverManager.Instance.Quit();
        }
    }
}
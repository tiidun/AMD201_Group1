using System;
using System.Diagnostics;

using NUnit.Framework;

using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

using URLShortener.Tests.Common;

namespace URLShortener.SeleniumTests
{
    public class SeleniumTestsBase
    {
        protected TestDb testDb;
        protected IWebDriver driver;
        protected TestUrlShortenerApp<Program> testUrlShortenerApp;
        protected string baseUrl;
        protected string username;
        protected string password;

        [OneTimeSetUp]
        public void OneTimeSetUpBase()
        {
            // Run the Web app in a local Web server
            this.testDb = new TestDb();

            this.testUrlShortenerApp = new TestUrlShortenerApp<Program>(this.testDb);
            this.testUrlShortenerApp.CreateClient();

            this.baseUrl = this.testUrlShortenerApp.HostUrl;

            // Setup the user
            this.username = $"user{DateTime.Now.Ticks.ToString()[10..]}";
            this.password = $"pass{DateTime.Now.Ticks.ToString()[10..]}";

            // Setup the ChromeDriver
            ChromeOptions chromeOptions = new ChromeOptions();
            if (!Debugger.IsAttached)
                chromeOptions.AddArguments("headless");
            chromeOptions.AddArgument("--window-size=1920,1080");
            chromeOptions.AddArguments("--start-maximized");
            this.driver = new ChromeDriver(chromeOptions);

            // Set an implicit wait for the UI interaction
            this.driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
        }

        [OneTimeTearDown]
        public void OneTimeTearDownBase()
        {
            // Stop and dispose the Selenium driver
            this.driver.Quit();

            // Stop and dispose the local Web server
            this.testUrlShortenerApp.Dispose();
        }
    }
}

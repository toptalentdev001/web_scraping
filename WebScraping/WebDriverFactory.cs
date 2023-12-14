using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;

namespace WebScraping
{
    public class WebDriverFactory
    {
        private static IWebDriver ?driver;

        public static IWebDriver InitializeChromeDriver()
        {
            var chromeOptions = new ChromeOptions();
            chromeOptions.AddArguments("--silent", "log-level=3");

            var chromeDriverService = ChromeDriverService.CreateDefaultService();
            chromeDriverService.HideCommandPromptWindow = true;
            chromeDriverService.SuppressInitialDiagnosticInformation = true;
            chromeDriverService.EnableVerboseLogging = false;
            driver = new ChromeDriver(chromeDriverService, chromeOptions);

            return driver;
        }

        public static void QuitDriver(IWebDriver driver)
        {
            if (driver != null)
            {
                try
                {
                    driver.Quit();
                }
                finally
                {
                    driver = null;
                }
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;

namespace WebScraping
{
    internal class WebDriverFactory
    {
        public static IWebDriver InitializeChromeDriver()
        {
            var chromeOptions = new ChromeOptions();
            chromeOptions.AddArguments("headless", "--silent", "log-level=3", "--disable-blink-features=AutomationControlled");

            var chromeDriverService = ChromeDriverService.CreateDefaultService();
            chromeDriverService.HideCommandPromptWindow = true;
            chromeDriverService.SuppressInitialDiagnosticInformation = true;
            chromeDriverService.EnableVerboseLogging = false;

            return new ChromeDriver(chromeDriverService, chromeOptions);
        }
    }
}

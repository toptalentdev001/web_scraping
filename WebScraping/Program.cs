using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Collections.Generic;

namespace WebScraping;

public static class HelloSelenium
{
    static IWebDriver InitializeChromeDriver()
    {
        var chromeOptions = new ChromeOptions();
        chromeOptions.AddArguments("headless", "--silent", "log-level=3");

        var chromeDriverService = ChromeDriverService.CreateDefaultService();
        chromeDriverService.HideCommandPromptWindow = true;
        chromeDriverService.SuppressInitialDiagnosticInformation = true;
        chromeDriverService.EnableVerboseLogging = false;

        return new ChromeDriver(chromeDriverService, chromeOptions);
    }

    public static void Main()
    {
        var welcome = "[i] Welcome to the DevOps Web Scraper!\n";
        var welcomePadded = welcome.PadLeft(10, ' ').PadRight(10, ' ');
        Console.WriteLine(welcomePadded);
        Menu menu = new Menu();
        MenuHandler menuHandler = new MenuHandler(menu);

        menu.Display();
        menuHandler.ProcessUserSelection();

    }
}
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Collections.Generic;
using Figgle;

namespace WebScraping;

public static class HelloSelenium
{
    public static void Main()
    {
        // Configure command prompt
        Console.Title = "DevOps WebScraper v2.0";
        Console.SetWindowSize(Console.LargestWindowWidth - 10, Console.LargestWindowHeight - 10);
        Console.SetWindowPosition(0, 0);

        // Hello message
        Console.WriteLine(FiggleFonts.Standard.Render("DevOps Webscraper v2.0"));
        Console.WriteLine("\n----- [i] Welcome to the DevOps Web Scraper! -----\n");

        // Display menu and process selection
        Menu menu = new();
        menu.Display();
        menu.ProcessUserSelection();

        // Ask for menu after process finishesµ
        menu.AskForMenu();
    }
}

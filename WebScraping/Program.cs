using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Collections.Generic;

namespace WebScraping;

public static class HelloSelenium
{
    public static void Main()
    {
        var welcome = "[i] Welcome to the DevOps Web Scraper!\n";
        var welcomePadded = welcome.PadLeft(10, ' ').PadRight(10, ' ');
        Console.WriteLine(welcomePadded);
        
        // Display menu and process selection
        Menu menu = new Menu();
        MenuHandler menuHandler = new();

        menu.Display();
        menuHandler.ProcessUserSelection();
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebScraping;
internal class MenuHandler
{
    public Menu menu = new();
        
    public MenuHandler() {}

    public void ProcessUserSelection()
    {
        string userSelection = menu.GetUserSelection();

        switch (userSelection)
        {
            case "Youtube": YoutubeScraper.ScrapeVideos();
                break;
            case "Ictjobs.be": IctjobsScraper.ScrapeJobs();
                break;
            case "Azerty.nl": AzertyScraper.ScrapeProducts();
                break;
            case "69": QrCode.PrintQRCode();
                    break;
        }
    }

    public void AskForMenu()
    {
        string decision;
        bool isToBeContinued = true;

        while (isToBeContinued)
        {
            Console.Write("\nDo you want to try another option? (Y/N) ");
            decision = Console.ReadLine().ToLower(); // Convert to lowercase immediately

            if (decision.Equals("y"))
            {
                menu.Display();
                ProcessUserSelection();
            }
            else if (decision.Equals("n"))
            {
                Console.WriteLine("Thanks for using the DevOps Web Scraper! :)");
                isToBeContinued = false; // Stop the loop from continuing
            }
            else
            {
                Console.WriteLine("[!] Invalid input. Please choose from the available options.");
                continue;
            }   
        }
    }
}

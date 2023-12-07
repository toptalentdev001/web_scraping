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

        do
        {
            Console.Write("\nDo you want to try another option? (Y/N) ");
            decision = Console.ReadLine();

            if (!string.IsNullOrWhiteSpace(decision) && decision.Equals("y")) {
                menu.Display();
                ProcessUserSelection();
            } else
            {
                Console.WriteLine("Thanks for using the DevOps Web Scraper! :)");
            }

        } while (string.IsNullOrWhiteSpace(decision));
    }
}

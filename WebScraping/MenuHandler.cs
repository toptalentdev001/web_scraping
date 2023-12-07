using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebScraping;
public class Menu
{
    public static readonly Dictionary<int, string> possibleChoices = new Dictionary<int, string> {
            {1, "Youtube"},
            {2, "Ictjobs.be"},
            {3, "Azerty.nl"}
    };

    public string userChoice;

    public Menu() {}

    public void Display()
    {
        Console.WriteLine("[?] Choose a website to scrape:\n");
        foreach (var choice in possibleChoices)
        {
            Console.WriteLine($"{choice.Key}: {choice.Value}");
        }
        Console.Write("\n");
    }

    public void ProcessUserSelection()
    {
        int parsedSelection;
        do
        {
            Console.Write("[?] Enter your choice: ");
            string selection = Console.ReadLine();

            if (!int.TryParse(selection, out parsedSelection))
            {
                Console.WriteLine("[!] Invalid input. Please enter a valid number.");
                continue;
            }

            if (!possibleChoices.ContainsKey(parsedSelection))
            {
                Console.WriteLine("[!] Invalid input. Please choose from the available options.");
            }

            if (parsedSelection == 69)
            {
                Console.Clear();
                break;
            }

        } while (!possibleChoices.ContainsKey(parsedSelection));

        switch (parsedSelection)
        {
            case 1:
                YoutubeScraper.ScrapeVideos();
                break;
            case 2:
                IctjobsScraper.ScrapeJobs();
                break;
            case 3:
                AzertyScraper.ScrapeProducts();
                break;
            case 69:
                QrCode.PrintQRCode();
                break;
        }
    }

    public void AskForMenu()
    {
        string decision;
        bool isToBeContinued = true;

        while (isToBeContinued)
        {
            Console.Write("\n[?] Do you want to try another option? (Y/N): ");
            decision = Console.ReadLine().ToLower(); // Convert to lowercase immediately

            if (decision.Equals("y"))
            {
                // Reset userChoice before processing the selection again
                userChoice = "";
                Display();
                ProcessUserSelection();
            }
            else if (decision.Equals("n"))
            {
                Console.WriteLine("\nThanks for using the DevOps Web Scraper! :)");
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

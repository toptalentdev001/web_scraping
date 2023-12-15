using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Figgle;

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
        Console.WriteLine("[i] Choose a website to scrape:\n");
        foreach (var choice in possibleChoices)
        {
            Console.WriteLine($"{choice.Key}: {choice.Value}");
        }
    }

    public void ProcessUserSelection()
    {
        int parsedSelection;
        do
        {
            Console.Write("\n[?] Enter your choice: ");
            string selection = Console.ReadLine();

            if (!int.TryParse(selection, out parsedSelection))
            {
                Console.WriteLine("[!] Invalid input. Please enter a valid number.");
                continue;
            }

            if (parsedSelection == 69)
            {
                break;
            }

            if (!possibleChoices.ContainsKey(parsedSelection))
            {
                Console.WriteLine("[!] Invalid input. Please choose from the available options.");
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
            decision = Console.ReadLine().ToLower(); // Convert to lowercase

            if (decision.Equals("y"))
            {
                Console.Clear();

                // Hello message
                Console.WriteLine(FiggleFonts.Standard.Render("DevOps Webscraper v2.0"));
                Console.WriteLine("\n----- [i] Welcome to the DevOps Web Scraper! -----\n");

                // Reset userChoice before processing the selection again
                userChoice = "";
                
                Display();
                ProcessUserSelection();
            }
            else if (decision.Equals("n"))
            {
                Console.Clear();
                Console.WriteLine("\n[i] Thank you for using the DevOps Web Scraper! :)\n");
                Console.WriteLine(FiggleFonts.Rectangles.Render("Thank You!"));
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

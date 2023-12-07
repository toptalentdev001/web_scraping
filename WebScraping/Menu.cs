using System;
using System.Collections.Generic;
using System.Linq;

namespace WebScraping;
internal class Menu
{
    private static readonly Dictionary<int, string> possibleChoices = new Dictionary<int, string> {
            {1, "Youtube"},
            {2, "Ictjobs.be"},
            {3, "Azerty.nl"}
    };

    public Menu() { }

    public void Display()
    {
        Console.WriteLine("Choose a website to scrape:\n");
        foreach (var choice in possibleChoices)
        {
            Console.WriteLine($"{choice.Key}: {choice.Value}");
        }
        Console.Write("\n");
    }

    public string GetUserSelection()
    {
        int parsedSelection;

        do
        {
            Console.Write("[?] Enter your choice: ");
            string selection = Console.ReadLine();

            if (selection is not null && selection.ToLower() == "s")
            {
                // If the user enters 's', exit the loop and the program
                Console.WriteLine("Goodbye...");
                Environment.Exit(0);
            }

            if (!int.TryParse(selection, out parsedSelection))
            {
                Console.WriteLine("[!] Invalid input. Please enter a valid number.");
                continue; // Skip the rest of the loop iteration
            }

            if (!possibleChoices.ContainsKey(parsedSelection))
            {
                Console.WriteLine("[!] Invalid selection. Please choose from the available options.");
            }

            if (parsedSelection == 69)
            {
                Console.Clear();
                return "69";
            }

        } while (!(possibleChoices.ContainsKey(parsedSelection)) && parsedSelection != 69);

        return possibleChoices[parsedSelection];
    }
}

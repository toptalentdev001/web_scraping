using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using System.Text;
using System.Xml.Linq;

namespace WebScraping
{
    public class IctjobsScraper
    {
        public static IWebDriver driver = WebDriverFactory.InitializeChromeDriver();

        public IctjobsScraper() {}

        private static string GetSearchTerm()
        {
            string searchTerm;

            do
            {
                Console.Write("\nEnter a search term: ");
                searchTerm = Console.ReadLine();

            } while (string.IsNullOrEmpty(searchTerm) || string.IsNullOrWhiteSpace(searchTerm));

            Console.WriteLine($"\nSearching for {searchTerm} on ictjob.be ...\n");

            return searchTerm;
        }

        public static List<string> ExtractElements(IReadOnlyList<IWebElement> elements, int count)
        {
            List<string> extractedTexts = new List<string>();

            for (int i = 0; extractedTexts.Count < 5; i++)
            {
                if (elements[i].Text.Length > 0)
                {
                    extractedTexts.Add(elements[i].Text);
                }
            }

            return extractedTexts;
        }

        public static List<string> GetJobTitles()
        {
            var jobs = driver.FindElements(By.ClassName("job-title"));
            var jobTitles = ExtractElements(jobs, 5);

            return jobTitles;
        }

        public static List<string> GetJobCompanies()
        {
            var companies = driver.FindElements(By.ClassName("job-company"));
            var jobCompanies = ExtractElements(companies, 5);

            return jobCompanies;
        }

        public static List<string> GetJobLocations()
        {
            var locations = driver.FindElements(By.XPath("//span[contains(@itemprop, 'addressLocality')]"));
            var jobLocations = ExtractElements(locations, 5);

            return jobLocations;
        }

        public static List<string> GetJobUrls()
        {
            var urls = driver.FindElements(By.CssSelector(".job-title.search-item-link"));
            List<string> jobUrls = new List<string>();

            for (int i = 0; jobUrls.Count < 5; i++)
            {
                var jobUrl = urls[i].GetAttribute("href");

                if (jobUrl != null)
                {
                    jobUrls.Add(jobUrl);
                }
            }

            return jobUrls;
        }

        public static void ScrapeJobs()
        {
            string jobSearchTerm = GetSearchTerm();
            driver.Navigate().GoToUrl($"https://www.ictjob.be/en/search-it-jobs?keywords={jobSearchTerm}");

            var cookieButton = driver.FindElement(By.CssSelector(".button.cookie-layer-button.close-layer-button"));
            var sortByDateButton = driver.FindElement(By.Id("sort-by-date"));
            
            var js = (IJavaScriptExecutor)driver;
            js.ExecuteScript("arguments[0].click();", cookieButton);
            js.ExecuteScript("arguments[0].click();", sortByDateButton);

            var vacancies = GetJobTitles();
            var hiringOrganizations = GetJobCompanies();
            var vacancyLocations = GetJobLocations();
            var vacancyUrls = GetJobUrls();

            for (int i = 0; i < 5; i++)
            {
                Console.WriteLine($"*---------------------------------------------*");
                Console.WriteLine($"| {vacancies[i]}");
                Console.WriteLine($"| @ {hiringOrganizations[i]}");
                Console.WriteLine($"| in {vacancyLocations[i]}");
                Console.WriteLine($"| URL: {vacancyUrls[i]}");
                Console.WriteLine($"*---------------------------------------------*\n");
            }

            driver.Quit();
        }
    }
}
